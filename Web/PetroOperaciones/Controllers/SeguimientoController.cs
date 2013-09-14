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
    public class SeguimientoController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Seguimiento/

        public ViewResult Index()
        {
            var seguimiento = db.Seguimiento.Include(s => s.Usuario).Include(s => s.DocumentoOperaciones);
            return View(seguimiento.ToList());
        }


        public ViewResult GetUltimosSeguimiento()
        {


            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);


            //if (User.IsInRole("Admin"))
            //{
            //    var seguimientos = (from p in db.Seguimiento
            //                        orderby p.SeguimientoID descending
            //                       select p).Take(20);
            //    return View(seguimientos.ToList());
            //}

                var seguimientos = (from p in db.Seguimiento
                                    orderby p.SeguimientoID descending
                                    select p).Take(20);
                return View(seguimientos.ToList());

        }


        public ActionResult GetList(int DOID)
        {
            var seguimientos = from p in db.Seguimiento
                               where p.DocumentoOperacionesID == DOID
                               orderby p.SeguimientoID descending
                              select p;
            return PartialView(seguimientos.ToList());
        }


        public ActionResult GetReporteFinal(int DOID)
        {
            var seguimientos = from p in db.Seguimiento
                               where p.DocumentoOperacionesID == DOID
                               && p.EsInformeFinal == true
                               orderby p.SeguimientoID descending
                               select p;
            return PartialView(seguimientos.ToList());
        }



        //
        // GET: /Seguimiento/Create

        public ActionResult Create(int DOID)
        {
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula");

            DocumentoOperaciones documentoOperaciones = getDocumentoOperaciones(DOID);

            ViewBag.NumeroDO = documentoOperaciones.NumeroDO;
            ViewBag.ClienteNm = documentoOperaciones.Cliente.NmCliente;

            Seguimiento seguimiento =  new Seguimiento();

            ViewBag.DOID = DOID;

            seguimiento.DocumentoOperacionesID = DOID;

            return View(seguimiento);
  
        }




        //
        // POST: /Seguimiento/Create

        [HttpPost]
        public ActionResult Create(Seguimiento seguimiento)
        {

            DocumentoOperaciones documentoOperaciones = getDocumentoOperaciones(seguimiento.DocumentoOperacionesID);

            ViewBag.NumeroDO = documentoOperaciones.NumeroDO;
            ViewBag.ClienteNm = documentoOperaciones.Cliente.NmCliente;

            if (ModelState.IsValid)
            {
                ValidadorUsuario vu = new ValidadorUsuario();
                Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
                seguimiento.UsuarioID = usuario.UsuarioID;

                seguimiento.FechaRegistro = CorrectorHoraServidor.getHoraActualColombia();

                db.Seguimiento.Add(seguimiento);
                db.SaveChanges();
                return RedirectToAction("Create", new { DOID = seguimiento.DocumentoOperacionesID });  
            }

            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", seguimiento.UsuarioID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", seguimiento.DocumentoOperacionesID);
            return View(seguimiento);
        }


        
        //
        // GET: /Seguimiento/Edit/5
 
        public ActionResult Edit(int id)
        {
            Seguimiento seguimiento = db.Seguimiento.Find(id);
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", seguimiento.UsuarioID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", seguimiento.DocumentoOperacionesID);
            return View(seguimiento);
        }

        //
        // POST: /Seguimiento/Edit/5

        [HttpPost]
        public ActionResult Edit(Seguimiento seguimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seguimiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", seguimiento.UsuarioID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", seguimiento.DocumentoOperacionesID);
            return View(seguimiento);
        }

        //
        // GET: /Seguimiento/Delete/5
 
        public ActionResult Delete(int id)
        {
            Seguimiento seguimiento = db.Seguimiento.Find(id);
            return View(seguimiento);
        }

        //
        // POST: /Seguimiento/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Seguimiento seguimiento = db.Seguimiento.Find(id);
            db.Seguimiento.Remove(seguimiento);
            db.SaveChanges();
            return RedirectToAction("Index");
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