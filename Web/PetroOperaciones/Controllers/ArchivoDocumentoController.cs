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
    public class ArchivoDocumentoController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /ArchivoDocumento/

        [HttpPost]
        public ActionResult SendFile(int DOID, HttpPostedFileBase file)
        {
            DocumentoOperaciones documentoOperaciones = getDocumentoOperaciones(DOID);
            ArchivoDocumento archivodocumento = new ArchivoDocumento();
            archivodocumento.DocumentoOperacionesID = DOID;
            ViewBag.NumeroDO = documentoOperaciones.NumeroDO;
            ViewBag.DOID = DOID;

            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    String nombreAchivo = documentoOperaciones.NumeroDO + "-" + fileName;
                    archivodocumento.NombreArchivo = nombreAchivo;


                    var path = Path.Combine(Server.MapPath("~/ArchivosSubidos/"), nombreAchivo);
                    file.SaveAs(path);
                }
            }

            ViewBag.TipoDocumentoID = new SelectList(db.TipoDocumentoes, "TipoDocumentoID", "TipoDocumentoNm", archivodocumento.TipoDocumentoID);
            return View("Create", archivodocumento);
        }

        public ViewResult Index()
        {
            var archivodocumento = db.ArchivoDocumento.Include(a => a.Usuario).Include(a => a.DocumentoOperaciones).Include(a => a.TipoDocumento);
            return View(archivodocumento.ToList());
        }


        public ViewResult GetListName()
        {
            ViewBag.TipoDocumentoID = new SelectList(db.TipoDocumentoes, "TipoDocumentoID", "TipoDocumentoNm");

            var archivos = from p in db.ArchivoDocumento
                           where p.DocumentoOperacionesID == null
                           orderby p.TipoDocumento.TipoDocumentoNm
                           select p;

            return View(archivos.ToList());
        }


        [HttpPost]
        public ActionResult GetListName(ArchivoDocumento archivodocumento)
        {

            if (archivodocumento.TipoDocumentoID == 0)
            {
                var archivos = from p in db.ArchivoDocumento
                               where p.NoDocumento.Contains(archivodocumento.NoDocumento)
                               orderby p.TipoDocumento.TipoDocumentoNm
                               select p;

                ViewBag.TipoDocumentoID = new SelectList(db.TipoDocumentoes, "TipoDocumentoID", "TipoDocumentoNm", archivodocumento.TipoDocumentoID);
                return View(archivos.ToList());
            }

            else
            {
                var archivos = from p in db.ArchivoDocumento
                               where p.NoDocumento.Contains(archivodocumento.NoDocumento)
                               && p.TipoDocumentoID == archivodocumento.TipoDocumentoID
                               orderby p.TipoDocumento.TipoDocumentoNm
                               select p;


                ViewBag.TipoDocumentoID = new SelectList(db.TipoDocumentoes, "TipoDocumentoID", "TipoDocumentoNm", archivodocumento.TipoDocumentoID);
                return View(archivos.ToList());
            }




        }

        //
        // GET: /ArchivoDocumento/Details/5

        public ViewResult Details(int id)
        {
            ArchivoDocumento archivodocumento = db.ArchivoDocumento.Find(id);
            return View(archivodocumento);
        }

        public ActionResult GetList(int DOID)
        {
            var archivos = from p in db.ArchivoDocumento
                               where p.DocumentoOperacionesID == DOID
                               orderby p.TipoDocumento.TipoDocumentoNm 
                               select p;

            return PartialView(archivos.ToList());
        }

        //
        // GET: /ArchivoDocumento/Create

        public ActionResult Create(int DOID)
        {
            ArchivoDocumento archivodocumento = new ArchivoDocumento();

            ViewBag.DOID = DOID;

            DocumentoOperaciones documentoOperaciones = getDocumentoOperaciones(DOID);

            ViewBag.NumeroDO = documentoOperaciones.NumeroDO;
            archivodocumento.DocumentoOperacionesID = DOID;
            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            //archivodocumento.UsuarioID = usuario.UsuarioID;
            archivodocumento.DocumentoOperacionesID = DOID;


            ViewBag.TipoDocumentoID = new SelectList(db.TipoDocumentoes, "TipoDocumentoID", "TipoDocumentoNm");
            return View(archivodocumento);
        } 

        //
        // POST: /ArchivoDocumento/Create

        [HttpPost]
        public ActionResult Create(ArchivoDocumento archivodocumento)
        {


            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            archivodocumento.UsuarioID = usuario.UsuarioID;
            archivodocumento.FechaRegistro = CorrectorHoraServidor.getHoraActualColombia();

            if (ModelState.IsValid)
            {
                db.ArchivoDocumento.Add(archivodocumento);
                db.SaveChanges();
                return RedirectToAction("Create", new { DOID = archivodocumento.DocumentoOperacionesID });    
            }

            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", archivodocumento.UsuarioID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", archivodocumento.DocumentoOperacionesID);
            ViewBag.TipoDocumentoID = new SelectList(db.TipoDocumentoes, "TipoDocumentoID", "TipoDocumentoNm", archivodocumento.TipoDocumentoID);
            return View(archivodocumento);
        }
        
        //
        // GET: /ArchivoDocumento/Edit/5
 
        public ActionResult Edit(int id)
        {
            ArchivoDocumento archivodocumento = db.ArchivoDocumento.Find(id);
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", archivodocumento.UsuarioID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", archivodocumento.DocumentoOperacionesID);
            ViewBag.TipoDocumentoID = new SelectList(db.TipoDocumentoes, "TipoDocumentoID", "TipoDocumentoNm", archivodocumento.TipoDocumentoID);
            return View(archivodocumento);
        }

        //
        // POST: /ArchivoDocumento/Edit/5

        //[HttpPost]
        //public ActionResult Edit(ArchivoDocumento archivodocumento)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(archivodocumento).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", archivodocumento.UsuarioID);
        //    ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", archivodocumento.DocumentoOperacionesID);
        //    ViewBag.TipoDocumentoID = new SelectList(db.TipoDocumentoes, "TipoDocumentoID", "TipoDocumentoNm", archivodocumento.TipoDocumentoID);
        //    return View(archivodocumento);
        //}

        //
        // GET: /ArchivoDocumento/Delete/5
 
        public ActionResult Delete(int id)
        {
            ArchivoDocumento archivodocumento = db.ArchivoDocumento.Find(id);
            return View(archivodocumento);
        }

        //
        // POST: /ArchivoDocumento/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            ArchivoDocumento archivodocumento = db.ArchivoDocumento.Find(id);
            db.ArchivoDocumento.Remove(archivodocumento);
            db.SaveChanges();
            return RedirectToAction("Create", new { DOID = archivodocumento.DocumentoOperacionesID });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private DocumentoOperaciones getDocumentoOperaciones(int DOID)
        {
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();
            DocumentoOperaciones documentoOperaciones = directorBLL.getDocumentoOperacionesByID(DOID);

            return documentoOperaciones;
        }
    }
}