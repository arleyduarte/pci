using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class Pais
    {
        public int PaisID { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "País")]
        public String PaisNm { get; set; }

    }
}