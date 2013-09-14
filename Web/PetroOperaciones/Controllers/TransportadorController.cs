using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models;

namespace PetroOperaciones.Controllers
{
    [Authorize]
    public class TransportadorController : Controller
    {

        private EfDbContext db = new EfDbContext();

        //
        // GET: /Transportador/

        public ViewResult Index()
        {
            var transportadores = db.Transportadores.Include(t => t.TipoTransportador);
            return View(transportadores.ToList().OrderBy(x => x.TransportadorNm));
        }

        //
        // GET: /Transportador/Details/5


        public ViewResult GetList(int? TipoTransportadorID)
        {
            var transportadores = db.Transportadores.Include(t => t.TipoTransportador);

            ViewBag.TipoTransportadorID = new SelectList(db.TipoTransportadors, "TipoTransportadorID", "TipoTransportadorNm");

            var trasportadores = (from p in db.Transportadores
                                 where p.TipoTransportador.TipoTransportadorID == TipoTransportadorID
                                 orderby p.TransportadorNm descending
                                  select p).OrderBy(x => x.TransportadorNm);
            return View(trasportadores.ToList());


        }

        public ViewResult Details(int id)
        {
            Transportador transportador = db.Transportadores.Find(id);
            return View(transportador);
        }

        //
        // GET: /Transportador/Create

        public ActionResult Create()
        {
            ViewBag.TipoTransportadorID = new SelectList(db.TipoTransportadors, "TipoTransportadorID", "TipoTransportadorNm");
            return View();
        } 

        //
        // POST: /Transportador/Create

        [HttpPost]
        public ActionResult Create(Transportador transportador)
        {
            if (ModelState.IsValid)
            {
                db.Transportadores.Add(transportador);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.TipoTransportadorID = new SelectList(db.TipoTransportadors, "TipoTransportadorID", "TipoTransportadorNm", transportador.TipoTransportadorID);
            return View(transportador);
        }
        
        //
        // GET: /Transportador/Edit/5
 
        public ActionResult Edit(int id)
        {
            Transportador transportador = db.Transportadores.Find(id);
            ViewBag.TipoTransportadorID = new SelectList(db.TipoTransportadors, "TipoTransportadorID", "TipoTransportadorNm", transportador.TipoTransportadorID);
            return View(transportador);
        }

        //
        // POST: /Transportador/Edit/5

        [HttpPost]
        public ActionResult Edit(Transportador transportador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transportador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoTransportadorID = new SelectList(db.TipoTransportadors, "TipoTransportadorID", "TipoTransportadorNm", transportador.TipoTransportadorID);
            return View(transportador);
        }

        //
        // GET: /Transportador/Delete/5
 
        public ActionResult Delete(int id)
        {
            Transportador transportador = db.Transportadores.Find(id);
            return View(transportador);
        }

        //
        // POST: /Transportador/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Transportador transportador = db.Transportadores.Find(id);
            db.Transportadores.Remove(transportador);
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