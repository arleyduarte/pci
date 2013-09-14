using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models;
using PetroOperaciones.Models.Facturacion;
using System.Xml.Linq;

namespace PetroOperaciones.Controllers.Facturacion
{
     [Authorize]
    public class ReportBillingController : Controller
    {
        private EfDbContext db = new EfDbContext();
        private ReportBuilderForBillingModule reportBuilder = new ReportBuilderForBillingModule();


        //*************************************************** BuscadorFactura **********************
         [Authorize(Roles = "Facturador")]
        public ActionResult BuscadorFactura()
        {                                
            var facturas = from p in db.FacturaEncabezado
                           where p.ClienteID == 0
                           orderby p.FechaCreacion descending
                           select p;

            FiltroReporte filtro = new FiltroReporte();
            filtro.FechaFinal = System.DateTime.Now;
            filtro.FechaInicial = System.DateTime.Now;
            var model = new BuscadorFacturaViewModel
            {
                Filtro = filtro,
                Facturas = facturas
            };

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            return View(model);
        }

        [HttpPost]
        public ActionResult BuscadorFactura(BuscadorFacturaViewModel viewModel)
        {

            var facturas = from p in db.FacturaEncabezado
                           where p.ClienteID == viewModel.Filtro.ClienteID
                            && p.FechaFactura >= viewModel.Filtro.FechaInicial
                            && p.FechaFactura <= viewModel.Filtro.FechaFinal
                            && p.Estado != EstadoFactura.CREADA
                           orderby p.FechaCreacion descending
                           select p;

            if (viewModel.Filtro.ClienteID == 0)
            {
                 facturas = from p in db.FacturaEncabezado
                            where p.FechaFactura >= viewModel.Filtro.FechaInicial
                                && p.FechaFactura <= viewModel.Filtro.FechaFinal
                                 && p.Estado != EstadoFactura.CREADA
                               orderby p.FechaCreacion descending
                               select p;
            }


            var model = new BuscadorFacturaViewModel
            {
                Filtro = viewModel.Filtro,
                Facturas = facturas
            };

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", viewModel.Filtro.ClienteID);
            return View(model);
        }
        //*************************************************** EDN BuscadorFactura **********************


        //*************************************************** BuscadorPrefactura **********************
        [Authorize(Roles = "Facturador, Admin, Auxiliar")]
        public ActionResult BuscadorPrefactura()
        {
            var prefacturas = from p in db.PrefacturaEncabezado
                           where p.DocumentoOperacionesID == 0
                           orderby p.FechaCreacion descending
                           select p;

            FiltroReporte filtro = new FiltroReporte();
            filtro.FechaFinal = System.DateTime.Now;
            filtro.FechaInicial = System.DateTime.Now;
            var model = new BuscadorPrefacturaViewModel
            {
                Filtro = filtro,
                Prefacturas = prefacturas
            };

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            return View(model);
        }

          [Authorize(Roles = "Facturador, Admin, Auxiliar")]
        [HttpPost]
        public ActionResult BuscadorPrefactura(BuscadorPrefacturaViewModel viewModel)
        {
            var prefacturas = from p in db.PrefacturaEncabezado
                           where p.DocumentoOperaciones.ClienteID == viewModel.Filtro.ClienteID
                            && p.FechaCreacion >= viewModel.Filtro.FechaInicial
                            && p.FechaCreacion <= viewModel.Filtro.FechaFinal

                           orderby p.FechaCreacion descending
                           select p;

            var model = new BuscadorPrefacturaViewModel
            {
                Filtro = viewModel.Filtro,
                Prefacturas = prefacturas
            };

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", viewModel.Filtro.ClienteID);
            return View(model);
        }
        //*************************************************** END BuscadorPrefactura **********************




        //*************************************************** Reportes ***************************
       [Authorize(Roles = "Facturador, Admin")]
        public ActionResult TotalVentasPorCliente()
        {
        
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Facturador, Admin")]
        public ActionResult TotalVentasPorCliente(RangoFechas filtro)
        {
            if (ModelState.IsValid)
            {
                RangoFechas rangoFechas = new RangoFechas();
                rangoFechas.FechaInicial = filtro.FechaInicial;
                rangoFechas.FechaFinal = filtro.FechaFinal;
                reportBuilder.Title = "Facturación en Pesos Colombianos";
                reportBuilder.Prefix = "";
                ViewBag.xml = reportBuilder.getXMLReportCustumerSales(rangoFechas);
            }
            return View(filtro);
        }

       [Authorize(Roles = "Facturador, Admin")]
        public ActionResult VentasMensualesPorCliente()
        {
            FiltroReporteAnoCliente filtro = new FiltroReporteAnoCliente();
            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            ViewBag.Ano = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm", System.DateTime.Now.Year);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Facturador, Admin")]
        public ActionResult VentasMensualesPorCliente(FiltroReporteAnoCliente filtro)
        {
            if (ModelState.IsValid)
            {
                RangoFechas rangoFechas = new RangoFechas();
                reportBuilder.Title = "Facturación en Pesos Colombianos";
                reportBuilder.Prefix = "";
                ViewBag.xml = reportBuilder.getXMLReportCustumerMonthSales(filtro);
            }
            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", filtro.ClienteID);
            ViewBag.Ano = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm", filtro.Ano);
            return View(filtro);
        }

