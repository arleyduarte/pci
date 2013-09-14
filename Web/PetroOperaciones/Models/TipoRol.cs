using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class TipoRol
    {


        public static string CLIENTE = "Cliente";

        [Key]
        public String Rol { get; set; }
        public String Descripcion { get; set; }
    }
}