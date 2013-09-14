using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models.Facturacion;
using PetroOperaciones.Models;
using System.Collections;
using invoiceToPDF;
using PetroOperaciones.Models.Cotizacion;

namespace PetroOperaciones.Controllers.Anticipo
{
    [Authorize]
    public class AnticipoController : Controller
    {
        private EfDbContext db = new EfDbContext();
        private ValidadorUsuario vu = new ValidadorUsuario();

        public ActionResult Create()
        {


            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado == EstadoDO.EN_SEGUIMIENTO
                                       orderby p.FechaCreacion descending
                                       select p;

            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");
            ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");
            ViewBag.MedioTransporteID = new SelectList(db.MedioTransportes, "MedioTransporteID", "MedioTransporteNm");
            return View();
        }

        [HttpPost]
        public ActionResult Create(AnticipoEncabezado anticipoEncabezado)
        {
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            anticipoEncabezado.UsuarioCreadorID = usuario.UsuarioID;
            anticipoEncabezado.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();

            if (ModelState.IsValid)
            {
                DocumentoOperacionesBLL documentoOperacionesBLL = new DocumentoOperacionesBLL();
                anticipoEncabezado.ClienteID = documentoOperacionesBLL.getDocumentoOperacionesByID(anticipoEncabezado.DocumentoOperacionesID).ClienteID;


                anticipoEncabezado.TasaDeCambio = CultureUtils.getDecimal(CultureUtils.getDecimalFromString(anticipoEncabezado.TasaDeCambio));
                db.AnticipoEncabezado.Add(anticipoEncabezado);
                db.SaveChanges();

                return RedirectToAction("../AnticipoDetalle/Create", new { AnticipoEncabezadoID = anticipoEncabezado.AnticipoEncabezadoID });
            }

            ViewBag.MedioTransporteID = new SelectList(db.MedioTransportes, "MedioTransporteID", "MedioTransporteNm");

            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado == EstadoDO.EN_SEGUIMIENTO
                                       orderby p.FechaCreacion descending
                                       select p;

            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");
            ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");


            return View(anticipoEncabezado);
        }



    }
}