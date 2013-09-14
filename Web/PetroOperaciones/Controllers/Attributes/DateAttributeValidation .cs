using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Controllers.Attributes
{
    public class DateAttributeValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            DateTime result;

            if (DateTime.TryParse(value.ToString(), out result))
            {
                return true;
            }
            return false;
        }
    }
}