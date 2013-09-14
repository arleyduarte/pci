using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace PetroOperaciones.Models
{
    public class SeguimientosBLL
    {
        private EfDbContext db = new EfDbContext();

        public  int getNoSegumientosRegistrados(int mes)
        {
            

            var seguimientos = (from p in db.Seguimiento
                                where p.FechaRegistro.Month == mes
                                select p).Count();




            return Convert.ToInt32(seguimientos);
        }

        public int getNoSegumientosRegistrados(int mes, int ano)
        {


            var seguimientos = (from p in db.Seguimiento
                                where p.FechaRegistro.Month == mes
                                && p.FechaRegistro.Year == ano
                                select p).Count();




            return Convert.ToInt32(seguimientos);
        }


        public int getNoSegumientosRegistrados(int mes, int ano, int usuarioId)
        {


            var seguimientos = (from p in db.Seguimiento
                                where p.FechaRegistro.Month == mes
                                && p.FechaRegistro.Year == ano
                                 && p.UsuarioID== usuarioId
                                select p).Count();




            return Convert.ToInt32(seguimientos);
        }




       public ArrayList getNotasFacturacion(int documentoOperacionesId)
        {
            var seguimientos = (from p in db.Seguimiento
                                where p.DocumentoOperacionesID == documentoOperacionesId
                                && p.AnotacionFacturacion == true
                                select p);

            ArrayList vSeguimientos = new ArrayList();

            foreach (Seguimiento seguimiento in seguimientos)
            {
                String nota = seguimiento.Observaciones;

                if (seguimiento.Observaciones.Length != 0)
                {
                    vSeguimientos.Add(seguimiento);
                }
                
            }

            return vSeguimientos;
        }
    }
}