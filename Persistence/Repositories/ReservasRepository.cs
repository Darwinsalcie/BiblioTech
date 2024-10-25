
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
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Repositories
{
    public class ReservasRepository : BaseRepository<Reservas, int>, IReservasRepository
    {
        private readonly ILogger<ReservasRepository> logger;
        private readonly IConfiguration configuration;
        private readonly BiblioTechDb biblioTechDb;

        public ReservasRepository(BiblioTechDb bibliotechDb,
                                        ILogger<ReservasRepository> logger,
                                        IConfiguration configuration) : base(bibliotechDb)
        {
            this.biblioTechDb = bibliotechDb;
            this.logger = logger;
            this.configuration = configuration;
        }


        public async Task<DataResults<List<ReservasModel>>> GetAll()
        {
            var results = new DataResults<List<ReservasModel>>();

            try
            {
                var query = await GetReservasBaseQuery().ToListAsync();
                results.Result = query;
                results.Sucess = true;

            }

            catch (Exception ex)
            {
                results.Message = this.configuration[GetEntityName() + ":get_courses"];
                results.Sucess = false;
                this.logger.LogError(this.configuration[GetEntityName() + ":get_courses"], ex.ToString());
            }

            return results;
        }

        public async Task<List<ReservasModel>> GetReservasById(int Id)
        {
            List<ReservasModel> result = new List<ReservasModel>();
            try
            {
                var librosQuery = GetReservasBaseQuery()
                    .Where(reserv => reserv.Id == Id);

                result = await librosQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }

        public async Task<List<ReservasModel>> GetReservasByUserId(int UserId)
        {
            List<ReservasModel> result = new List<ReservasModel>();
            try
            {
                var librosQuery = GetReservasBaseQuery()
                    .Where(reserv => reserv.UserId == UserId);

                result = await librosQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }


        private IQueryable<ReservasModel> GetReservasBaseQuery()
        {
            return from reserv in this.biblioTechDb.Reservas
                   where reserv.isDeleted != true
                   select new ReservasModel()
                   {
                       Id = reserv.Id,
                       UserId = reserv.UserId,
                       ReservationDate = reserv.ReservationDate,
                       Status = reserv.Status,

                   };
        }

        public override async Task<bool> Create(Reservas entity)
        {
            bool result = false;
            try
            {
                if (await base.Exists(reserv => reserv.Id == entity.Id))

                    throw new EntityDataException(this.configuration[GetEntityName + ":id_exists"]);
                result = await base.Create(entity);



            }
            catch (Exception ex)
            {
                result = false;
                this.logger.LogError(this.configuration[GetEntityName + ":error_create"], ex.ToString());
            }
            return result;
        }

        public async override Task<bool> Update(Reservas entity)
        {
            bool result = false;
            try
            {
                // Buscar la entidad existente en la base de datos
                Reservas? reservaToUpdate = await this.biblioTechDb.Reservas.FirstOrDefaultAsync();

                // Comprobar si la entidad fue encontrada
                if (reservaToUpdate != null)
                {
                    // Actualizar las propiedades manualmente
                    reservaToUpdate.Id = entity.Id;
                    reservaToUpdate.UserId = entity.UserId;
                    reservaToUpdate.BookId = entity.BookId;
                    reservaToUpdate.ReservationDate = entity.ReservationDate;
                    reservaToUpdate.Status = entity.Status;
                    reservaToUpdate.ModifyUser = entity.ModifyUser; // Actualizar usuario de modificación
                    reservaToUpdate.ModifyDate = DateTime.UtcNow; // Asignar la fecha de modificación

                    // Guardar los cambios en la base de datos
                    result = await base.Update(reservaToUpdate);
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
                Reservas? reservaToRemove = await this.biblioTechDb.Reservas.FindAsync(id);
                if (reservaToRemove != null)
                {
                    reservaToRemove.isDeleted = true; // Marcar como eliminado
                    result = await base.Update(reservaToRemove);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[GetEntityName() + ":error_remove"], ex.ToString());

            }
            return result;
        }

    }
}
