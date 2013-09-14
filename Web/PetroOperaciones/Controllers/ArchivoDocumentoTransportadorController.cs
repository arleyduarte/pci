using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models;
using System.IO;

namespace PetroOperaciones.Controllers
{
    [Authorize]
    public class ArchivoDocumentoTransportadorController : Controller
    {
        private EfDbContext db = new EfDbContext();


        [HttpPost]
        public ActionResult SendFile(int TransportadorID, HttpPostedFileBase file)
        {
            ArchivoDocumentoTransportador archivodocumento = new ArchivoDocumentoTransportador();

            ViewBag.TransportadorID = TransportadorID;

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                String nombreAchivo = TransportadorID + "-" + fileName;
                archivodocumento.NombreArchivo = nombreAchivo;


                var path = Path.Combine(Server.MapPath("~/ArchivosSubidos/DocTransportadores"), nombreAchivo);
                file.SaveAs(path);
            }
            ViewBag.TipoDocumentoTerceroID = new SelectList(db.TipoDocumentoTerceros, "TipoDocumentoTerceroID", "TipoDocumentoTerceroNm", archivodocumento.TipoDocumentoTerceroID);
            return View("Create", archivodocumento);
        }

        //
        // GET: /ArchivoDocumentoTransportador/

        public ViewResult Index()
        {
            var archivodocumentotransportador = db.ArchivoDocumentoTransportador.Include(a => a.TipoDocumentoTercero);


            return View(archivodocumentotransportador.ToList());
        }

        //
        // GET: /ArchivoDocumentoTransportador/Details/5

        public ViewResult Details(int id)
        {
            ArchivoDocumentoTransportador archivodocumentotransportador = db.ArchivoDocumentoTransportador.Find(id);
            return View(archivodocumentotransportador);
        }

        //
        // GET: /ArchivoDocumentoTransportador/Create

        public ActionResult Create(int TransportadorID)
        {
            ArchivoDocumentoTransportador archivodocumento = new ArchivoDocumentoTransportador();

            ViewBag.TransportadorID = TransportadorID;
            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            //archivodocumento.UsuarioID = usuario.UsuarioID;
            archivodocumento.TransportadorID = TransportadorID;


            ViewBag.TipoDocumentoTerceroID = new SelectList(db.TipoDocumentoTerceros, "TipoDocumentoTerceroID", "TipoDocumentoTerceroNm");
            return View(archivodocumento);
        } 

        //
        // POST: /ArchivoDocumentoTransportador/Create

        [HttpPost]
        public ActionResult Create(ArchivoDocumentoTransportador archivodocumentotransportador)
        {

            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            archivodocumentotransportador.UsuarioID = usuario.UsuarioID;
            archivodocumentotransportador.FechaRegistro = CorrectorHoraServidor.getHoraActualColombia();

            if (ModelState.IsValid)
            {
                db.ArchivoDocumentoTransportador.Add(archivodocumentotransportador);
                db.SaveChanges();
                return RedirectToAction("Create", new { TransportadorID = archivodocumentotransportador.TransportadorID });
            }


            ViewBag.TipoDocumentoID = new SelectList(db.TipoDocumentoTerceros, "TipoDocumentoTerceroID", "TipoDocumentoTerceroNm", archivodocumentotransportador.TipoDocumentoTerceroID);
            return View(archivodocumentotransportador);


        }
        
        //
        // GET: /ArchivoDocumentoTransportador/Edit/5
 
        public ActionResult Edit(int id)
        {
            ArchivoDocumentoTransportador archivodocumentotransportador = db.ArchivoDocumentoTransportador.Find(id);
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", archivodocumentotransportador.UsuarioID);
            return View(archivodocumentotransportador);
        }

        public ActionResult GetList(int TransportadorID)
        {
            var archivos = from p in db.ArchivoDocumentoTransportador
                           where p.TransportadorID == TransportadorID
                           orderby p.TipoDocumentoTercero.TipoDocumentoTerceroNm 

                           select p;

            return PartialView(archivos.ToList());
        }


        //
        // POST: /ArchivoDocumentoTransportador/Edit/5

        //[HttpPost]
        //public ActionResult Edit(ArchivoDocumentoTransportador archivodocumentotransportador)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(archivodocumentotransportador).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", archivodocumentotransportador.UsuarioID);
        //    return View(archivodocumentotransportador);
        //}

        //
        // GET: /ArchivoDocumentoTransportador/Delete/5
 
        public ActionResult Delete(int id)
        {
            ArchivoDocumentoTransportador archivodocumentotransportador = db.ArchivoDocumentoTransportador.Find(id);
            return View(archivodocumentotransportador);
        }

        //
        // POST: /ArchivoDocumentoTransportador/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            ArchivoDocumentoTransportador archivodocumentotransportador = db.ArchivoDocumentoTransportador.Find(id);
            db.ArchivoDocumentoTransportador.Remove(archivodocumentotransportador);
            db.SaveChanges();
            return RedirectToAction("Create", new { TransportadorID = archivodocumentotransportador.TransportadorID });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}