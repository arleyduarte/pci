using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models.Facturacion;
using PetroOperaciones.Models;
using System.Collections;
using invoiceToPDF;
using PetroOperaciones.Models.Cotizacion;

namespace PetroOperaciones.Controllers.Cotizacion
{
    [Authorize]
    public class CotizacionController : Controller
    {
        private EfDbContext db = new EfDbContext();
        private ValidadorUsuario vu = new ValidadorUsuario();

        public ActionResult Create()
        {

            var vItems = (from p in db.Clientes
                          select p).OrderBy(x => x.NmCliente);

            ArrayList vAux = new ArrayList();
            String arrayClients = "";
            
            foreach (Cliente cliente in vItems)
            {
                String clienteName = "\""+cliente.NmCliente+"\"";

                arrayClients += clienteName + ",";

                
            }

            if (arrayClients.EndsWith(","))
            {
               arrayClients =  arrayClients.Remove(arrayClients.Length-1, 1);
            }



            ViewBag.datasource = "[" + arrayClients + "]";



            ViewBag.AsesorComercialID = new SelectList(db.Usuarios, "UsuarioID", "Nombre");
            ViewBag.PuertoOrigen = new SelectList(db.Puertos.OrderBy(x => x.PuertoNm), "PuertoID", "PuertoNm");
            ViewBag.PuertoDestino = new SelectList(db.Puertos.OrderBy(x => x.PuertoNm), "PuertoID", "PuertoNm");

            return View();
        }

        [HttpPost]
        public ActionResult Create(CotizacionEncabezado cotizacionEncabezado)
        {
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            cotizacionEncabezado.UsuarioCreadorID = usuario.UsuarioID;
            cotizacionEncabezado.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();

            if (ModelState.IsValid)
            {
                cotizacionEncabezado.TasaDeCambio = CultureUtils.getDecimal(CultureUtils.getDecimalFromString(cotizacionEncabezado.TasaDeCambio));
                db.CotizacionEncabezado.Add(cotizacionEncabezado);
                db.SaveChanges();

                return RedirectToAction("../CotizacionDetalle/Create", new { CotizacionEncabezadoID = cotizacionEncabezado.CotizacionEncabezadoID });
            }

            ViewBag.AsesorComercialID = new SelectList(db.Usuarios, "UsuarioID", "Nombre");
            ViewBag.PuertoOrigen = new SelectList(db.Puertos.OrderBy(x => x.PuertoNm), "PuertoID", "PuertoNm");
            ViewBag.PuertoDestino = new SelectList(db.Puertos.OrderBy(x => x.PuertoNm), "PuertoID", "PuertoNm");

            return View(cotizacionEncabezado);
        }

        public ActionResult Find()
        {
            var cotizaciones = from p in db.CotizacionEncabezado
                           where p.AsesorComercialID == 0
                           orderby p.FechaCreacion descending
                           select p;

            FiltroReporte filtro = new FiltroReporte();
            filtro.FechaFinal = System.DateTime.Now;
            filtro.FechaInicial = System.DateTime.Now;
            var model = new BuscadorCotizacionViewModel
            {
                Filtro = filtro,
                Cotizaciones = cotizaciones
            };


            return View(model);
        }


        [HttpPost]
        public ActionResult Find(BuscadorCotizacionViewModel viewModel)
        {
            var cotizaciones = from p in db.CotizacionEncabezado
                           where 
                             p.FechaCotizacion >= viewModel.Filtro.FechaInicial
                            && p.FechaCotizacion <= viewModel.Filtro.FechaFinal

                           orderby p.FechaCreacion descending
                           select p;

            FiltroReporte filtro = new FiltroReporte();
            filtro.FechaFinal = System.DateTime.Now;
            filtro.FechaInicial = System.DateTime.Now;
            var model = new BuscadorCotizacionViewModel
            {
                Filtro = filtro,
                Cotizaciones = cotizaciones
            };


            return View(model);
        }




    }
}