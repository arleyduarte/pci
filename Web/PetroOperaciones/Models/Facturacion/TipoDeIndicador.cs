using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Facturacion
{
    public class TipoDeIndicador
    {
        public static int VENTAS_TOTALES = 4;
        public static int INGRESOS_PROPIOS_NO_GRAVADOS = 1;
        public static int INGRESOS_A_TERCEROS = 2;
        public static int INGRESOS_PROPIOS = 3;
        public static int EXPENSES = 5;
        [Key]
        public int TipoDeIndicadorID { get; set; }
        public String TipoDeIndicadorNm { get; set; }
    }
}