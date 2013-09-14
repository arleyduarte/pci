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
    public class ClienteController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Cliente/

        public ViewResult Index()
        {
            return View(db.Clientes.ToList());
        }

        //
        // GET: /Cliente/Details/5

        public ViewResult Details(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            return View(cliente);
        }

        //
        // GET: /Cliente/Create

        public ActionResult Create()

        {

            ViewBag.PaisID = new SelectList(db.Paises, "PaisID", "PaisNm",1);
            return View();
        } 

        //
        // POST: /Cliente/Create

        [HttpPost]
        public ActionResult Create(Cliente cliente)
        {

            //if (ModelState.IsValid)
            //{
               db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");

            //}
            //return View(cliente);
        }
        
        //
        // GET: /Cliente/Edit/5
 
        public ActionResult Edit(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            ViewBag.PaisID = new SelectList(db.Paises, "PaisID", "PaisNm", cliente.PaisID);

            return View(cliente);
        }

        //
        // POST: /Cliente/Edit/5

        [HttpPost]
        public ActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        //
        // GET: /Cliente/Delete/5
 
        public ActionResult Delete(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            return View(cliente);
        }

        //
        // POST: /Cliente/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
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