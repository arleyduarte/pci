using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class FiltroReporte
    {
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime FechaInicial { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime FechaFinal { get; set; }
        public int ClienteID { get; set; }
    }
}