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
using System.IO;
using PetroOperaciones.Controllers.Facturacion.invoiceToPDF.tools;

namespace PetroOperaciones.Controllers.Facturacion
{
    [Authorize]
    public class FacturaDetalleController : Controller
    {
        private EfDbContext db = new EfDbContext();
        private static String totalFacturaCOP = "";
        private FacturaBLL facturaBLL = new FacturaBLL();

        public InvoicePDFResult GetInvoicePDFFile(int FacturaEncabezadoId, String tipoMoneda)
        {
            String tmpInvoicesPath = Path.Combine(Server.MapPath("~/Content/invoices/invoicesTmp/"));
            String templateInvoicesPath = Path.Combine(Server.MapPath("~/Content/invoices/invoicesTemplates/"));


            FileManager fileManager = new FileManager();
            fileManager.deletePDFFiles(tmpInvoicesPath);

            FacturaModel facturaModel = facturaBLL.getFacturaModel(FacturaEncabezadoId);
            InvoiceToPDF fc = new InvoiceToPDF(templateInvoicesPath, tmpInvoicesPath, tipoMoneda);
            String invoiceFileName  =fc.makeInvoiceToPdf(facturaModel);
            String finalInvoicesPath = "~/Content/invoices/invoicesTmp/"+invoiceFileName;

            return new InvoicePDFResult
            {
                FileName = invoiceFileName,
                Path = finalInvoicesPath
            };
        }




        //
        // GET: /PreDetalle/

        public ViewResult Index()
        {

            FacturaEncabezado encabezado = new FacturaEncabezado();
            encabezado.FacturaEncabezadoID = 5;


            var detalles = from p in db.FacturaDetalle
                           where p.FacturaEncabezadoID == encabezado.FacturaEncabezadoID
                           orderby p.TipoDeIngresoID descending
                           select p;

            //var model = new PrefacturaViewModel

            var model = new FacturaViewModel
            {
                // TODO: fetch from the repository instead of hardcoding
                Detalles = detalles,
                FacturaEncabezado = encabezado
            };



            return View(model);
        }

        //
        // GET: /PreDetalle/Details/5

        public ViewResult Details(int id)
        {
            FacturaDetalle FacturaDetalle = db.FacturaDetalle.Find(id);
            return View(FacturaDetalle);
        }

        //, string valorUSD, string valorCOP, string detalles, string soporte
        // GET: /PreDetalle/Create

        public int AddData(int tipoIngreso, int ConceptoID, String moneda, String valorUSD, String valorCOP, String detalles, String soporte, int FacturaEncabezadoId)
        {

            FacturaDetalle facturaDetalle = new FacturaDetalle();
            facturaDetalle.FacturaEncabezadoID = FacturaEncabezadoId;
            facturaDetalle.TipoDeIngresoID = tipoIngreso;
            facturaDetalle.Detalles = detalles.Trim();
            facturaDetalle.FacturaSoporte = soporte;

            var concepto  = (from p in db.Concepto
                            where p.ConceptoID == ConceptoID
                            select p).SingleOrDefault();
            facturaDetalle.ConceptoID = ConceptoID;
            facturaDetalle.ConceptoNm = concepto.ConceptoNm;

            var FacturaEncabezado = (from p in db.FacturaEncabezado
                                        where p.FacturaEncabezadoID == FacturaEncabezadoId
                                        select p).SingleOrDefault() ;

            facturaDetalle.ValorCOP = valorCOP.Replace('$', ' ').Trim();
            facturaDetalle.ValorUSD = valorUSD.Replace('$', ' ').Trim();

            totalFacturaCOP = facturaDetalle.ValorCOP;

            db.FacturaDetalle.Add(facturaDetalle);
            db.SaveChanges();

            FacturaEncabezado encabezado = new FacturaEncabezado();
            encabezado.FacturaEncabezadoID = FacturaEncabezadoId;
             ViewBag.xml  = encabezado.getTotalIVA(FacturaBLL.PESOS_COLOMBIANOS);

            return facturaDetalle.FacturaDetalleID;
         }

