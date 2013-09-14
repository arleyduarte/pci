using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Facturacion
{
    public class PrefacturaEncabezado
    {
        public int PrefacturaEncabezadoID { get; set; }

        [Required(ErrorMessage = "*")]
        public int DocumentoOperacionesID { get; set; }

        [RegularExpression(@"^(\d{1,3}.(\d{3}.)*\d{3}(\,\d{1,3})?|\d{1,3}(\,\d{1,5})?)$", ErrorMessage = "El Valor debe ser un número formato 1.999,00")]
        [Required(ErrorMessage = "*")]
        public String TasaDeCambio { get; set; }
        public int ClienteID { get; set; }
        [StringLength(1900, ErrorMessage = "Longitud Máxima 2000")]
        public String Anotaciones { get; set; }


        [RegularExpression(@"^(\d{1,3}.(\d{3}.)*\d{3}(\,\d{1,3})?|\d{1,3}(\,\d{1,5})?)$", ErrorMessage = "El Valor debe ser un número formato 1.999,00")]
        public String TotalAnticipos { get; set; }
        public int Estado { get; set; }
        public int UsuarioCreadorID { get; set; }
        public DateTime FechaCreacion { get; set; }


        public String NIT { get; set; }
        public String Direccion { get; set; }
        public String NmCliente { get; set; }
        public String Telefono1 { get; set; }
        public String Telefono2 { get; set; }

        public String AtencionSr { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual DocumentoOperaciones DocumentoOperaciones { get; set; }
        public virtual Cliente Cliente { get; set; }

    }
}