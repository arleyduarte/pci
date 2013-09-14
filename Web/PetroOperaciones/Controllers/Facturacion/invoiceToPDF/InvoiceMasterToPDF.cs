using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using System.Globalization;
using PetroOperaciones.Models.Facturacion;

namespace invoiceToPDF
{
    public class InvoiceMasterToPDF
    {
        private static String DOLAR_SIGN = "$ ";

        public void fillInvoiceMaster(FacturaEncabezado invoiceHeader, PdfStamper stamper)
        {
            AcroFields af = stamper.AcroFields;
            af.SetField("TituloDocumento", invoiceHeader.getTituloDocumento());
            af.SetField("NoDocumentoOperaciones", invoiceHeader.DocumentoOperaciones.NumeroDO);
            af.SetField("NoFacturaSoporte", invoiceHeader.NoFacturaSoporte.ToString());
            af.SetField("FechaFactura", invoiceHeader.FechaFactura.ToString("dd/MM/yyyy"));
            af.SetField("TasaDeCambio", invoiceHeader.TasaDeCambio);
            af.SetField("PlazoFacturaNm", invoiceHeader.PlazoFactura.PlazoFacturaNm);

            if (invoiceHeader.FechaVencimiento != null && invoiceHeader.FechaVencimiento.Length > 0)
            {
              
                af.SetField("FechaVencimiento", invoiceHeader.FechaFactura.ToString("dd/MM/yyyy"));
            }
            else
            {
                af.SetField("FechaVencimiento", "");
            }
            
          
            af.SetField("NmCliente", invoiceHeader.Cliente.NmCliente);
            af.SetField("AtencionSr", invoiceHeader.AtencionSr);
            af.SetField("ClienteDireccion", invoiceHeader.Cliente.Direccion);
            af.SetField("ClienteTelefono", invoiceHeader.Cliente.Telefono1);
            af.SetField("ClienteCiudadNm", invoiceHeader.Cliente.CiudadNm);
            af.SetField("ClienteNIT", invoiceHeader.Cliente.NIT);
            af.SetField("Referencias1", invoiceHeader.Referencias1);
            af.SetField("Referencias2", invoiceHeader.Referencias2);
            af.SetField("DocumentosTransporte", invoiceHeader.DocumentoOperaciones.getDocumentosTransporte());
            af.SetField("ResumenPiezas", invoiceHeader.DocumentoOperaciones.getResumenPiezas());
            af.SetField("PesoBruto", invoiceHeader.DocumentoOperaciones.PesoBruto);

            ConfiguracionEmpresaBLL configBLL = new ConfiguracionEmpresaBLL();
            ConfiguracionEmpresa config = configBLL.getConfiguracionEmpresa(ConfiguracionEmpresaBLL.PRETROCARGO_CONFIGURACION);
            af.SetField("DireccionFacturador", config.DireccionFiscal);


            stamper.FormFlattening = true;
        }

        public void fillTotalsInvoiceMaster(FacturaEncabezado invoiceHeader, PdfStamper stamper, String CurrencyType)
        {

            AcroFields af = stamper.AcroFields;
            af.SetField("BaseIVA", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(invoiceHeader.getBaseIVA(CurrencyType)));
            af.SetField("SaldoAFavor", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(invoiceHeader.getSaldoAFavor(CurrencyType)));
            af.SetField("SaldoACargo", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(invoiceHeader.getSaldoACargo(CurrencyType)));
            af.SetField("SubTotal", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(invoiceHeader.getSubTotal(CurrencyType)));
            af.SetField("TotalIVA", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(invoiceHeader.getTotalIVA(CurrencyType)));
            af.SetField("TotalFactura", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(invoiceHeader.getTotalFactura(CurrencyType)));
            af.SetField("TotalAnticipos", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(invoiceHeader.getTotalAnticipos(CurrencyType)));

            decimal valorNeto = invoiceHeader.getValorNeto(CurrencyType);
            String sValorNeto = CultureUtils.getDecimalWithOutCents(valorNeto);
            String valorNetoLetras = "";
            int numero = CultureUtils.getIntegerWithOutMilesSeparator(sValorNeto);
            numero = Math.Abs(numero);
            valorNetoLetras = Numalet.ToCardinal(Convert.ToInt32(numero));

 


            af.SetField("ValorNeto", DOLAR_SIGN + sValorNeto);
            af.SetField("ValorNetoLetras", valorNetoLetras.ToUpper() +" M/CTE");
            
            stamper.FormFlattening = true;
        }

    }
}
