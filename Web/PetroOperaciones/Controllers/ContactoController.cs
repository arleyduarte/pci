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
    public class ContactoController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Contacto/

        public ViewResult Index()
        {
            var contactos = db.Contactos.Include(c => c.Cliente);
            return View(contactos.ToList());
        }

        public ViewResult ClienteList(int ClienteID)
        {
            var contactos = db.Contactos.Include(c => c.Cliente);
            ViewBag.TerceroID = ClienteID;

            var query = from c in db.Contactos
                        where c.TerceroID == ClienteID
                        select c;

            return View(query);

        }

        //
        // GET: /Contacto/Details/5

        public ViewResult Details(int id)
        {
            Contacto contacto = db.Contactos.Find(id);
            return View(contacto);
        }

        //
        // GET: /Contacto/Create

        public ActionResult Create()
        {
            ViewBag.TerceroID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            return View();
        }

        //
        // POST: /Contacto/Create

        [HttpPost]
        public ActionResult Create(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                db.Contactos.Add(contacto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TerceroID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente),  "ClienteID", "NmCliente", contacto.TerceroID);
            return View(contacto);
        }

        public ActionResult CreateByCliente(int ClienteID)
        {
            ViewBag.TerceroID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", ClienteID);
            return View();
        }

        [HttpPost]
        public ActionResult CreateByCliente(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                db.Contactos.Add(contacto);
                db.SaveChanges();
                return RedirectToAction("ClienteList", new { ClienteID = contacto.TerceroID });
            }

            ViewBag.TerceroID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", contacto.TerceroID);
            return View(contacto);
        }


        
        //
        // GET: /Contacto/Edit/5
 
        public ActionResult Edit(int id)
        {
            Contacto contacto = db.Contactos.Find(id);
            ViewBag.TerceroID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", contacto.TerceroID);
            return View(contacto);
        }

        //
        // POST: /Contacto/Edit/5

        [HttpPost]
        public ActionResult Edit(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contacto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TerceroID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", contacto.TerceroID);
            return View(contacto);
        }


        //
        // GET: /Contacto/Edit/5

        public ActionResult EditByCliente(int id)
        {
            Contacto contacto = db.Contactos.Find(id);
            ViewBag.TerceroID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", contacto.TerceroID);
            return View(contacto);
        }

        //
        // POST: /Contacto/Edit/5

        [HttpPost]
        public ActionResult EditByCliente(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contacto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ClienteList", new { ClienteID = contacto.TerceroID });
            }
            ViewBag.TerceroID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", contacto.TerceroID);
            return View(contacto);
        }

        //
        // GET: /Contacto/Delete/5
 
        public ActionResult Delete(int id)
        {
            Contacto contacto = db.Contactos.Find(id);
            return View(contacto);
        }

        //
        // POST: /Contacto/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Contacto contacto = db.Contactos.Find(id);
            db.Contactos.Remove(contacto);
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