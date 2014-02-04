using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

namespace PetroOperaciones.Models
{
    public class GeneradorXMLReportes
    {

        private StringBuilder xmlData  = new StringBuilder();
        private EfDbContext db = new EfDbContext();

        public String TituloGrafico { get; set; }
        public String Prefijo { get; set; }
        public String YAxisName { get; set; }

        public string getReporteTotalOperacionesByTipo(RangoFechas rangoFechas, int clienteID)
        {
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();

            
            xmlData.Append("<chart caption='" + TituloGrafico + "' numberPrefix='" + Prefijo + "' bgcolor='ffffff' bordercolor='ffffff'>");

            var vItems = from p in db.TiposOperaciones
                         select p;

            foreach (TipoOperacion item in vItems)
            {
                int totalOperaciones = directorBLL.getTotalOperaciones(clienteID, item.TipoOperacionID, rangoFechas);

                if (totalOperaciones > 0)
                {
                    xmlData.Append("<set value='" + totalOperaciones + "' label='" + item.TipoOperacionNm + "'/>");
                }
                
            }


            return formatXML();
        }


        public string getReporteTotalMercanciaMovilizadaByTipo(RangoFechas rangoFechas)
        {
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();


            xmlData.Append("<chart caption='" + TituloGrafico + "' numberPrefix='" + Prefijo + "' bgcolor='ffffff' bordercolor='ffffff'>");

            var vItems = from p in db.MedioTransportes
                         select p;

            foreach (MedioTransporte item in vItems)
            {
                int totalOperaciones = directorBLL.getTotalOperaciones(item.MedioTransporteID, rangoFechas);

                if (totalOperaciones > 0)
                {
                    xmlData.Append("<set value='" + totalOperaciones + "' label='" + item.MedioTransporteNm + "'/>");
                }

            }


            return formatXML();
        }


        public static int TIPO_PUERTO_DESTINO=1;
        public static int TIPO_PUERTO_ORIGEN=2;
        public string getReporteTotalOperacionesByPuerto(RangoFechas rangoFechas, int tipoPuerto, int ClienteID)
        {
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();


            xmlData.Append("<chart caption='" + TituloGrafico + "' numberPrefix='" + Prefijo + "' bgcolor='ffffff' bordercolor='ffffff'>");

            var vItems = from p in db.Puertos
                         select p;

            foreach (Puerto item in vItems)
            {
                int totalOperaciones = 0;

                if (tipoPuerto == TIPO_PUERTO_DESTINO)
                {
                    totalOperaciones = directorBLL.getTotalOperacionesPuertoDestino(item, rangoFechas, ClienteID);
                }

                if (tipoPuerto == TIPO_PUERTO_ORIGEN)
                {
                    totalOperaciones = directorBLL.getTotalOperacionesPuertoOrigen(item, rangoFechas, ClienteID);
                }                

                if (totalOperaciones > 0)
                {
                    xmlData.Append("<set value='" + totalOperaciones + "' label='" + item.PuertoNm + "'/>");
                }

            }


            return formatXML();
        }



        public string getReporteTotalOperacionesByUsuario(RangoFechas rangoFechas)
        {
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();


            xmlData.Append("<chart caption='" + TituloGrafico + "' numberPrefix='" + Prefijo + "' bgcolor='ffffff' bordercolor='ffffff'>");

            var vItems = from p in db.Usuarios
                         select p;

            foreach (Usuario item in vItems)
            {
                int totalOperaciones = directorBLL.getTotalOperaciones(item, rangoFechas);

                if (totalOperaciones > 0)
                {
                    xmlData.Append("<set value='" + totalOperaciones + "' label='" + item.Nombre + "'/>");
                }

            }


            return formatXML();
        }

        //OperacionesPorTipo
        public string getReporteTotalOperacionesByTipo(int ano)
        {
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();
            xmlData.Append("<chart caption='" + TituloGrafico + "'  plotFillAlpha='95' showValues='0' formatNumberScale='0' showPercentValues='1' showSum='1' numberPrefix='" + Prefijo + "' yAxisName='" + YAxisName + "'>");
            agregarCategoriaMeses();

            var vItems = from p in db.TiposOperaciones
                         select p;

            foreach (TipoOperacion item in vItems)
            {

                if (directorBLL.exitenOperaciones(item.TipoOperacionID))
                {
                    xmlData.Append("<dataset seriesName='" + item.TipoOperacionNm + "'>");
                    foreach (string mes in Utilidades.getMesesEjecutados())
                    {
                        int totalOperaciones = directorBLL.getTotalOperacionesByMes(Utilidades.getNoMes(mes), ano, item.TipoOperacionID);
                        xmlData.Append("<set value='" + totalOperaciones + "' toolText='" + item.TipoOperacionNm + "," + totalOperaciones + "' />");
                    }

                    xmlData.Append("</dataset>");

                }

            }

            return formatXML();
        }

