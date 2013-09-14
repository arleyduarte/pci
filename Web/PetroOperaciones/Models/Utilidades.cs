using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;

namespace PetroOperaciones.Models
{
    public class Utilidades
    {
        public static int ANO_INICIAL = 2012;


        public static int FECHA_MINIMA = 1990;

        public void agregarCategoriaMeses(StringBuilder xmlData)
        {

            xmlData.Append("<categories>");
            foreach (string mes in Utilidades.getMesesEjecutados())
            {
                xmlData.Append("<category label='" + mes + "'/>");
            }
            xmlData.Append("</categories>");
        }


        public static ArrayList getMesesEjecutados()
        {
            ArrayList meses = new ArrayList();

            int nMes = System.DateTime.Now.Month;

            for (int mes = 1; mes <= nMes; mes++)
            {
                meses.Add(getNombreMes(mes));
            }

            return meses;
        }

        public static ArrayList getAnosDisponibles()
        {
            ArrayList anosDisponibles = new ArrayList();

            for (int i = ANO_INICIAL; i <= System.DateTime.Now.Year; i++)
            {
                anosDisponibles.Add(new ListItem(i, i.ToString()));
            }

            return anosDisponibles;
        }

        public static ArrayList getMesesEjecutados(int year)
        {
            ArrayList meses = new ArrayList();

            int actualYear = System.DateTime.Now.Year;

            int nMes = 12;
            if (year == actualYear)
            {
                nMes = System.DateTime.Now.Month;
            }


            for (int mes = 1; mes <= nMes; mes++)
            {
                meses.Add(getNombreMes(mes));
            }

            return meses;
        }

        public static ArrayList getMeses()
        {
            ArrayList meses = new ArrayList();

            int actualYear = System.DateTime.Now.Year;

            for (int mes = 1; mes <= 12; mes++)
            {
                meses.Add(new ListItem(mes, getNombreMes(mes)));

            }

            return meses;
        }


       public static string getNombreMes(int MES)
        {
            switch (MES)
            {
                case 1: return "Enero";
                case 2: return "Febrero";
                case 3: return "Marzo";
                case 4: return "Abril";
                case 5: return "Mayo";
                case 6: return "Junio";
                case 7: return "Julio";
                case 8: return "Agosto";
                case 9: return "Septiembre";
                case 10: return "Octubre";
                case 11: return "Noviembre";
                case 12: return "Diciembre";
                default: return "";

            }
        }

       public static String calcularPorcentaje(int parte, int total)
       {
           if (total != 0)
           {
               decimal porcentaje = Convert.ToDecimal(Convert.ToDecimal(parte) / Convert.ToDecimal(total))*100;
               porcentaje = Math.Round(porcentaje, 2);
               return porcentaje.ToString();
           }

           return "0";
           
       }

       public static int getNoMes(String mes)
       {
           if(mes=="Enero"){
               return 1;
           }

           else if (mes == "Febrero")
           {
               return 2;
           }


           else if (mes == "Marzo")
           {
               return 3;
           }

           else if (mes == "Abril")
           {
               return 4;
           }

           else if (mes == "Mayo")
           {
               return 5;
           }

           else if (mes == "Junio")
           {
               return 6;
           }

           else if (mes == "Julio")
           {
               return 7;
           }

           else if (mes == "Agosto")
           {
               return 8;
           }

           else if (mes == "Septiembre")
           {
               return 9;
           }

           else if (mes == "Octubre")
           {
               return 10;
           }

           else if (mes == "Noviembre")
           {
               return 11;
           }

           else if (mes == "Diciembre")
           {
               return 12;
           }

           else
           {
               return 0;
           }


       }
    }
}