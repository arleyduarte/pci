using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetroOperaciones.Models.Facturacion;

namespace PetroOperaciones.Models.Facturacion
{
    public class PrefacturaViewModel
    {

        public IEnumerable<PrefacturaDetalle> Detalles { get; set; }
        public PrefacturaEncabezado PrefacturaEncabezado { get; set; }

    }
}