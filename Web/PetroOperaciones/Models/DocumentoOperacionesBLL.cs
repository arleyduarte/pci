using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Globalization;

namespace PetroOperaciones.Models
{
    public class DocumentoOperacionesBLL
    {
        private static String SEPARADOR_NO_DO = "-";

        private EfDbContext db = new EfDbContext();

        public DocumentoOperaciones getDocumentoOperacionesByID(int id)
        {
            DocumentoOperaciones item = (from p in db.DocumentoOperaciones
                                         where p.DocumentoOperacionesID == id
                                         select p).SingleOrDefault();


            return item;
        }


        public DocumentoOperaciones getDOSerializableByID(int id)
        {
            DocumentoOperaciones item = (from p in db.DocumentoOperaciones
                                         where p.DocumentoOperacionesID == id
                                         select p).SingleOrDefault();

            DocumentoOperaciones document = new DocumentoOperaciones();
            document.NumeroDO = item.NumeroDO;
            document.AWB = item.AWB;
            document.BL = item.BL;
            document.Buque = item.Buque;
            document.ClaseMercancia = item.ClaseMercancia;
            document.NoFacturaShipper = item.NoFacturaShipper;
            document.PesoBruto = item.PesoBruto;
            document.HAWB = item.HAWB;
            document.HBL = item.HBL;
            document.Observaciones = item.Observaciones;

            return document;
        }
        public int getTotalOperaciones(int clienteId, int tipoOperacion, RangoFechas rangoFechas)
        {

            var vItems = (from p in db.DocumentoOperaciones
                          where p.ClienteID == clienteId
                          && p.TipoOperacionID == tipoOperacion
                          && p.FechaCreacion >= rangoFechas.FechaInicial
                          && p.FechaCreacion <= rangoFechas.FechaFinal
                          select p).Count();


            return vItems;
        }


        public int getTotalOperacionesPuertoOrigen(Puerto puerto, RangoFechas rangoFechas, int ClienteID)
        {

            if (ClienteID == 0)
            {
                return (from p in db.DocumentoOperaciones
                        where p.PuertoOrigen == puerto.PuertoID
                        && p.FechaCreacion >= rangoFechas.FechaInicial
                        && p.FechaCreacion <= rangoFechas.FechaFinal
                        select p).Count();
            }
            else
            {
                return (from p in db.DocumentoOperaciones
                        where
                        p.ClienteID == ClienteID
                        && p.PuertoOrigen == puerto.PuertoID
                        && p.FechaCreacion >= rangoFechas.FechaInicial
                        && p.FechaCreacion <= rangoFechas.FechaFinal
                        select p).Count();
            }

        }

        public int getTotalOperacionesPuertoDestino(Puerto puerto, RangoFechas rangoFechas, int ClienteID)
        {

            if (ClienteID != 0)
            {
                return (from p in db.DocumentoOperaciones
                        where
                        p.ClienteID == ClienteID
                        && p.PuertoDestino == puerto.PuertoID
                        && p.FechaCreacion >= rangoFechas.FechaInicial
                        && p.FechaCreacion <= rangoFechas.FechaFinal
                        select p).Count();
            }
            else
            {
                return (from p in db.DocumentoOperaciones
                        where p.PuertoDestino == puerto.PuertoID
                        && p.FechaCreacion >= rangoFechas.FechaInicial
                        && p.FechaCreacion <= rangoFechas.FechaFinal
                        select p).Count();
            }

        }

        public int getTotalOperaciones(Usuario usuario, RangoFechas rangoFechas)
        {

            var vItems = (from p in db.DocumentoOperaciones
                          where p.UsuarioResponsableID == usuario.UsuarioID
                          && p.FechaCreacion >= rangoFechas.FechaInicial
                          && p.FechaCreacion <= rangoFechas.FechaFinal
                          select p).Count();


            return vItems;
        }

        public int getTotalOperaciones(int medioTransporteId, RangoFechas rangoFechas)
        {

            var vItems = (from p in db.DocumentoOperaciones
                          where p.MedioTransporteID == medioTransporteId
                          && p.FechaCreacion >= rangoFechas.FechaInicial
                          && p.FechaCreacion <= rangoFechas.FechaFinal
                          select p).Count();


            return vItems;
        }

        public int getTotalOperacionesByMes(int mes, int ano, int tipoModalidadAduaneraID)
        {

            var vItems = (from p in db.DocumentoOperaciones
                          where p.FechaCreacion.Month == mes
                          && p.FechaCreacion.Year == ano
                          && p.TipoModalidadAduaneraID == tipoModalidadAduaneraID
                          select p).Count();


            return vItems;
        }

