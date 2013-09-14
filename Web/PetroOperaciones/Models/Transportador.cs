using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class Transportador
    {
        public int TransportadorID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Nombre")]
        public String TransportadorNm { get; set; }
        
        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        [Required(ErrorMessage = "*")]
        public String NIT { get; set; }

        [StringLength(150, ErrorMessage = "Longitud Máxima 150")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Dirección")]
        public String Direccion { get; set; }

        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Teléfono")]
        public String Telefono { get; set; }

        [StringLength(50, ErrorMessage = "Longitud Máxima 50")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Nombre Contacto Principal")]
        public String Contacto1Nm { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Cargo")]
        public String Contacto1Cargo { get; set; }


        [Display(Name = "Cargo")]
        public String Contacto2Cargo { get; set; }

        [StringLength(50, ErrorMessage = "Longitud Máxima 50")]
        [Display(Name = "Correo Contacto Principal")]
        [DataType(DataType.EmailAddress)]
        public String Contacto1Email { get; set; }

        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        [Display(Name = "Teléfono Contacto Principal")]
        public String Contacto1Telefono { get; set; }


        [StringLength(50, ErrorMessage = "Longitud Máxima 50")]
        [Display(Name = "Nombre Contacto Secundario")]
        public String Contacto2Nm { get; set; }

        [StringLength(50, ErrorMessage = "Longitud Máxima 50")]
        [Display(Name = "Correo Contacto Secundario")]
        [DataType(DataType.EmailAddress)]
        public String Contacto2Email { get; set; }

        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        [Display(Name = "Teléfono Contacto Secundario")]
        public String Contacto2Telefono { get; set; }

        public int TipoTransportadorID { get; set; }
        public virtual TipoTransportador TipoTransportador { get; set; }

    }
}