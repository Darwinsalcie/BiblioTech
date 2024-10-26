using BiblioTech.Domain.Entities;
using BiblioTech.Domain.Interfaces;
using BiblioTech.Domain.Models;
using Persistence.Models;


namespace Persistence.Interfaces
{
    public interface IUsuariosRepository : IRepository<Usuarios, int>
    {
        Task<DataResults<List<UsuariosModel>>> GetAll();
        Task<List<UsuariosModel>> GetUsuariosByName(string usuario);
        Task<List<UsuariosModel>> GetUsuariosByRoleName(string RoleName);
        Task<List<UsuariosModel>> GetUsuarioById(int id);
        Task<bool> Create(Usuarios entity);
        Task<bool> Update(Usuarios entity);
        Task<bool> Remove(int id);
    }
}