        public int getTotalOperacionesByTipo(int tipoOperacion)
        {

            var vItems = (from p in db.DocumentoOperaciones
                          where
                          p.TipoOperacionID == tipoOperacion
                          select p).Count();


            return vItems;
        }


        public int getTotalCargaTransportadaByMes(int mes, int ano, int ClienteID)
        {
            var vItems = from p in db.DocumentoOperaciones
                         where
                          p.ClienteID == ClienteID
                          && p.FechaCreacion.Month == mes
                          && p.FechaCreacion.Year == ano
                         select p;

            return SumarCarga(vItems);
        }

        public int getTotalCargaTransportadaByMes(int mes, int ano)
        {
            var vItems = from p in db.DocumentoOperaciones
                         where
                          p.FechaCreacion.Month == mes
                          && p.FechaCreacion.Year == ano
                         select p;

            return SumarCarga(vItems);
        }

        private int SumarCarga(IQueryable documentosOperaciones)
        {
            Double totalCarga = 0;

            foreach (DocumentoOperaciones documento in documentosOperaciones)
            {

                try
                {
                    totalCarga += Double.Parse(documento.PesoBruto, new CultureInfo("es-ES"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }

            return Convert.ToInt32(totalCarga);
        }


        public bool exitenOperaciones(int tipoOperacion)
        {
            int numeroOperaciones = getTotalOperacionesByTipo(tipoOperacion);

            if (numeroOperaciones == 0)
            {
                return false;
            }

            else
            {
                return true;
            }
        }



        public bool existeDocumentoOperaciones(String numero)
        {
            DocumentoOperaciones item = (from p in db.DocumentoOperaciones
                                         where p.NumeroDO == numero
                                         select p).SingleOrDefault();


            if (item != null)
            {
                return true;
            }

            return false;
        }

        public DocumentoOperaciones getDocumentoOperacionesByNoDO(String numero)
        {
            DocumentoOperaciones item = (from p in db.DocumentoOperaciones
                                         where p.NumeroDO == numero
                                         select p).SingleOrDefault();


            return item;
        }

        public Cliente getClienteByDO(int documentoOperacionesId)
        {
            DocumentoOperaciones item = (from p in db.DocumentoOperaciones
                                         where p.DocumentoOperacionesID == documentoOperacionesId
                                         select p).SingleOrDefault();


            var var = (from p in db.Clientes
                       where p.ClienteID == item.ClienteID
                       select p).SingleOrDefault();


            Cliente cliente = new Cliente();
            cliente.ClienteID = var.ClienteID;
            cliente.NIT = var.NIT;
            cliente.NmCliente = var.NmCliente;
            cliente.Direccion = var.Direccion;
            cliente.Telefono1 = var.Telefono1;
            cliente.Telefono2 = var.Telefono2;
            cliente.Pais = var.Pais;


            return cliente;
        }

        public String getNumeroDocumento()
        {
            String noDocumento = "";


            int ano = System.DateTime.Now.Year;
            int mes = System.DateTime.Now.Month;

            String sAno = "" + ano;
            sAno = sAno.Substring(2, 2);

            return noDocumento = getUltimoConsecutivo(mes, ano) + SEPARADOR_NO_DO + getMes() + SEPARADOR_NO_DO + sAno;
        }

        private String getMes()
        {
            String mes = "";
            int noMes = System.DateTime.Now.Month;


            if (noMes < 10)
            {
                return mes = "0" + noMes;
            }



            return "" + noMes;
        }


        private String getUltimoConsecutivo(int mes, int ano)
        {
            String consecutivo = "";


            DocumentoOperaciones item = null;


            try
            {
                item = (from p in db.DocumentoOperaciones
                        where p.FechaCreacion.Month == mes
                              &&
                              p.FechaCreacion.Year == ano

                        select p).OrderByDescending(x => x.NumeroDO).First();


            }
            catch
            {
            }

            if (item != null)
            {
                int noConsecutivo = getConsecutivoNoDocumento(item.NumeroDO) + 1;

                if (noConsecutivo < 10)
                {
                    consecutivo = "0" + noConsecutivo;
                }

                else
                {
                    consecutivo = "" + noConsecutivo;
                }

            }

            else
            {
                return "01";
            }

            return consecutivo;

        }


        private int getConsecutivoNoDocumento(string noDocumento)
        {
            int consecutivo = 0;

            if (noDocumento != null)
            {
                noDocumento = noDocumento.Substring(0, noDocumento.IndexOf(SEPARADOR_NO_DO));
            }

            try
            {
                consecutivo = Convert.ToInt32(noDocumento);
            }
            catch
            {
            }


            return consecutivo;
        }



    }
}