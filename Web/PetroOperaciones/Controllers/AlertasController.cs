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
    [Authorize(Roles = "Admin")]
    public class AlertasController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Alertas/

        public ViewResult Index()
        {
            return View(db.Alertas.ToList());
        }


 
        public ActionResult Edit(int id)
        {
            Alertas alertas = db.Alertas.Find(id);
            return View(alertas);
        }

        //
        // POST: /Alertas/Edit/5

        [HttpPost]
        public ActionResult Edit(Alertas alertas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alertas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alertas);
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}