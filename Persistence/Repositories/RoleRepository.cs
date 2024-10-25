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
    public class RoleRepository : BaseRepository<Roles, int>, IRoleRepository
    {
        private readonly ILogger<RoleRepository> logger;
        private readonly IConfiguration configuration;
        private readonly BiblioTechDb biblioTechDb;
        public RoleRepository(BiblioTechDb bibliotechDb,
                                 ILogger<RoleRepository> logger,
                                 IConfiguration configuration) : base(bibliotechDb)

        {
            this.biblioTechDb = bibliotechDb;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<List<RolesModel>> GetRolesByRole(string role)
        {
            List<RolesModel> result = new List<RolesModel>();
            try
            {
                var rolesQuery = GetRoleBaseQuery()
                    .Where(rol => rol.Role == role);

                result = await rolesQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }

        public async Task<List<RolesModel>> GetRoleById(int Id)
        {
            List<RolesModel> result = new List<RolesModel>();
            try
            {
                var rolesQuery = GetRoleBaseQuery()
                    .Where(rol => rol.Id == Id);

                result = await rolesQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(this.configuration[":error_getting" + GetEntityName()], ex.ToString());
                throw;
            }
            return result;
        }

        public async Task<DataResults<List<RolesModel>>> GetAll()
        {
            var results = new DataResults<List<RolesModel>>();

            try
            {
                var query = await GetRoleBaseQuery().ToListAsync();
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
        private IQueryable<RolesModel> GetRoleBaseQuery()
        {
            return from rol in this.biblioTechDb.Roles
                   where rol.isDeleted != true
                   select new RolesModel()
                   {
                       Id = rol.Id,
                       Role = rol.Role,

                       //CreationUser = libro.CreationUser,
                       //CreationDate = libro.CreationDate,
                       //ModifyDate = libro.ModifyDate,
                   };
        }


        public override async Task<bool> Create(Roles entity)
        {
            bool result = false;
            try
            {
                if (await base.Exists(rol => rol.Id == entity.Id))

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

        public async override Task<bool> Update(Roles entity)
        {
            bool result = false;
            try
            {
                // Buscar la entidad existente en la base de datos
                Roles? roleToUpdate = await this.biblioTechDb.Roles.FindAsync(entity.Id);

                // Comprobar si la entidad fue encontrada
                if (roleToUpdate != null)
                {
                    // Actualizar las propiedades manualmente
                    roleToUpdate.Role = entity.Role;
                    roleToUpdate.Id = entity.Id;
                    roleToUpdate.ModifyDate = DateTime.UtcNow; // Asignar la fecha de modificación

                    // Guardar los cambios en la base de datos
                    result = await base.Update(roleToUpdate);
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
                Roles? roleToRemove = await this.biblioTechDb.Roles.FindAsync(id);
                if (roleToRemove != null)
                {
                    roleToRemove.isDeleted = true; // Marcar como eliminado
                    result = await base.Update(roleToRemove);
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
