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
    public class TipoOperacionController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /TipoOperacion/

        public ViewResult Index()
        {
            return View(db.TiposOperaciones.ToList());
        }

        //
        // GET: /TipoOperacion/Details/5

        public ViewResult Details(int id)
        {
            TipoOperacion tipooperacion = db.TiposOperaciones.Find(id);
            return View(tipooperacion);
        }

        //
        // GET: /TipoOperacion/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TipoOperacion/Create

        [HttpPost]
        public ActionResult Create(TipoOperacion tipooperacion)
        {

           TipoOperacion  item = (from p in db.TiposOperaciones
                    select p).OrderByDescending(x => x.TipoOperacionID).First();
           tipooperacion.TipoOperacionID = item.TipoOperacionID + 1;
            if (ModelState.IsValid)
            {

                db.TiposOperaciones.Add(tipooperacion);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(tipooperacion);
        }
        
        //
        // GET: /TipoOperacion/Edit/5
 
        public ActionResult Edit(int id)
        {
            TipoOperacion tipooperacion = db.TiposOperaciones.Find(id);
            return View(tipooperacion);
        }

        //
        // POST: /TipoOperacion/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoOperacion tipooperacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipooperacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipooperacion);
        }

        //
        // GET: /TipoOperacion/Delete/5
 
        public ActionResult Delete(int id)
        {
            TipoOperacion tipooperacion = db.TiposOperaciones.Find(id);
            return View(tipooperacion);
        }

        //
        // POST: /TipoOperacion/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            TipoOperacion tipooperacion = db.TiposOperaciones.Find(id);
            db.TiposOperaciones.Remove(tipooperacion);
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