using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }


        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "NIT")]
        public String NIT { get; set; }


        [StringLength(150, ErrorMessage = "Longitud Máxima 150")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Nombre")]
        public String NmCliente { get; set; }



        [StringLength(50, ErrorMessage = "Longitud Máxima 50")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Razón Social")]
        public String RazonSocial { get; set; }

        [StringLength(400, ErrorMessage = "Longitud Máxima 400")]
        public String ObjetoSocial { get; set; }
        

        [Required(ErrorMessage = "*")]
        [Display(Name = "País")]
        public int PaisID { get; set; }

        [StringLength(150, ErrorMessage = "Longitud Máxima 150")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Dirección")]
        public String Direccion { get; set; }


        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        public String CiudadNm { get; set; }

        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Teléfono")]
        public String Telefono1 { get; set; }

        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        [Display(Name = "Teléfono")]
        public String Telefono2 { get; set; }
        
        [Required(ErrorMessage = "*")]
        [Display(Name = "Fecha Cumpleaños")]
        [DataType(DataType.Date)]
        public DateTime FechaCumpleanos { get; set; }
        public int Estado { get; set; }





        public virtual Pais Pais { get; set; }

    }
}