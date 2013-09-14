using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace PetroOperaciones.Models.Facturacion
{
    public class TotalesFacturaModel
    {

        public TotalesFacturaModel(int facturaEncabezadoID)
        {
            FacturaEncabezado encabezado = new FacturaEncabezado();
            encabezado.FacturaEncabezadoID = facturaEncabezadoID;

            this.TotalIVA = encabezado.getTotalIVA(FacturaBLL.PESOS_COLOMBIANOS).ToString("N", new CultureInfo("es-ES"));
        }

        public String TotalAnticipos { get; set; }
        public String TotalIVA { get; set; }
    }
}