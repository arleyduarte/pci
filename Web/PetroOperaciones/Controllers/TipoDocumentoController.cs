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
    public class TipoDocumentoController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /TipoDocumento/

        public ViewResult Index()
        {
            return View(db.TipoDocumentoes.ToList());
        }

        //
        // GET: /TipoDocumento/Details/5

        public ViewResult Details(int id)
        {
            TipoDocumento tipodocumento = db.TipoDocumentoes.Find(id);
            return View(tipodocumento);
        }

        //
        // GET: /TipoDocumento/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TipoDocumento/Create

        [HttpPost]
        public ActionResult Create(TipoDocumento tipodocumento)
        {
            if (ModelState.IsValid)
            {
                db.TipoDocumentoes.Add(tipodocumento);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(tipodocumento);
        }
        
        //
        // GET: /TipoDocumento/Edit/5
 
        public ActionResult Edit(int id)
        {
            TipoDocumento tipodocumento = db.TipoDocumentoes.Find(id);
            return View(tipodocumento);
        }

        //
        // POST: /TipoDocumento/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoDocumento tipodocumento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipodocumento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipodocumento);
        }

        //
        // GET: /TipoDocumento/Delete/5
 
        public ActionResult Delete(int id)
        {
            TipoDocumento tipodocumento = db.TipoDocumentoes.Find(id);
            return View(tipodocumento);
        }

        //
        // POST: /TipoDocumento/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            TipoDocumento tipodocumento = db.TipoDocumentoes.Find(id);
            db.TipoDocumentoes.Remove(tipodocumento);
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