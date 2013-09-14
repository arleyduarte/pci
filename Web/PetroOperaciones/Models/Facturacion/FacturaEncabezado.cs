using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Collections;

namespace PetroOperaciones.Models.Facturacion
{
    public class FacturaEncabezado
    {




        public int FacturaEncabezadoID { get; set; }
        [Required(ErrorMessage = "*")]
        public int DocumentoOperacionesID { get; set; }
        public int ClienteID { get; set; }
        public int UsuarioCreadorID { get; set; }
        public int PrefacturaEncabezadoID { get; set; }

        [RegularExpression(@"^(\d{1,3}.(\d{3}.)*\d{3}(\,\d{1,3})?|\d{1,3}(\,\d{1,5})?)$", ErrorMessage = "El Valor debe ser un número formato 1.999,00")]
        [Required(ErrorMessage = "*")]
        public String TasaDeCambio { get; set; }

        [StringLength(99, ErrorMessage = "Longitud Máxima 100")]
        public String Tarifa { get; set; }

        [StringLength(1000, ErrorMessage = "Longitud Máxima 1000")]
        public String Anotaciones { get; set; }
        [StringLength(1000, ErrorMessage = "Longitud Máxima 1000")]
        public String Referencias1 { get; set; }
        [StringLength(1000, ErrorMessage = "Longitud Máxima 1000")]        
        public String Referencias2 { get; set; }
        public int Estado { get; set; }
        public int NoFacturaSoporte { get; set; }
        [RegularExpression(@"^(\d{1,3}.(\d{3}.)*\d{3}(\,\d{1,3})?|\d{1,3}(\,\d{1,5})?)$", ErrorMessage = "El Valor debe ser un número formato 1.999,00")]
        public String TotalAnticipos { get; set; }
        //public String TotalFacturaCOP { get; set; }
        //public String TotalFacturaUSD { get; set; }
        public DateTime FechaCreacion { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "*")]
        public DateTime FechaFactura { get; set; }

        public int PlazoFacturaID { get; set; }
        public String FechaVencimiento { get; set; }


        public String AtencionSr { get; set; }

        public virtual PlazoFactura PlazoFactura { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual EstadoFactura EstadoFactura { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual DocumentoOperaciones DocumentoOperaciones { get; set; }

        private ArrayList vFacturaDetalles;
        public void setFacturaDetalles(ArrayList vFacturaDetalles)
        {
            this.vFacturaDetalles = vFacturaDetalles;
        }
        public decimal getSubTotal(String tipoMoneda)
        {
            FacturaBLL facturaBLL = new FacturaBLL();
            return facturaBLL.getSubTotal(this.FacturaEncabezadoID, tipoMoneda, vFacturaDetalles);
        }

        public decimal getTotalIVA(String tipoMoneda)
        {
            FacturaBLL facturaBLL = new FacturaBLL();
            return facturaBLL.getIVACalculado(this.FacturaEncabezadoID, vFacturaDetalles, tipoMoneda);
        }

        public decimal getTotalFactura(String tipoMoneda)
        {
            return getSubTotal(tipoMoneda) + getTotalIVA(tipoMoneda);
        }

        public decimal getTotalAnticipos(String tipoMoneda)
        {

            if (this.TotalAnticipos != null)
            {
                if (tipoMoneda == FacturaBLL.DOLARES_AMERICANOS)
                {
                    return Decimal.Parse(this.TotalAnticipos, new CultureInfo("es-ES")) / Decimal.Parse(this.TasaDeCambio, new CultureInfo("es-ES"));
                }
                else
                {
                    return Decimal.Parse(this.TotalAnticipos, new CultureInfo("es-ES"));
                }
            }
            else
            {
                return 0;
            }


            
        }

        public decimal getValorNeto(String tipoMoneda)
        {
            decimal totalAnticipos = 0;

            totalAnticipos = getTotalAnticipos(tipoMoneda);
 
            return getTotalFactura(tipoMoneda) - totalAnticipos;
        }


        public decimal getSaldoAFavor(String tipoMoneda)
        {
            decimal salgoAFavor = 0;
            decimal valorNeto = getValorNeto(tipoMoneda);

            if (valorNeto < 0)
            {
                salgoAFavor = valorNeto*-1;
            }

            return salgoAFavor;
        }


        public decimal getSaldoACargo(String tipoMoneda)
        {
            decimal salgoACargo = 0;
            decimal valorNeto = getValorNeto(tipoMoneda);
           
            if ( valorNeto> 0)
            {
                salgoACargo = valorNeto;
            }

            return salgoACargo;
        }



        public decimal getBaseIVA(String tipoMoneda)
        {
            FacturaBLL facturaBLL = new FacturaBLL();
            return facturaBLL.getBaseIVA(this.FacturaEncabezadoID, vFacturaDetalles, tipoMoneda);
        }

        private bool esPrefactura = false;

        public void convertirEnPrefactura()
        {
            esPrefactura = true;
        }

        public String getTituloDocumento()
        {
            String titulo = "FACTURA DE VENTA";
            if (esPrefactura)
            {
                titulo = "PREFACTURA";
            }

            return titulo;
        }
    }
}