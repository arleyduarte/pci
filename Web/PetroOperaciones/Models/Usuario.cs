using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class Usuario
    {

        public int UsuarioID { get; set; }
        public static bool ACTIVO = true;
        public static bool INACTIVO = false;

        [StringLength(20, ErrorMessage = "Longitud Máxima 10")]
        [Required(ErrorMessage = "*")]
        public String Cedula { get; set; }

        [StringLength(50, ErrorMessage = "Longitud Máxima 40")]
        [Required(ErrorMessage = "*")]
        public String Nombre { get; set; }


        public String Rol{ get; set; }

        [StringLength(10, ErrorMessage = "Longitud Máxima 10")]
        [Required(ErrorMessage = "*")]
        public String Telefono { get; set; }

        [StringLength(10, ErrorMessage = "Longitud Máxima 10")]
        [Required(ErrorMessage = "*")]
        public String Celular { get; set; }

        [StringLength(49, ErrorMessage = "Longitud Máxima 49")]
        [Required(ErrorMessage = "*")]
        public String Email { get; set; }

        [StringLength(40, ErrorMessage = "Longitud Máxima 40")]
        [Required(ErrorMessage = "*")]
        public String NombreUsuario { get; set; }


        [StringLength(50, ErrorMessage = "Longitud Máxima 50")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]  
        public String Clave { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }


        public virtual TipoRol TipoRol { get; set; }

    }
}