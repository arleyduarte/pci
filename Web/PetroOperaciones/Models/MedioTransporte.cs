using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetroOperaciones.Models
{
    public class MedioTransporte
    {
        public static int AEREO = 1;
        public static int MARITIMO = 2; 

        public int MedioTransporteID { get; set; }
        public String MedioTransporteNm { get; set; }
    }
}