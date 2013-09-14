using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class EstadoFactura
    {
        //no entra en la contabilidad ni suma consecutivo
        public static int CREADA = 0;
        public static int APROBADA = 1;
        //no entra en la contabilidad  suma consecutivo
        public static int ANULADA = 2;

        [Key]
        public int EstadoID { get; set; }
        public String EstadoNm { get; set; }
        public String EstadoDesc { get; set; }
    }
}