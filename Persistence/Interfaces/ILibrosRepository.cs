

using BiblioTech.Domain.Entities;
using BiblioTech.Domain.Interfaces;
using BiblioTech.Domain.Models;
using Persistence.Models;

namespace Persistence.Interfaces
{
    
    /// <summary>
    /// Metodods especificos para Libro
    /// </summary>
    
    public interface ILibrosRepository : IRepository<Libros, int>
    {
        Task<DataResults<List<LibrosModel>>> GetAll();
        Task<List<LibrosModel>> GetLibrosByAuthor(string autor);
        Task<List<LibrosModel>> GetLibrosByGenero(string genero);
        Task<List<LibrosModel>> GetLibrosByYear(int year);
        Task<DataResults<LibrosModel>> GetLibroByName(string Tittle);
        Task<DataResults<LibrosModel>> GetLibroById(int id);
        Task<bool> Create(Libros entity);
        Task<bool> Update(Libros entity);
        Task<bool> Remove(int id);

    }
}
