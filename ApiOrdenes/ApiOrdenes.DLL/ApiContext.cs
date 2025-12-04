using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiOrdenes.DLL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiOrdenes.DLL
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

        public virtual DbSet<Orden> Ordenes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Se ejecuta en el program.cs porque se usa migración por consola
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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