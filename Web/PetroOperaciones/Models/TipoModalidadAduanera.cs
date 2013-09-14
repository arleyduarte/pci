using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;

namespace PetroOperaciones.Models
{

    public class TipoModalidadAduanera 
    {


        public int TipoModalidadAduaneraID { get; set; }
        public String TipoModalidadAduaneraNm { get; set; }
    }
}