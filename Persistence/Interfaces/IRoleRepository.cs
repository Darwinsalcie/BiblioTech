using BiblioTech.Domain.Entities;
using BiblioTech.Domain.Interfaces;
using BiblioTech.Domain.Models;
using Persistence.Models;


namespace Persistence.Interfaces
{
    public interface IRoleRepository : IRepository<Roles, int>
    {
        Task<DataResults<List<RolesModel>>> GetAll();
        Task<List<RolesModel>> GetRolesByRole(string rol);
        Task<List<RolesModel>> GetRoleById(int id);
        Task<bool> Create(Roles entity);
        Task<bool> Update(Roles entity);
        Task<bool> Remove(int id);
    }
}
