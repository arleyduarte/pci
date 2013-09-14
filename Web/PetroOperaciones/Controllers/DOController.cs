using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models;
using System.Collections;

namespace PetroOperaciones.Controllers
{

    [Authorize]
    public class DOController : Controller
    {

        
        private EfDbContext db = new EfDbContext();

        //
        // GET: /DO/

        public ViewResult Index()
        {
            var documentooperaciones = from p in db.DocumentoOperaciones

                                       orderby p.FechaCreacion descending
                                       select p;
            
            return View(documentooperaciones.ToList());
        }

        public ViewResult GetList()
        {

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");

            var documentooperaciones = from p in db.DocumentoOperaciones
                            where p.ClienteID == null
                                       orderby p.FechaCreacion descending
                            select p;

            
            return View(documentooperaciones.ToList());
        }

        [HttpPost]
        public ActionResult GetList(DocumentoOperaciones documentooperaciones)
        {
            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", documentooperaciones.ClienteID);
            var documento = from p in db.DocumentoOperaciones
                            where p.ClienteID == documentooperaciones.ClienteID
                            orderby p.FechaCreacion descending
                                       select p;
            return View(documento.ToList());

        }

        public ViewResult GetListDate()
        {
            ViewBag.Estado = new SelectList(db.Estados, "EstadoID", "EstadoNm");

            var documento = from p in db.DocumentoOperaciones
                            where p.ClienteID == null
                            orderby p.FechaCreacion descending
                            select p;
            return View(documento.ToList());
        }

        [HttpPost]
        public ActionResult GetListDate(DocumentoOperaciones documentooperaciones)
        {
            ViewBag.Estado = new SelectList(db.Estados, "EstadoID", "EstadoNm");

            if (documentooperaciones.FechaCreacion.Year > Utilidades.FECHA_MINIMA & documentooperaciones.FechaFinalizacion != null)
            {


                DateTime fechaFinal = Convert.ToDateTime(documentooperaciones.FechaFinalizacion);
                fechaFinal = fechaFinal.AddHours(10);

                if (documentooperaciones.Estado != 0)
                {
                    var documento = from p in db.DocumentoOperaciones
                                    where
                                     p.FechaCreacion >= documentooperaciones.FechaCreacion
                                     && p.FechaCreacion <= fechaFinal
                                     && p.Estado == documentooperaciones.Estado
                                    orderby p.NumeroDO descending
                                    select p;
                    return View(documento.ToList());
                }

                else
                {
                    var documento = from p in db.DocumentoOperaciones
                                    where
                                     p.FechaCreacion >= documentooperaciones.FechaCreacion
                                     && p.FechaCreacion <= fechaFinal
                                    orderby p.NumeroDO descending
                                    select p;
                    return View(documento.ToList());

                }


            }

            else
            {
                var documento = db.DocumentoOperaciones.Include(d => d.Cliente).Include(d => d.Transportador).Include(d => d.MedioTransporte).Include(d => d.TipoContenedor).Include(d => d.Pais).Include(d => d.PuertoO).Include(d => d.PuertoD).Include(d => d.UsuarioC).Include(d => d.UsuarioR).Include(d => d.EstadoDO).Include(d => d.TipoPieza);
                return View(documento.ToList());
            }



        }


        public ViewResult GetByDO()
        {

            var documentosOperaciones = from p in db.DocumentoOperaciones
                                        where p.NumeroDO == null

                                        orderby p.FechaCreacion descending
                                        select p;

            DocumentoOperaciones filtro = new DocumentoOperaciones();

            var model = new BuscadorDOViewModel
            {
                FiltroDocumentoOperaciones = filtro,
                DocumentoOperaciones = documentosOperaciones
            };

            return View(model);
            ;
        }


        [HttpPost]
        public ActionResult GetByDO(BuscadorDOViewModel buscadorDO)
        {



            var documentosOperaciones = from p in db.DocumentoOperaciones
                                        where p.NumeroDO.Contains(buscadorDO.FiltroDocumentoOperaciones.NumeroDO)

                           orderby p.FechaCreacion descending
                           select p;



            var model = new BuscadorDOViewModel
            {
                FiltroDocumentoOperaciones = buscadorDO.FiltroDocumentoOperaciones,
                DocumentoOperaciones = documentosOperaciones
            };

            return View(model);


           
        }



        //
        // GET: /DO/Details/5

        public ViewResult Details(int id)
        {
            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(id);
            return View(documentooperaciones);
        }

        //
        // GET: /DO/Create

