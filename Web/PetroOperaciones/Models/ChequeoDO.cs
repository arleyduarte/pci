using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetroOperaciones.Models
{
    public class ChequeoDO
    {
        public int ChequeoDOID { get; set; }
        public int UsuarioID { get; set; }
        public int DocumentoOperacionesID { get; set; }
        public bool Cotizacion { get; set; }
        public bool Invoice { get; set; }
        public bool ListaEmpaque { get; set; }
        public bool CertificadoOrigen { get; set; }
        public bool CertificadoVistos { get; set; }
        public bool Licencias { get; set; }
        public bool PolizaCumplimiento { get; set; }
        public bool SeguroMercancia { get; set; }
        public bool InstructivoDocumento { get; set; }
        public bool BL { get; set; }
        public bool HBL { get; set; }
        public bool DocumentosRadicadosMuisca { get; set; }
        public bool FacturaAgenteExterior { get; set; }
        public bool FacturaManejo { get; set; }
        public bool SolicitudAnticipo { get; set; }
        public bool CertificacionFletes { get; set; }
        public bool RemisionDocumentoAduana { get; set; }
        public bool OrdenServicioTransporte { get; set; }
        public bool RegistroFotograficoOrigen { get; set; }
        public bool RegistroFotograficoDestino { get; set; }
        public bool FormatoLLenanoContenedor { get; set; }
        public bool RegistroFotograficoEntregado { get; set; }
        public String Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual DocumentoOperaciones DocumentoOperaciones { get; set; }

    }
}