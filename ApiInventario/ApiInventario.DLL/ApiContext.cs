using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiInventario.DLL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiInventario.DLL
{
    public partial class ApiContext : DbContext
    {
        public ApiContext()
        {
        }

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Orden> Ordenes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Se ejecuta en el program.cs porque se usa migración por consola
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Producto");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Nombre).HasMaxLength(200);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Orden>(entity =>
            {
                entity.ToTable("Orden");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
                entity.Property(e => e.Cantidad);
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.Estado).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}