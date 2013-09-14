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
    public class DOController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /DO/

        public ViewResult Index()
        {
            var documentooperaciones = db.DocumentoOperaciones.Include(d => d.PuertoO).Include(d => d.PuertoD).Include(d => d.Estado);
            return View(documentooperaciones.ToList());
        }

        //
        // GET: /DO/Details/5

        public ViewResult Details(int id)
        {
            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(id);
            return View(documentooperaciones);
        }

        //
        // GET: /DO/Create

        public ActionResult Create()
        {
            ViewBag.PuertoOrigen = new SelectList(db.Puertos, "PuertoID", "PuertoNm");
            ViewBag.PuertoDestino = new SelectList(db.Puertos, "PuertoID", "PuertoNm");
            ViewBag.EstadoDO = new SelectList(db.Estados, "EstadoID", "EstadoNm");
            return View();
        } 

        //
        // POST: /DO/Create

        [HttpPost]
        public ActionResult Create(DocumentoOperaciones documentooperaciones)
        {
            if (ModelState.IsValid)
            {
                db.DocumentoOperaciones.Add(documentooperaciones);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PuertoOrigen = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoDestino);
            ViewBag.EstadoDO = new SelectList(db.Estados, "EstadoID", "EstadoNm", documentooperaciones.EstadoDO);
            return View(documentooperaciones);
        }
        
        //
        // GET: /DO/Edit/5
 
        public ActionResult Edit(int id)
        {
            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(id);
            ViewBag.PuertoOrigen = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoDestino);
            ViewBag.EstadoDO = new SelectList(db.Estados, "EstadoID", "EstadoNm", documentooperaciones.EstadoDO);
            return View(documentooperaciones);
        }

        //
        // POST: /DO/Edit/5

        [HttpPost]
        public ActionResult Edit(DocumentoOperaciones documentooperaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentooperaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PuertoOrigen = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoDestino);
            ViewBag.EstadoDO = new SelectList(db.Estados, "EstadoID", "EstadoNm", documentooperaciones.EstadoDO);
            return View(documentooperaciones);
        }

        //
        // GET: /DO/Delete/5
 
        public ActionResult Delete(int id)
        {
            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(id);
            return View(documentooperaciones);
        }

        //
        // POST: /DO/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(id);
            db.DocumentoOperaciones.Remove(documentooperaciones);
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