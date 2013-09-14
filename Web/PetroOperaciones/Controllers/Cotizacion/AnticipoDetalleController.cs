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

namespace PetroOperaciones.Controllers.Anticipo
{
    [Authorize]
    public class AnticipoDetalleController : Controller
    {
        private EfDbContext db = new EfDbContext();



        //, string valorUSD, string valorCOP, string detalles, string soporte
        // GET: /PreDetalle/Create

        public int AddData(int tipoConcepto, int ConceptoID, String moneda, String valorUSD, String valorCOP, String detalles, String soporte, int AnticipoEncabezadoID)
        {

            AnticipoDetalle anticipoDetalle = new AnticipoDetalle();
            anticipoDetalle.AnticipoEncabezadoID = AnticipoEncabezadoID;
            anticipoDetalle.TipoConceptoCotizacionID = tipoConcepto;
            anticipoDetalle.Detalles = detalles.Trim();


            var concepto  = (from p in db.Concepto
                            where p.ConceptoID == ConceptoID
                            select p).SingleOrDefault();
            anticipoDetalle.ConceptoID = ConceptoID;
            anticipoDetalle.ConceptoNm = concepto.ConceptoNm;



            anticipoDetalle.ValorCOP = valorCOP.Replace('$', ' ').Trim();
            anticipoDetalle.ValorUSD = valorUSD.Replace('$', ' ').Trim();



            db.AnticipoDetalle.Add(anticipoDetalle);
            db.SaveChanges();



            return anticipoDetalle.AnticipoEncabezadoID;
         }

        public string DeleteData(int id)
        {
            try
            {
                AnticipoDetalle detalle = (from p in db.AnticipoDetalle
                                                       where p.AnticipoDetalleID == id
                                                       select p).SingleOrDefault();

                db.AnticipoDetalle.Remove(detalle);
                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


 
        }


        public ActionResult Create(int AnticipoEncabezadoID)
        {

            var conceptos = from p in db.Concepto
                            orderby p.ConceptoNm
                            select p;

            ViewBag.ConceptoID = new SelectList(conceptos, "ConceptoID", "ConceptoNm");

            AnticipoEncabezado encabezado = db.AnticipoEncabezado.Find(AnticipoEncabezadoID);
            var detalles = from p in db.AnticipoDetalle
                           where p.AnticipoEncabezadoID == AnticipoEncabezadoID
                           orderby p.TipoConceptoCotizacionID
                           select p;

            var model = new AnticipoViewModel
            {
                Detalles = detalles,
                AnticipoEncabezado = encabezado
            };



            return View(model);

        } 

    }
}