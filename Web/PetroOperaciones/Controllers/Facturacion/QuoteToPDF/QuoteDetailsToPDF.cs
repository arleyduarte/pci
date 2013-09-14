using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text;
using PetroOperaciones.Models.Facturacion;
using PetroOperaciones.Models.Cotizacion;

namespace invoiceToPDF
{
    public class QuoteDetailsToPDF
    {
        private String currencyType;
        private BaseFont bfArial = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        private Font fTitleEntry;

        public void fillMainTable(ArrayList invoiceDetails, PdfPTable mainTable, String currencyType)
        {
            this.currencyType = currencyType;
            fTitleEntry = new Font(bfArial, 7.5f, Font.BOLD, BaseColor.BLACK);

            ArrayList vEntrances = new ArrayList();
            PdfPCell cellEntranceCode = new PdfPCell();
            Chunk cktTitle;
            Paragraph paragraphTitle;
            PdfPCell cellEntranceTitle;
            PdfPCell cellEntranceValueCOP;
            PdfPCell cellEntranceValueUSD;

            //COSTOS_EN_ORIGEN_O_FLETES ******************************************
            vEntrances = getEntrancesByType(invoiceDetails, TipoConceptoCotizacion.GASTOS_POR_FLETE_M_A_INTERNACIONAL);
            if (vEntrances.Count > 0)
            {

                cellEntranceCode.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceCode);

                cktTitle = new Chunk("GASTOS POR FLETE (MARITIMO/AEREO) INTERNACIONAL");
                cktTitle.Font = fTitleEntry;
                paragraphTitle = new Paragraph(cktTitle);

                cellEntranceTitle = new PdfPCell(paragraphTitle);
                cellEntranceTitle.HorizontalAlignment = Element.ALIGN_LEFT;
                cellEntranceTitle.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceTitle);

                cellEntranceValueCOP = new PdfPCell();
                cellEntranceValueCOP.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceValueCOP);

