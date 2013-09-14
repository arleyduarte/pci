using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class TipoMoneda
    {
        [Key]
        public String TipoMonedaID { get; set; }
        public String TipoMonedaNm { get; set; }
    }
}