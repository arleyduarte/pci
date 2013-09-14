using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using PetroOperaciones.Models;
using PetroOperaciones.Models.Facturacion;

namespace PetroOperaciones.Controllers.Facturacion
{
    public class FacturacionServicesController : Controller
    {


        private EfDbContext db = new EfDbContext();

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetClienteById(int ClienteId)
        {

            ArrayList list = new ArrayList();
            Cliente var = db.Clientes.Find(ClienteId);
            Cliente cliente = new Cliente();
            cliente.ClienteID = var.ClienteID;
            cliente.NIT = var.NIT;
            cliente.NmCliente = var.NmCliente;
            cliente.Direccion = var.Direccion;
            cliente.Telefono1 = var.Telefono1;
            cliente.Telefono2 = var.Telefono2;
            cliente.Pais = var.Pais;


            if (cliente != null)
            {
                list.Add(cliente);
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetClienteByDO(int documentoOperacionesId)
        {

            ArrayList list = new ArrayList();
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();
            Cliente cliente = directorBLL.getClienteByDO(documentoOperacionesId);

            if (cliente != null)
            {
                list.Add(cliente);
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult AceptarFactura(int FacturaEncabezadoId)
        {
            ArrayList list = new ArrayList();
            FacturaBLL facturaBLL = new FacturaBLL();            

            list.Add(new ListItem(facturaBLL.cambiarEstadoFactura(FacturaEncabezadoId, EstadoFactura.APROBADA), "OK"));
            return Json(list, JsonRequestBehavior.AllowGet);            
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult AnularFactura(int FacturaEncabezadoId)
        {
            ArrayList list = new ArrayList();
            FacturaBLL facturaBLL = new FacturaBLL();

            list.Add(new ListItem(facturaBLL.cambiarEstadoFactura(FacturaEncabezadoId, EstadoFactura.ANULADA), "OK"));
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetContactosByDO(int documentoOperacionesId)
        {
            ArrayList list = new ArrayList();

            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();
            Cliente cliente = directorBLL.getClienteByDO(documentoOperacionesId);

            if (cliente != null)
            {
                var contactos = from p in db.Contactos
                                where p.TerceroID == cliente.ClienteID
                                select p;

                foreach (Contacto contacto in contactos)
                {
                    Contacto aux = new Contacto();
                    aux.ContactoNm = contacto.ContactoNm;
                    aux.ContactoID = contacto.ContactoID;
                    list.Add(aux);
                }
                
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetContactosByCliente(int ClienteId)
        {
            ArrayList list = new ArrayList();

            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();
            Cliente cliente = db.Clientes.Find(ClienteId);

            if (cliente != null)
            {
                var contactos = from p in db.Contactos
                                where p.TerceroID == cliente.ClienteID
                                select p;

                foreach (Contacto contacto in contactos)
                {
                    Contacto aux = new Contacto();
                    aux.ContactoNm = contacto.ContactoNm;
                    aux.ContactoID = contacto.ContactoID;
                    list.Add(aux);
                }

            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDocumentoOperacionesById(int documentoOperacionesId)
        {
            ArrayList list = new ArrayList();
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();
            DocumentoOperaciones documentoOperaciones = directorBLL.getDOSerializableByID(documentoOperacionesId);
            list.Add(documentoOperaciones);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetTotalesFacturaById(int facturaEncabezadoID)
        {
            ArrayList list = new ArrayList();
            TotalesFacturaModel totalesFactura = new TotalesFacturaModel(facturaEncabezadoID);
            list.Add(totalesFactura);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetNotasFacturacionByDocumentoOperacionesId(int documentoOperacionesId)
        {
            SeguimientosBLL seguimientoBLL = new SeguimientosBLL();
            ArrayList list = seguimientoBLL.getNotasFacturacion(documentoOperacionesId);
            
            String JEditableObject = "{";
            
            Seguimiento notaFacturacion =  new Seguimiento();
            foreach (Seguimiento item in list)
            {
                notaFacturacion.Observaciones = item.Observaciones+" , "+notaFacturacion.Observaciones;
            }
            
            ArrayList vSeguimiento = new ArrayList();
            vSeguimiento.Add(notaFacturacion);
            return Json(notaFacturacion, JsonRequestBehavior.AllowGet);
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
