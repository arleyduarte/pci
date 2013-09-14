using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace PetroOperaciones.Models.Facturacion
{
    public class ConfiguracionEmpresaBLL
    {
        public static int PRETROCARGO_CONFIGURACION = 1;
        private EfDbContext db = new EfDbContext();

        public  ConfiguracionEmpresa getConfiguracionEmpresa(int id)
        {
            

            ConfiguracionEmpresa item = (from p in db.ConfiguracionEmpresa
                                         where p.ConfiguracionEmpresaID == id
                                         select p).SingleOrDefault();




            return item;
        }


        public  int getConsecutivoFactura(int idConfiguracionEmpresa){
            ConfiguracionEmpresa config = getConfiguracionEmpresa(idConfiguracionEmpresa);

            return config.UltimaFactura + 1;

        }

        public void actualizarConsecutivoFactura(int consecutivoFacturaActual)
        {
            ConfiguracionEmpresa item = (from p in db.ConfiguracionEmpresa
                                         where p.ConfiguracionEmpresaID == PRETROCARGO_CONFIGURACION
                                         select p).SingleOrDefault();

            item.UltimaFactura = consecutivoFacturaActual;

            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();

        }

    }
}