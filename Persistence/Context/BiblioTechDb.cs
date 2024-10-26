

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
        public DbSet<Notificaciones> Notificaciones { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para establecer el valor predeterminado de isDeleted
            modelBuilder.Entity<Libros>()
                .Property(libro => libro.isDeleted)
                .HasDefaultValue(false); // Valor predeterminado

            modelBuilder.Entity<Notificaciones>()
                .Property(notificacion => notificacion.isDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Reservas>()
                .Property(reserv => reserv.isDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Roles>()
                .Property(rol => rol.isDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Usuarios>()
                .Property(us => us.isDeleted)
                .HasDefaultValue(false);


            base.OnModelCreating(modelBuilder);
        }


    }
}
