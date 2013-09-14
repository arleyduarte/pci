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

namespace PetroOperaciones.Controllers.Facturacion
{
    [Authorize]
    public class PrefacturaController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /Prefactura/

        public ViewResult Index()
        {
            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado != EstadoDO.FINALIZADO
                                       orderby p.FechaCreacion descending
                                       select p;

            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");


            var prefacturaencabezado = from p in db.PrefacturaEncabezado
                            where p.DocumentoOperacionesID == null
                            orderby p.FechaCreacion descending
                            select p;
            return View(prefacturaencabezado.ToList());
        }

        [HttpPost]
        public ViewResult Index(PrefacturaEncabezado prefacturaencabezado)
        {
            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado != EstadoDO.FINALIZADO
                                       orderby p.FechaCreacion descending
                                       select p;

            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");

            if (prefacturaencabezado.FechaCreacion.Year > Utilidades.FECHA_MINIMA & prefacturaencabezado.FechaCreacion != null)
            {


                if (prefacturaencabezado.DocumentoOperacionesID != 0)
                {
                    var prefacturas = from p in db.PrefacturaEncabezado
                                      where p.DocumentoOperacionesID == prefacturaencabezado.DocumentoOperacionesID
                                      && p.FechaCreacion >= prefacturaencabezado.FechaCreacion
                                      orderby p.FechaCreacion descending
                                      select p;

                    return View(prefacturas.ToList());
                }
                else
                {

                    var prefacturas = from p in db.PrefacturaEncabezado
                                      where
                                       p.FechaCreacion >= prefacturaencabezado.FechaCreacion
                                      orderby p.FechaCreacion descending
                                      select p;

                    return View(prefacturas.ToList());

                }




                    
                

            }

            else
            {
                var prefacturas = from p in db.PrefacturaEncabezado
                                   orderby p.FechaCreacion descending
                                  select p;

                return View(prefacturas.ToList());
            }




        }

        //
        // GET: /Prefactura/Details/5

        public ViewResult Details(int id)
        {
            PrefacturaEncabezado prefacturaencabezado = db.PrefacturaEncabezado.Find(id);
            return View(prefacturaencabezado);
        }


        public ActionResult Create()
        {

            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado == EstadoDO.EN_SEGUIMIENTO
                                       orderby p.FechaCreacion descending
                                       select p;
            var contactos = from p in db.Contactos
                            where p.TerceroID == 0
                            select p;
            ViewBag.AtencionSr = new SelectList(contactos, "ContactoID", "ContactoNm");
            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");
            return View();
        } 


