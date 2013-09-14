using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetroOperaciones.Models.Facturacion
{
    public class BuscadorFacturaViewModel
    {
        public FiltroReporte Filtro { get; set; }
        public IEnumerable<FacturaEncabezado> Facturas { get; set; }
    }
}