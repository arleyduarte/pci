using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Cotizacion
{
    public class AnticipoEncabezado
    {
        public int AnticipoEncabezadoID { get; set; }

        [Required(ErrorMessage = "*")]
        public int DocumentoOperacionesID { get; set; }

        [RegularExpression(@"^(\d{1,3}.(\d{3}.)*\d{3}(\,\d{1,3})?|\d{1,3}(\,\d{1,5})?)$", ErrorMessage = "El Valor debe ser un número formato 1.999,00")]
        [Required(ErrorMessage = "*")]
        public String TasaDeCambio { get; set; }
        public String Anotaciones { get; set; }
        public int Estado { get; set; }
        public int UsuarioCreadorID { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String AtencionSr { get; set; }
        [Required(ErrorMessage = "*")]
        public int ClienteID { get; set; }




        [Required(ErrorMessage = "*")]
        public int MedioTransporteID { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "*")]
        public DateTime FechaAnticipo { get; set; }


        public virtual Usuario Usuario { get; set; }
        public virtual DocumentoOperaciones DocumentoOperaciones { get; set; }
        public virtual Cliente Cliente { get; set; }


    }
}