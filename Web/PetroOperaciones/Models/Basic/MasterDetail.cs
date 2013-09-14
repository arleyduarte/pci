using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace PetroOperaciones.Models.Basic
{
    public class MasterDetail
    {
        public ArrayList Details { get; set; }
        public IMasterEntity IMasterEntity  { get; set; }
    }
}