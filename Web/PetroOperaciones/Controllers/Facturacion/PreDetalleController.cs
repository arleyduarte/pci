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
using System.IO;
using PetroOperaciones.Controllers.Facturacion.invoiceToPDF.tools;
using invoiceToPDF;

namespace PetroOperaciones.Controllers.Facturacion
{
    [Authorize]
    public class PreDetalleController : Controller
    {
        private EfDbContext db = new EfDbContext();


        public InvoicePDFResult GetInvoicePDFFile(int prefacturaEncabezadoId, String tipoMoneda)
        {
            FacturaBLL facturaBLL = new FacturaBLL();

            String tmpInvoicesPath = Path.Combine(Server.MapPath("~/Content/invoices/invoicesTmp/"));
            String templateInvoicesPath = Path.Combine(Server.MapPath("~/Content/invoices/invoicesTemplates/"));


            FileManager fileManager = new FileManager();
            fileManager.deletePDFFiles(tmpInvoicesPath);
            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            FacturaModel facturaModel = facturaBLL.getFalsaFactura(prefacturaEncabezadoId,usuario);
            InvoiceToPDF fc = new InvoiceToPDF(templateInvoicesPath, tmpInvoicesPath, tipoMoneda);
            String invoiceFileName = fc.makePreInvoiceToPdf(facturaModel);
            String finalInvoicesPath = "~/Content/invoices/invoicesTmp/" + invoiceFileName;

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

            PrefacturaEncabezado encabezado = new PrefacturaEncabezado();
            encabezado.PrefacturaEncabezadoID = 5;


            var detalles = from p in db.PrefacturaDetalle
                           where p.PrefacturaEncabezadoID == encabezado.PrefacturaEncabezadoID
                           orderby p.TipoDeIngresoID descending
                           select p;

            //var model = new PrefacturaViewModel

            var model = new PrefacturaViewModel
            {
                // TODO: fetch from the repository instead of hardcoding
                Detalles = detalles,
                PrefacturaEncabezado = encabezado
            };



            return View(model);
        }

        //
        // GET: /PreDetalle/Details/5

        public ViewResult Details(int id)
        {
            PrefacturaDetalle prefacturadetalle = db.PrefacturaDetalle.Find(id);
            return View(prefacturadetalle);
        }

        //, string valorUSD, string valorCOP, string detalles, string soporte
        // GET: /PreDetalle/Create

        public int AddData(int tipoIngreso, int ConceptoID, String moneda, String valorUSD, String valorCOP, String detalles, String soporte, int prefacturaEncabezadoId)
        {

            PrefacturaDetalle prefacturaDetalle = new PrefacturaDetalle();
            prefacturaDetalle.PrefacturaEncabezadoID = prefacturaEncabezadoId;
            prefacturaDetalle.TipoDeIngresoID = tipoIngreso;
            prefacturaDetalle.Detalles = detalles.Trim();
            prefacturaDetalle.FacturaSoporte = soporte;

            var concepto  = (from p in db.Concepto
                            where p.ConceptoID == ConceptoID
                            select p).SingleOrDefault();
            prefacturaDetalle.ConceptoID = ConceptoID;
            prefacturaDetalle.ConceptoNm = concepto.ConceptoNm;

            var prefacturaEncabezado = (from p in db.PrefacturaEncabezado
                                        where p.PrefacturaEncabezadoID == prefacturaEncabezadoId
                                        select p).SingleOrDefault() ;

            prefacturaDetalle.ValorCOP = valorCOP.Replace('$', ' ').Trim();
            prefacturaDetalle.ValorUSD = valorUSD.Replace('$', ' ').Trim();

            db.PrefacturaDetalle.Add(prefacturaDetalle);
            db.SaveChanges();
            return prefacturaDetalle.PrefacturaDetalleID;
         }

        public string DeleteData(int id)
        {
            try
            {
                PrefacturaDetalle prefacturaDetalle = (from p in db.PrefacturaDetalle
                                                       where p.PrefacturaDetalleID == id
                                                       select p).SingleOrDefault();

                db.PrefacturaDetalle.Remove(prefacturaDetalle);
                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


 
        }

        public ActionResult Create(int PrefacturaEncabezadoID)
        {

            var conceptos = from p in db.Concepto
                            orderby p.ConceptoNm
                            select p;

            ViewBag.ConceptoID = new SelectList(conceptos, "ConceptoID", "ConceptoNm");



            PrefacturaEncabezado encabezado = db.PrefacturaEncabezado.Find(PrefacturaEncabezadoID);


            var detalles = from p in db.PrefacturaDetalle
                           where p.PrefacturaEncabezadoID == PrefacturaEncabezadoID
                           orderby p.TipoDeIngresoID
                           select p;

            var model = new PrefacturaViewModel
            {
                // TODO: fetch from the repository instead of hardcoding
                Detalles = detalles,
                PrefacturaEncabezado = encabezado
            };



            return View(model);

         
            
        } 

        //
        // POST: /PreDetalle/Create

        [HttpPost]
        public ActionResult Create(PrefacturaDetalle prefacturadetalle)
        {
            if (ModelState.IsValid)
            {
                db.PrefacturaDetalle.Add(prefacturadetalle);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PrefacturaEncabezadoID = new SelectList(db.PrefacturaEncabezado, "PrefacturaEncabezadoID", "TasaDeCambio", prefacturadetalle.PrefacturaEncabezadoID);
            return View(prefacturadetalle);
        }
        
        //
        // GET: /PreDetalle/Edit/5
 
        public ActionResult Edit(int id)
        {
            PrefacturaDetalle prefacturadetalle = db.PrefacturaDetalle.Find(id);
            ViewBag.PrefacturaEncabezadoID = new SelectList(db.PrefacturaEncabezado, "PrefacturaEncabezadoID", "TasaDeCambio", prefacturadetalle.PrefacturaEncabezadoID);
            return View(prefacturadetalle);
        }

        //
        // POST: /PreDetalle/Edit/5

        [HttpPost]
        public ActionResult Edit(PrefacturaDetalle prefacturadetalle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prefacturadetalle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PrefacturaEncabezadoID = new SelectList(db.PrefacturaEncabezado, "PrefacturaEncabezadoID", "TasaDeCambio", prefacturadetalle.PrefacturaEncabezadoID);
            return View(prefacturadetalle);
        }

        //
        // GET: /PreDetalle/Delete/5
 
        public ActionResult Delete(int id)
        {
            PrefacturaDetalle prefacturadetalle = db.PrefacturaDetalle.Find(id);
            return View(prefacturadetalle);
        }

        //
        // POST: /PreDetalle/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            PrefacturaDetalle prefacturadetalle = db.PrefacturaDetalle.Find(id);
            db.PrefacturaDetalle.Remove(prefacturadetalle);
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