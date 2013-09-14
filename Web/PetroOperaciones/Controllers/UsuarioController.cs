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
    public class UsuarioController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Usuario/

        public ViewResult Index()
        {
            //var usuarios = db.Usuarios.Include(u => u.TipoRol);
            var usuarios = db.Usuarios;
            return View(usuarios.ToList());
        }

        //
        // GET: /Usuario/Details/5

        public ViewResult Details(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            return View(usuario);
        }

        //
        // GET: /Usuario/Create
       [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Rol = new SelectList(db.TipoRoles, "Rol", "Descripcion");
            return View();
        } 

        //
        // POST: /Usuario/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {

            usuario.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();
            usuario.Estado = Usuario.ACTIVO;



            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.Rol = new SelectList(db.TipoRoles, "Rol", "Descripcion", usuario.Rol);
            return View(usuario);
        }
        
        //
        // GET: /Usuario/Edit/5
         [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            ViewBag.Rol = new SelectList(db.TipoRoles, "Rol", "Descripcion", usuario.Rol);
            return View(usuario);
        }

        //
        // POST: /Usuario/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Usuario usuario)
        {
            usuario.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();
            


            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Rol = new SelectList(db.TipoRoles, "Rol", "Descripcion", usuario.Rol);
            return View(usuario);
        }

        //
        // GET: /Usuario/Delete/5
         [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            return View(usuario);
        }

        //
        // POST: /Usuario/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Usuario usuario = db.Usuarios.Find(id);
            usuario.Estado = Usuario.INACTIVO;
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