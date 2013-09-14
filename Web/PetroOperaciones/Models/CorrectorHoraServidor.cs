using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PetroOperaciones.Models
{
    public class CorrectorHoraServidor
    {

        public static string NO_HORAS_AJUSTAR = ConfigurationSettings.AppSettings["NO_HORAS_AJUSTAR"];

        private static int getCantidadHorasAjustar(){

            int cantidad = Convert.ToInt32(NO_HORAS_AJUSTAR);

            return cantidad;
        }

        public static DateTime getHoraActualColombia()
        {
            DateTime hora = System.DateTime.Now;

            hora = hora.AddHours(getCantidadHorasAjustar());

            return hora;
        }
    }
}