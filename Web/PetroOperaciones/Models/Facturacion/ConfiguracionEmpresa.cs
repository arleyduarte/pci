using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Facturacion
{
    public class ConfiguracionEmpresa
    {
        [Key]
        public int ConfiguracionEmpresaID { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(14, ErrorMessage = "Longitud Máxima 14")]
        public String NIT { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(1, ErrorMessage = "Longitud Máxima 1")]
        public String DV { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Longitud Máxima 50")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "*")]
        public String Prefijo { get; set; }

        [Required(ErrorMessage = "*")]
        public String RazonSocial { get; set; }

        [Required(ErrorMessage = "*")]
        public String DireccionFiscal { get; set; }

        [Required(ErrorMessage = "*")]
        public String Telefono { get; set; }

        [Required(ErrorMessage = "*")]
        public String Email { get; set; }

        [Required(ErrorMessage = "*")]
        public String Resolucion { get; set; }

        [Required(ErrorMessage = "*")]
        public int NoInicial { get; set; }

        [Required(ErrorMessage = "*")]
        public int NoIFinal { get; set; }

        [Required(ErrorMessage = "*")]
        public int UltimaFactura { get; set; }
        public String Logo { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime FechaResolucion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String Regimen { get; set; }
        



    }
}