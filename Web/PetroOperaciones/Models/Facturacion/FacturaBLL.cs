using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Collections;
using System.Data;
using invoiceToPDF;

namespace PetroOperaciones.Models.Facturacion
{
    public class FacturaBLL
    {
        public static String PESOS_COLOMBIANOS = "COP";
        public static String DOLARES_AMERICANOS = "USD";
        public static String PESOSDOLARES_COLOMBIANOS = "COPUSD";
        public static int TODOS_LOS_CLIENTES = 0;
        private EfDbContext db = new EfDbContext();


        public FacturaEncabezado getFacturaEncabezadoByPrefactura(int PreFacturaEncabezadoID)
        {

            FacturaEncabezado encabezado = new FacturaEncabezado();

            var encabezados = (from p in db.FacturaEncabezado
                          where p.PrefacturaEncabezadoID == PreFacturaEncabezadoID
                          select p);

            foreach (FacturaEncabezado auxEncabezado in encabezados)
            {
                encabezado = auxEncabezado;
            }

            if (encabezado.Cliente == null)
            {
                encabezado.Cliente = (from p in db.Clientes
                                    where p.ClienteID == encabezado.ClienteID
                                   select p).SingleOrDefault();

            }

            if (encabezado.DocumentoOperaciones == null)
            {
                encabezado.DocumentoOperaciones = (from p in db.DocumentoOperaciones
                                      where p.DocumentoOperacionesID == encabezado.DocumentoOperacionesID
                                      select p).SingleOrDefault();

            }


            return encabezado;

            
        }

        public FacturaEncabezado getFacturaEncabezadoByID(int FacturaEncabezadoID)
        {

            FacturaEncabezado encabezado = new FacturaEncabezado();

            var encabezados = (from p in db.FacturaEncabezado
                               where p.FacturaEncabezadoID == FacturaEncabezadoID
                               select p);

            foreach (FacturaEncabezado auxEncabezado in encabezados)
            {
                encabezado = auxEncabezado;
            }

            if (encabezado.Cliente == null)
            {
                encabezado.Cliente = (from p in db.Clientes
                                      where p.ClienteID == encabezado.ClienteID
                                      select p).SingleOrDefault();

            }

            if (encabezado.DocumentoOperaciones == null)
            {
                encabezado.DocumentoOperaciones = (from p in db.DocumentoOperaciones
                                                   where p.DocumentoOperacionesID == encabezado.DocumentoOperacionesID
                                                   select p).SingleOrDefault();
            }


            return encabezado;


        }



        public bool existeFactura(int PreFacturaEncabezadoID)
        {


            FacturaEncabezado encabezado = (from p in db.FacturaEncabezado
                                            where p.PrefacturaEncabezadoID == PreFacturaEncabezadoID
                                            select p).SingleOrDefault();


            if (encabezado == null)
            {
                return false;
            }


            return true;
        }

        public void crearFactura(int PreFacturaEncabezadoID, Usuario usuario)
        {
            PrefacturaEncabezado prefacturaEncabezado = db.PrefacturaEncabezado.Find(PreFacturaEncabezadoID);
            FacturaEncabezado facturaEncabezado  =  crearEncabezadoFactura(prefacturaEncabezado, usuario);
            ArrayList vFacturasDetalle = crearDetalleFactura(prefacturaEncabezado, facturaEncabezado);

        }

        public decimal getSubTotal(int facturaEncabezadoID, String tipoMoneda, ArrayList vDetallesFactura)
        {

            if (vDetallesFactura == null)
            {
                vDetallesFactura = new ArrayList();
                var detalles = (from p in db.FacturaDetalle
                                where p.FacturaEncabezadoID == facturaEncabezadoID
                                select p);

                foreach (FacturaDetalle facturaDetalle in detalles)
                {
                    vDetallesFactura.Add(facturaDetalle);
                }
            }



            decimal sumatoriaCOP = 0;
            decimal sumatoriaUSD = 0;
            foreach (FacturaDetalle detalle in vDetallesFactura)
            {
                sumatoriaCOP = sumatoriaCOP + Decimal.Parse(detalle.ValorCOP, new CultureInfo("es-ES"));
                sumatoriaUSD = sumatoriaUSD + Decimal.Parse(detalle.ValorUSD, new CultureInfo("es-ES"));
            }

            if (tipoMoneda == PESOS_COLOMBIANOS)
            {
                return sumatoriaCOP;
            }
            else if (tipoMoneda == DOLARES_AMERICANOS)
            {
                return sumatoriaUSD;
            }

            else
            {
                return sumatoriaCOP;
            }


        }


