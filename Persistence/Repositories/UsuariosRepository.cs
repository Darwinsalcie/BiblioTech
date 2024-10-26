

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
    public class UsuariosRepository : BaseRepository<Usuarios, int>, IUsuariosRepository
    {
        private readonly ILogger<UsuariosRepository> logger;
        private readonly IConfiguration configuration;
        private readonly BiblioTechDb biblioTechDb;
        public UsuariosRepository(BiblioTechDb bibliotechDb,
                                        ILogger<UsuariosRepository> logger,
                                        IConfiguration configuration) : base(bibliotechDb)
        {
            this.biblioTechDb = bibliotechDb;
            this.logger = logger;
            this.configuration = configuration;
        }



        public async Task<List<UsuariosModel>> GetUsuariosByName(string usuario)
        {
            List<UsuariosModel> result = new List<UsuariosModel>();
            try
            {
                var usuarioQuery = GetUsuariosBaseQuery()
                    .Where(us => us.Name == usuario);

                result = await usuarioQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }

        public async Task<List<UsuariosModel>> GetUsuariosByRoleName(string RoleName)
        {
            List<UsuariosModel> result = new List<UsuariosModel>();
            try
            {
                var usuarioQuery = GetUsuariosBaseQuery()
                    .Where(us => us.RoleName == RoleName);

                result = await usuarioQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }

        public async Task<List<UsuariosModel>> GetUsuarioById(int Id)
        {
            List<UsuariosModel> result = new List<UsuariosModel>();
            try
            {
                var usuarioQuery = GetUsuariosBaseQuery()
                    .Where(us => us.Id == Id);

                result = await usuarioQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }

        public async Task<DataResults<List<UsuariosModel>>> GetAll()
        {
            var results = new DataResults<List<UsuariosModel>>();

            try
            {
                var query = await GetUsuariosBaseQuery().ToListAsync();
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

        public override async Task<bool> Create(Usuarios entity)
        {
            bool result = false;
            try
            {
                if (await base.Exists(usuario => usuario.Id == entity.Id))

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

        public override async Task<bool> Update(Usuarios entity)
        {
            bool result = false;
            try
            {
                // Buscar la entidad existente en la base de datos
                Usuarios? usuarioToUpdate = await this.biblioTechDb.Usuarios.FindAsync(entity.Id);

                // Comprobar si la entidad fue encontrada
                if (usuarioToUpdate != null)
                {
                    // Actualizar las propiedades manualmente
                    usuarioToUpdate.Name = entity.Name;
                    usuarioToUpdate.eMail = entity.eMail;
                    usuarioToUpdate.Password = entity.Password;
                    usuarioToUpdate.RoleName = entity.RoleName;
                    usuarioToUpdate.RoleId = entity.RoleId;
                    usuarioToUpdate.ModifyUser = entity.ModifyUser;
                    usuarioToUpdate.ModifyDate = DateTime.UtcNow;

                    result = await base.Update(usuarioToUpdate);

                    // Guardar los cambios en la base de datos
                    result = await base.Update(usuarioToUpdate);
                }
                else
                {
                    // Manejar el caso en que no se encuentra el libro
                    this.logger.LogWarning($"Usuario con ID {entity.Id} no encontrado.");
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
                Usuarios? usuarioToRemove = await this.biblioTechDb.Usuarios.FindAsync(id);
                if (usuarioToRemove != null)
                {
                    usuarioToRemove.isDeleted = true; // Marcar como eliminado
                    result = await base.Update(usuarioToRemove);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[GetEntityName() + ":error_remove"], ex.ToString());

            }
            return result;
        }
        private IQueryable<UsuariosModel> GetUsuariosBaseQuery()
        {
            return from us in this.biblioTechDb.Usuarios
                   where us.isDeleted != true
                   select new UsuariosModel()
                   {
                       Id = us.Id,
                       Name = us.Name,
                       eMail = us.eMail,
                       Password = us.Password,
                       RoleName = us.RoleName,

                   };
        }
    }
}
