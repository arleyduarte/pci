using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System.IO;

namespace invoiceToPDF
{
    public class InvoiceToPDF
    {
        
        private PdfPTable mainTable;
        private ArrayList vInvoiceFiles = new ArrayList();
        private String invoiceDestinationFile;
        private String currencyType = "COP";
        private String pathOfTemplates;
        private String pathOfInvoiceTmp;
        private static String INVOICE_NAME = "Factura-";
        private static String PRE_INVOICE_NAME = "Prefactura-";

        public InvoiceToPDF(String pathOfTemplates, String pathOfInvoiceTmp, String currencyType)
        {
            this.currencyType = currencyType;
            this.pathOfTemplates = pathOfTemplates;
            this.pathOfInvoiceTmp = pathOfInvoiceTmp;
            
        }
        public String makePreInvoiceToPdf(FacturaModel invoiceModel)
        {
            invoiceModel.FacturaEncabezado.convertirEnPrefactura();
            mainTable = new PdfPTable(PDFInvoiceHelper.tableWidths);
            InvoiceDetailsToPDF invoiceDetailsToPDF = new InvoiceDetailsToPDF();
            invoiceDetailsToPDF.fillMainTable(invoiceModel.Detalles, mainTable, currencyType);

            PDFInvoiceHelper invoiceHelper = new PDFInvoiceHelper(pathOfTemplates, pathOfInvoiceTmp, vInvoiceFiles);
            invoiceHelper.createInvoicesPages(invoiceModel.FacturaEncabezado, mainTable, currencyType);


            invoiceDestinationFile = pathOfInvoiceTmp + "Prefactura_Tmp_" + invoiceModel.FacturaEncabezado.NoFacturaSoporte + ".pdf";
            PdfMerge.MergeFiles(invoiceDestinationFile, getInvoiceSubFiles());


            String timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");
            String[] labeledFiles = { pathOfInvoiceTmp + "Original_" + timeStamp + ".pdf" };
            int numberOfPages = getInvoiceSubFiles().Length;

            labelWithCopyOriginal(numberOfPages, labeledFiles[0], "Original");

            String finalInvoiceName = PRE_INVOICE_NAME + invoiceModel.FacturaEncabezado.NoFacturaSoporte + ".pdf";

            String finalInvoicePath = pathOfInvoiceTmp + finalInvoiceName;

            PdfMerge.MergeFiles(finalInvoicePath, labeledFiles);

            return finalInvoiceName;
        }


        public String makeInvoiceToPdf(FacturaModel invoiceModel)
        {

            mainTable = new PdfPTable(PDFInvoiceHelper.tableWidths);
            InvoiceDetailsToPDF invoiceDetailsToPDF = new InvoiceDetailsToPDF();
            invoiceDetailsToPDF.fillMainTable(invoiceModel.Detalles, mainTable, currencyType);

            PDFInvoiceHelper invoiceHelper = new PDFInvoiceHelper(pathOfTemplates, pathOfInvoiceTmp, vInvoiceFiles);
            invoiceHelper.createInvoicesPages(invoiceModel.FacturaEncabezado, mainTable, currencyType);


            invoiceDestinationFile = pathOfInvoiceTmp+"Factura_Tmp_" + invoiceModel.FacturaEncabezado.NoFacturaSoporte + ".pdf";
            PdfMerge.MergeFiles(invoiceDestinationFile, getInvoiceSubFiles());


            String timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");
            String[] labeledFiles = {pathOfInvoiceTmp+"Original_" + timeStamp + ".pdf", pathOfInvoiceTmp+"Copy_" + timeStamp + ".pdf", pathOfInvoiceTmp+"BookkeperCopy_" + timeStamp + ".pdf" };
            int numberOfPages = getInvoiceSubFiles().Length;

            labelWithCopyOriginal(numberOfPages, labeledFiles[0], "Original");
            labelWithCopyOriginal(numberOfPages, labeledFiles[1], "Copia");
            labelWithCopyOriginal(numberOfPages, labeledFiles[2], "Copia Contable");

            String finalInvoiceName = INVOICE_NAME + invoiceModel.FacturaEncabezado.NoFacturaSoporte + ".pdf";

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
            cb.SetTextMatrix(0, 1, -1, 0,580, 300); 
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