        //public string getReporteTotalSeguimientosRegistrados(int ano)
        //{
        //    SeguimientosBLL directorBLL = new SeguimientosBLL();
        //    xmlData.Append("<chart caption='" + TituloGrafico + "'   showValues='1' formatNumberScale='0' showPercentValues='1' numberPrefix='" + Prefijo + "' yAxisName='" + YAxisName + "' paletteColors='91c728'>");

        //    foreach (string mes in Utilidades.getMesesEjecutados())
        //    {
        //        int total = directorBLL.getNoSegumientosRegistrados(Utilidades.getNoMes(mes), ano);
        //        xmlData.Append("<set label='" + mes + "' value='" + total + "'/>");
        //    }





        //    return formatXML();
        //}

        public string getReporteTotalSeguimientosRegistrados(int year)
        {

            SeguimientosBLL directorBLL = new SeguimientosBLL();

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootElement = xmlDoc.CreateElement("chart");
            rootElement.SetAttribute("caption", TituloGrafico + " " + year);
            rootElement.SetAttribute("plotFillAlpha", "95");
            rootElement.SetAttribute("showSum", "1");
            rootElement.SetAttribute("showValues", "0");
            rootElement.SetAttribute("formatNumberScale", "0");
            rootElement.SetAttribute("numberPrefix", Prefijo);
            rootElement.SetAttribute("yAxisName", YAxisName);

            rootElement.AppendChild(getElementCategoriesMeses(xmlDoc, year));

            var vItems = from p in db.Usuarios
                         where p.Rol != TipoRol.CLIENTE
                         select p;



            foreach (Usuario item in vItems)
            {

                XmlElement elementDataset = xmlDoc.CreateElement("dataset");
                elementDataset.SetAttribute("seriesName", item.Nombre);
                bool tieneSeguimientos = false;

                    foreach (string mes in Utilidades.getMesesEjecutados(year))
                    {
                        int totalSeguimientos = directorBLL.getNoSegumientosRegistrados(Utilidades.getNoMes(mes), year, item.UsuarioID);
                        XmlElement elementSet = xmlDoc.CreateElement("set");
                        elementSet.SetAttribute("value", "" + totalSeguimientos);
                        elementSet.SetAttribute("toolText", "" + item.Nombre + ", " + totalSeguimientos + ", " + Utilidades.calcularPorcentaje(totalSeguimientos, directorBLL.getNoSegumientosRegistrados(Utilidades.getNoMes(mes), year))+"%");
                        if (totalSeguimientos != 0)
                        {
                               tieneSeguimientos = true;
                        }
                        elementDataset.AppendChild(elementSet);
                       
                    }

                    if (tieneSeguimientos)
                    {
                        rootElement.AppendChild(elementDataset);
                    }
                   

                

            }

            xmlDoc.AppendChild(rootElement);
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xmlDoc.WriteTo(tx);
            return sw.ToString();

            /*
            SeguimientosBLL directorBLL = new SeguimientosBLL();

            xmlData.Append("<chart caption='" + TituloGrafico + "'  plotFillAlpha='95' showValues='0' formatNumberScale='0' showPercentValues='1' showSum='1' numberPrefix='" + Prefijo + "' yAxisName='" + YAxisName + "'>");
            agregarCategoriaMeses();

            var vItems = from p in db.Usuarios
                         select p;

            foreach (Usuario item in vItems)
            {

                xmlData.Append("<dataset seriesName='" + item.Nombre + "'>");
                foreach (string mes in Utilidades.getMesesEjecutados())
                {
                    int totalSeguimientos = directorBLL.getNoSegumientosRegistrados(Utilidades.getNoMes(mes), ano, item.UsuarioID);
       
                    xmlData.Append("<set value='" + totalSeguimientos + "' toolText='" + item.Nombre + "," + totalSeguimientos + "' />");
      
                   
                   
                }

                xmlData.Append("</dataset>");



            }

            return formatXML();
             * */
        }

