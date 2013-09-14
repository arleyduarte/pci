using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetroOperaciones.Models;

namespace PetroOperaciones.Controllers.PCI.Clientes
{
    [Authorize]
    public class PCIClientesDOController : Controller
    {

        private EfDbContext db = new EfDbContext();


        public ViewResult Index()
        {
            int clienteId = GetClienteId();
            var documentooperaciones = from p in db.DocumentoOperaciones
                                       where p.ClienteID == clienteId
                                       orderby p.FechaCreacion descending
                                       select p;

            return View(documentooperaciones.ToList());
        }
        public ViewResult GetListDate()
        {
            ViewBag.Estado = new SelectList(db.Estados, "EstadoID", "EstadoNm");

            var documento = from p in db.DocumentoOperaciones
                            where p.DocumentoOperacionesID == null
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
                int clienteId = GetClienteId();


                if (documentooperaciones.Estado != 0)
                {
                    var documento = from p in db.DocumentoOperaciones
                                    where
                                     p.ClienteID == clienteId
                                     && p.FechaCreacion >= documentooperaciones.FechaCreacion
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
                                     p.ClienteID == clienteId
                                     && p.FechaCreacion >= documentooperaciones.FechaCreacion
                                     && p.FechaCreacion <= fechaFinal

                                    orderby p.NumeroDO descending
                                    select p;
                    return View(documento.ToList());

                }


            }

            else
            {
                var documento = from p in db.DocumentoOperaciones
                                where p.DocumentoOperacionesID == null
                                select p;
                return View(documento.ToList());
            }



        }


        public ViewResult Details(int id)
        {
            int clienteId = GetClienteId();
            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(id);
            if (documentooperaciones.ClienteID != clienteId)
            {
                //Se elimina el documento de operaciones si el Do no pertenece al cliente logeado
                documentooperaciones = null;
            }
            return View(documentooperaciones);
        }



        public ActionResult Seguimientos(int DOID)
        {
            var seguimientos = from p in db.Seguimiento
                               where p.DocumentoOperacionesID == DOID
                               && p.VisibleCliente == true
                               orderby p.SeguimientoID descending
                               select p;


            DocumentoOperaciones documentooperaciones = db.DocumentoOperaciones.Find(DOID);
            ViewBag.NumeroDO = documentooperaciones.NumeroDO;

            if (!PerteneceAlCliente(DOID))
            {
                seguimientos = null;
            }

            return View(seguimientos.ToList());
        }


        public ActionResult ArchivoDocumentos(int DOID)
        {
            var archivos = from p in db.ArchivoDocumento
                           where p.DocumentoOperacionesID == DOID
                           && p.VisibleCliente == true
                           orderby p.TipoDocumento.TipoDocumentoNm
                           select p;

            if (!PerteneceAlCliente(DOID))
            {
                archivos = null;
            }

            return View(archivos.ToList());
        }

        private int GetClienteId()
        {
            ValidadorUsuario vu = new ValidadorUsuario();
            return vu.GetClienteByLogin(User.Identity.Name).ClienteID;
        }

        private bool PerteneceAlCliente(int DOID)
        {
            if (db.DocumentoOperaciones.Find(DOID).ClienteID == GetClienteId())
            {
                return true;
            }

            else
            {
                return false;
            }
        }




    }
}
