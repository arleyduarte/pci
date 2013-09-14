using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using System.Collections;
using iTextSharp.text;
using System.IO;
using PetroOperaciones.Models.Facturacion;

namespace invoiceToPDF
{
    public class PDFInvoiceHelper
    {

        public static String TEMPLATE_INVOICE_FORMAT = "Formato-Factura-Petrocargo.pdf";
        public static String TEMPLATE_BIG_INVOICE_FORMAT = "Formato-Factura-Petrocargo-Continuo.pdf";
        private static int VERTICAL_SPACE = 331;
        private int MAX_ROWS_PER_PAGE = 21;
        private int MAX_ROWS_PER_PAGE_MULTIPLE_PAGES = 29;
        
        private static float CODE_COLUM_WIDTH = 29f;
        private static float CONCEPT_COLUM_WIDTH = 211f;
        private static float VALUECOP_COLUM_WIDTH = 54f;
        private static float VALUEUSD_COLUM_WIDTH = 54f;

        public static float[] tableWidths = { CODE_COLUM_WIDTH, CONCEPT_COLUM_WIDTH, VALUECOP_COLUM_WIDTH, VALUEUSD_COLUM_WIDTH };
        
        private PdfPTable mainTable;
        private string templateFileMultiplePages;
        private string templateFile;
        private ArrayList vInvoiceFiles;
        private String pathOfInvoiceTmp;
        private FacturaEncabezado invoiceHeader;
        private String currencyType;

        public PDFInvoiceHelper(String pathOfTemplates, String pathOfInvoiceTmp, ArrayList vInvoiceFiles)
        {
            this.vInvoiceFiles = vInvoiceFiles;
            templateFileMultiplePages = pathOfTemplates + TEMPLATE_BIG_INVOICE_FORMAT;
            templateFile = pathOfTemplates + TEMPLATE_INVOICE_FORMAT;
            this.pathOfInvoiceTmp = pathOfInvoiceTmp;

        }

        public void createInvoicesPages(FacturaEncabezado invoiceHeader, PdfPTable mainTable, String currencyType)
        {
            this.mainTable = mainTable;
            this.invoiceHeader = invoiceHeader;
            this.currencyType = currencyType;

            if (mainTable.Rows.Count > MAX_ROWS_PER_PAGE)
            {
                ArrayList vSubTables = getSubTables();
                int numberOfSubTables = vSubTables.Count;

                for (int i = 0; i < numberOfSubTables; i++)
                {
                    PdfPTable subtable = (PdfPTable)vSubTables[i];
                    if (i == numberOfSubTables - 1)
                    {
                        makePage(subtable, true);
                    }
                    else
                    {
                        makePage(subtable, false);
                    }

                }
            }
            else
            {
                makePage(mainTable, true);
            }
        }

        private void makePage(PdfPTable subtable, bool isTotaSubTable)
        {
            String templeteToUse = templateFileMultiplePages;
            if (isTotaSubTable)
            {
                templeteToUse = templateFile;
            }

            PdfReader reader = new PdfReader(templeteToUse);
            string invoiceAuxFile = pathOfInvoiceTmp+"InvoicePart_" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf";

            vInvoiceFiles.Add(invoiceAuxFile);

            using (FileStream fs = new FileStream(invoiceAuxFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (PdfStamper stamper = new PdfStamper(reader, fs))
                {
                    PdfContentByte cb = stamper.GetOverContent(1);
                    ColumnText ct = new ColumnText(cb);


                    ct.SetSimpleColumn(-9, 0,
                              PageSize.LETTER.Width + 9, PageSize.LETTER.Height - VERTICAL_SPACE,
                              18, Element.ALIGN_JUSTIFIED);


                    InvoiceMasterToPDF invoiceMasterToPDF = new InvoiceMasterToPDF();
                    invoiceMasterToPDF.fillInvoiceMaster(invoiceHeader, stamper);

                    if (isTotaSubTable)
                    {
                        invoiceMasterToPDF.fillTotalsInvoiceMaster(invoiceHeader, stamper, currencyType);
                    }



                    ct.AddElement(subtable);
                    ct.Go();
                    stamper.Close();
                }
            }
        }

        private ArrayList getSubTables()
        {
            ArrayList tables = new ArrayList();

            int numberOfRows = mainTable.Rows.Count;
            int numberOfTables = 1;
            int startRow = 0;
            int endRow = numberOfRows;


            if (numberOfRows > MAX_ROWS_PER_PAGE)
            {
                numberOfTables = numberOfRows / MAX_ROWS_PER_PAGE_MULTIPLE_PAGES;
                //En este caso ocurre que no cabe en la tabla de 21 espacios pero no llena la de 28
                if (numberOfTables == 0)
                {
                    numberOfTables = 2;
                }

                else
                {
                    int capacity = MAX_ROWS_PER_PAGE + (MAX_ROWS_PER_PAGE_MULTIPLE_PAGES * (numberOfTables - 1));
                    int difCapacity = numberOfRows - capacity;
                    if (difCapacity > 0)
                    {
                        double div = Convert.ToDouble(difCapacity) / Convert.ToDouble(MAX_ROWS_PER_PAGE);
                        int pagesToSum = 1;
                        if (div > 1)
                        {
                            if ((difCapacity % MAX_ROWS_PER_PAGE) > 1)
                            {
                                pagesToSum = 2;
                            }


                        }

                        numberOfTables = numberOfTables + pagesToSum;


                    }

                }

                if (MAX_ROWS_PER_PAGE_MULTIPLE_PAGES > numberOfRows)
                {
                    endRow = numberOfRows;
                }
                else
                {
                    endRow = MAX_ROWS_PER_PAGE_MULTIPLE_PAGES;
                }

                for (int i = 1; i <= numberOfTables; i++)
                {

                    PdfPTable auxTable = new PdfPTable(tableWidths);
                    makeAuxTable(auxTable, startRow, endRow);
                    tables.Add(auxTable);

                    startRow = endRow;

                    //TABLA FINAL 
                    if (numberOfTables == i)
                    {
                        endRow = endRow + MAX_ROWS_PER_PAGE;
                    }

                    else
                    {
                        if (endRow + MAX_ROWS_PER_PAGE_MULTIPLE_PAGES < numberOfRows)
                        {
                            endRow = endRow + MAX_ROWS_PER_PAGE_MULTIPLE_PAGES;
                        }
                        else
                        {
                            endRow = numberOfRows;
                        }
                    }

                }

            }
            else
            {
                PdfPTable auxTable = new PdfPTable(tableWidths);
                makeAuxTable(auxTable, startRow, endRow);
                tables.Add(auxTable);
            }




            return tables;
        }


        private void makeAuxTable(PdfPTable auxPdfTable, int startRow, int endRow)
        {
            List<PdfPRow> rows = mainTable.GetRows(startRow, endRow);
            foreach (PdfPRow r in rows)
            {
                PdfPCell[] cells = r.GetCells();
                foreach (PdfPCell ce in cells)
                {
                    auxPdfTable.AddCell(ce);
                }
            }
        }
    }
}
