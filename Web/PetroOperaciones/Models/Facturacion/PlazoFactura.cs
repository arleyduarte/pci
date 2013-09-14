using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class PlazoFactura
    {

        public static int CERO = 0;
        public static int TREINTA_DIAS = 2;

        [Key]
        public int PlazoFacturaID { get; set; }
        public String PlazoFacturaNm { get; set; }
    }
}