using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class Puerto
    {
        public static String AEROPUERTO = "A";
        public static String PUERTO = "P";

        public int PuertoID { get; set; }
        [Display(Name = "Puerto")]
        public String PuertoNm { get; set; }

        public String Tipo { get; set; }
    }
}