using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PetroOperaciones.Controllers.Attributes;

namespace PetroOperaciones.Models
{
    public class RangoFechas
    {
        
        [Required(ErrorMessage = "*")]
        [DateAttributeValidation(ErrorMessage = "You can't add someone who hasn't been born yet!")]
        public DateTime FechaInicial { get; set; }
        [Required(ErrorMessage = "*")]
        [DateAttributeValidation(ErrorMessage = "You can't add someone who hasn't been born yet!")]
        public DateTime FechaFinal { get; set; }
    }
}