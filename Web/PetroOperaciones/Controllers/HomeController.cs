using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace PetroOperaciones.Controllers
{
    public class HomeController : Controller
    {

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Bienvenido al sistema de gestión de DO";

            if (User.IsInRole("Admin"))
            {

            }

            return View();
        }

        public ActionResult About()
        {


            return View();
        }

        public string GetXML()
        {
            //This page demonstrates the ease of generating charts using FusionCharts.
            //For this chart, we've used a string variable to contain our entire XML data.

            //Ideally, you would generate XML data documents at run-time, after interfacing with
            //forms or databases etc.Such examples are also present.
            //Here, we've kept this example very simple.




            //Create an XML data document in a string variable
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<chart caption='ESTADÍSTICA GENERAL DE USO LA HERRAMIENTA'  numberPrefix='Solicitudes: '      use3DLighting='1'  showValues='1'    animation='1' formatNumberScale='0'  formatNumber='1'    thousandSeparator ='.'  pieSliceDepth='50'  bgColor='406181, 6DA5DB' bgAlpha='100' baseFontColor='000000' canvasBgAlpha='0' canvasBorderColor='FFFFFF' divLineColor='FFFFFF' divLineAlpha='100' numVDivlines='10' vDivLineisDashed='1' showAlternateVGridColor='1' lineColor='BBDA00' anchorRadius='4' anchorBgColor='BBDA00' anchorBorderColor='FFFFFF' anchorBorderThickness='2'  toolTipBgColor='FFFFFF' toolTipBorderColor='406181' alternateHGridAlpha='5'>");

            int nMes = System.DateTime.Now.Month;
            String sMes = "";



            xmlData.Append("<categories>");


            for (int i = 1; i <= nMes; i++)
            {
                if (i == 1) { sMes = "Enero"; }
                if (i == 2) { sMes = "Febrero"; }
                if (i == 3) { sMes = "Marzo"; }
                if (i == 4) { sMes = "Abril"; }
                if (i == 5) { sMes = "Mayo"; }
                if (i == 6) { sMes = "Junio"; }
                if (i == 7) { sMes = "Julio"; }
                if (i == 8) { sMes = "Agosto"; }
                if (i == 9) { sMes = "Septiembre"; }
                if (i == 10) { sMes = "Octubre"; }
                if (i == 11) { sMes = "Noviembre"; }
                if (i == 12) { sMes = "Diciembre"; }



                xmlData.Append("<category label='" + sMes + "'/>");

            }

            xmlData.Append("</categories>");




            //Todo
            xmlData.Append("<dataset seriesName='Solicitudes' color='01DF01' anchorBorderColor='01DF01'>");
            for (int i = 1; i <= nMes; i++)
            {
                if (i == 1) { sMes = "Enero"; }
                if (i == 2) { sMes = "Febrero"; }
                if (i == 3) { sMes = "Marzo"; }
                if (i == 4) { sMes = "Abril"; }
                if (i == 5) { sMes = "Mayo"; }
                if (i == 6) { sMes = "Junio"; }
                if (i == 7) { sMes = "Julio"; }
                if (i == 8) { sMes = "Agosto"; }
                if (i == 9) { sMes = "Septiembre"; }
                if (i == 10) { sMes = "Octubre"; }
                if (i == 11) { sMes = "Noviembre"; }
                if (i == 12) { sMes = "Diciembre"; }


                xmlData.Append("<set value='" +5 + "'/>");

            }
            xmlData.Append("</dataset>");


            xmlData.Append("<styles>");
            xmlData.Append("<definition>");
            xmlData.Append("<style type='font' name='CaptionFont' size='20' color='000000'/>");
            xmlData.Append("<style type='font' name='SubCaptionFont' size='20' color='000000' bold='0'/>");
            xmlData.Append("<style type='font' name='DataLabelsFont' size='10' color='000000' bold='1'/>");
            xmlData.Append("</definition>");
            xmlData.Append("<application>");
            xmlData.Append("<apply toObject='caption' styles='CaptionFont'/>");
            xmlData.Append("<apply toObject='SubCaption' styles='SubCaptionFont'/>");
            xmlData.Append("<apply toObject='DataLabels' styles='DataLabelsFont'/>");
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