        [HttpPost]
        public ActionResult Create(PrefacturaEncabezado prefacturaencabezado)
        {
            DocumentoOperaciones documentoOpera = db.DocumentoOperaciones.Find(prefacturaencabezado.DocumentoOperacionesID);
            prefacturaencabezado.ClienteID = documentoOpera.ClienteID;

            if (createPrefactura(prefacturaencabezado))
            {
                return RedirectToAction("../PreDetalle/Create", new { PrefacturaEncabezadoID = prefacturaencabezado.PrefacturaEncabezadoID });
            }

            ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");
            ViewBag.UsuarioCreadorID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", prefacturaencabezado.UsuarioCreadorID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", prefacturaencabezado.DocumentoOperacionesID);
            return View(prefacturaencabezado);
        }

        private bool createPrefactura(PrefacturaEncabezado prefacturaencabezado)
        {
            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            prefacturaencabezado.UsuarioCreadorID = usuario.UsuarioID;
            prefacturaencabezado.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();

            Cliente cliente = db.Clientes.Find(prefacturaencabezado.ClienteID);
            prefacturaencabezado.Direccion = cliente.Direccion;
            prefacturaencabezado.NIT = cliente.NIT;
            prefacturaencabezado.NmCliente = cliente.NmCliente;
            prefacturaencabezado.Telefono1 = cliente.Telefono1;
            prefacturaencabezado.Telefono2 = cliente.Telefono2;

            if (prefacturaencabezado.AtencionSr != null)
            {
                String idContacto = prefacturaencabezado.AtencionSr;
                idContacto = idContacto.Replace("\"", "");

                int contacID = Convert.ToInt32(idContacto);


                var contacto = (from p in db.Contactos
                                where p.ContactoID == contacID
                                select p).SingleOrDefault();

                prefacturaencabezado.AtencionSr = contacto.ContactoNm;
            }


            if (ModelState.IsValid)
            {
                prefacturaencabezado.TasaDeCambio = CultureUtils.getDecimal(CultureUtils.getDecimalFromString(prefacturaencabezado.TasaDeCambio));
                prefacturaencabezado.TotalAnticipos = CultureUtils.getDecimal(CultureUtils.getDecimalFromString(prefacturaencabezado.TotalAnticipos));

                db.PrefacturaEncabezado.Add(prefacturaencabezado);
                db.SaveChanges();

                return true;
               
            }

            return false;

        }


        public ActionResult CreateWithClient()
        {

            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado != EstadoDO.FINALIZADO
                                       orderby p.FechaCreacion descending
                                       select p;

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");
            ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");
            return View();
        }


        [HttpPost]
        public ActionResult CreateWithClient(PrefacturaEncabezado prefacturaEncabezado)
        {


            if (createPrefactura(prefacturaEncabezado))
            {
                return RedirectToAction("../PreDetalle/Create", new { PrefacturaEncabezadoID = prefacturaEncabezado.PrefacturaEncabezadoID });
            }

            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado != EstadoDO.FINALIZADO
                                       orderby p.FechaCreacion descending
                                       select p;

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");
            ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");
            return View(prefacturaEncabezado);
        }

        //
        // GET: /Prefactura/Edit/5
 
        public ActionResult Edit(int id)
        {
            PrefacturaEncabezado prefacturaencabezado = db.PrefacturaEncabezado.Find(id);
            ViewBag.UsuarioCreadorID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", prefacturaencabezado.UsuarioCreadorID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", prefacturaencabezado.DocumentoOperacionesID);

               ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm",prefacturaencabezado.AtencionSr);
            return View(prefacturaencabezado);
        }

        //
        // POST: /Prefactura/Edit/5

        [HttpPost]
        public ActionResult Edit(PrefacturaEncabezado prefacturaencabezado)
        {

            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            prefacturaencabezado.UsuarioCreadorID = usuario.UsuarioID;
            prefacturaencabezado.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();

            if (ModelState.IsValid)
            {
                prefacturaencabezado.TasaDeCambio = CultureUtils.getDecimal(CultureUtils.getDecimalFromString(prefacturaencabezado.TasaDeCambio));
                prefacturaencabezado.TotalAnticipos = CultureUtils.getDecimal(CultureUtils.getDecimalFromString(prefacturaencabezado.TotalAnticipos));

                db.Entry(prefacturaencabezado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../PreDetalle/Create", new { PrefacturaEncabezadoID = prefacturaencabezado.PrefacturaEncabezadoID });
            }

            ViewBag.UsuarioCreadorID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", prefacturaencabezado.UsuarioCreadorID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", prefacturaencabezado.DocumentoOperacionesID);
            return View(prefacturaencabezado);


        }


        public ActionResult Delete(int id)
        {
            PrefacturaEncabezado prefacturaencabezado = db.PrefacturaEncabezado.Find(id);
            return View(prefacturaencabezado);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(PrefacturaEncabezado prefacturaencabezado)
        {
            prefacturaencabezado = db.PrefacturaEncabezado.Find(prefacturaencabezado.PrefacturaEncabezadoID);            
            FacturaBLL facturaBLL = new FacturaBLL();

            foreach (PrefacturaDetalle detalle in facturaBLL.getDetallesPrefactura(prefacturaencabezado.PrefacturaEncabezadoID))
            {
                db.PrefacturaDetalle.Remove(db.PrefacturaDetalle.Find(detalle.PrefacturaDetalleID));
            }

            db.PrefacturaEncabezado.Remove(prefacturaencabezado);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetConceptos()
        {
            var items = from p in db.Concepto
                        orderby p.ConceptoNm
                        select p;

            String JEditableObject = "{";

            foreach (Concepto item in items)
            {
                JEditableObject += "'" + item.ConceptoID + "':'" + item.ConceptoNm + "',";
            }
            JEditableObject += "}";


            return Json(JEditableObject, JsonRequestBehavior.AllowGet);

        }	


    }
}