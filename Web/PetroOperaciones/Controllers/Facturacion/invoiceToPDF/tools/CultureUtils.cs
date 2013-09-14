using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace invoiceToPDF
{
    public class CultureUtils
    {
        public static String getDecimalWithOutCents(decimal value)
        {
            String rsValue = "";

            rsValue = value.ToString("N", new CultureInfo("es-ES"));

            if (rsValue.Contains(","))
            {
                rsValue = rsValue.Substring(0, rsValue.Length - 3);
            }

            return rsValue;
        }

        public static int getIntegerWithOutMilesSeparator(String value)
        {
            int rsValue=0;


            if (value.Contains("."))
            {
                value = value.Replace(".", String.Empty);
                
            }

            try
            {
                rsValue = Convert.ToInt32(value);
            }
            catch
            {
            }

            return rsValue;
        }

        public static decimal getDecimalFromString(String valorOriginal)
        {
           decimal valor = 0;
           if (valorOriginal != null)
           {
               try
               {
                   valor = Decimal.Parse(valorOriginal, new CultureInfo("es-ES"));

               }
               catch (OverflowException e)
               {
               }
           }


            return valor;
        }

        public static String getDecimal(decimal value)
        {
            String rsValue = "";

            rsValue = value.ToString("N", new CultureInfo("es-ES"));


            return rsValue;
        }

            
    }
}
