using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetroOperaciones.Models
{
    public class DocumentoOperaciones
    {

        public bool ChequeoRealizado { get; set; }
        public String NombreUsuarioFinalizadorDO { get; set; }

        public int DocumentoOperacionesID { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(14, ErrorMessage = "Longitud Máxima 14")]
        public String NumeroDO { get; set; }
        public int ClienteID { get; set; }
        public int PaisID { get; set; }
        public int PaisDestinoID { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(400, ErrorMessage = "Longitud Máxima 400")]
        public String ClaseMercancia { get; set; }
        public int TipoContenedorID { get; set; }
        public int TipoModalidadAduaneraID { get; set; }

        [RegularExpression(@"^(\d{1,3}.(\d{3}.)*\d{3}(\,\d{1,3})?|\d{1,3}(\,\d{1,5})?)$", ErrorMessage = "El Peso debe ser un número")]
        [Required(ErrorMessage = "*")]
        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        public String PesoBruto { get; set; }
        public int NoPiezas { get; set; }
        public int TipoPiezaID { get; set; }

        [RegularExpression(@"^(\d{1,3}.(\d{3}.)*\d{3}(\,\d{1,3})?|\d{1,3}(\,\d{1,5})?)$", ErrorMessage = "El Volumen debe ser un número")]
        [Required(ErrorMessage = "*")]
        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        public String Volumen { get; set; }

        public String TipoUnidadVolumenID { get; set; }

        [RegularExpression(@"^(\d{1,3}.(\d{3}.)*\d{3}(\,\d{1,3})?|\d{1,3}(\,\d{1,5})?)$", ErrorMessage = "El Valor debe ser un número")]
        [Required(ErrorMessage = "*")]
        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]        
        public String Valor { get; set; }
        public String TipoMonedaID { get; set; }

        [Required(ErrorMessage = "*")]
        public int MedioTransporteID { get; set; }
        [StringLength(30, ErrorMessage = "Longitud Máxima 30")]
        public String AWB { get; set; }

        [StringLength(30, ErrorMessage = "Longitud Máxima 30")]
        public String HAWB { get; set; }

        [StringLength(30, ErrorMessage = "Longitud Máxima 30")]
        public String BL { get; set; }

        [StringLength(30, ErrorMessage = "Longitud Máxima 30")]
        public String HBL { get; set; }

        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        public String Buque { get; set; }

        [StringLength(15, ErrorMessage = "Longitud Máxima 15")]
        public String Vuelo { get; set; }

        public int TransportadorID { get; set; }
        public int TransportadorNacionalID { get; set; }
        public int FreightForwarderID { get; set; }
        public int TipoModalidadID { get; set; }

        [StringLength(99, ErrorMessage = "Longitud Máxima 100")]
        public String Shipper { get; set; }

        [StringLength(99, ErrorMessage = "Longitud Máxima 100")]
        public String NoFacturaShipper { get; set; }


        public int PuertoOrigen { get; set; }
        public int PuertoDestino { get; set; }
        public int Estado { get; set; }
        public int UsuarioCreadorID { get; set; }
        public int UsuarioResponsableID { get; set; }
        [Required(ErrorMessage = "*")]

        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaRegistro { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaCierre { get; set; }
        public int TipoOperacionID { get; set; }
        public DateTime? FechaFinalizacion { get; set; }

        [StringLength(190, ErrorMessage = "Longitud Máxima 190")]
        public String Observaciones { get; set; }


        [StringLength(20, ErrorMessage = "Longitud Máxima 20")]
        public String PosicionArancelaria { get; set; }


        public virtual Cliente Cliente { get; set; }
        public virtual Transportador Transportador { get; set; }
        public virtual Transportador TransportadorNacional { get; set; }
        public virtual Transportador FreightForwarder { get; set; }
        public virtual MedioTransporte MedioTransporte { get; set; }
        public virtual TipoContenedor TipoContenedor { get; set; }
        public virtual TipoPieza TipoPieza { get; set; }
        public virtual TipoUnidadVolumen TipoUnidadVolumen { get; set; }
        public virtual Pais Pais { get; set; }
        public virtual Pais PaisD { get; set; }

        public virtual Puerto PuertoO { get; set; }
        public virtual Puerto PuertoD { get; set; }

        public virtual Usuario UsuarioC { get; set; }
        public virtual Usuario UsuarioR { get; set; }

        public virtual EstadoDO EstadoDO { get; set; }

        public virtual TipoModalidad TipoModalidad { get; set; }
        public virtual TipoModalidadAduanera TipoModalidadAduanera { get; set; }
        public virtual TipoOperacion TipoOperacion { get; set; }
        public virtual TipoMoneda TipoMoneda { get; set; }

        public String getDocumentosTransporte()
        {
            String documentosTransporte = "";

            if (this.AWB != null)
            {
                documentosTransporte = "AWB "+this.AWB;
            }
            if (this.HAWB != null)
            {
                documentosTransporte += " HAWB " + this.HAWB;
            }

            if (this.BL != null)
            {
                documentosTransporte += " BL " + this.BL;
            }


            if (this.HBL != null)
            {
                documentosTransporte += " HBL " + this.HBL;
            }

            return documentosTransporte;
        }

        public String getResumenPiezas()
        {
            String rPiezas = "";
            String nombreEmpaque = "";

            if (this.TipoPieza != null)
            {
                nombreEmpaque = TipoPieza.TipoPiezaNm;
            }
 


            rPiezas = this.NoPiezas + " " + nombreEmpaque;
            return rPiezas;
            
        }
    }
}