        //OperacionesPorTipo
        public string getXMLReporteTotalOperacionesByTipo(int year)
        {
            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootElement = xmlDoc.CreateElement("chart");
            rootElement.SetAttribute("caption", TituloGrafico + " " + year);
            rootElement.SetAttribute("plotFillAlpha", "95");
            rootElement.SetAttribute("showSum", "1");
            rootElement.SetAttribute("showValues", "0");
            rootElement.SetAttribute("formatNumberScale", "0");
            rootElement.SetAttribute("showPercentValues", "1");
            rootElement.SetAttribute("numberPrefix", Prefijo);
            rootElement.SetAttribute("yAxisName", YAxisName);

            rootElement.AppendChild(getElementCategoriesMeses(xmlDoc, year));

            var vItems = from p in db.TipoModalidadAduanera
                         select p;

            foreach (TipoModalidadAduanera item in vItems)
            {

                if (directorBLL.exitenOperaciones(item.TipoModalidadAduaneraID))
                {
                    XmlElement elementDataset = xmlDoc.CreateElement("dataset");
                    elementDataset.SetAttribute("seriesName", item.TipoModalidadAduaneraNm);

                    foreach (string mes in Utilidades.getMesesEjecutados(year))
                    {
                        int totalOperaciones = directorBLL.getTotalOperacionesByMes(Utilidades.getNoMes(mes), year, item.TipoModalidadAduaneraID);
                        XmlElement elementSet= xmlDoc.CreateElement("set");
                        elementSet.SetAttribute("value", "" + totalOperaciones);
                        elementSet.SetAttribute("toolText", "" + item.TipoModalidadAduaneraNm + "," + totalOperaciones);
                        elementDataset.AppendChild(elementSet);
                    }

                    rootElement.AppendChild(elementDataset);

                }

            }

            xmlDoc.AppendChild(rootElement);
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xmlDoc.WriteTo(tx);
            return sw.ToString();
        }





        public String getXMLReporteTotalCargaConsolidada(int ano, int ClienteID)
        {

            DocumentoOperacionesBLL directorBLL = new DocumentoOperacionesBLL();

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootElement = xmlDoc.CreateElement("chart");
            rootElement.SetAttribute("caption", TituloGrafico + " " + ano);
            rootElement.SetAttribute("showValues", "1");
            rootElement.SetAttribute("formatNumberScale", "0");
            rootElement.SetAttribute("showPercentValues", "1");
            rootElement.SetAttribute("numberPrefix", Prefijo);
            rootElement.SetAttribute("yAxisName", YAxisName);

            foreach (string mes in Utilidades.getMesesEjecutados(ano))
            {
                XmlElement element = xmlDoc.CreateElement("set");
                int totalCargaTransportada = directorBLL.getTotalCargaTransportadaByMes(Utilidades.getNoMes(mes), ano, ClienteID);
                element.SetAttribute("label", mes);
                element.SetAttribute("value", "" + totalCargaTransportada);
                rootElement.AppendChild(element);
            }
            xmlDoc.AppendChild(rootElement);
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xmlDoc.WriteTo(tx);
            return sw.ToString();

        }



        private void agregarCategoriaMeses()
        {

            Utilidades utilidades = new Utilidades();
            utilidades.agregarCategoriaMeses(xmlData);
        }

        private XmlElement getElementCategoriesMeses(XmlDocument xmlDoc,  int year)
        {
            XmlElement elementCategories = xmlDoc.CreateElement("categories");

            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                XmlElement elementCategory = xmlDoc.CreateElement("category");
                elementCategory.SetAttribute("label", ""+mes);
                elementCategories.AppendChild(elementCategory);
            }

            return elementCategories;
        }

        private String formatXML(){

            xmlData.Append("<trendlines>");
            xmlData.Append("<line startValue='0' color='91C728' displayValue='' showOnTop='1'/>");
            xmlData.Append("</trendlines>");


            xmlData.Append("<styles>");
            xmlData.Append("<definition>");
            xmlData.Append("<style type='font' name='CaptionFont' size='12' color='000000'/>");
            xmlData.Append("<style type='font' name='SubCaptionFont' size='15' color='000000' bold='0'/>");
            xmlData.Append("<style type='font' name='DataLabelsFont' size='9' color='000000' bold='1'/>");
            xmlData.Append("<style type='font' name='DataValuesFont' size='9' color='000000' bold='1'/>");

            xmlData.Append("</definition>");
            xmlData.Append("<application>");
            xmlData.Append("<apply toObject='caption' styles='CaptionFont'/>");
            xmlData.Append("<apply toObject='SubCaption' styles='SubCaptionFont'/>");
            xmlData.Append("<apply toObject='DataLabels' styles='DataLabelsFont'/>");
            xmlData.Append("<apply toObject='YAXISVALUES' styles='DataLabelsFont'/>");
            xmlData.Append("<apply toObject='YAXISVALUES' styles='DataLabelsFont'/>");
            xmlData.Append("<apply toObject='YAXISNAME' styles='DataLabelsFont'/>");
            xmlData.Append("<apply toObject='XAXISNAME' styles='DataLabelsFont'/>");
            xmlData.Append("</application>");
            xmlData.Append("</styles>");

            xmlData.Append("</chart>");
            String s = xmlData.ToString();
            s = s.Insert(0, "\"");
            s = s.Insert(s.Length, "\"");
            return s;
        }







    }
}