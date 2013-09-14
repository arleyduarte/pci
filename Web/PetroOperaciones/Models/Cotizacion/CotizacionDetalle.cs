using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Cotizacion
{
    public class CotizacionDetalle
    {
        public int CotizacionDetalleID { get; set; }
        public int CotizacionEncabezadoID { get; set; }
        public int ConceptoID { get; set; }
        public String ConceptoNm { get; set; }

        [StringLength(490, ErrorMessage = "Longitud Máxima 490")]
        public String Detalles { get; set; }
        public String ValorUSD { get; set; }
        public String ValorCOP { get; set; }
        public String Unidades { get; set; }

        [Required(ErrorMessage = "*")]
        public int TipoConceptoCotizacionID { get; set; }


        public virtual CotizacionEncabezado CotizacionEncabezado { get; set; }
        public virtual TipoConceptoCotizacion TipoConceptoCotizacion { get; set; }
    }
}