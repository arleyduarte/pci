using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Facturacion
{
    public class Concepto
    {
        public int ConceptoID { get; set; }
        [Display(Name = "Concepto")]
        public String ConceptoNm { get; set; }

    }
}