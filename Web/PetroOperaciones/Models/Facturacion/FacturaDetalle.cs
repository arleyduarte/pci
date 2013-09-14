using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Facturacion
{
    public class FacturaDetalle
    {
        public int FacturaDetalleID { get; set; }
        [Required(ErrorMessage = "*")]
        public int FacturaEncabezadoID { get; set; }
        public int ConceptoID { get; set; }
        public String ConceptoNm { get; set; }

        [StringLength(490, ErrorMessage = "Longitud Máxima 490")]
        public String Detalles { get; set; }
        public String ValorUSD { get; set; }
        public String ValorCOP { get; set; }
        public String FacturaSoporte { get; set; }

        [Required(ErrorMessage = "*")]
        public int TipoDeIngresoID { get; set; }


        public virtual FacturaEncabezado FacturaEncabezado { get; set; }
        public virtual TipoDeIngreso TipoDeIngreso { get; set; }
    }
}