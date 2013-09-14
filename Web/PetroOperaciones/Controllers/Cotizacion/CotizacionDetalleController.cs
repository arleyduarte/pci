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
using PetroOperaciones.Models.Cotizacion;
using PetroOperaciones.Controllers.Facturacion;
using PetroOperaciones.Models.Basic;
using QuoteToPDF;

namespace PetroOperaciones.Controllers.Cotizacion
{
    [Authorize]
    public class CotizacionDetalleController : Controller
    {
        private EfDbContext db = new EfDbContext();

        public InvoicePDFResult GetQuotePDFFile(int CotizacionEncabezadoID, String tipoMoneda)
        {
            String tmpFilesPath = Path.Combine(Server.MapPath("~/Content/invoices/invoicesTmp/"));
            String templatePath = Path.Combine(Server.MapPath("~/Content/invoices/invoicesTemplates/"));


            FileManager fileManager = new FileManager();
            fileManager.deletePDFFiles(tmpFilesPath);

            CotizacionBLL directorBLL = new CotizacionBLL();

            MasterDetail masterDetail = directorBLL.getQuote(CotizacionEncabezadoID);

            QuoteToPDFGenerator fc = new QuoteToPDFGenerator(templatePath, tmpFilesPath, tipoMoneda);
            String invoiceFileName = fc.makeQuoteToPdf(masterDetail);

            String finalInvoicesPath = "~/Content/invoices/invoicesTmp/" + invoiceFileName;

            return new InvoicePDFResult
            {
                FileName = invoiceFileName,
                Path = finalInvoicesPath
            };
        }

        //, string valorUSD, string valorCOP, string detalles, string soporte
        // GET: /PreDetalle/Create

        public int AddData(int tipoConcepto, int ConceptoID, String moneda, String valorUSD, String valorCOP, String detalles, String soporte, int CotizacionEncabezadoID)
        {

            CotizacionDetalle cotizacionDetalle = new CotizacionDetalle();
            cotizacionDetalle.CotizacionEncabezadoID = CotizacionEncabezadoID;
            cotizacionDetalle.TipoConceptoCotizacionID = tipoConcepto;
            cotizacionDetalle.Detalles = detalles.Trim();
            cotizacionDetalle.Unidades = soporte;

            var concepto  = (from p in db.Concepto
                            where p.ConceptoID == ConceptoID
                            select p).SingleOrDefault();
            cotizacionDetalle.ConceptoID = ConceptoID;
            cotizacionDetalle.ConceptoNm = concepto.ConceptoNm;



            cotizacionDetalle.ValorCOP = valorCOP.Replace('$', ' ').Trim();
            cotizacionDetalle.ValorUSD = valorUSD.Replace('$', ' ').Trim();



            db.CotizacionDetalle.Add(cotizacionDetalle);
            db.SaveChanges();



            return cotizacionDetalle.CotizacionDetalleID;
         }

        public string DeleteData(int id)
        {
            try
            {
                CotizacionDetalle cotizacionDetalle = (from p in db.CotizacionDetalle
                                                       where p.CotizacionDetalleID == id
                                                       select p).SingleOrDefault();

                db.CotizacionDetalle.Remove(cotizacionDetalle);
                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


 
        }


        public ActionResult Create(int CotizacionEncabezadoID)
        {

            var conceptos = from p in db.Concepto
                            orderby p.ConceptoNm
                            select p;

            ViewBag.ConceptoID = new SelectList(conceptos, "ConceptoID", "ConceptoNm");

            CotizacionEncabezado encabezado = db.CotizacionEncabezado.Find(CotizacionEncabezadoID);
            var detalles = from p in db.CotizacionDetalle
                           where p.CotizacionEncabezadoID == CotizacionEncabezadoID
                           orderby p.TipoConceptoCotizacionID
                           select p;

            var model = new CotizacionViewModel
            {
                Detalles = detalles,
                CotizacionEncabezado = encabezado
            };



            return View(model);

        } 

    }
}