                cellEntranceValueUSD = new PdfPCell();
                cellEntranceValueUSD.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceValueUSD);

                foreach (CotizacionDetalle invoiceDatail in vEntrances)
                {
                    drawInvoiceDetail(invoiceDatail, mainTable);
                }
            }
            //COSTOS_EN_ORIGEN_O_FLETES **************************************************



            //GASTOS_EN_ORIGEN *******************************************************
            vEntrances = getEntrancesByType(invoiceDetails, TipoConceptoCotizacion.GASTOS_EN_ORIGEN);
            if (vEntrances.Count > 0)
            {
                cellEntranceCode = new PdfPCell();
                cellEntranceCode.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceCode);

                cktTitle = new Chunk("GASTOS EN ORIGEN");
                cktTitle.Font = fTitleEntry;
                paragraphTitle = new Paragraph(cktTitle);
                cellEntranceTitle = new PdfPCell(paragraphTitle);
                cellEntranceTitle.HorizontalAlignment = Element.ALIGN_LEFT;
                cellEntranceTitle.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceTitle);

                cellEntranceValueCOP = new PdfPCell();
                cellEntranceValueCOP.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceValueCOP);

                cellEntranceValueUSD = new PdfPCell();
                cellEntranceValueUSD.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceValueUSD);

                foreach (CotizacionDetalle invoiceDatail in vEntrances)
                {
                    drawInvoiceDetail(invoiceDatail, mainTable);
                }
            }
            //END GASTOS_EN_ORIGEN ***************************************************


            //GASTOS EN DESTINO ********************************************************
            vEntrances = getEntrancesByType(invoiceDetails, TipoConceptoCotizacion.GASTOS_EN_DESTINO);
            if (vEntrances.Count > 0)
            {
                cellEntranceCode = new PdfPCell();
                cellEntranceCode.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceCode);

                cktTitle = new Chunk("GASTOS EN DESTINO");
                cktTitle.Font = fTitleEntry;
                paragraphTitle = new Paragraph(cktTitle);
                cellEntranceTitle = new PdfPCell(paragraphTitle);
                cellEntranceTitle.HorizontalAlignment = Element.ALIGN_LEFT;
                cellEntranceTitle.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceTitle);

                cellEntranceValueCOP = new PdfPCell();
                cellEntranceValueCOP.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceValueCOP);

                cellEntranceValueUSD = new PdfPCell();
                cellEntranceValueUSD.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceValueUSD);


                foreach (CotizacionDetalle invoiceDatail in vEntrances)
                {
                    drawInvoiceDetail(invoiceDatail, mainTable);
                }
            }
            //END GASTOS EN DESTINO *********************************************************


            //AGENCIAMIENTO ADUANEROS ********************************************************
            vEntrances = getEntrancesByType(invoiceDetails, TipoConceptoCotizacion.AGENCIAMIENTO_ADUANEROS);
            if (vEntrances.Count > 0)
            {
                cellEntranceCode = new PdfPCell();
                cellEntranceCode.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceCode);

                cktTitle = new Chunk("AGENCIAMIENTO ADUANEROS");
                cktTitle.Font = fTitleEntry;
                paragraphTitle = new Paragraph(cktTitle);
                cellEntranceTitle = new PdfPCell(paragraphTitle);
                cellEntranceTitle.HorizontalAlignment = Element.ALIGN_LEFT;
                cellEntranceTitle.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceTitle);

                cellEntranceValueCOP = new PdfPCell();
                cellEntranceValueCOP.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceValueCOP);

                cellEntranceValueUSD = new PdfPCell();
                cellEntranceValueUSD.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(cellEntranceValueUSD);


                foreach (CotizacionDetalle invoiceDatail in vEntrances)
                {
                    drawInvoiceDetail(invoiceDatail, mainTable);
                }
            }
            //END AGENCIAMIENTO ADUANEROS *********************************************************

        }

        private ArrayList getEntrancesByType(ArrayList detalles, int type)
        {
            ArrayList l = new ArrayList();


            foreach (CotizacionDetalle invoiceDatail in detalles)
            {

                if (invoiceDatail.TipoConceptoCotizacionID == type)
                {
                    l.Add(invoiceDatail);
                }

            }

            return l;
        }

        private void drawInvoiceDetail(CotizacionDetalle invoiceDetail, PdfPTable table)
        {
            BaseColor borderColor = new BaseColor(0, 0, 0);

            BaseFont bfArial = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            Font times = new Font(bfArial, 7.5f, Font.NORMAL, BaseColor.BLACK);
            Chunk ck = new Chunk(""+invoiceDetail.ConceptoID);
            ck.Font = times;
            Paragraph P = new Paragraph(ck);
            PdfPCell cellCode = new PdfPCell(P);
            cellCode.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCode.Border = Rectangle.NO_BORDER;
            cellCode.BorderColor = borderColor;
            table.AddCell(cellCode);


            Chunk ckConcept = new Chunk(invoiceDetail.ConceptoNm +" "+invoiceDetail.Detalles);
            ckConcept.Font = times;
            Paragraph pConcept = new Paragraph(ckConcept);
            PdfPCell cellConcept = new PdfPCell(pConcept);
            cellConcept.HorizontalAlignment = Element.ALIGN_LEFT;
            cellConcept.Border = Rectangle.NO_BORDER;
            cellConcept.BorderColor = borderColor;
            table.AddCell(cellConcept);

            String valorUSD = "";
            if (currencyType == "USD" || currencyType == "COPUSD")
            {
                valorUSD = invoiceDetail.ValorUSD;
            }

            Chunk ckUSD = new Chunk(valorUSD);
            ckUSD.Font = times;
            Paragraph pUSD = new Paragraph(ckUSD);
            PdfPCell cellUSD = new PdfPCell(pUSD);
            cellUSD.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellUSD.Border = Rectangle.NO_BORDER;
            cellUSD.BorderColor = borderColor;
            table.AddCell(cellUSD);

            String valorCOP = "";
            if (currencyType == "COP" || currencyType == "COPUSD")
            {
                valorCOP = invoiceDetail.ValorCOP;
            }

            Chunk ckCOP = new Chunk(valorCOP);
            ckCOP.Font = times;
            Paragraph pCOP = new Paragraph(ckCOP);
            PdfPCell cellCOP = new PdfPCell(pCOP);
            cellCOP.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellCOP.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellCOP.Border = Rectangle.NO_BORDER;
            cellCOP.BorderColor = borderColor;
            table.AddCell(cellCOP);


        }
    }
}
