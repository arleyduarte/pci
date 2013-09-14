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
    public class PuertoController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Puerto/

        public ViewResult Index()
        {
            return View(db.Puertos.ToList());
        }

        //
        // GET: /Puerto/Details/5

        public ViewResult Details(int id)
        {
            Puerto puerto = db.Puertos.Find(id);
            return View(puerto);
        }

        //
        // GET: /Puerto/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Puerto/Create

        [HttpPost]
        public ActionResult Create(Puerto puerto)
        {
            if (ModelState.IsValid)
            {
                db.Puertos.Add(puerto);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(puerto);
        }
        
        //
        // GET: /Puerto/Edit/5
 
        public ActionResult Edit(int id)
        {
            Puerto puerto = db.Puertos.Find(id);
            return View(puerto);
        }

        //
        // POST: /Puerto/Edit/5

        [HttpPost]
        public ActionResult Edit(Puerto puerto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(puerto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(puerto);
        }

        //
        // GET: /Puerto/Delete/5
 
        public ActionResult Delete(int id)
        {
            Puerto puerto = db.Puertos.Find(id);
            return View(puerto);
        }

        //
        // POST: /Puerto/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Puerto puerto = db.Puertos.Find(id);
            db.Puertos.Remove(puerto);
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