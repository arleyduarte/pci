using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetroOperaciones.Models.Facturacion;

namespace PetroOperaciones.Models.Facturacion
{
    public class FacturaViewModel
    {

        public IEnumerable<FacturaDetalle> Detalles { get; set; }
        public FacturaEncabezado FacturaEncabezado { get; set; }

    }
}