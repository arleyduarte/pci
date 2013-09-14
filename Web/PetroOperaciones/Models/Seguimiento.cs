using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class Seguimiento
    {
        public int SeguimientoID { get; set; }
        public int DocumentoOperacionesID { get; set; }
        public int UsuarioID { get; set; }

        [StringLength(399, ErrorMessage = "Longitud Máxima 400")]
        [Display(Name = "Observaciones")]
        [Required(ErrorMessage = "*")]
        public String Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }

        public bool EsInformeFinal { get; set; }
        public bool AnotacionFacturacion { get; set; }
        public bool VisibleCliente { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual DocumentoOperaciones DocumentoOperaciones { get; set; }

    }
}