        public ActionResult Create()
        {
            DocumentoOperaciones documentooperaciones = new DocumentoOperaciones();
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();
            documentooperaciones.NumeroDO = directorBLL.getNumeroDocumento();


            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente");           
            ViewBag.MedioTransporteID = new SelectList(db.MedioTransportes, "MedioTransporteID", "MedioTransporteNm");
            ViewBag.TipoContenedorID = new SelectList(db.TipoContenedors.OrderBy(x => x.TipoContenedorNm), "TipoContenedorID", "TipoContenedorNm");
            ViewBag.PaisID = new SelectList(db.Paises.OrderBy(x => x.PaisNm), "PaisID", "PaisNm");
            ViewBag.PaisDestinoID = new SelectList(db.Paises.OrderBy(x => x.PaisNm), "PaisID", "PaisNm");
            ViewBag.TipoPiezaID = new SelectList(db.TipoPiezas.OrderBy(x => x.TipoPiezaNm), "TipoPiezaID", "TipoPiezaNm");
            ViewBag.TransportadorID = new SelectList(db.Transportadores.OrderBy(x => x.TransportadorNm), "TransportadorID", "TransportadorNm");
            ViewBag.PuertoOrigen = new SelectList(db.Puertos.OrderBy(x => x.PuertoNm), "PuertoID", "PuertoNm");
            ViewBag.PuertoDestino = new SelectList(db.Puertos.OrderBy(x => x.PuertoNm), "PuertoID", "PuertoNm");
            ViewBag.UsuarioResponsableID = new SelectList(db.Usuarios.OrderBy(x => x.Nombre), "UsuarioID", "Nombre");
            ViewBag.Estado = new SelectList(db.Estados, "EstadoID", "EstadoNm");
            ViewBag.TipoOperacionID = new SelectList(db.TiposOperaciones.OrderBy(x => x.TipoOperacionNm), "TipoOperacionID", "TipoOperacionNm");
            ViewBag.TipoUnidadVolumenID = new SelectList(db.TipoUnidadVolumen, "TipoUnidadVolumenID", "TipoUnidadVolumenNm");
            ViewBag.TipoMonedaID = new SelectList(db.TipoMoneda, "TipoMonedaID", "TipoMonedaNm");
            ViewBag.TipoModalidadID = new SelectList(db.TipoModalidad, "TipoModalidadID", "TipoModalidadNm");
     
            ViewBag.TipoModalidadAduaneraID = new SelectList(db.TipoModalidadAduanera.OrderBy(x => x.TipoModalidadAduaneraNm), "TipoModalidadAduaneraID", "TipoModalidadAduaneraNm");
            

            var vItems = (from p in db.Transportadores
                          where p.TipoTransportadorID == TipoTransportador.TERRESTRE
                          select p).OrderBy(x => x.TransportadorNm);
            ViewBag.TransportadorNacionalID = new SelectList(vItems, "TransportadorID", "TransportadorNm");

            var vFreightForwarder = (from p in db.Transportadores
                                     where p.TipoTransportadorID == TipoTransportador.FREIGHT_FORWARDER
                          select p).OrderBy(x => x.TransportadorNm);
            ViewBag.FreightForwarderID = new SelectList(vFreightForwarder, "TransportadorID", "TransportadorNm");



            return View(documentooperaciones);
        } 

        //
        // POST: /DO/Create

