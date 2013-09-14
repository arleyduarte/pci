using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class Contacto
    {
        public int ContactoID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Nombre")]
        public String ContactoNm { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Teléfono")]
        public String Telefono { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(14)]
        public String Celular { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Fecha Cumpleaños")]
        [DataType(DataType.Date)]
        public DateTime FechaCumpleanos { get; set; }

        [Required(ErrorMessage = "*")]
        public String Cargo { get; set; }
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required(ErrorMessage = "*")]
        public int TerceroID { get; set; }

        public virtual Cliente Cliente { get; set; }

    }
}