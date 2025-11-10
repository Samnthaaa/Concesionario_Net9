using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Models;

namespace CrudRazorApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Auto> Autos { get; set; }
        public DbSet<Conductor> Conductores { get; set; }
        public DbSet<ReservaAuto> ReservasAuto { get; set; }
        public DbSet<Mantenimiento> Mantenimientos { get; set; }
        public DbSet<ReporteVehiculo> ReportesVehiculo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Asegurar los nombres de tablas
            modelBuilder.Entity<Auto>().ToTable("Auto");
            modelBuilder.Entity<Conductor>().ToTable("Conductor");
            modelBuilder.Entity<ReservaAuto>().ToTable("ReservaAuto");
            modelBuilder.Entity<Mantenimiento>().ToTable("Mantenimiento");
            modelBuilder.Entity<ReporteVehiculo>().ToTable("ReporteVehiculo");

            // Auto.id es IDENTITY
            modelBuilder.Entity<Auto>().Property(a => a.Id).ValueGeneratedOnAdd();

            // ============================================
            // RELACIONES CON RESTRICT
            // ============================================
            // Esto evita el cascade delete automático.
            // Si intentas eliminar un Auto/Conductor que tiene reservas,
            // recibirás un error y deberás eliminar las reservas primero.

            // ReservaAuto -> Auto (RESTRICT)
            modelBuilder.Entity<ReservaAuto>()
                .HasOne(r => r.Auto)
                .WithMany(a => a.Reservas)
                .HasForeignKey(r => r.AutoId)
                .HasConstraintName("FK_ReservaAuto_Auto")
                .OnDelete(DeleteBehavior.Restrict); // Cambiado a Restrict

            // ReservaAuto -> Conductor (RESTRICT)
            modelBuilder.Entity<ReservaAuto>()
                .HasOne(r => r.Conductor)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ConductorId)
                .HasConstraintName("FK_ReservaAuto_Conductor")
                .OnDelete(DeleteBehavior.Restrict); // Cambiado a Restrict

            // Mantenimiento -> Auto (RESTRICT)
            modelBuilder.Entity<Mantenimiento>()
                .HasOne(m => m.Auto)
                .WithMany(a => a.Mantenimientos)
                .HasForeignKey(m => m.AutoId)
                .OnDelete(DeleteBehavior.Restrict); // Cambiado a Restrict

            // Mantenimiento -> Conductor (RESTRICT)
            modelBuilder.Entity<Mantenimiento>()
                .HasOne(m => m.Conductor)
                .WithMany(c => c.Mantenimientos)
                .HasForeignKey(m => m.ConductorId)
                .OnDelete(DeleteBehavior.Restrict); // Cambiado a Restrict

            // ReporteVehiculo -> Auto (RESTRICT)
            modelBuilder.Entity<ReporteVehiculo>()
                .HasOne(rv => rv.Auto)
                .WithMany(a => a.ReportesVehiculo)
                .HasForeignKey(rv => rv.AutoId)
                .OnDelete(DeleteBehavior.Restrict); // Cambiado a Restrict
        }
    }
}