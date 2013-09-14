using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class Cliente
    {
        [Key]
        public String NIT { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Nombre")]
        public String NmCliente { get; set; }


        [Required(ErrorMessage = "La razón social  es obligatoria")]
        [MaxLength(300)]
        [Display(Name = "Razón Social")]
        public String RazonSocial { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [MaxLength(40)]
        [Display(Name = "Dirección")]
        public String Direccion { get; set; }

        [Required(ErrorMessage = "La teléfono es obligatorio")]
        [MaxLength(15)]
        [Display(Name = "Teléfono 1")]
        public String Telefono1 { get; set; }

        [Display(Name = "Teléfono 2")]
        public String Telefono2 { get; set; }

        [Display(Name = "Fecha Cumpleaños")]
        public DateTime FechaCumpleanos { get; set; }

        
        public virtual ICollection<Pais> Paises { get; set; }

    }
}