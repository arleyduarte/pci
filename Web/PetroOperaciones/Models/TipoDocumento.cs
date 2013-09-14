using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class TipoDocumento
    {
        public int TipoDocumentoID { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "Longitud Máxima 100")]
        public String TipoDocumentoNm { get; set; }
    }
}