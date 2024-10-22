

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


    }
}
