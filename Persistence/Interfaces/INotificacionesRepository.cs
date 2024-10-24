using BiblioTech.Domain.Entities;
using BiblioTech.Domain.Interfaces;
using BiblioTech.Domain.Models;
using Persistence.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Interfaces
{
    /// <summary>
    /// Métodos específicos para Notificaciones
    /// </summary>
    public interface INotificacionesRepository : IRepository<Notificaciones, int>
    {
        Task<DataResults<List<NotificacionesModel>>> GetAll();
        Task<List<NotificacionesModel>> GetNotificacionesByUsuarioDestino(int UserId);
        Task<List<NotificacionesModel>> GetNotificacionById(int id);
        Task<bool> Create(Notificaciones entity);
        Task<bool> Update(Notificaciones entity);
        Task<bool> Remove(int id);
    }
}