        [HttpPost]
        public ActionResult Create(DocumentoOperaciones documentooperaciones)
        {

            ValidadorUsuario vu = new ValidadorUsuario();
            Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
            documentooperaciones.UsuarioCreadorID = usuario.UsuarioID;
            documentooperaciones.FechaRegistro = CorrectorHoraServidor.getHoraActualColombia();



            if (documentooperaciones.FechaCreacion.Year < Utilidades.FECHA_MINIMA)
            {
                documentooperaciones.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();
            }
            

            if (documentooperaciones.Estado == EstadoDO.FINALIZADO)
            {
                
                documentooperaciones.FechaCierre = CorrectorHoraServidor.getHoraActualColombia();
            }

            documentooperaciones.ChequeoRealizado = false;

            if (ModelState.IsValid)
            {

                DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();

                if (directorBLL.existeDocumentoOperaciones(documentooperaciones.NumeroDO) == false)
                {
                    db.DocumentoOperaciones.Add(documentooperaciones);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else
                {
                    ViewBag.Mensaje = "Debe verificar el número del DO, pues este ya existe " + documentooperaciones.NumeroDO;
                }


            }

            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(x => x.NmCliente), "ClienteID", "NmCliente", documentooperaciones.ClienteID);
            
            ViewBag.MedioTransporteID = new SelectList(db.MedioTransportes, "MedioTransporteID", "MedioTransporteNm", documentooperaciones.MedioTransporteID);
            ViewBag.TipoContenedorID = new SelectList(db.TipoContenedors.OrderBy(x => x.TipoContenedorNm), "TipoContenedorID", "TipoContenedorNm", documentooperaciones.TipoContenedorID);
            ViewBag.TipoPiezaID = new SelectList(db.TipoPiezas, "TipoPiezaID", "TipoPiezaNm", documentooperaciones.TipoPieza);
            ViewBag.PaisID = new SelectList(db.Paises.OrderBy(x => x.PaisNm), "PaisID", "PaisNm", documentooperaciones.PaisID);
            ViewBag.PaisDestinoID = new SelectList(db.Paises.OrderBy(x => x.PaisNm), "PaisID", "PaisNm", documentooperaciones.PaisDestinoID);
            ViewBag.PuertoOrigen = new SelectList(db.Puertos.OrderBy(x => x.PuertoNm), "PuertoID", "PuertoNm", documentooperaciones.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puertos.OrderBy(x => x.PuertoNm), "PuertoID", "PuertoNm", documentooperaciones.PuertoDestino);
            ViewBag.UsuarioResponsableID = new SelectList(db.Usuarios.OrderBy(x => x.Nombre), "UsuarioID", "Nombre", documentooperaciones.UsuarioResponsableID);
            ViewBag.Estado = new SelectList(db.Estados, "EstadoID", "EstadoNm", documentooperaciones.Estado);
            ViewBag.TipoOperacionID = new SelectList(db.TiposOperaciones.OrderBy(x => x.TipoOperacionNm), "TipoOperacionID", "TipoOperacionNm");
            ViewBag.TipoUnidadVolumenID = new SelectList(db.TipoUnidadVolumen, "TipoUnidadVolumenID", "TipoUnidadVolumenNm");
            ViewBag.TipoMonedaID = new SelectList(db.TipoMoneda, "TipoMonedaID", "TipoMonedaNm", documentooperaciones.TipoMonedaID);
            ViewBag.TipoModalidadID = new SelectList(db.TipoModalidad, "TipoModalidadID", "TipoModalidadNm", documentooperaciones.TipoModalidadID);
            ViewBag.TransportadorID = new SelectList(db.Transportadores.OrderBy(x => x.TransportadorNm), "TransportadorID", "TransportadorNm", documentooperaciones.TransportadorID);
            ViewBag.TipoModalidadAduaneraID = new SelectList(db.TipoModalidadAduanera.OrderBy(x => x.TipoModalidadAduaneraNm), "TipoModalidadAduaneraID", "TipoModalidadAduaneraNm", documentooperaciones.TipoModalidadAduaneraID);
            var vItems = (from p in db.Transportadores
                          where p.TipoTransportadorID == TipoTransportador.TERRESTRE
                          select p).OrderBy(x => x.TransportadorNm);
            ViewBag.TransportadorNacionalID = new SelectList(vItems, "TransportadorID", "TransportadorNm");
            var vFreightForwarder = (from p in db.Transportadores
                                     where p.TipoTransportadorID == TipoTransportador.FREIGHT_FORWARDER
                                     select p).OrderBy(x => x.TransportadorNm);
            ViewBag.FreightForwarderID = new SelectList(vFreightForwarder, "TransportadorID", "TransportadorNm");


            //iewBag.TransportadorNacionalID = new SelectList(db.Transportadores, "TransportadorID", "TransportadorNm").Where(r => r.TipoTransportadorID == 1);
            return View(documentooperaciones);
        }
        
        //
        // GET: /DO/Edit/5
 
