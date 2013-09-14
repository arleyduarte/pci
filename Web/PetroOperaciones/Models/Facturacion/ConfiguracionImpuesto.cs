using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace PetroOperaciones.Models.Facturacion
{
    public class ConfiguracionImpuesto
    {
        public static string IVA = "IVA";

        public int ConfiguracionImpuestoID { get; set; }
        public String PorcentajeAplicar { get; set; }
        public String Impuesto { get; set; }
        public String DescripcionImpuesto { get; set; }


        public decimal getPocentajeImpuesto(String impuesto)
        {
            decimal porcentaje=0;

            EfDbContext db = new EfDbContext();

            var configuracionImpuesto = (from p in db.ConfiguracionImpuesto
                                         where p.Impuesto == impuesto
                                         select p).SingleOrDefault();

            if (configuracionImpuesto != null)
            {
                porcentaje = Decimal.Parse(configuracionImpuesto.PorcentajeAplicar, new CultureInfo("es-ES")); 
            }


            return porcentaje;
        }
    }
}