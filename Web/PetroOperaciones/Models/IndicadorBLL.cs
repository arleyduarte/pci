using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetroOperaciones.Models.Facturacion;

namespace PetroOperaciones.Models
{
    public class IndicadorBLL
    {
        private EfDbContext db = new EfDbContext();

        public int getMetaProyectadaIndicador(int tipoIndicador, int ano, int mes)
        {
            int total =0;

            var indicadores = from p in db.Indicador
                              where p.PeriodoMes == mes
                              && p.PeriodoAno == ano
                              && p.TipoDeIndicadorID == tipoIndicador
                              select p;

            foreach (Indicador indicador in indicadores)
            {
                total += indicador.MetaProyectada;
            }


            return total;
        }
    }
}