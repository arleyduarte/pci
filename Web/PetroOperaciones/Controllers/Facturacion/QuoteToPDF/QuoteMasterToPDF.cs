using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using System.Globalization;
using PetroOperaciones.Models.Facturacion;
using PetroOperaciones.Models.Cotizacion;

namespace invoiceToPDF
{
    public class QuoteMasterToPDF
    {
        private static String DOLAR_SIGN = "$ ";

        public void fillInvoiceMaster(CotizacionEncabezado invoiceHeader, PdfStamper stamper)
        {
            AcroFields af = stamper.AcroFields;
            af.SetField("NoFacturaSoporte", invoiceHeader.CotizacionEncabezadoID.ToString());
            af.SetField("TasaDeCambio", invoiceHeader.TasaDeCambio);
            af.SetField("NmCliente", invoiceHeader.ClienteNm);
            af.SetField("AtencionSr", invoiceHeader.AtencionSr);
            af.SetField("ClienteDireccion", invoiceHeader.Direccion);
            af.SetField("ClienteTelefono", invoiceHeader.Telefono);
            af.SetField("Dimensiones", invoiceHeader.Dimensiones);
            af.SetField("Email", invoiceHeader.Email);
            af.SetField("KGAF", invoiceHeader.KGAF);
            af.SetField("PesoAprox", invoiceHeader.PesoAprox);

            af.SetField("PuertoDestino", invoiceHeader.PuertoD.PuertoNm);
            af.SetField("PuertoOrigen", invoiceHeader.PuertoO.PuertoNm);
            af.SetField("TTTransito", invoiceHeader.TTTransito);
            af.SetField("UsuarioAsesor", invoiceHeader.UsuarioAsesor.Nombre);
            af.SetField("Volumen", invoiceHeader.Volumen);
            af.SetField("FechaVigencia", invoiceHeader.FechaVigencia);
            af.SetField("FechaCotizacion", invoiceHeader.FechaCotizacion.ToString("dd.MM.yyyy"));
            af.SetField("KGBR", invoiceHeader.KGBR);
            af.SetField("Piezas", invoiceHeader.Piezas);
            af.SetField("Producto", invoiceHeader.Producto);
            af.SetField("Servicio", invoiceHeader.Servicio);
            af.SetField("Anotaciones", invoiceHeader.Anotaciones);
            af.SetField("FechaCreacion", invoiceHeader.FechaCreacion.ToShortDateString());


            stamper.FormFlattening = true;

   
        }

        public void fillTotalsMaster(CotizacionEncabezado header, PdfStamper stamper, String CurrencyType)
        {

            AcroFields af = stamper.AcroFields;
            CotizacionBLL cotizacionBLL = new CotizacionBLL();
            SubTotalesCotizacion subTotalesCotizacion = cotizacionBLL.getSubTotalesCotizacion(header.CotizacionEncabezadoID, CurrencyType);

            af.SetField("FleteMaritimo", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(subTotalesCotizacion.FleteMaritimo));
            af.SetField("GatosOrigen", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(subTotalesCotizacion.GatosOrigen));
            af.SetField("GatosDestino", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(subTotalesCotizacion.GatosDestino));
            af.SetField("AgenciamientoAduanero", DOLAR_SIGN + CultureUtils.getDecimalWithOutCents(subTotalesCotizacion.AgenciamientoAduanero));
            af.SetField("ValorNeto", DOLAR_SIGN  +CultureUtils.getDecimalWithOutCents(subTotalesCotizacion.getValorNeto()));
            stamper.FormFlattening = true;
        }

    }
}
