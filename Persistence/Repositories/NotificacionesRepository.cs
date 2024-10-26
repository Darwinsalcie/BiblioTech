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


namespace Persistence.Repositories
{
    public class NotificacionesRepository : BaseRepository<Notificaciones, int>, INotificacionesRepository
    {
        private readonly ILogger<NotificacionesRepository> logger;
        private readonly IConfiguration configuration;
        private readonly BiblioTechDb biblioTechDb;

        public NotificacionesRepository(BiblioTechDb bibliotechDb,
                                        ILogger<NotificacionesRepository> logger,
                                        IConfiguration configuration) : base(bibliotechDb)
        {
            this.biblioTechDb = bibliotechDb;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<DataResults<List<NotificacionesModel>>> GetAll()
        {
            var results = new DataResults<List<NotificacionesModel>>();

            try
            {
                var query = await GetNotificacionesBaseQuery().ToListAsync();
                results.Result = query;
                results.Sucess = true;
            }
            catch (Exception ex)
            {
                results.Message = this.configuration[GetEntityName() + ":get_notificaciones"];
                results.Sucess = false;
                this.logger.LogError(this.configuration[GetEntityName() + ":get_notificaciones"], ex.ToString());
            }

            return results;
        }

        public async Task<List<NotificacionesModel>> GetNotificacionesByUsuarioDestino(int UserId)
        {
            List<NotificacionesModel> result = new List<NotificacionesModel>();
            try
            {
                var notificacionesQuery = GetNotificacionesBaseQuery()
                    .Where(notif => notif.UserId == UserId);

                result = await notificacionesQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }

        public async Task<List<NotificacionesModel>> GetNotificacionById(int id)
        {
            List<NotificacionesModel> result = new List<NotificacionesModel>();
            try
            {
                var notificacionesQuery = GetNotificacionesBaseQuery()
                    .Where(notif => notif.Id == id);

                result = await notificacionesQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }
    
        public override async Task<bool> Create(Notificaciones entity)
        {
            bool result = false;
            try
            {
                if(await base.Exists(notificacion => notificacion.Id == entity.Id))

                    throw new EntityDataException(this.configuration[GetEntityName + ":id_exists"]);

                result = await base.Create(entity);
            }
            catch (Exception ex)
            {
                result = false;
                this.logger.LogError(this.configuration[GetEntityName() + ":error_create"], ex.ToString());
            }
            return result;
        }

        public override async Task<bool> Update(Notificaciones entity)
        {
            bool result = false;
            try
            {
                // Buscar la entidad existente en la base de datos
                Notificaciones? notificacionToUpdate = await this.biblioTechDb.Notificaciones.FindAsync(entity.Id);

                // Comprobar si la entidad fue encontrada
                if (notificacionToUpdate != null)
                {
                    // Actualizar las propiedades manualmente
                    notificacionToUpdate.Type = entity.Type;
                    notificacionToUpdate.Message = entity.Message;
                    notificacionToUpdate.UserId = entity.UserId;
                    notificacionToUpdate.FechaEnvio = entity.FechaEnvio;
                    notificacionToUpdate.ModifyUser = entity.ModifyUser;
                    notificacionToUpdate.ModifyDate = DateTime.UtcNow;

                    result = await base.Update(notificacionToUpdate);

                    // Guardar los cambios en la base de datos
                    result = await base.Update(notificacionToUpdate);
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

        public override async Task<bool> Remove(int id)
        {
            bool result = false;
            try
            {
                Notificaciones? notificacionToRemove = await this.biblioTechDb.Notificaciones.FindAsync(id);
                if (notificacionToRemove != null)
                {
                    notificacionToRemove.isDeleted = true; // Marcar como eliminado
                    result = await base.Update(notificacionToRemove);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[GetEntityName() + ":error_remove"], ex.ToString());

            }
            return result;
        }
        private IQueryable<NotificacionesModel> GetNotificacionesBaseQuery()
        {
            return from n in this.biblioTechDb.Notificaciones
                   where n.isDeleted != true
                   select new NotificacionesModel
                   {
                       Id = n.Id,
                       Type = n.Type,
                       Message = n.Message,
                       UserId = n.UserId,
                       FechaEnvio = n.FechaEnvio,

                   };
        }
    }
}
