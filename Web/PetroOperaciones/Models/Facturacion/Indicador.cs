using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models.Facturacion
{
    public class Indicador
    {
        public int IndicadorID { get; set; }
        public int TipoDeIndicadorID { get; set; }
        [Required(ErrorMessage = "*")]
        public int MetaProyectada { get; set; }

        public int Cumplimiento { get; set; }
        [Required(ErrorMessage = "*")]        
        public int PeriodoMes { get; set; }
        [Required(ErrorMessage = "*")]
        public int PeriodoAno { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual TipoDeIndicador TipoDeIndicador { get; set; }

        public bool existeIndicador()
        {
            EfDbContext db = new EfDbContext();
            var aux = from p in db.Indicador
                      where p.TipoDeIndicadorID == this.TipoDeIndicadorID
                      && p.PeriodoAno == this.PeriodoAno
                      && p.PeriodoMes == this.PeriodoMes
                      select p;

            foreach (Indicador indicador in aux)
            {
                return true;
            }

            return false;
        }

    }
}