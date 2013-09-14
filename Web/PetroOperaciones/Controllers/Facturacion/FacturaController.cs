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
    [Authorize(Roles = "Facturador")]
    public class FacturaController : Controller
    {
        private EfDbContext db = new EfDbContext();
        private ValidadorUsuario vu = new ValidadorUsuario();



        [Authorize(Roles = "Facturador")]
        public ViewResult GetFacturaName()
        {
            var facturas = from p in db.FacturaEncabezado
                           where p.ClienteID == 0
                           orderby p.FechaCreacion descending
                           select p;

            return View(facturas.ToList());
        }

        [HttpPost]
        public ActionResult GetFacturaName(FacturaEncabezado encabezado)
        {   var facturas = from p in db.FacturaEncabezado
                           where p.NoFacturaSoporte == encabezado.NoFacturaSoporte
                           && p.Estado != EstadoFactura.CREADA
                           orderby p.FechaCreacion descending
                           select p;

            return View(facturas.ToList());
        }


        //
        // GET: /Prefactura/

        public ViewResult Index()
        {
            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado == EstadoDO.EN_SEGUIMIENTO
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
                                       where p.Estado == EstadoDO.EN_SEGUIMIENTO
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


        // GET: /Prefactura/Create

        public ActionResult CreateWithPrefactura(int PreFacturaEncabezadoID)
        {

            FacturaBLL facturaBLL = new FacturaBLL();
            FacturaEncabezado encabezado = new FacturaEncabezado();
            
            if (!facturaBLL.existeFactura(PreFacturaEncabezadoID))
            {
                ValidadorUsuario vu = new ValidadorUsuario();
                Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
                facturaBLL.crearFactura(PreFacturaEncabezadoID, usuario);
            }
            encabezado = facturaBLL.getFacturaEncabezadoByPrefactura(PreFacturaEncabezadoID);

            var contactos = from p in db.Contactos
                            where p.TerceroID == encabezado.ClienteID
                            select p;

            ViewBag.AtencionSr = new SelectList(contactos, "ContactoID", "ContactoNm");
            ViewBag.PlazoFacturaID = new SelectList(db.PlazoFactura, "PlazoFacturaID", "PlazoFacturaNm");
            return View(encabezado);
        } 

        [HttpPost]
        public ActionResult CreateWithPrefactura(FacturaEncabezado facturaencabezado)
        {

            if (editEncabezadoFactura(facturaencabezado))
            {
                return RedirectToAction("../FacturaDetalle/Create", new { EncabezadoID = facturaencabezado.FacturaEncabezadoID, tienePrefactura = false });
            }
            

            return View(facturaencabezado);
        }

        //
        // GET: /Prefactura/Create

        public ActionResult Create()
        {

            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado != EstadoDO.FINALIZADO
                                       orderby p.FechaCreacion descending
                                       select p;


            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");
            ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");
            ViewBag.PlazoFacturaID = new SelectList(db.PlazoFactura, "PlazoFacturaID", "PlazoFacturaNm");
            return View();
        } 

        //
        // POST: /Prefactura/Create

        [HttpPost]
        public ActionResult Create(FacturaEncabezado facturaencabezado)
        {
            DocumentoOperacionesBLL documentoOperacionesBLL = new DocumentoOperacionesBLL();
            facturaencabezado.ClienteID = documentoOperacionesBLL.getDocumentoOperacionesByID(facturaencabezado.DocumentoOperacionesID).ClienteID;


            if (createFactura(facturaencabezado))
            {
               return RedirectToAction("../FacturaDetalle/Create", new { EncabezadoID = facturaencabezado.FacturaEncabezadoID, tienePrefactura = false });
            }

            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado != EstadoDO.FINALIZADO
                                       orderby p.FechaCreacion descending
                                       select p;


            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");
            ViewBag.PlazoFacturaID = new SelectList(db.PlazoFactura, "PlazoFacturaID", "PlazoFacturaNm");
            ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");

            return View(facturaencabezado);
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
            ViewBag.PlazoFacturaID = new SelectList(db.PlazoFactura, "PlazoFacturaID", "PlazoFacturaNm");
            return View();
        }


        [HttpPost]
        public ActionResult CreateWithClient(FacturaEncabezado facturaencabezado)
        {


            if (createFactura(facturaencabezado))
            {
                return RedirectToAction("../FacturaDetalle/Create", new { EncabezadoID = facturaencabezado.FacturaEncabezadoID, tienePrefactura = false });
            }

            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado != EstadoDO.FINALIZADO
                                       orderby p.FechaCreacion descending
                                       select p;

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");
            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO");
            ViewBag.PlazoFacturaID = new SelectList(db.PlazoFactura, "PlazoFacturaID", "PlazoFacturaNm");
            ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");

            return View(facturaencabezado);
        }


        private bool createFactura(FacturaEncabezado facturaencabezado)
        {
            facturaencabezado.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();
            facturaencabezado.Estado = EstadoFactura.CREADA;


                       
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            facturaencabezado.UsuarioCreadorID = usuario.UsuarioID;
            ConfiguracionEmpresaBLL config = new ConfiguracionEmpresaBLL();
            facturaencabezado.NoFacturaSoporte = config.getConsecutivoFactura(ConfiguracionEmpresaBLL.PRETROCARGO_CONFIGURACION);

            if (facturaencabezado.AtencionSr != null)
            {
                String idContacto = facturaencabezado.AtencionSr;
                idContacto = idContacto.Replace("\"", "");

                int contacID = Convert.ToInt32(idContacto);


                var contacto = (from p in db.Contactos
                                where p.ContactoID == contacID
                                select p).SingleOrDefault();

                facturaencabezado.AtencionSr = contacto.ContactoNm;
            }


            if (ModelState.IsValid)
            {

                facturaencabezado.TasaDeCambio = CultureUtils.getDecimal(CultureUtils.getDecimalFromString(facturaencabezado.TasaDeCambio));
                facturaencabezado.TotalAnticipos = CultureUtils.getDecimal(CultureUtils.getDecimalFromString(facturaencabezado.TotalAnticipos));


                db.FacturaEncabezado.Add(facturaencabezado);
                db.SaveChanges();
                return true;
            }

            return false;


        }
        
        //
        // GET: /Prefactura/Edit/5
 
        public ActionResult Edit(int id)
        {
            FacturaEncabezado facturaencabezado = db.FacturaEncabezado.Find(id);

            

            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado != EstadoDO.FINALIZADO
                                       orderby p.FechaCreacion descending
                                       select p;


            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO",facturaencabezado.DocumentoOperacionesID);
            ViewBag.PlazoFacturaID = new SelectList(db.PlazoFactura, "PlazoFacturaID", "PlazoFacturaNm");
            ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");

            return View(facturaencabezado);
        }

        //
        // POST: /Prefactura/Edit/5

        [HttpPost]
        public ActionResult Edit(FacturaEncabezado facturaencabezado)
        {

            if(editEncabezadoFactura(facturaencabezado))
            {
                return RedirectToAction("../FacturaDetalle/Create", new { EncabezadoID = facturaencabezado.FacturaEncabezadoID, tienePrefactura = false });
            }
            ViewBag.UsuarioCreadorID = new SelectList(db.Usuarios, "UsuarioID", "Cedula", facturaencabezado.UsuarioCreadorID);
            ViewBag.DocumentoOperacionesID = new SelectList(db.DocumentoOperaciones, "DocumentoOperacionesID", "NumeroDO", facturaencabezado.DocumentoOperacionesID);

            var contactos = (from p in db.Contactos
                            where p.ContactoNm == facturaencabezado.AtencionSr
                            select p);

            Contacto contacto = new Contacto();
            foreach (Contacto aux in contactos)
            {
                contacto = aux;
            }

            if (contacto.ContactoNm != null)
            {
                ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm", contacto.ContactoID);
            }
            else
            {
                ViewBag.AtencionSr = new SelectList(db.Contactos, "ContactoID", "ContactoNm");
            }

            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.Estado != EstadoDO.FINALIZADO
                                       orderby p.FechaCreacion descending
                                       select p;

            ViewBag.DocumentoOperacionesID = new SelectList(documentooperaciones, "DocumentoOperacionesID", "NumeroDO", facturaencabezado.DocumentoOperacionesID);
            ViewBag.PlazoFacturaID = new SelectList(db.PlazoFactura, "PlazoFacturaID", "PlazoFacturaNm", facturaencabezado.PlazoFacturaID);
            return View(facturaencabezado);
        }

        private bool editEncabezadoFactura(FacturaEncabezado facturaencabezado)
        {

            facturaencabezado.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();

           // FacturaEncabezado facuturaEncabezadoOrg = db.FacturaEncabezado.Find(facturaencabezado.FacturaEncabezadoID);
            
           // DocumentoOperacionesBLL documentoOperacionesBLL = new DocumentoOperacionesBLL();
            //facturaencabezado.ClienteID = documentoOperacionesBLL.getDocumentoOperacionesByID(facturaencabezado.DocumentoOperacionesID).ClienteID;

            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            facturaencabezado.UsuarioCreadorID = usuario.UsuarioID;
            //ConfiguracionEmpresaBLL config = new ConfiguracionEmpresaBLL();

           // facturaencabezado.NoFacturaSoporte = facuturaEncabezadoOrg.NoFacturaSoporte;

            if (facturaencabezado.AtencionSr != null)
            {
                String idContacto = facturaencabezado.AtencionSr;
                idContacto = idContacto.Replace("\"", "");

                int contacID = Convert.ToInt32(idContacto);


                var contacto = (from p in db.Contactos
                                where p.ContactoID == contacID
                                select p).SingleOrDefault();

                facturaencabezado.AtencionSr = contacto.ContactoNm;
            }

            if (ModelState.IsValid)
            {
                db.Entry(facturaencabezado).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }

            return false;
        }

        //
        // GET: /Prefactura/Delete/5
 
        public ActionResult Delete(int id)
        {
            PrefacturaEncabezado prefacturaencabezado = db.PrefacturaEncabezado.Find(id);
            return View(prefacturaencabezado);
        }

        //
        // POST: /Prefactura/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            PrefacturaEncabezado prefacturaencabezado = db.PrefacturaEncabezado.Find(id);
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