       [Authorize(Roles = "Facturador, Admin")]
        public ActionResult IngresosOperacionales()
        {
            FiltroReporteAnoCliente filtro = new FiltroReporteAnoCliente();
            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            ViewBag.Ano = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm", System.DateTime.Now.Year);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Facturador, Admin")]
        public ActionResult IngresosOperacionales(FiltroReporteAnoCliente filtro)
        {

            RangoFechas rangoFechas = new RangoFechas();
            reportBuilder.Title = "Ingresos Operacionales en Pesos Colombianos";
            reportBuilder.Prefix = "";

            int[] tipoIngreso = {TipoDeIngreso.INGRESOS_PROPIOS, TipoDeIngreso.INGRESOS_PROPIOS_NO_GRAVADOS };
            ViewBag.xml = reportBuilder.getXMLCustumerMonthIncoming(filtro, tipoIngreso);
          
            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", filtro.ClienteID);
            ViewBag.Ano = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm", filtro.Ano);
            return View(filtro);
        }

       [Authorize(Roles = "Facturador, Admin")]
        public ActionResult IngresosTerceros()
        {
            FiltroReporteAnoCliente filtro = new FiltroReporteAnoCliente();
            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            ViewBag.Ano = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm", System.DateTime.Now.Year);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Facturador, Admin")]
        public ActionResult IngresosTerceros(FiltroReporteAnoCliente filtro)
        {
            RangoFechas rangoFechas = new RangoFechas();
            reportBuilder.Title = "Ingresos Terceros en Pesos Colombianos";
            reportBuilder.Prefix = "";
            int[] tipoIngreso = { TipoDeIngreso.INGRESOS_A_TERCEROS};
            ViewBag.xml = reportBuilder.getXMLCustumerMonthIncoming(filtro, tipoIngreso);

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", filtro.ClienteID);
            ViewBag.Ano = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm", filtro.Ano);
            return View(filtro);
        }

       [Authorize(Roles = "Facturador, Admin")]
        public ActionResult VentasVsMeta()
        {
              return View();
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public XDocument GetVentasVsMeta(int year)
        {
            reportBuilder.Title = "Ventas Totas vs Proyección";
            reportBuilder.YAxisName = "Ventas en Miles de Pesos";
            return XDocument.Parse(reportBuilder.getXMLSalesVsTarget(year));
        }


       [Authorize(Roles = "Facturador, Admin")]
        public ActionResult IngresosPropiosVsMeta()
        {
            return View();
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public XDocument GetIngresosPropiosVsMeta(int year)
        {
            reportBuilder.Title = "Total Ingresos Propios vs Proyección";
            reportBuilder.YAxisName = "Ventas en Miles de Pesos";
            return XDocument.Parse(reportBuilder.getXMLOwnIncomingVsTarget(year));
        }




              [Authorize(Roles = "Facturador, Admin")]
        public ActionResult IngresosTercerosVsMeta()
        {
            return View();
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public XDocument GetIngresosTercerosVsMeta(int year)
        {
            reportBuilder.Title = "Total Ingresos Terceros vs Proyección";
            reportBuilder.YAxisName = "Ventas en Miles de Pesos";
            return XDocument.Parse(reportBuilder.getXMLThridIncomingVsTarget(year));
        }

              [Authorize(Roles = "Facturador, Admin")]
        public ActionResult IngresosPropiosNoGravadosVsMeta()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public XDocument GetIngresosPropiosNoGravadosVsMeta(int year)
        {
            reportBuilder.Title = "Total Ingresos Propios No Gravados vs Proyección";
            reportBuilder.YAxisName = "Ventas en Miles de Pesos";
            return XDocument.Parse(reportBuilder.getXMLOwnNoTaxedIncomingVsTarget(year));
        }
        //*************************************************** END Reportes ***************************

    }
}
