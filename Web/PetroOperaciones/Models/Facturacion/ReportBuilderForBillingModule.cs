using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportBuilderForFusionChart;
using System.Text;
using System.IO;
using System.Xml;

namespace PetroOperaciones.Models.Facturacion
{
    public class ReportBuilderForBillingModule : ReportBuilder
    {

        private StringBuilder xmlData = new StringBuilder();
        private EfDbContext db = new EfDbContext();
        private FacturaBLL directorBLL = new FacturaBLL();

        public String getXMLReportCustumerSales(RangoFechas range)
        {
            xmlData.Append("<chart caption='" + Title + "' numberPrefix='" + Prefix + "' bgcolor='ffffff' bordercolor='ffffff'>");

            var vItems = from p in db.Clientes
                         select p;

            foreach (Cliente item in vItems)
            {
                decimal totalFactura = directorBLL.getTotalFacturadoByClienteCOP(item.ClienteID,  range);

                if (totalFactura > 0)
                {
                    xmlData.Append("<set value='" + Convert.ToInt32(totalFactura) + "' label='" + item.NmCliente + "'/>");
                }

            }


            return formatXML();
        }

        public String getXMLReportCustumerMonthSales(FiltroReporteAnoCliente filtro)
        {
            xmlData.Append("<chart caption='" + Title + "' numberPrefix='" + Prefix + "' bgColor='FFFFFF'  divLineColor='91AF46' divLineAlpha='30' alternateHGridAlpha='5'  baseFontColor='666666' lineColor='91AF46' numVDivlines='11' showAlternateVGridColor='1' borderColor='FFFFFF'>");
            decimal totalFactura = 0;
            for (int mes = 1; mes<=12; mes++)
            {
                totalFactura = directorBLL.getTotalFacturadoByClienteCOP(filtro.ClienteID, filtro.Ano, mes);
                xmlData.Append("<set value='" + Convert.ToInt32(totalFactura) + "' label='" + Utilidades.getNombreMes(mes) + "'/>");
            }
            return formatXML();
        }
        public String getXMLCustumerMonthIncoming(FiltroReporteAnoCliente filtro, int[] tipoIngreso)
        {
            xmlData.Append("<chart caption='" + Title + "' numberPrefix='" + Prefix + "' bgColor='FFFFFF'  divLineColor='91AF46' divLineAlpha='30' alternateHGridAlpha='5'  baseFontColor='666666' lineColor='91AF46' numVDivlines='11' showAlternateVGridColor='1' borderColor='FFFFFF'>");
            decimal totalFactura = 0;
         

            for (int mes = 1; mes <= 12; mes++)
            {
                totalFactura = directorBLL.getTotalFacturadoByClienteTipoIngresoCOP(filtro.ClienteID, filtro.Ano, mes, tipoIngreso);
                xmlData.Append("<set value='" + Convert.ToInt32(totalFactura) + "' label='" + Utilidades.getNombreMes(mes) + "'/>");
            }
            return formatXML();
        }

        private String formatXML()
        {

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


        public string getXMLThridIncomingVsTarget(int year)
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootElement = xmlDoc.CreateElement("chart");
            xmlHeader(xmlDoc, rootElement, year);
            XmlElement elementDataset = xmlDoc.CreateElement("dataset");
            elementDataset.SetAttribute("seriesName", "Ingresos Terceros");
            elementDataset.SetAttribute("color", "01DF01");

            FacturaBLL facturaBLL = new FacturaBLL();
            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                int[] tipoIngreso = { TipoDeIngreso.INGRESOS_A_TERCEROS };

                decimal totalVentas = facturaBLL.getTotalFacturadoByClienteTipoIngresoCOP(FacturaBLL.TODOS_LOS_CLIENTES, Convert.ToInt32(year), Utilidades.getNoMes(mes), tipoIngreso);
                XmlElement elementSet = xmlDoc.CreateElement("set");
                elementSet.SetAttribute("value", "" + Convert.ToInt32(totalVentas));
                elementDataset.AppendChild(elementSet);
            }

            rootElement.AppendChild(elementDataset);

