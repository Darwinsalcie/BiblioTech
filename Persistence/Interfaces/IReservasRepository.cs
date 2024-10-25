

using BiblioTech.Domain.Entities;
using BiblioTech.Domain.Interfaces;
using BiblioTech.Domain.Models;
using Persistence.Models;

namespace Persistence.Interfaces
{
    public interface IReservasRepository : IRepository<Reservas, int>
    {
        Task<DataResults<List<ReservasModel>>> GetAll();
        Task<List<ReservasModel>> GetReservasById(int Id);
        Task<List<ReservasModel>> GetReservasByUserId(int Id);
        Task<bool> Create(Reservas entity);
        Task<bool> Update(Reservas entity);
        Task<bool> Remove(int id);
    }
}