        public decimal getIVACalculado(int facturaEncabezadoID, ArrayList vDetallesFactura, String tipoMoneda)
        {
            //Solamente se calcula el IVA sobre los Ingresos Propios
            decimal totalIVA = 0;


            decimal sumatoria = getBaseIVA(facturaEncabezadoID, vDetallesFactura, tipoMoneda);


            if (sumatoria > 0)
            {
                ConfiguracionImpuesto configuracionImpuesto = new ConfiguracionImpuesto();
                decimal iva = configuracionImpuesto.getPocentajeImpuesto(ConfiguracionImpuesto.IVA);
                totalIVA = sumatoria * (iva / 100);
            }

            return totalIVA;
        }


        public decimal getBaseIVA(int facturaEncabezadoID, ArrayList vDetallesFactura, String tipoMoneda)
        {
            //Solamente se calcula el IVA sobre los Ingresos Propios
            decimal baseIVA = 0;

            if (vDetallesFactura == null)
            {
                vDetallesFactura = new ArrayList();
                var detalles = (from p in db.FacturaDetalle
                                where p.FacturaEncabezadoID == facturaEncabezadoID
                                && p.TipoDeIngresoID == TipoDeIngreso.INGRESOS_PROPIOS
                                select p);

                foreach (FacturaDetalle facturaDetalle in detalles)
                {
                    vDetallesFactura.Add(facturaDetalle);
                }
            }



            foreach (FacturaDetalle detalle in vDetallesFactura)
            {
                if (detalle.TipoDeIngresoID == TipoDeIngreso.INGRESOS_PROPIOS)
                {
                    if (tipoMoneda == PESOS_COLOMBIANOS || tipoMoneda == PESOSDOLARES_COLOMBIANOS)
                    {
                        if (detalle.ValorCOP.Length != 0)
                        {
                            baseIVA = baseIVA + Decimal.Parse(detalle.ValorCOP, new CultureInfo("es-ES"));
                        }
                    }

                    else if (tipoMoneda == DOLARES_AMERICANOS)
                    {
                        if (detalle.ValorUSD.Length != 0)
                        {
                            baseIVA = baseIVA + Decimal.Parse(detalle.ValorUSD, new CultureInfo("es-ES"));
                        }
                    }
  
                   
                }

                

            }


            return baseIVA;
        }

        public FacturaModel getFalsaFactura(int prefacturaEncabezadoId, Usuario usuario)
        {
            FacturaModel vm = new FacturaModel();
            PrefacturaEncabezado prefacturaEncabezado = db.PrefacturaEncabezado.Find(prefacturaEncabezadoId);
            FacturaEncabezado facturaEncabezado = getEncabezadoFacturaFromPrefactura(prefacturaEncabezado, usuario);
            PlazoFactura plazoFactura = new PlazoFactura();
            plazoFactura.PlazoFacturaID = PlazoFactura.CERO;
            plazoFactura.PlazoFacturaNm = "";
            facturaEncabezado.PlazoFactura = plazoFactura;
            facturaEncabezado.NoFacturaSoporte = prefacturaEncabezadoId;

            facturaEncabezado.Cliente.CiudadNm = prefacturaEncabezado.NmCliente;
            facturaEncabezado.Cliente.NIT = prefacturaEncabezado.NIT;
            facturaEncabezado.Cliente.Direccion = prefacturaEncabezado.Direccion;
            facturaEncabezado.Cliente.Telefono1 = prefacturaEncabezado.Telefono1;
            facturaEncabezado.Cliente.Telefono2 = prefacturaEncabezado.Telefono2;
            
            ArrayList vFacturaDetalles = getFacturaDetallesFromPrefactura(prefacturaEncabezado,prefacturaEncabezadoId);
            facturaEncabezado.setFacturaDetalles(vFacturaDetalles);
            vm.FacturaEncabezado = facturaEncabezado;
            vm.Detalles = vFacturaDetalles;
            return vm;
        }

