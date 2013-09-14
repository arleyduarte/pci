using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetroOperaciones.Models
{
    public class TipoTransportador
    {
        public static int FREIGHT_FORWARDER = 5;
        public static int MULTIMODAL = 4;
        public static int TERRESTRE = 3;
        public static int MARITIMO = 2; 
        public static int AERERO = 1; 

        public int TipoTransportadorID { get; set; }
        public String TipoTransportadorNm { get; set; }

    }
}