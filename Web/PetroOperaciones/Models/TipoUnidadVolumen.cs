using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class TipoUnidadVolumen
    {
        [Key]
        public String TipoUnidadVolumenID { get; set; }
        public String TipoUnidadVolumenNm { get; set; }
    }
}