        private FacturaEncabezado getEncabezadoFacturaFromPrefactura(PrefacturaEncabezado prefacturaencabezado, Usuario usuario)
        {
            FacturaEncabezado facturaEncabezado = new FacturaEncabezado();
            if (prefacturaencabezado != null)
            {
                facturaEncabezado.DocumentoOperacionesID = prefacturaencabezado.DocumentoOperacionesID;
                facturaEncabezado.TasaDeCambio = prefacturaencabezado.TasaDeCambio;
                facturaEncabezado.Anotaciones = prefacturaencabezado.Anotaciones;
                facturaEncabezado.AtencionSr = prefacturaencabezado.AtencionSr;
                facturaEncabezado.Estado = prefacturaencabezado.Estado;
                facturaEncabezado.PrefacturaEncabezadoID = prefacturaencabezado.PrefacturaEncabezadoID;
                facturaEncabezado.TotalAnticipos = prefacturaencabezado.TotalAnticipos;
                facturaEncabezado.FechaFactura = System.DateTime.Now;
                facturaEncabezado.ClienteID = prefacturaencabezado.ClienteID;
                facturaEncabezado.UsuarioCreadorID = usuario.UsuarioID;
                facturaEncabezado.FechaCreacion = CorrectorHoraServidor.getHoraActualColombia();
                facturaEncabezado.DocumentoOperaciones = db.DocumentoOperaciones.Find(prefacturaencabezado.DocumentoOperacionesID);
                facturaEncabezado.Cliente = db.Clientes.Find(prefacturaencabezado.ClienteID);
                
            }

            return facturaEncabezado;
        }

        private FacturaEncabezado crearEncabezadoFactura(PrefacturaEncabezado prefacturaencabezado, Usuario usuario)
        {
            if (prefacturaencabezado != null)
            {
                FacturaEncabezado facturaEncabezado = getEncabezadoFacturaFromPrefactura(prefacturaencabezado, usuario);                
                ConfiguracionEmpresaBLL config = new ConfiguracionEmpresaBLL();
                facturaEncabezado.NoFacturaSoporte = config.getConsecutivoFactura(ConfiguracionEmpresaBLL.PRETROCARGO_CONFIGURACION);

                db.FacturaEncabezado.Add(facturaEncabezado);
                db.SaveChanges();
                return facturaEncabezado;

            }

            return null;
        }

        private ArrayList crearDetalleFactura(PrefacturaEncabezado preFacturaEncabezado, FacturaEncabezado facturaEncabezado)
        {
            ArrayList vFacturasDetalle = new ArrayList();
            foreach (FacturaDetalle facturaDetalle in getFacturaDetallesFromPrefactura(preFacturaEncabezado, facturaEncabezado.FacturaEncabezadoID))
            {
                db.FacturaDetalle.Add(facturaDetalle);

            }
            db.SaveChanges();

            return vFacturasDetalle;
        }

        private ArrayList getFacturaDetallesFromPrefactura(PrefacturaEncabezado preFacturaEncabezado, int FacturaEncabezadoID)
        {
            ArrayList vFacturaDetalles = new ArrayList();
            ArrayList prefacturadetalles = getDetallesPrefactura(preFacturaEncabezado.PrefacturaEncabezadoID);
            FacturaDetalle facturaDetalle = null;

            foreach (PrefacturaDetalle prefacturaDetalle in prefacturadetalles)
            {
                facturaDetalle = new FacturaDetalle();
                facturaDetalle.FacturaEncabezadoID = FacturaEncabezadoID;
                facturaDetalle.TipoDeIngresoID = prefacturaDetalle.TipoDeIngresoID;
                facturaDetalle.ConceptoID = prefacturaDetalle.ConceptoID;
                facturaDetalle.ConceptoNm = prefacturaDetalle.ConceptoNm;
                facturaDetalle.Detalles = prefacturaDetalle.Detalles;
                facturaDetalle.ValorUSD = prefacturaDetalle.ValorUSD;
                facturaDetalle.ValorCOP = prefacturaDetalle.ValorCOP;                
                vFacturaDetalles.Add(facturaDetalle);
            }
            return vFacturaDetalles;
        }

        public ArrayList getDetallesPrefactura(int prefacturaEncabezadoId)
        {
            ArrayList vDetalles = new ArrayList();
            var prefacturadetalles = from p in db.PrefacturaDetalle
                                     where p.PrefacturaEncabezadoID == prefacturaEncabezadoId
                                     orderby p.TipoDeIngresoID
                                     select p;

            foreach (PrefacturaDetalle prefacturaDetalle in prefacturadetalles)
            {
                vDetalles.Add(prefacturaDetalle);
            }

            return vDetalles;
        }

        public ArrayList getDetallesByEncabezadoId(int FacturaEncabezadoId)
        {
            ArrayList vDetalles = new ArrayList();

            var detalles = from p in db.FacturaDetalle
                           where p.FacturaEncabezadoID == FacturaEncabezadoId
                           select p;

            foreach (FacturaDetalle detalle in detalles)
            {
                vDetalles.Add(detalle);
            }

            return vDetalles;

        }


        public FacturaModel getFacturaModel(int FacturaEncabezadoId)
        {
            FacturaModel vm = new FacturaModel();
            vm.FacturaEncabezado = getFacturaEncabezadoByID(FacturaEncabezadoId);
            vm.Detalles = getDetallesByEncabezadoId(FacturaEncabezadoId);
            return vm;
        }

