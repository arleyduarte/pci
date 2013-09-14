using System.Data.Entity;

using System.Data.Entity.ModelConfiguration;

using System.Data.Entity.ModelConfiguration.Conventions;
using PetroOperaciones.Models.Facturacion;
using PetroOperaciones.Models.Cotizacion;

namespace PetroOperaciones.Models
{
    public class EfDbContext : DbContext
    {



        public DbSet<Pais> Paises { get; set; }

        public DbSet<Alertas> Alertas { get; set; }
        public DbSet<TipoOperacion> TiposOperaciones { get; set; }
        public DbSet<TipoModalidad> TipoModalidad { get; set; }
        public DbSet<TipoModalidadAduanera> TipoModalidadAduanera { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<DocumentoOperaciones> DocumentoOperaciones { get; set; }
        public DbSet<Puerto> Puertos { get; set; }
        public DbSet<EstadoDO> Estados { get; set; }


        public DbSet<Usuario> Usuarios { get; set; }


        public DbSet<ChequeoDO> ChequeoDO { get; set; }
        public DbSet<Seguimiento> Seguimiento { get; set; }
        public DbSet<ArchivoDocumento> ArchivoDocumento { get; set; }
        public DbSet<ArchivoDocumentoTransportador> ArchivoDocumentoTransportador { get; set; }
        public DbSet<ArchivoDocumentoCliente> ArchivoDocumentoCliente { get; set; }
        public DbSet<Transportador> Transportadores { get; set; }


        //Facturacion
        public DbSet<Concepto> Concepto { get; set; }
        public DbSet<PrefacturaDetalle> PrefacturaDetalle { get; set; }
        public DbSet<PrefacturaEncabezado> PrefacturaEncabezado { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalle { get; set; }
        public DbSet<FacturaEncabezado> FacturaEncabezado { get; set; }
        public DbSet<ConfiguracionEmpresa> ConfiguracionEmpresa { get; set; }
        public DbSet<EstadoFactura> EstadoFactura { get; set; }
        public DbSet<PlazoFactura> PlazoFactura { get; set; }
        public DbSet<ConfiguracionImpuesto> ConfiguracionImpuesto { get; set; }

        //Cotizacion
        public DbSet<TipoConceptoCotizacion> TipoConceptoCotizacion { get; set; }
        public DbSet<CotizacionEncabezado> CotizacionEncabezado { get; set; }
        public DbSet<CotizacionDetalle> CotizacionDetalle { get; set; }
        public DbSet<AnticipoEncabezado> AnticipoEncabezado { get; set; }
        public DbSet<AnticipoDetalle> AnticipoDetalle { get; set; }

        public DbSet<TipoDeIndicador> TipoDeIndicador { get; set; }
        public DbSet<Indicador> Indicador { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TipoOperacion>().HasKey(b => b.IdTipoOperacion);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            modelBuilder.Entity<Usuario>()
                .HasRequired(n => n.TipoRol)
                .WithMany()
                .HasForeignKey(n => n.Rol);


            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.Pais)
                .WithMany()
                .HasForeignKey(n => n.PaisID);

            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.PaisD)
                .WithMany()
                .HasForeignKey(n => n.PaisDestinoID);


            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.PuertoD)
                .WithMany()
                .HasForeignKey(n => n.PuertoDestino);

            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.PuertoO)
                .WithMany()
                .HasForeignKey(n => n.PuertoOrigen);