        public ActionResult Edit(int id)
        {
            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(id);
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "NmCliente", documentooperaciones.ClienteID);
            ViewBag.TransportadorID = new SelectList(db.Transportadores, "TransportadorID", "TransportadorNm", documentooperaciones.TransportadorID);
            ViewBag.MedioTransporteID = new SelectList(db.MedioTransportes, "MedioTransporteID", "MedioTransporteNm", documentooperaciones.MedioTransporteID);
            ViewBag.TipoContenedorID = new SelectList(db.TipoContenedors, "TipoContenedorID", "TipoContenedorNm", documentooperaciones.TipoContenedorID);
            ViewBag.PaisID = new SelectList(db.Paises, "PaisID", "PaisNm", documentooperaciones.PaisID);
            ViewBag.PaisDestinoID = new SelectList(db.Paises, "PaisID", "PaisNm", documentooperaciones.PaisDestinoID);
            ViewBag.PuertoOrigen = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoOrigen);
            ViewBag.TipoPiezaID = new SelectList(db.TipoPiezas, "TipoPiezaID", "TipoPiezaNm", documentooperaciones.TipoPiezaID);
            ViewBag.PuertoDestino = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoDestino);
            ViewBag.UsuarioCreadorID = new SelectList(db.Usuarios, "UsuarioID", "Nombre", documentooperaciones.UsuarioCreadorID);
            ViewBag.UsuarioResponsableID = new SelectList(db.Usuarios, "UsuarioID", "Nombre", documentooperaciones.UsuarioResponsableID);
            ViewBag.Estado = new SelectList(db.Estados, "EstadoID", "EstadoNm", documentooperaciones.Estado);
            ViewBag.TipoOperacionID = new SelectList(db.TiposOperaciones, "TipoOperacionID", "TipoOperacionNm", documentooperaciones.TipoOperacionID);
            ViewBag.TipoUnidadVolumenID = new SelectList(db.TipoUnidadVolumen, "TipoUnidadVolumenID", "TipoUnidadVolumenNm", documentooperaciones.TipoUnidadVolumenID);
            ViewBag.TipoMonedaID = new SelectList(db.TipoMoneda, "TipoMonedaID", "TipoMonedaNm", documentooperaciones.TipoMonedaID);
            ViewBag.TipoModalidadAduaneraID = new SelectList(db.TipoModalidadAduanera.OrderBy(x => x.TipoModalidadAduaneraNm), "TipoModalidadAduaneraID", "TipoModalidadAduaneraNm", documentooperaciones.TipoModalidadAduaneraID);
            ViewBag.TipoModalidadID = new SelectList(db.TipoModalidad, "TipoModalidadID", "TipoModalidadNm", documentooperaciones.TipoModalidadID);

            var vItems = (from p in db.Transportadores
                          where p.TipoTransportadorID == TipoTransportador.TERRESTRE
                          select p);
            ViewBag.TransportadorNacionalID = new SelectList(vItems, "TransportadorID", "TransportadorNm", documentooperaciones.TransportadorNacionalID);
            var vFreightForwarder = (from p in db.Transportadores
                                     where p.TipoTransportadorID == TipoTransportador.FREIGHT_FORWARDER
                                     select p).OrderBy(x => x.TransportadorNm);
            ViewBag.FreightForwarderID = new SelectList(vFreightForwarder, "TransportadorID", "TransportadorNm", documentooperaciones.FreightForwarderID);


            return View(documentooperaciones);
        }

        //
        // POST: /DO/Edit/5

        [HttpPost]
        public ActionResult Edit(DocumentoOperaciones documentooperaciones)
        {
            if (ModelState.IsValid)
            {

                ValidadorUsuario vu = new ValidadorUsuario();
                Usuario usuario = vu.getUsuarioByLogin(User.Identity.Name);
                documentooperaciones.UsuarioCreadorID = usuario.UsuarioID;


                //
              

                DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();
                DocumentoOperaciones doEditado = directorBLL.getDocumentoOperacionesByID(documentooperaciones.DocumentoOperacionesID);

                documentooperaciones.DocumentoOperacionesID = doEditado.DocumentoOperacionesID;

                if (documentooperaciones.FechaCreacion.Year < Utilidades.FECHA_MINIMA)
                {
                    documentooperaciones.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();
                }

                if (documentooperaciones.Estado == EstadoDO.FINALIZADO)
                {
                    documentooperaciones.FechaCierre = CorrectorHoraServidor.getHoraActualColombia();
                }
                else
                {
                    documentooperaciones.FechaCierre = doEditado.FechaCierre;
                }
                documentooperaciones.FechaRegistro = doEditado.FechaRegistro;


                db.Entry(documentooperaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "NmCliente", documentooperaciones.ClienteID);
            ViewBag.TransportadorID = new SelectList(db.Transportadores, "TransportadorID", "TransportadorNm", documentooperaciones.TransportadorID);
            ViewBag.MedioTransporteID = new SelectList(db.MedioTransportes, "MedioTransporteID", "MedioTransporteNm", documentooperaciones.MedioTransporteID);
            ViewBag.TipoContenedorID = new SelectList(db.TipoContenedors, "TipoContenedorID", "TipoContenedorNm", documentooperaciones.TipoContenedorID);
            ViewBag.TipoPiezaID = new SelectList(db.TipoPiezas, "TipoPiezaID", "TipoPiezaNm", documentooperaciones.TipoPiezaID);
            ViewBag.PaisID = new SelectList(db.Paises, "PaisID", "PaisNm", documentooperaciones.PaisID);
            ViewBag.PaisDestinoID = new SelectList(db.Paises, "PaisID", "PaisNm", documentooperaciones.PaisDestinoID);
            ViewBag.PuertoOrigen = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puertos, "PuertoID", "PuertoNm", documentooperaciones.PuertoDestino);
            ViewBag.UsuarioCreadorID = new SelectList(db.Usuarios, "UsuarioID", "Nombre", documentooperaciones.UsuarioCreadorID);
            ViewBag.UsuarioResponsableID = new SelectList(db.Usuarios, "UsuarioID", "Nombre", documentooperaciones.UsuarioResponsableID);
            ViewBag.Estado = new SelectList(db.Estados, "EstadoID", "EstadoNm", documentooperaciones.Estado);
            ViewBag.TipoOperacionID = new SelectList(db.TiposOperaciones, "TipoOperacionID", "TipoOperacionNm", documentooperaciones.TipoOperacionID);
            ViewBag.TipoUnidadVolumenID = new SelectList(db.TipoUnidadVolumen, "TipoUnidadVolumenID", "TipoUnidadVolumenNm", documentooperaciones.TipoUnidadVolumenID);
            ViewBag.TipoMonedaID = new SelectList(db.TipoMoneda, "TipoMonedaID", "TipoMonedaNm", documentooperaciones.TipoMonedaID);
            ViewBag.TipoModalidadAduaneraID = new SelectList(db.TipoModalidadAduanera.OrderBy(x => x.TipoModalidadAduaneraNm), "TipoModalidadAduaneraID", "TipoModalidadAduaneraNm", documentooperaciones.TipoModalidadAduaneraID);
            ViewBag.TipoModalidadID = new SelectList(db.TipoModalidad, "TipoModalidadID", "TipoModalidadNm", documentooperaciones.TipoModalidadID);

            var vItems = (from p in db.Transportadores
                          where p.TipoTransportadorID == TipoTransportador.TERRESTRE
                          select p);
            ViewBag.TransportadorNacionalID = new SelectList(vItems, "TransportadorID", "TransportadorNm", documentooperaciones.TransportadorNacionalID);
            var vFreightForwarder = (from p in db.Transportadores
                                     where p.TipoTransportadorID == TipoTransportador.FREIGHT_FORWARDER
                                     select p).OrderBy(x => x.TransportadorNm);
            ViewBag.FreightForwarderID = new SelectList(vFreightForwarder, "TransportadorID", "TransportadorNm", documentooperaciones.FreightForwarderID);


            return View(documentooperaciones);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetListPuertos(int medioTransporteID)
        {

            if (medioTransporteID == MedioTransporte.AEREO)
            {
                var vPuertos = (from p in db.Puertos
                                where p.Tipo == Puerto.AEROPUERTO
                                select p);

                return Json(vPuertos, JsonRequestBehavior.AllowGet);
            }
            else if (medioTransporteID == MedioTransporte.MARITIMO)
            {
                var vPuertos = (from p in db.Puertos
                                where p.Tipo == Puerto.PUERTO
                                select p);

                return Json(vPuertos, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var vPuertos = (from p in db.Puertos                               
                                select p);

                return Json(vPuertos, JsonRequestBehavior.AllowGet);
            }


            
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetListTransportadores(int medioTransporteID)
        {

            ArrayList list = new ArrayList();

            if (medioTransporteID == MedioTransporte.AEREO)
            {
                var vTransportadores = (from p in db.Transportadores
                                where p.TipoTransportadorID == TipoTransportador.AERERO
                                        select p).OrderBy(x => x.TransportadorNm);

                foreach (Transportador tranportador in vTransportadores)
                {
                    list.Add(new ListItem(tranportador.TransportadorID, tranportador.TransportadorNm));

                }

                
            }
            else if (medioTransporteID == MedioTransporte.MARITIMO)
            {
                var vTransportadores = (from p in db.Transportadores
                                        where p.TipoTransportadorID == TipoTransportador.MARITIMO
                                        select p).OrderBy(x => x.TransportadorNm);

                foreach (Transportador tranportador in vTransportadores)
                {
                    list.Add(new ListItem(tranportador.TransportadorID, tranportador.TransportadorNm));

                }
            }
            else
            {
                var vTransportadores = (from p in db.Transportadores

                                        select p);

                foreach (Transportador tranportador in vTransportadores)
                {
                    list.Add(new ListItem(tranportador.TransportadorID, tranportador.TransportadorNm));

                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);



        }

        //
        // GET: /DO/Delete/5
 
        public ActionResult Delete(int id)
        {
            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(id);
            return View(documentooperaciones);
        }

        //
        // POST: /DO/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(id);
            db.DocumentoOperaciones.Remove(documentooperaciones);
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