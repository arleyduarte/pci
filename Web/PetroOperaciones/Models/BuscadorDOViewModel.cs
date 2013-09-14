using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetroOperaciones.Models
{
    public class BuscadorDOViewModel
    {
        public DocumentoOperaciones FiltroDocumentoOperaciones { get; set; }
        public IEnumerable<DocumentoOperaciones> DocumentoOperaciones { get; set; }
    }
}