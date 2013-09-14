using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Facturacion
{
    public class TipoDeIngreso
    {
        public static int INGRESOS_PROPIOS_NO_GRAVADOS = 1;
        public static int INGRESOS_A_TERCEROS = 2;
        public static int INGRESOS_PROPIOS = 3;

        [Key]
        public int TipoDeIngresoID { get; set; }
        public String TipoDeIngresoNm { get; set; }
    }
}