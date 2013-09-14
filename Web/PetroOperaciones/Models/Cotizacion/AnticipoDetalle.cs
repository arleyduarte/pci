using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Cotizacion
{
    public class AnticipoDetalle
    {
        public int AnticipoDetalleID { get; set; }
        public int AnticipoEncabezadoID { get; set; }
        public int ConceptoID { get; set; }
        public String ConceptoNm { get; set; }

        [StringLength(490, ErrorMessage = "Longitud Máxima 490")]
        public String Detalles { get; set; }
        public String ValorUSD { get; set; }
        public String ValorCOP { get; set; }


        [Required(ErrorMessage = "*")]
        public int TipoConceptoCotizacionID { get; set; }


        public virtual AnticipoEncabezado AnticipoEncabezado { get; set; }
        public virtual TipoConceptoCotizacion TipoConceptoCotizacion { get; set; }
    }
}