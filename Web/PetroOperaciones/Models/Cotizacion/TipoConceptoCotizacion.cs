using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Cotizacion
{
    public class TipoConceptoCotizacion
    {

        public static int GASTOS_POR_FLETE_M_A_INTERNACIONAL = 1;
        public static int GASTOS_EN_ORIGEN = 2;
        public static int GASTOS_EN_DESTINO = 3;
        public static int AGENCIAMIENTO_ADUANEROS = 4;

        [Key]
        public int TipoConceptoCotizacionID { get; set; }
        public String TipoConceptoCotizacionNm { get; set; }

    }
}