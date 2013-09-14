using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PetroOperaciones.Models.Basic;

namespace PetroOperaciones.Models.Cotizacion
{
    public class CotizacionEncabezado : IMasterEntity
    {
        public int CotizacionEncabezadoID { get; set; }
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
        public String ClienteNm { get; set; }
        public String PesoAprox { get; set; }
        public String Producto { get; set; }

        [Required(ErrorMessage = "*")]
        public int PuertoOrigen { get; set; }

        [Required(ErrorMessage = "*")]
        public int PuertoDestino { get; set; }
        [Required(ErrorMessage = "*")]
        public String FechaVigencia { get; set; }



        [DataType(DataType.Date)]
        [Required(ErrorMessage = "*")]
        public DateTime FechaCotizacion { get; set; }


        public String Direccion { get; set; }
        public String Telefono { get; set; }
        public String Email { get; set; }
        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        public String TTTransito { get; set; }

        public String Servicio { get; set; }
        public String Dimensiones { get; set; }
        public String Piezas { get; set; }
        public String KGBR { get; set; }
        public String KGAF { get; set; }
        public String Volumen { get; set; }

        [Required(ErrorMessage = "*")]
        public int AsesorComercialID { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario UsuarioAsesor { get; set; }
        public virtual DocumentoOperaciones DocumentoOperaciones { get; set; }
        public virtual Puerto PuertoO { get; set; }
        public virtual Puerto PuertoD { get; set; }

    }
}