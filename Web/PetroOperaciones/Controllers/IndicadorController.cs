using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models.Facturacion;
using PetroOperaciones.Models;

namespace PetroOperaciones.Controllers
{ 
    public class IndicadorController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Indicador/

        public ViewResult Index()
        {
            var indicador = db.Indicador.Include(i => i.TipoDeIndicador);
            return View(indicador.ToList());
        }

        //
        // GET: /Indicador/Details/5

        public ViewResult Details(int id)
        {
            Indicador indicador = db.Indicador.Find(id);
            return View(indicador);
        }

        //
        // GET: /Indicador/Create

        public ActionResult Create()
        {
            ViewBag.TipoDeIndicadorID = new SelectList(db.TipoDeIndicador, "TipoDeIndicadorID", "TipoDeIndicadorNm");
            ViewBag.PeriodoMes = new SelectList(Utilidades.getMeses(), "ListItemID", "ListItemNm");
            ViewBag.PeriodoAno = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm");

            return View();
        } 

        //
        // POST: /Indicador/Create

        [HttpPost]
        public ActionResult Create(Indicador indicador)
        {
            indicador.FechaRegistro = CorrectorHoraServidor.getHoraActualColombia();


            if (ModelState.IsValid & !indicador.existeIndicador())
            {
                db.Indicador.Add(indicador);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PeriodoMes = new SelectList(Utilidades.getMeses(), "ListItemID", "ListItemNm", indicador.PeriodoMes);
            ViewBag.PeriodoAno = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm", indicador.PeriodoAno);
            ViewBag.TipoDeIndicadorID = new SelectList(db.TipoDeIndicador, "TipoDeIndicadorID", "TipoDeIndicadorNm", indicador.TipoDeIndicadorID);
            return View(indicador);
        }
        
        //
        // GET: /Indicador/Edit/5
 
        public ActionResult Edit(int id)
        {
            Indicador indicador = db.Indicador.Find(id);
            //ViewBag.PeriodoMes = new SelectList(Utilidades.getMeses(), "ListItemID", "ListItemNm");
            //ViewBag.PeriodoAno = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm");

            ViewBag.TipoDeIndicadorID = new SelectList(db.TipoDeIndicador, "TipoDeIndicadorID", "TipoDeIndicadorNm", indicador.TipoDeIndicadorID);
            return View(indicador);
        }

        //
        // POST: /Indicador/Edit/5

        [HttpPost]
        public ActionResult Edit(Indicador indicador)
        {
            indicador.FechaRegistro = CorrectorHoraServidor.getHoraActualColombia();

            if (ModelState.IsValid)
            {
                db.Entry(indicador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.PeriodoMes = new SelectList(Utilidades.getMeses(), "ListItemID", "ListItemNm");
            //ViewBag.PeriodoAno = new SelectList(Utilidades.getAnosDisponibles(), "ListItemID", "ListItemNm");

            ViewBag.TipoDeIndicadorID = new SelectList(db.TipoDeIndicador, "TipoDeIndicadorID", "TipoDeIndicadorNm", indicador.TipoDeIndicadorID);
            return View(indicador);
        }

        //
        // GET: /Indicador/Delete/5
 
        public ActionResult Delete(int id)
        {
            Indicador indicador = db.Indicador.Find(id);
            return View(indicador);
        }

        //
        // POST: /Indicador/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Indicador indicador = db.Indicador.Find(id);
            db.Indicador.Remove(indicador);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}