            //Relación Transportadores
            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.Transportador)
                .WithMany()
                .HasForeignKey(n => n.TransportadorID);

            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.TransportadorNacional)
                .WithMany()
                .HasForeignKey(n => n.TransportadorNacionalID);

            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.FreightForwarder)
                .WithMany()
                .HasForeignKey(n => n.FreightForwarderID);

            modelBuilder.Entity<DocumentoOperaciones>()
            .HasRequired(n => n.TipoModalidadAduanera)
            .WithMany()
            .HasForeignKey(n => n.TipoModalidadAduaneraID);


            modelBuilder.Entity<DocumentoOperaciones>()
            .HasRequired(n => n.TipoModalidad)
            .WithMany()
            .HasForeignKey(n => n.TipoModalidadID);


            modelBuilder.Entity<ChequeoDO>()
                .HasRequired(n => n.DocumentoOperaciones)
                .WithMany()
                .HasForeignKey(n => n.DocumentoOperacionesID);

            //Usuarios
            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.UsuarioC)
                .WithMany()
                .HasForeignKey(n => n.UsuarioCreadorID);

            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.UsuarioR)
                .WithMany()
                .HasForeignKey(n => n.UsuarioResponsableID);

            modelBuilder.Entity<DocumentoOperaciones>()
                .HasRequired(n => n.EstadoDO)
                .WithMany()
                .HasForeignKey(n => n.Estado);

            modelBuilder.Entity<Contacto>()
                .HasRequired(n => n.Cliente)
                .WithMany()
                .HasForeignKey(n => n.TerceroID);

            //----------Facturacion
            modelBuilder.Entity<PrefacturaEncabezado>()
            .HasRequired(n => n.Usuario)
            .WithMany()
            .HasForeignKey(n => n.UsuarioCreadorID);

            modelBuilder.Entity<PrefacturaEncabezado>()
            .HasRequired(n => n.DocumentoOperaciones)
            .WithMany()
            .HasForeignKey(n => n.DocumentoOperacionesID);

            modelBuilder.Entity<PrefacturaEncabezado>()
            .HasRequired(n => n.Cliente)
            .WithMany()
            .HasForeignKey(n => n.ClienteID);

            modelBuilder.Entity<PrefacturaDetalle>()
            .HasRequired(n => n.TipoDeIngreso)
            .WithMany()
            .HasForeignKey(n => n.TipoDeIngresoID);

            modelBuilder.Entity<PrefacturaDetalle>()
            .HasRequired(n => n.PrefacturaEncabezado)
            .WithMany()
            .HasForeignKey(n => n.PrefacturaEncabezadoID);





            modelBuilder.Entity<FacturaEncabezado>()
            .HasRequired(n => n.Usuario)
            .WithMany()
            .HasForeignKey(n => n.UsuarioCreadorID);

            modelBuilder.Entity<FacturaEncabezado>()
            .HasRequired(n => n.Cliente)
            .WithMany()
            .HasForeignKey(n => n.ClienteID);

            modelBuilder.Entity<FacturaEncabezado>()
            .HasRequired(n => n.PlazoFactura)
            .WithMany()
            .HasForeignKey(n => n.PlazoFacturaID);

            modelBuilder.Entity<FacturaEncabezado>()
            .HasRequired(n => n.DocumentoOperaciones)
            .WithMany()
            .HasForeignKey(n => n.DocumentoOperacionesID);


            modelBuilder.Entity<FacturaEncabezado>()
            .HasRequired(n => n.EstadoFactura)
            .WithMany()
            .HasForeignKey(n => n.Estado);

            modelBuilder.Entity<FacturaDetalle>()
            .HasRequired(n => n.TipoDeIngreso)
            .WithMany()
            .HasForeignKey(n => n.TipoDeIngresoID);

            modelBuilder.Entity<FacturaDetalle>()
            .HasRequired(n => n.FacturaEncabezado)
            .WithMany()
            .HasForeignKey(n => n.FacturaEncabezadoID);


            modelBuilder.Entity<Indicador>()
            .HasRequired(n => n.TipoDeIndicador)
            .WithMany()
            .HasForeignKey(n => n.TipoDeIndicadorID);



            //Cotizacion----------------------------------------------
            modelBuilder.Entity<CotizacionEncabezado>()
            .HasRequired(n => n.Usuario)
            .WithMany()
            .HasForeignKey(n => n.UsuarioCreadorID);

            modelBuilder.Entity<CotizacionEncabezado>()
            .HasRequired(n => n.UsuarioAsesor)
            .WithMany()
            .HasForeignKey(n => n.AsesorComercialID);


            modelBuilder.Entity<CotizacionEncabezado>()
                .HasRequired(n => n.PuertoD)
                .WithMany()
                .HasForeignKey(n => n.PuertoDestino);

            modelBuilder.Entity<CotizacionEncabezado>()
                .HasRequired(n => n.PuertoO)
                .WithMany()
                .HasForeignKey(n => n.PuertoOrigen);


            modelBuilder.Entity<CotizacionDetalle>()
            .HasRequired(n => n.TipoConceptoCotizacion)
            .WithMany()
            .HasForeignKey(n => n.TipoConceptoCotizacionID);

            modelBuilder.Entity<CotizacionDetalle>()
            .HasRequired(n => n.CotizacionEncabezado)
            .WithMany()
            .HasForeignKey(n => n.CotizacionEncabezadoID);


            modelBuilder.Entity<AnticipoDetalle>()
            .HasRequired(n => n.TipoConceptoCotizacion)
            .WithMany()
            .HasForeignKey(n => n.TipoConceptoCotizacionID);

            modelBuilder.Entity<AnticipoDetalle>()
            .HasRequired(n => n.AnticipoEncabezado)
            .WithMany()
            .HasForeignKey(n => n.AnticipoEncabezadoID);

            modelBuilder.Entity<AnticipoEncabezado>()
            .HasRequired(n => n.DocumentoOperaciones)
            .WithMany()
            .HasForeignKey(n => n.DocumentoOperacionesID);

            modelBuilder.Entity<AnticipoEncabezado>()
            .HasRequired(n => n.Cliente)
            .WithMany()
            .HasForeignKey(n => n.ClienteID);

            modelBuilder.Entity<AnticipoEncabezado>()
                .HasRequired(n => n.Usuario)
                .WithMany()
                .HasForeignKey(n => n.UsuarioCreadorID);



          }

        public DbSet<TipoTransportador> TipoTransportadors { get; set; }

        public DbSet<MedioTransporte> MedioTransportes { get; set; }

        public DbSet<TipoContenedor> TipoContenedors { get; set; }

        public DbSet<TipoDocumento> TipoDocumentoes { get; set; }

        public DbSet<TipoDocumentoTercero> TipoDocumentoTerceros { get; set; }

        public DbSet<TipoRol> TipoRoles { get; set; }

        public DbSet<TipoPieza> TipoPiezas { get; set; }
        public DbSet<TipoUnidadVolumen> TipoUnidadVolumen { get; set; }
        public DbSet<TipoMoneda> TipoMoneda { get; set; }


       //------------------Facturación

        public DbSet<TipoDeIngreso> TipoDeIngreso { get; set; }


    }
}