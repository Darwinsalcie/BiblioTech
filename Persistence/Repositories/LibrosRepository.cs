
using BiblioTech.Domain.Entities;
using BiblioTech.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using Persistence.Exceptions;
using Persistence.Interfaces;
using Persistence.Models;
using Persistence.RepositoryBase;
using System.Data;

namespace Persistence.Repositories
{
    public class LibrosRepository : BaseRepository<Libros, int>, ILibrosRepository
    {
        private readonly ILogger<LibrosRepository> logger;
        private readonly IConfiguration configuration;
        private readonly BiblioTechDb biblioTechDb;
        public LibrosRepository(BiblioTechDb bibliotechDb,
                                 ILogger<LibrosRepository> logger,
                                 IConfiguration configuration) : base(bibliotechDb)

        {
            this.biblioTechDb = bibliotechDb;
            this.logger = logger;
            this.configuration = configuration;
        }


        public async Task<DataResults<List<LibrosModel>>> GetAll()
        {
            var results = new DataResults<List<LibrosModel>>();

            try
            {
                var query = await GetLibrosBaseQuery().ToListAsync();
                results.Result = query;
                results.Sucess = true;

            }

            catch (Exception ex) 
            {
                results.Message = this.configuration[GetEntityName()+":get_courses"];
                results.Sucess = false;
                this.logger.LogError(this.configuration[GetEntityName()+":get_courses"], ex.ToString());
            }

            return results;
        }

        public async Task<List<LibrosModel>> GetLibrosByAuthor(string autor)
        {
            List<LibrosModel> result = new List<LibrosModel>();
            try
            {
                var librosQuery = GetLibrosBaseQuery()
                    .Where(libro => libro.Autor == autor);

                result = await librosQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }

        public async Task<List<LibrosModel>> GetLibrosByGenero(string genero)
        {
            List<LibrosModel> result = new List<LibrosModel>();
            try
            {
                var librosQuery = GetLibrosBaseQuery()
                    .Where(libro => libro.Genero == genero);

                result = await librosQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[GetEntityName() + ":error_get_libros_by_genero"], ex.ToString());
                throw;
            }
            return result;
        }

        public async Task<List<LibrosModel>> GetLibrosByYear(int year)
        {
            List<LibrosModel> result = new List<LibrosModel>();
            try
            {

                var librosQuery = GetLibrosBaseQuery()
                    .Where(libro => libro.PublicationDate >= new DateTime(year, 1, 1) &&
                    libro.PublicationDate < new DateTime(year + 1, 1, 1));

                result = await librosQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[GetEntityName() + ":error_get_libros_by_year"], ex.ToString());

            }
            return result;
        }
        public async Task<DataResults<LibrosModel>> GetLibroByName(string Tittle)
        {
            DataResults<LibrosModel> result = new DataResults<LibrosModel>();
            try
            {
                var libro = GetLibrosBaseQuery()
                    .Where(libro => libro.Tittle == Tittle && libro.isDeleted != true);

            }
            catch (Exception ex)
            {
                result.Sucess = false;
                result.Message = this.configuration[GetEntityName() + ":error_get_libro_name"];
                this.logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<DataResults<LibrosModel>> GetLibroById(int id)
        {
            DataResults<LibrosModel> result = new DataResults<LibrosModel>();
            try
            {
                var libro = await base.GetByCondition(l => l.Id == id && l.isDeleted != true);


            }
            catch (Exception ex)
            {
                result.Sucess = false;
                result.Message = this.configuration["Libros:error_get_libro_by_id"];
                this.logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        private IQueryable<LibrosModel> GetLibrosBaseQuery()
        {
            return from libro in this.biblioTechDb.Libros
                   where libro.isDeleted != true
                   select new LibrosModel()
                   {
                       Id = libro.Id,
                       Tittle = libro.Tittle,
                       Autor = libro.Autor,
                       PublicationDate = libro.PublicationDate,
                       Status = libro.Status,
                       Genero = libro.Genero,
                       ISBN = libro.ISBN,
                       //CreationUser = libro.CreationUser,
                       //CreationDate = libro.CreationDate,
                       //ModifyDate = libro.ModifyDate,
                   };
        }

        public override async Task<bool> Create(Libros entity)
        {
            bool result = false;
            try
            {
                if (await base.Exists(libro => libro.Id == entity.Id))
                
                    throw new EntityDataException(this.configuration[GetEntityName + ":id_exists"]);
                    result = await base.Create(entity);
                

               
            }
            catch (Exception ex)
            {
                result = false;
                this.logger.LogError(this.configuration[GetEntityName+":error_create"], ex.ToString());
            }
            return result;
        }

        public async override Task<bool> Update(Libros entity)
        {
            bool result = false;
            try
            {
                // Buscar la entidad existente en la base de datos
                Libros? libroToUpdate = await this.biblioTechDb.Libros.FindAsync(entity.Id);

                // Comprobar si la entidad fue encontrada
                if (libroToUpdate != null)
                {
                    // Actualizar las propiedades manualmente
                    libroToUpdate.Tittle = entity.Tittle;
                    libroToUpdate.Autor = entity.Autor;
                    libroToUpdate.Genero = entity.Genero;
                    libroToUpdate.PublicationDate = entity.PublicationDate;
                    libroToUpdate.ExpireDate = entity.ExpireDate;
                    libroToUpdate.Status = entity.Status;
                    libroToUpdate.ModifyUser = entity.ModifyUser; // Actualizar usuario de modificación
                    libroToUpdate.ModifyDate = DateTime.UtcNow; // Asignar la fecha de modificación

                    // Guardar los cambios en la base de datos
                    result = await base.Update(libroToUpdate);
                }
                else
                {
                    // Manejar el caso en que no se encuentra el libro
                    this.logger.LogWarning($"Libro con ID {entity.Id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration["Libros:error_update"], ex.ToString());
            }
            return result;
        }

        public async override Task<bool> Remove(int id)
        {
            bool result = false;
            try
            {
                Libros? libroToRemove = await this.biblioTechDb.Libros.FindAsync(id);
                if (libroToRemove != null)
                {
                    libroToRemove.isDeleted = true; // Marcar como eliminado
                    result = await base.Update(libroToRemove);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[GetEntityName()+":error_remove"], ex.ToString());

            }
            return result;
        }


    }
}



