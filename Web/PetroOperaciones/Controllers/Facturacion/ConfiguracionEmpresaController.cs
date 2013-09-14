using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models.Facturacion;
using PetroOperaciones.Models;

namespace PetroOperaciones.Controllers.Facturacion
{ 
    public class ConfiguracionEmpresaController : Controller
    {
        private EfDbContext db = new EfDbContext();







        //
        // GET: /ConfiguracionEmpresa/Edit/5
 
        public ActionResult Edit(int id)
        {
            ConfiguracionEmpresa configuracionempresa = db.ConfiguracionEmpresa.Find(id);
            return View(configuracionempresa);
        }

        //
        // POST: /ConfiguracionEmpresa/Edit/5

        [HttpPost]
        public ActionResult Edit(ConfiguracionEmpresa configuracionempresa)
        {
            if (ModelState.IsValid)
            {
                configuracionempresa.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();
                configuracionempresa.Estado = true;

                db.Entry(configuracionempresa).State = EntityState.Modified;
                db.SaveChanges();
                return View(configuracionempresa);
            }
            return View(configuracionempresa);
        }

    }
}