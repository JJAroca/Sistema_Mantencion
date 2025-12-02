using Microsoft.EntityFrameworkCore;
using SistemaMantencion.Models;

namespace SistemaMantencion.Data
{
    public class MantencionDbContext : DbContext
    {
        public MantencionDbContext(DbContextOptions<MantencionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Camioneta> Camionetas { get; set; }
        public DbSet<RegistroMantencion> Mantenciones { get; set; }
        public DbSet<HistorialCamioneta> Historial { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración Camioneta
            modelBuilder.Entity<Camioneta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Patente).IsUnique();
                entity.Property(e => e.Patente).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Marca).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Modelo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Estado).IsRequired();
            });

            // Configuración RegistroMantencion
            modelBuilder.Entity<RegistroMantencion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Costo).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.Camioneta)
                      .WithMany(c => c.Mantenciones)
                      .HasForeignKey(e => e.CamionetaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración HistorialCamioneta
            modelBuilder.Entity<HistorialCamioneta>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.Camioneta)
                      .WithMany()
                      .HasForeignKey(e => e.CamionetaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Datos iniciales
            modelBuilder.Entity<Camioneta>().HasData(
                new Camioneta 
                { 
                    Id = 1, 
                    Patente = "ABCD12", 
                    Marca = "Toyota", 
                    Modelo = "Hilux", 
                    Anio = 2022, 
                    Kilometraje = 15000, 
                    Estado = "Disponible",
                    FechaRegistro = DateTime.Now
                },
                new Camioneta 
                { 
                    Id = 2, 
                    Patente = "EFGH34", 
                    Marca = "Ford", 
                    Modelo = "Ranger", 
                    Anio = 2023, 
                    Kilometraje = 8000, 
                    Estado = "Disponible",
                    FechaRegistro = DateTime.Now
                },
                new Camioneta 
                { 
                    Id = 3, 
                    Patente = "IJKL56", 
                    Marca = "Chevrolet", 
                    Modelo = "Colorado", 
                    Anio = 2021, 
                    Kilometraje = 25000, 
                    Estado = "Disponible",
                    FechaRegistro = DateTime.Now
                },
                new Camioneta 
                { 
                    Id = 4, 
                    Patente = "MNOP78", 
                    Marca = "Nissan", 
                    Modelo = "Frontier", 
                    Anio = 2022, 
                    Kilometraje = 18000, 
                    Estado = "Disponible",
                    FechaRegistro = DateTime.Now
                },
                new Camioneta 
                { 
                    Id = 5, 
                    Patente = "QRST90", 
                    Marca = "Mitsubishi", 
                    Modelo = "L200", 
                    Anio = 2023, 
                    Kilometraje = 5000, 
                    Estado = "Disponible",
                    FechaRegistro = DateTime.Now
                }
            );
        }
    }
}