using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetroOperaciones.Models.Cotizacion
{
    public class SubTotalesCotizacion
    {
        public decimal FleteMaritimo { get; set; }
        public decimal GatosOrigen { get; set; }
        public decimal GatosDestino { get; set; }
        public decimal AgenciamientoAduanero { get; set; }

        public decimal getValorNeto()
        {
            return FleteMaritimo + GatosOrigen + GatosDestino + AgenciamientoAduanero;
        }
    }
}