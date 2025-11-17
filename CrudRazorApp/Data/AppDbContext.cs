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
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Asegurar los nombres de tablas
            modelBuilder.Entity<Auto>().ToTable("Auto");
            modelBuilder.Entity<Conductor>().ToTable("Conductor");
            modelBuilder.Entity<ReservaAuto>().ToTable("ReservaAuto");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");

            // Auto.id es IDENTITY
            modelBuilder.Entity<Auto>().Property(a => a.Id).ValueGeneratedOnAdd();

            // Usuario - Configuración adicional
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // RELACIONES CON RESTRICT
            // ReservaAuto -> Auto (RESTRICT)
            modelBuilder.Entity<ReservaAuto>()
                .HasOne(r => r.Auto)
                .WithMany(a => a.Reservas)
                .HasForeignKey(r => r.AutoId)
                .HasConstraintName("FK_ReservaAuto_Auto")
                .OnDelete(DeleteBehavior.Restrict);

            // ReservaAuto -> Conductor (RESTRICT)
            modelBuilder.Entity<ReservaAuto>()
                .HasOne(r => r.Conductor)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ConductorId)
                .HasConstraintName("FK_ReservaAuto_Conductor")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}