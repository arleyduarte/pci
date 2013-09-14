using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetroOperaciones.Models.Cotizacion
{
    public class BuscadorCotizacionViewModel
    {
        public FiltroReporte Filtro { get; set; }
        public IEnumerable<CotizacionEncabezado> Cotizaciones { get; set; }
    }
}