            XmlElement elementDatasetTarget = xmlDoc.CreateElement("dataset");
            elementDatasetTarget.SetAttribute("seriesName", "Meta");
            elementDatasetTarget.SetAttribute("color", "0101DF");
            elementDatasetTarget.SetAttribute("renderAs", "Line");

            IndicadorBLL indicadorBLL = new IndicadorBLL();
            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                int totalMeta = indicadorBLL.getMetaProyectadaIndicador(TipoDeIndicador.INGRESOS_A_TERCEROS, Convert.ToInt32(year), Utilidades.getNoMes(mes));
                XmlElement elementSet = xmlDoc.CreateElement("set");
                elementSet.SetAttribute("value", "" + totalMeta);
                elementDatasetTarget.AppendChild(elementSet);
            }

            rootElement.AppendChild(elementDatasetTarget);
            xmlDoc.AppendChild(rootElement);
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xmlDoc.WriteTo(tx);
            return sw.ToString();
        }

        public string getXMLOwnNoTaxedIncomingVsTarget(int year)
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootElement = xmlDoc.CreateElement("chart");
            xmlHeader(xmlDoc, rootElement, year);
            XmlElement elementDataset = xmlDoc.CreateElement("dataset");
            elementDataset.SetAttribute("seriesName", "Ingresos Propios No Gravados");
            elementDataset.SetAttribute("color", "01DF01");

            FacturaBLL facturaBLL = new FacturaBLL();
            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                int[] tipoIngreso = { TipoDeIngreso.INGRESOS_PROPIOS_NO_GRAVADOS };

                decimal totalVentas = facturaBLL.getTotalFacturadoByClienteTipoIngresoCOP(FacturaBLL.TODOS_LOS_CLIENTES, Convert.ToInt32(year), Utilidades.getNoMes(mes), tipoIngreso);
                XmlElement elementSet = xmlDoc.CreateElement("set");
                elementSet.SetAttribute("value", "" + Convert.ToInt32(totalVentas));
                elementDataset.AppendChild(elementSet);
            }

            rootElement.AppendChild(elementDataset);

            XmlElement elementDatasetTarget = xmlDoc.CreateElement("dataset");
            elementDatasetTarget.SetAttribute("seriesName", "Meta");
            elementDatasetTarget.SetAttribute("color", "0101DF");
            elementDatasetTarget.SetAttribute("renderAs", "Line");

            IndicadorBLL indicadorBLL = new IndicadorBLL();
            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                int totalMeta = indicadorBLL.getMetaProyectadaIndicador(TipoDeIndicador.INGRESOS_PROPIOS_NO_GRAVADOS, Convert.ToInt32(year), Utilidades.getNoMes(mes));
                XmlElement elementSet = xmlDoc.CreateElement("set");
                elementSet.SetAttribute("value", "" + totalMeta);
                elementDatasetTarget.AppendChild(elementSet);
            }

            rootElement.AppendChild(elementDatasetTarget);
            xmlDoc.AppendChild(rootElement);
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xmlDoc.WriteTo(tx);
            return sw.ToString();
        }

        public string getXMLOwnIncomingVsTarget(int year)
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootElement = xmlDoc.CreateElement("chart");
            xmlHeader(xmlDoc, rootElement, year);
            XmlElement elementDataset = xmlDoc.CreateElement("dataset");
            elementDataset.SetAttribute("seriesName", "Ingresos Propios");
            elementDataset.SetAttribute("color", "01DF01");

            FacturaBLL facturaBLL = new FacturaBLL();
            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                int[] tipoIngreso = { TipoDeIngreso.INGRESOS_PROPIOS};

                decimal totalVentas = facturaBLL.getTotalFacturadoByClienteTipoIngresoCOP(FacturaBLL.TODOS_LOS_CLIENTES, Convert.ToInt32(year), Utilidades.getNoMes(mes), tipoIngreso);
                XmlElement elementSet = xmlDoc.CreateElement("set");
                elementSet.SetAttribute("value", "" + Convert.ToInt32(totalVentas));
                elementDataset.AppendChild(elementSet);
            }

            rootElement.AppendChild(elementDataset);

            IndicadorBLL indicadorBLL = new IndicadorBLL();

            XmlElement elementExpenses = xmlDoc.CreateElement("dataset");
            elementExpenses.SetAttribute("seriesName", "Gastos");
            elementExpenses.SetAttribute("color", "B40431");
            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                int totalMeta = indicadorBLL.getMetaProyectadaIndicador(TipoDeIndicador.EXPENSES, Convert.ToInt32(year), Utilidades.getNoMes(mes));
                XmlElement elementSet = xmlDoc.CreateElement("set");
                elementSet.SetAttribute("value", "" + Convert.ToInt32(totalMeta));
                elementExpenses.AppendChild(elementSet);
            }

            rootElement.AppendChild(elementExpenses);

            XmlElement elementDatasetTarget = xmlDoc.CreateElement("dataset");
            elementDatasetTarget.SetAttribute("seriesName", "Meta");
            elementDatasetTarget.SetAttribute("color", "0101DF");
            elementDatasetTarget.SetAttribute("renderAs", "Line");

 
            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                int totalMeta = indicadorBLL.getMetaProyectadaIndicador(TipoDeIndicador.INGRESOS_PROPIOS, Convert.ToInt32(year), Utilidades.getNoMes(mes));
                XmlElement elementSet = xmlDoc.CreateElement("set");
                elementSet.SetAttribute("value", "" + totalMeta);
                elementDatasetTarget.AppendChild(elementSet);
            }

            rootElement.AppendChild(elementDatasetTarget);
            xmlDoc.AppendChild(rootElement);
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xmlDoc.WriteTo(tx);
            return sw.ToString();
        }


        public string getXMLSalesVsTarget(int year)
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootElement = xmlDoc.CreateElement("chart");            
            rootElement.AppendChild(getElementCategoriesMeses(xmlDoc, year));
            
            XmlElement elementDataset = xmlDoc.CreateElement("dataset");
            elementDataset.SetAttribute("seriesName", "Ventas");
            elementDataset.SetAttribute("color", "01DF01");

            FacturaBLL facturaBLL = new FacturaBLL();

            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                decimal totalVentas = facturaBLL.getTotalFacturadoByClienteCOP(FacturaBLL.TODOS_LOS_CLIENTES,Convert.ToInt32(year),Utilidades.getNoMes(mes));
                XmlElement elementSet = xmlDoc.CreateElement("set");
                elementSet.SetAttribute("value", "" + Convert.ToInt32(totalVentas));
                elementDataset.AppendChild(elementSet);
            }

            rootElement.AppendChild(elementDataset);

            XmlElement elementDatasetTarget = xmlDoc.CreateElement("dataset");
            elementDatasetTarget.SetAttribute("seriesName", "Meta");
            elementDatasetTarget.SetAttribute("color", "0101DF");
            elementDatasetTarget.SetAttribute("renderAs", "Line");

            IndicadorBLL indicadorBLL = new IndicadorBLL();
            foreach (string mes in Utilidades.getMesesEjecutados(year))
            {
                int totalMeta = indicadorBLL.getMetaProyectadaIndicador(TipoDeIndicador.VENTAS_TOTALES, Convert.ToInt32(year), Utilidades.getNoMes(mes));
                XmlElement elementSet = xmlDoc.CreateElement("set");
                elementSet.SetAttribute("value", "" + totalMeta);
                elementDatasetTarget.AppendChild(elementSet);
            }

            rootElement.AppendChild(elementDatasetTarget);            
            xmlDoc.AppendChild(rootElement);
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xmlDoc.WriteTo(tx);
            return sw.ToString();
        }


        private void xmlHeader(XmlDocument xmlDoc, XmlElement rootElement,int year)
        {

            rootElement.SetAttribute("caption", Title + " " + year);
            rootElement.SetAttribute("plotFillAlpha", "95");
            rootElement.SetAttribute("showSum", "1");
            rootElement.SetAttribute("showValues", "1");
            rootElement.SetAttribute("formatNumberScale", "1");
            rootElement.SetAttribute("showPercentValues", "1");
            rootElement.SetAttribute("numberPrefix", Prefix);
            rootElement.SetAttribute("yAxisName", YAxisName);
            rootElement.AppendChild(getElementCategoriesMeses(xmlDoc, year));

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

         private void agregarCategoriaMeses()
         {
             Utilidades utilidades = new Utilidades();
             utilidades.agregarCategoriaMeses(xmlData);
         }
    }
}