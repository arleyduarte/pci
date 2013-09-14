using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models;
using System.Text;
using System.Xml.Linq;

namespace PetroOperaciones.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private EfDbContext db = new EfDbContext();
        private GeneradorXMLReportes generadorXML = new GeneradorXMLReportes();

        
        public ActionResult TotalOperacionesCliente()
        {
            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            return View();
        }


        [HttpPost]
        public ActionResult TotalOperacionesCliente(FiltroReporte filtro)
        {
            if (ModelState.IsValid)
            {
                RangoFechas rangoFechas = new RangoFechas();
                rangoFechas.FechaInicial = filtro.FechaInicial;
                rangoFechas.FechaFinal = filtro.FechaFinal;
                generadorXML.TituloGrafico = "Total Operaciones";
                generadorXML.Prefijo = "";
                ViewBag.xml = generadorXML.getReporteTotalOperacionesByTipo(rangoFechas, filtro.ClienteID);

            }

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            return View(filtro);
        }



        public ActionResult TotalOperacionesPuertoO()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TotalOperacionesPuertoO(RangoFechas filtro)
        {
            if (ModelState.IsValid)
            {
                RangoFechas rangoFechas = new RangoFechas();
                rangoFechas.FechaInicial = filtro.FechaInicial;
                rangoFechas.FechaFinal = filtro.FechaFinal;
                generadorXML.TituloGrafico = "Total Operaciones Por Origen";
                generadorXML.Prefijo = "";

                if (User.IsInRole(TipoRol.CLIENTE))
                {
                    ViewBag.xml = generadorXML.getReporteTotalOperacionesByPuerto(rangoFechas, GeneradorXMLReportes.TIPO_PUERTO_ORIGEN, GetClienteId());
                }
                else
                {
                    ViewBag.xml = generadorXML.getReporteTotalOperacionesByPuerto(rangoFechas, GeneradorXMLReportes.TIPO_PUERTO_ORIGEN, 0);
                }


            }

            return View(filtro);
        }

        public ActionResult TotalOperacionesPuertoD()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TotalOperacionesPuertoD(RangoFechas filtro)
        {
            if (ModelState.IsValid)
            {
                RangoFechas rangoFechas = new RangoFechas();
                rangoFechas.FechaInicial = filtro.FechaInicial;
                rangoFechas.FechaFinal = filtro.FechaFinal;

                generadorXML.TituloGrafico = "Total Operaciones Por Destino";
                generadorXML.Prefijo = "";

                if (User.IsInRole(TipoRol.CLIENTE))
                {
                    ViewBag.xml = generadorXML.getReporteTotalOperacionesByPuerto(rangoFechas, GeneradorXMLReportes.TIPO_PUERTO_DESTINO, GetClienteId());
                }
                else
                {
                    ViewBag.xml = generadorXML.getReporteTotalOperacionesByPuerto(rangoFechas, GeneradorXMLReportes.TIPO_PUERTO_DESTINO, 0);
                }
                

            }


            return View(filtro);
        }


        public ActionResult TotalOperacionesUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TotalOperacionesUsuario(RangoFechas filtro)
        {
            if (ModelState.IsValid)
            {
                RangoFechas rangoFechas = new RangoFechas();
                rangoFechas.FechaInicial = filtro.FechaInicial;
                rangoFechas.FechaFinal = filtro.FechaFinal;
                generadorXML.TituloGrafico = "Total Operaciones";
                generadorXML.Prefijo = "";
                ViewBag.xml = generadorXML.getReporteTotalOperacionesByUsuario(rangoFechas);

            }

            
            return View(filtro);
        }


        public ActionResult TotalMercanciaMovilizada()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TotalMercanciaMovilizada(RangoFechas filtro)
        {
            if (ModelState.IsValid)
            {
                RangoFechas rangoFechas = new RangoFechas();
                rangoFechas.FechaInicial = filtro.FechaInicial;
                rangoFechas.FechaFinal = filtro.FechaFinal;
                generadorXML.TituloGrafico = "Modo de Transporte";
                generadorXML.Prefijo = "";
                ViewBag.xml = generadorXML.getReporteTotalMercanciaMovilizadaByTipo(rangoFechas);

            }

           
            return View(filtro);
        }






        public ActionResult TotalCarga()
        {
            generadorXML.TituloGrafico = "Cantidad Carga Consolidada";
            generadorXML.YAxisName = "Total Carga Transportada en Kilogramos";
           // ViewBag.xml = generadorXML.getReporteTotalCargaConsolidada(System.DateTime.Now.Year);
            return View();
        }


        public ActionResult TotalSeguimientos()
        {
            generadorXML.TituloGrafico = "Seguimientos Registrados";
            generadorXML.YAxisName = "No";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public XDocument GetTotalSeguimientos(int? year)
        {
            generadorXML.TituloGrafico = "Seguimientos Registrados";
            generadorXML.YAxisName = "No";
            int selectedYear = System.DateTime.Now.Year;
            if (year != null)
            {
                 selectedYear = Convert.ToInt32(year);
              
            }

            return XDocument.Parse(generadorXML.getReporteTotalSeguimientosRegistrados(selectedYear));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public XDocument GetTotalCarga(int year)
        {            
            generadorXML.TituloGrafico = "Cantidad Carga Consolidada";
            generadorXML.YAxisName = "Total Carga Transportada en Kilogramos";

            if (User.IsInRole(TipoRol.CLIENTE))
            {
                return XDocument.Parse(generadorXML.getXMLReporteTotalCargaConsolidada(year, GetClienteId()));
            }
            else
            {
                return XDocument.Parse(generadorXML.getXMLReporteTotalCargaConsolidada(year, 0));   
            }

                             
        }

         [AcceptVerbs(HttpVerbs.Get)]
        public XDocument GetOperacionesPorTipo(int year)
        {
            generadorXML.TituloGrafico = "Operaciones por Tipo";
            generadorXML.YAxisName = "No Operaciones Realizadas";
            return XDocument.Parse(generadorXML.getXMLReporteTotalOperacionesByTipo(year)); 
        }

         public ActionResult OperacionesPorTipo()
         {
             generadorXML.TituloGrafico = "Operaciones por Tipo";
             generadorXML.YAxisName = "No Operaciones Realizadas";
             return View();
         }


         private int GetClienteId()
         {
             ValidadorUsuario vu = new ValidadorUsuario();
             return vu.GetClienteByLogin(User.Identity.Name).ClienteID;
         }



    }
}
