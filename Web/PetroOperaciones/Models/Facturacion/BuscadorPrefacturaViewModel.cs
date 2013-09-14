using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetroOperaciones.Models.Facturacion
{
    public class BuscadorPrefacturaViewModel
    {
        public FiltroReporte Filtro { get; set; }
        public IEnumerable<PrefacturaEncabezado> Prefacturas { get; set; }
    }
}