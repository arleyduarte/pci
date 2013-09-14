using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class ArchivoDocumento
    {
        public int ArchivoDocumentoID { get; set; }
        public int DocumentoOperacionesID { get; set; }
        public int UsuarioID { get; set; }
        public int TipoDocumentoID { get; set; }

        [StringLength(35, ErrorMessage = "Longitud Máxima 30")]
        [Display(Name = "No Documento")]
        [Required(ErrorMessage = "*")]
        public String NoDocumento { get; set; }

        [Required(ErrorMessage = "*")]
        public String NombreArchivo { get; set; }
        public DateTime FechaRegistro { get; set; }

        public bool VisibleCliente { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual DocumentoOperaciones DocumentoOperaciones { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
    }
}