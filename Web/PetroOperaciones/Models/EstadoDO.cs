using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class EstadoDO
    {

        public static int FINALIZADO = 1;
        public static int EN_SEGUIMIENTO = 2;

        [Key]
        public int EstadoID { get; set; }
        public String EstadoNm { get; set; }
    }
}