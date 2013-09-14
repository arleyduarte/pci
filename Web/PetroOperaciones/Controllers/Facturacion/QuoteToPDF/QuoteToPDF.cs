using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System.IO;
using PetroOperaciones.Models.Basic;
using invoiceToPDF;
using PetroOperaciones.Models.Cotizacion;

namespace QuoteToPDF
{
    public class QuoteToPDFGenerator
    {
        



        private String invoiceDestinationFile;
        private String currencyType = "COP";
        private String pathOfTemplates;
        private String pathOfInvoiceTmp;
        private static String QUOTE_PREFIX_NAME = "Cotizacion-";

        private ArrayList vInvoiceFiles = new ArrayList();

        public QuoteToPDFGenerator(String pathOfTemplates, String pathOfFilesTmp, String currencyType)
        {
            this.pathOfTemplates = pathOfTemplates;
            this.pathOfInvoiceTmp = pathOfFilesTmp;
            this.currencyType = currencyType;
            
        }


        public String makeQuoteToPdf(MasterDetail masterDetail)
        {

            CotizacionEncabezado encabezado = (CotizacionEncabezado)masterDetail.IMasterEntity;
            String quoteNumber = encabezado.CotizacionEncabezadoID.ToString();


            PdfPTable mainTable = new PdfPTable(PDFQuoteHelper.tableWidths);
            QuoteDetailsToPDF invoiceDetailsToPDF = new QuoteDetailsToPDF();
            invoiceDetailsToPDF.fillMainTable(masterDetail.Details, mainTable, currencyType);

            PDFQuoteHelper invoiceHelper = new PDFQuoteHelper(pathOfTemplates, pathOfInvoiceTmp, vInvoiceFiles);
            invoiceHelper.createInvoicesPages(encabezado, mainTable, currencyType);


            invoiceDestinationFile = pathOfInvoiceTmp + "Prefactura_Tmp_" + quoteNumber + ".pdf";
            PdfMerge.MergeFiles(invoiceDestinationFile, getInvoiceSubFiles());


            String timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");
            String[] labeledFiles = { pathOfInvoiceTmp + "Original_" + timeStamp + ".pdf" };
            int numberOfPages = getInvoiceSubFiles().Length;

            labelWithCopyOriginal(numberOfPages, labeledFiles[0], "");


            String finalInvoiceName = QUOTE_PREFIX_NAME + quoteNumber + ".pdf";

            String finalInvoicePath = pathOfInvoiceTmp + finalInvoiceName;

            PdfMerge.MergeFiles(finalInvoicePath, labeledFiles);

            return finalInvoiceName;
        }

        private void labelWithCopyOriginal(int totalNumberOfPages, String file, String label)
        {

            PdfReader reader = new PdfReader(invoiceDestinationFile);

            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
            {

                using (PdfStamper stamper = new PdfStamper(reader, fs))
                {


                    for (int i = 1; i <= totalNumberOfPages; i++)
                    {
                        PdfContentByte cb = stamper.GetOverContent(i);
                        tagPageWithCopyMessage(cb, label);

                    }

                    stamper.Close();
                }



            }


        }
        private void tagPageWithCopyMessage(PdfContentByte cb, String label)
        {
            cb.SaveState();
            BaseFont bf = BaseFont.CreateFont();
            cb.BeginText();
            cb.SetTextRenderingMode(
            PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
            cb.SetLineWidth(0.5f);
            cb.SetRGBColorStroke(0x00, 0x00, 0x00);
            cb.SetRGBColorFill(0x00, 0x00, 0x00);
            cb.SetFontAndSize(bf, 9);
            cb.SetTextMatrix(0, 1, -1, 0, 580, 300);
            cb.ShowText(label);
            cb.EndText();
            cb.RestoreState();

        }

        private String[] getInvoiceSubFiles()
        {
            String[] invoiceFiles = new String[vInvoiceFiles.Count];

            for (int i = 0; i < vInvoiceFiles.Count; i++)
            {
                invoiceFiles[i] = vInvoiceFiles[i].ToString();
            }

            return invoiceFiles;
        }


    }
}
