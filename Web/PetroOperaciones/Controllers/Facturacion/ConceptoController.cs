using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models.Facturacion;
using PetroOperaciones.Models;

namespace PetroOperaciones.Controllers.Facturacion
{
    [Authorize]
    public class ConceptoController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Concepto/

        public ViewResult Index()
        {
            return View(db.Concepto.ToList().OrderBy(x => x.ConceptoNm));
        }

        //
        // GET: /Concepto/Details/5

        public ViewResult Details(int id)
        {
            Concepto concepto = db.Concepto.Find(id);
            return View(concepto);
        }

        //
        // GET: /Concepto/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Concepto/Create

        [HttpPost]
        public ActionResult Create(Concepto concepto)
        {
            if (ModelState.IsValid)
            {
                db.Concepto.Add(concepto);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(concepto);
        }
        
        //
        // GET: /Concepto/Edit/5
 
        public ActionResult Edit(int id)
        {
            Concepto concepto = db.Concepto.Find(id);
            return View(concepto);
        }

        //
        // POST: /Concepto/Edit/5

        [HttpPost]
        public ActionResult Edit(Concepto concepto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(concepto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(concepto);
        }

        //
        // GET: /Concepto/Delete/5
 
        public ActionResult Delete(int id)
        {
            Concepto concepto = db.Concepto.Find(id);
            return View(concepto);
        }

        //
        // POST: /Concepto/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Concepto concepto = db.Concepto.Find(id);
            db.Concepto.Remove(concepto);
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