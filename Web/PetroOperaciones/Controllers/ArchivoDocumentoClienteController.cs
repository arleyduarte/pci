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
    public class ArchivoDocumentoClienteController : Controller
    {
        private EfDbContext db = new EfDbContext();

        [HttpPost]
        public ActionResult SendFile(int ClienteID, HttpPostedFileBase file)
        {
            ArchivoDocumentoCliente archivodocumento = new ArchivoDocumentoCliente();
            archivodocumento.ClienteID = ClienteID;
            ViewBag.ClienteID = ClienteID;

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                String nombreAchivo = ClienteID + "-" + fileName;
                archivodocumento.NombreArchivo = nombreAchivo;


                var path = Path.Combine(Server.MapPath("~/ArchivosSubidos/DocClientes"), nombreAchivo);
                file.SaveAs(path);
            }
            ViewBag.TipoDocumentoTerceroID = new SelectList(db.TipoDocumentoTerceros, "TipoDocumentoTerceroID", "TipoDocumentoTerceroNm", archivodocumento.TipoDocumentoTerceroID);
            return View("Create", archivodocumento);
        }

        public ViewResult Index()
        {
            var archivodocumentocliente = db.ArchivoDocumentoCliente.Include(a => a.Usuario).Include(a => a.TipoDocumentoTercero).Include(a => a.Cliente);
            ViewBag.FechaVencimiento = System.DateTime.Now.ToShortDateString();

            return View(archivodocumentocliente.ToList());
        }

        //
        // GET: /ArchivoDocumentoCliente/Details/5

        public ViewResult Details(int id)
        {
            ArchivoDocumentoCliente archivodocumentocliente = db.ArchivoDocumentoCliente.Find(id);
            return View(archivodocumentocliente);
        }

        //
        // GET: /ArchivoDocumentoCliente/Create

        public ActionResult Create(int ClienteID)
        {
            ArchivoDocumentoCliente archivodocumento = new ArchivoDocumentoCliente();

            ViewBag.ClienteID = ClienteID;
            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            archivodocumento.ClienteID = ClienteID;


            ViewBag.TipoDocumentoTerceroID = new SelectList(db.TipoDocumentoTerceros, "TipoDocumentoTerceroID", "TipoDocumentoTerceroNm");
            return View(archivodocumento);
        } 

        //
        // POST: /ArchivoDocumentoCliente/Create


        [HttpPost]
        public ActionResult Create(ArchivoDocumentoCliente archivodocumento)
        {

            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            archivodocumento.UsuarioID = usuario.UsuarioID;
            archivodocumento.FechaRegistro = CorrectorHoraServidor.getHoraActualColombia();

            if (ModelState.IsValid)
            {
                db.ArchivoDocumentoCliente.Add(archivodocumento);
                db.SaveChanges();
                return RedirectToAction("Create", new { ClienteID = archivodocumento.ClienteID });
            }


            ViewBag.TipoDocumentoTerceroID = new SelectList(db.TipoDocumentoTerceros, "TipoDocumentoTerceroID", "TipoDocumentoTerceroNm", archivodocumento.TipoDocumentoTerceroID);
            return View(archivodocumento);


        }
        
        //
        // GET: /ArchivoDocumentoCliente/Edit/5
 
        public ActionResult Edit(int id)
        {
            ArchivoDocumentoCliente archivodocumentocliente = db.ArchivoDocumentoCliente.Find(id);
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", archivodocumentocliente.UsuarioID);
            ViewBag.TipoDocumentoTerceroID = new SelectList(db.TipoDocumentoTerceros, "TipoDocumentoTerceroID", "TipoDocumentoTerceroNm", archivodocumentocliente.TipoDocumentoTerceroID);
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "NIT", archivodocumentocliente.ClienteID);
            return View(archivodocumentocliente);
        }

        //
        // POST: /ArchivoDocumentoCliente/Edit/5

        [HttpPost]
        public ActionResult Edit(ArchivoDocumentoCliente archivodocumentocliente)
        {

            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            archivodocumentocliente.UsuarioID = usuario.UsuarioID;
            archivodocumentocliente.FechaRegistro = CorrectorHoraServidor.getHoraActualColombia();


            if (ModelState.IsValid)
            {
                db.Entry(archivodocumentocliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", new { ClienteID = archivodocumentocliente.ClienteID });
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", archivodocumentocliente.UsuarioID);
            ViewBag.TipoDocumentoTerceroID = new SelectList(db.TipoDocumentoTerceros, "TipoDocumentoTerceroID", "TipoDocumentoTerceroNm", archivodocumentocliente.TipoDocumentoTerceroID);
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "NIT", archivodocumentocliente.ClienteID);
            return View(archivodocumentocliente);
        }

        public ActionResult GetList(int ClienteID)
        {
            var archivos = from p in db.ArchivoDocumentoCliente
                           where p.ClienteID == ClienteID
                           orderby p.TipoDocumentoTercero.TipoDocumentoTerceroNm

                           select p;

            return PartialView(archivos.ToList());
        }

        //
        // GET: /ArchivoDocumentoCliente/Delete/5
 
        public ActionResult Delete(int id)
        {
            ArchivoDocumentoCliente archivodocumentocliente = db.ArchivoDocumentoCliente.Find(id);
            return View(archivodocumentocliente);
        }

        //
        // POST: /ArchivoDocumentoCliente/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            ArchivoDocumentoCliente archivodocumentocliente = db.ArchivoDocumentoCliente.Find(id);
            db.ArchivoDocumentoCliente.Remove(archivodocumentocliente);
            db.SaveChanges();
            return RedirectToAction("Create", new { ClienteID = archivodocumentocliente.ClienteID });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}