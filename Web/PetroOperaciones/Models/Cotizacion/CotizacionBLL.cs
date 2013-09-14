using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetroOperaciones.Models.Basic;
using System.Collections;
using System.Globalization;

namespace PetroOperaciones.Models.Cotizacion
{
    public class CotizacionBLL
    {
        private EfDbContext db = new EfDbContext();

        public MasterDetail getQuote(int CotizacionEncabezadoID)
        {
            MasterDetail vm = new MasterDetail();
            vm.IMasterEntity = getCotizacionById(CotizacionEncabezadoID);
            vm.Details = getDetallesCotizacion(CotizacionEncabezadoID);
            return vm;
        }

        public CotizacionEncabezado getCotizacionById(int CotizacionEncabezadoID)
        {
            CotizacionEncabezado encabezado = (from p in db.CotizacionEncabezado
                                               where p.CotizacionEncabezadoID == CotizacionEncabezadoID
                                               select p).SingleOrDefault();

            return encabezado;
        }

        public ArrayList getDetallesCotizacion(int CotizacionEncabezadoID)
        {
            ArrayList vDetalles = new ArrayList();
            var detalles = from p in db.CotizacionDetalle
                           where p.CotizacionEncabezadoID == CotizacionEncabezadoID
                           orderby p.TipoConceptoCotizacionID
                           select p;

            foreach (CotizacionDetalle detalle in detalles)
            {
                vDetalles.Add(detalle);
            }

            return vDetalles;
        }

        public SubTotalesCotizacion getSubTotalesCotizacion(int CotizacionEncabezadoID, String tipoMoneda)
        {
            SubTotalesCotizacion subTotalesCotizacion = new SubTotalesCotizacion();

            ArrayList vDetalles = getDetallesCotizacion(CotizacionEncabezadoID);

            subTotalesCotizacion.FleteMaritimo = getSubTotal(tipoMoneda,
                getDetallesPorTipo(vDetalles, TipoConceptoCotizacion.GASTOS_POR_FLETE_M_A_INTERNACIONAL));

            subTotalesCotizacion.GatosOrigen = getSubTotal(tipoMoneda,
                        getDetallesPorTipo(vDetalles, TipoConceptoCotizacion.GASTOS_EN_ORIGEN));

            subTotalesCotizacion.GatosDestino = getSubTotal(tipoMoneda,
                        getDetallesPorTipo(vDetalles, TipoConceptoCotizacion.GASTOS_EN_DESTINO));

            subTotalesCotizacion.AgenciamientoAduanero = getSubTotal(tipoMoneda,
                        getDetallesPorTipo(vDetalles, TipoConceptoCotizacion.AGENCIAMIENTO_ADUANEROS));

            return subTotalesCotizacion;
        }

        public decimal getSubTotal(String tipoMoneda, ArrayList vDetallesFactura)
        {

            decimal sumatoriaCOP = 0;
            decimal sumatoriaUSD = 0;
            foreach (CotizacionDetalle detalle in vDetallesFactura)
            {
                sumatoriaCOP = sumatoriaCOP + Decimal.Parse(detalle.ValorCOP, new CultureInfo("es-ES"));
                sumatoriaUSD = sumatoriaUSD + Decimal.Parse(detalle.ValorUSD, new CultureInfo("es-ES"));
            }

            if (tipoMoneda == Constants.PESOS_COLOMBIANOS)
            {
                return sumatoriaCOP;
            }
            else if (tipoMoneda == Constants.DOLARES_AMERICANOS)
            {
                return sumatoriaUSD;
            }

            else
            {
                return sumatoriaCOP;
            }
        }


        private ArrayList getDetallesPorTipo(ArrayList detalles, int type)
        {
            ArrayList l = new ArrayList();
            foreach (CotizacionDetalle invoiceDatail in detalles)
            {

                if (invoiceDatail.TipoConceptoCotizacionID == type)
                {
                    l.Add(invoiceDatail);
                }

            }
            return l;
        }


    }
}