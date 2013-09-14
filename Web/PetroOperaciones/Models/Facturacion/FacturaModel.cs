using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PetroOperaciones.Models.Facturacion;

namespace invoiceToPDF
{
    public class FacturaModel
    {

        public ArrayList Detalles { get; set; }
        public FacturaEncabezado FacturaEncabezado { get; set; }
    }
}
