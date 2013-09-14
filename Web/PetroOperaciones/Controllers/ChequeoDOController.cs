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
    public class ChequeoDOController : Controller
    {
        private EfDbContext db = new EfDbContext();



        //
        // GET: /ChequeoDO/Create

        public ActionResult Create(int DOID)
        {
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula");
            DocumentoOperaciones documentoOperaciones = db.DocumentoOperaciones.Find(DOID);
            ViewBag.NumeroDO = documentoOperaciones.NumeroDO;
            ViewBag.ClienteNm = documentoOperaciones.Cliente.NmCliente;
            ViewBag.DOID = DOID;

            ChequeoDO chequeodo = getChequeoDOByDO(DOID);
            chequeodo.DocumentoOperacionesID = DOID;


            return View(chequeodo);
        } 

        //
        // POST: /ChequeoDO/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(ChequeoDO chequeodo)
        {

            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            chequeodo.UsuarioID = usuario.UsuarioID;
            chequeodo.FechaCreacion = System.DateTime.Now;

            DocumentoOperaciones documentoOpearciones = db.DocumentoOperaciones.Find(chequeodo.DocumentoOperacionesID);
            documentoOpearciones.FechaCierre = System.DateTime.Now;
            documentoOpearciones.ChequeoRealizado = true;
            documentoOpearciones.Estado = EstadoDO.FINALIZADO;
            documentoOpearciones.NombreUsuarioFinalizadorDO = usuario.Nombre;

            if (ModelState.IsValid)
            {
                db.Entry(documentoOpearciones).State = EntityState.Modified;
                if (chequeodo.ChequeoDOID == 0)
                {
                    db.ChequeoDO.Add(chequeodo);
                }else{
                    db.Entry(chequeodo).State = EntityState.Modified;
                }

                db.SaveChanges();
            }

            return RedirectToAction("../DO/Details/" + chequeodo.DocumentoOperacionesID); 
        }

        private ChequeoDO getChequeoDOByDO(int DOId)
        {
            ChequeoDO item = null;


            try
            {
                item = (from p in db.ChequeoDO
                        where p.DocumentoOperacionesID == DOId
                        select p).OrderByDescending(x => x.ChequeoDOID).First();

               

            }
            catch
            {

            }

            if (item == null)
            {
                item = new ChequeoDO();
                return item;
            }
            else
            {
                return item;
            }
        }


        //
        // GET: /ChequeoDO/Edit/5
 
        public ActionResult Edit(int id)
        {
            ChequeoDO chequeodo = db.ChequeoDO.Find(id);
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", chequeodo.UsuarioID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", chequeodo.DocumentoOperacionesID);
            return View(chequeodo);
        }

        //
        // POST: /ChequeoDO/Edit/5

        [HttpPost]
        public ActionResult Edit(ChequeoDO chequeodo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chequeodo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", chequeodo.UsuarioID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", chequeodo.DocumentoOperacionesID);
            return View(chequeodo);
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}