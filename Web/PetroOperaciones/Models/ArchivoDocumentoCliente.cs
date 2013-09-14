using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class ArchivoDocumentoCliente
    {
        public int ArchivoDocumentoClienteID { get; set; }
        public int ClienteID { get; set; }
        public int UsuarioID { get; set; }
        public int TipoDocumentoTerceroID { get; set; }
        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        [Display(Name = "No Documento")]
        [Required(ErrorMessage = "*")]
        public String NoDocumento { get; set; }
        [Required(ErrorMessage = "*")]
        public String NombreArchivo { get; set; }
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime FechaVencimiento { get; set; }
        

        public virtual Usuario Usuario { get; set; }
        public virtual TipoDocumentoTercero TipoDocumentoTercero { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}