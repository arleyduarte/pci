using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetroOperaciones.Models.Facturacion;

namespace PetroOperaciones.Models.Cotizacion
{
    public class CotizacionViewModel
    {

        public IEnumerable<CotizacionDetalle> Detalles { get; set; }
        public CotizacionEncabezado CotizacionEncabezado { get; set; }

    }
}