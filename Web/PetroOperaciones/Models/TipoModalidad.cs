using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;

namespace PetroOperaciones.Models
{

    public class TipoModalidad 
    {


        public int TipoModalidadID { get; set; }
        public String TipoModalidadNm { get; set; }
    }
}