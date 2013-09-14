using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class Alertas
    {
        [Key]
        public int AlertaID { get; set; }
        public String AlertaNm { get; set; }
        public String EmailTo { get; set; }
        public String EmailCC { get; set; }
        public String EmailBCC { get; set; }
        public String Asunto { get; set; }
        public int Estado { get; set; }
        public DateTime FechaCreacion { get; set; }	

    }
}