        public string DeleteData(int id)
        {
            try
            {
                FacturaDetalle FacturaDetalle = (from p in db.FacturaDetalle
                                                       where p.FacturaDetalleID == id
                                                       select p).SingleOrDefault();

                db.FacturaDetalle.Remove(FacturaDetalle);
                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


 
        }

        public ActionResult Create(int EncabezadoID, bool tienePrefactura)
        {
            DocumentoOperacionesBLL documentoOperacionBLL = new DocumentoOperacionesBLL();
            ValidadorUsuario vu = new ValidadorUsuario();
            FacturaEncabezado encabezado = new FacturaEncabezado();
            int PreFacturaEncabezadoID;

            if (tienePrefactura)
            {
                PreFacturaEncabezadoID = EncabezadoID;
                if (!facturaBLL.existeFactura(PreFacturaEncabezadoID))
                {
                    Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
                    facturaBLL.crearFactura(PreFacturaEncabezadoID, usuario);

                    encabezado = (from p in db.FacturaEncabezado
                                  where p.PrefacturaEncabezadoID == PreFacturaEncabezadoID
                                  select p).SingleOrDefault();

                }

            }
            else
            {
                encabezado = (from p in db.FacturaEncabezado
                              where p.FacturaEncabezadoID == EncabezadoID
                              select p).SingleOrDefault();
            }



            var conceptos = from p in db.Concepto
                            orderby p.ConceptoNm
                            select p;
            ViewBag.ConceptoID = new SelectList(conceptos, "ConceptoID", "ConceptoNm");



           
            encabezado.DocumentoOperaciones = documentoOperacionBLL.getDocumentoOperacionesByID(encabezado.DocumentoOperacionesID);
            encabezado.Usuario = vu.getUsuarioById(encabezado.UsuarioCreadorID);



            var detalles = from p in db.FacturaDetalle
                       where p.FacturaEncabezadoID == encabezado.FacturaEncabezadoID
                       orderby p.TipoDeIngreso.TipoDeIngresoID
                       select p;

            foreach (FacturaDetalle facturaDetalle in detalles)
            {
                if (facturaDetalle.TipoDeIngreso == null)
                {
                    TipoDeIngreso tipoDeIngreso = (from p in db.TipoDeIngreso
                                                   where p.TipoDeIngresoID == facturaDetalle.TipoDeIngresoID
                                                    select p).SingleOrDefault();

                    facturaDetalle.TipoDeIngreso = tipoDeIngreso;
                }
            }



            var model = new FacturaViewModel
            {

                Detalles = detalles,
                FacturaEncabezado = encabezado
            };

            return View(model);

         
            
        }

    
        //
        // POST: /PreDetalle/Create
        [HttpPost]
        public ActionResult Create(FacturaViewModel FacturaViewModel)
        {

           // ViewBag.FacturaEncabezadoID = new SelectList(db.FacturaEncabezado, "FacturaEncabezadoID", "TasaDeCambio", FacturaDetalle.FacturaEncabezadoID);
            return View(FacturaViewModel);
        }
        
        //
        // GET: /PreDetalle/Edit/5
 
        public ActionResult Edit(int id)
        {
            FacturaDetalle FacturaDetalle = db.FacturaDetalle.Find(id);
            ViewBag.FacturaEncabezadoID = new SelectList(db.FacturaEncabezado, "FacturaEncabezadoID", "TasaDeCambio", FacturaDetalle.FacturaEncabezadoID);
            return View(FacturaDetalle);
        }

        //
        // POST: /PreDetalle/Edit/5

        [HttpPost]
        public ActionResult Edit(FacturaDetalle FacturaDetalle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(FacturaDetalle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FacturaEncabezadoID = new SelectList(db.FacturaEncabezado, "FacturaEncabezadoID", "TasaDeCambio", FacturaDetalle.FacturaEncabezadoID);
            return View(FacturaDetalle);
        }

        //
        // GET: /PreDetalle/Delete/5
 
        public ActionResult Delete(int id)
        {
            FacturaDetalle FacturaDetalle = db.FacturaDetalle.Find(id);
            return View(FacturaDetalle);
        }

        //
        // POST: /PreDetalle/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            FacturaDetalle FacturaDetalle = db.FacturaDetalle.Find(id);
            db.FacturaDetalle.Remove(FacturaDetalle);
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