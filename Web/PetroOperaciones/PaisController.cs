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
    public class PaisController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Pais/

        public ViewResult Index()
        {
            return View(db.Paises.ToList());
        }

        //
        // GET: /Pais/Details/5

        public ViewResult Details(int id)
        {
            Pais pais = db.Paises.Find(id);
            return View(pais);
        }

        //
        // GET: /Pais/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Pais/Create

        [HttpPost]
        public ActionResult Create(Pais pais)
        {
            if (ModelState.IsValid)
            {
                db.Paises.Add(pais);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(pais);
        }
        
        //
        // GET: /Pais/Edit/5
 
        public ActionResult Edit(int id)
        {
            Pais pais = db.Paises.Find(id);
            return View(pais);
        }

        //
        // POST: /Pais/Edit/5

        [HttpPost]
        public ActionResult Edit(Pais pais)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pais).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pais);
        }

        //
        // GET: /Pais/Delete/5
 
        public ActionResult Delete(int id)
        {
            Pais pais = db.Paises.Find(id);
            return View(pais);
        }

        //
        // POST: /Pais/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Pais pais = db.Paises.Find(id);
            db.Paises.Remove(pais);
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