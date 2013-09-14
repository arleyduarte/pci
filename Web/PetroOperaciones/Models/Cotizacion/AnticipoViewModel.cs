using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetroOperaciones.Models.Facturacion;

namespace PetroOperaciones.Models.Cotizacion
{
    public class AnticipoViewModel
    {

        public IEnumerable<AnticipoDetalle> Detalles { get; set; }
        public AnticipoEncabezado AnticipoEncabezado { get; set; }

    }
}