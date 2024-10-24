

using BiblioTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class BiblioTechDb : DbContext
    {
        public BiblioTechDb(DbContextOptions<BiblioTechDb> options) : base(options)
        {
        }

        public DbSet<Libros> Libros { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para establecer el valor predeterminado de isDeleted
            modelBuilder.Entity<Libros>()
                .Property(libro => libro.isDeleted)
                .HasDefaultValue(false); // Valor predeterminado

            base.OnModelCreating(modelBuilder);
        }


    }
}