        public int cambiarEstadoFactura(int FacturaEncabezadoId, int estado)
        {
            FacturaEncabezado facturaEncabezado = db.FacturaEncabezado.Find(FacturaEncabezadoId);
            facturaEncabezado.Estado = estado;
            db.Entry(facturaEncabezado).State = EntityState.Modified;
            db.SaveChanges();
            ConfiguracionEmpresaBLL config = new ConfiguracionEmpresaBLL();
            config.actualizarConsecutivoFactura(facturaEncabezado.NoFacturaSoporte);
            return facturaEncabezado.NoFacturaSoporte;
        }



        //********************************************************** Reportes ***********************

        public RangoFechas getRangoFechas(int ano, int mes)
        {
            RangoFechas range = new RangoFechas();
            range.FechaInicial = new DateTime(ano, mes, 1, 1, 1, 0);
            range.FechaFinal = new DateTime(ano, mes, 1).AddMonths(1).AddDays(-1);
            return range;
        }

        public decimal getTotalFacturadoByClienteTipoIngresoCOP(int clienteID, int ano, int mes, int[] tipoIngreso)
        {
            return getTotalFacturadoByClienteByTipoIngreso(clienteID, getRangoFechas(ano, mes), tipoIngreso);
        }

        public decimal getTotalFacturadoByClienteCOP(int clienteID, int ano, int mes)
        {
            return getTotalFacturadoByClienteCOP(clienteID, getRangoFechas(ano,mes));
        }
        public decimal getTotalFacturadoByClienteCOP(int clienteID, RangoFechas range)
        {
            int[] tipoIngreso = { TipoDeIngreso.INGRESOS_A_TERCEROS, TipoDeIngreso.INGRESOS_PROPIOS, TipoDeIngreso.INGRESOS_PROPIOS_NO_GRAVADOS};
            return getTotalFacturadoByClienteByTipoIngreso(clienteID,  range, tipoIngreso);
        }



        private decimal getTotalFacturadoByClienteByTipoIngreso(int clienteID, RangoFechas range, int[] tiposIngreso)
        {
            ArrayList vFacturasFiltradas = cargarFacturas(clienteID,range);
            
            decimal totalFactura = 0;
            foreach (FacturaEncabezado factura in vFacturasFiltradas)
            {
                ArrayList vDetallesFacturaFiltrados = new ArrayList();
                foreach (int tipoIngreso in tiposIngreso)
                {
                    agregaDetalles(vDetallesFacturaFiltrados, factura, tipoIngreso);
                }

                decimal sumatoria = 0;
                foreach (FacturaDetalle detalle in vDetallesFacturaFiltrados)
                {
                    sumatoria = sumatoria + Decimal.Parse(detalle.ValorCOP, new CultureInfo("es-ES")); ;
                }

                totalFactura = totalFactura + sumatoria;
            }
            return totalFactura;
        }

        //********************************************************** END Reportes ***********************

        private void agregaDetalles(ArrayList vDetallesFactura, FacturaEncabezado factura, int tipoIngreso)
        {
            var detalles = (from p in db.FacturaDetalle
                                           where p.FacturaEncabezadoID == factura.FacturaEncabezadoID
                                           && p.TipoDeIngresoID == tipoIngreso
                                           select p);

            foreach (FacturaDetalle detalle in detalles)
            {
                vDetallesFactura.Add(detalle);
            }
        }

        private ArrayList cargarFacturas(int clienteID, RangoFechas range)
        {
            ArrayList vFacturasFiltradas = new ArrayList();
            //NO llega cliente se selecionan todos
            if (clienteID == 0)
            {
                var facturas = from p in db.FacturaEncabezado
                               where p.Estado == EstadoFactura.APROBADA
                               && p.FechaFactura >= range.FechaInicial
                               && p.FechaFactura <= range.FechaFinal
                               select p;

                foreach (FacturaEncabezado factura in facturas)
                {
                    vFacturasFiltradas.Add(factura);
                }

            }
            else
            {
                var facturas = from p in db.FacturaEncabezado
                               where p.DocumentoOperaciones.ClienteID == clienteID
                               && p.Estado == EstadoFactura.APROBADA
                               && p.FechaFactura >= range.FechaInicial
                               && p.FechaFactura <= range.FechaFinal
                               select p;

                foreach (FacturaEncabezado factura in facturas)
                {
                    vFacturasFiltradas.Add(factura);
                }
            }

            return vFacturasFiltradas;
        }
    }
}