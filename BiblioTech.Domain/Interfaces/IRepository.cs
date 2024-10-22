

using BiblioTech.Domain.Entities;
using System.Linq.Expressions;

namespace BiblioTech.Domain.Interfaces
{
    public interface IRepository<TEntity, TType> where TEntity : class
    {
        //TEntity: Tipo de entidad
        //TType: Tipo de dato el Id

        /// <summary>
        /// Metodo para guardar una entidad
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Create(TEntity entity);

        /// <summary>
        /// Retorna lista de tipo TEntity
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAll();

        /// <summary>
        /// Busca una entidad que cumple con el filtro especificado.
        /// Recibe una expresión que representa una función (Func) que toma una entidad
        /// y devuelve un valor booleano que indica si la entidad cumple con la condición.
        /// 
        /// El parámetro "filter" encapsula la expresión lambda que define la condición
        /// que se usará para buscar la entidad en la fuente de datos. Esta expresión
        /// se evalúa en el contexto de la consulta y se traduce en una consulta en la base de datos.
        /// 
        /// </summary>
        /// <param name="filter">Expresión que define el criterio de búsqueda.</param>
        /// <returns>Devuelve una tarea que representa la operación asincrónica, 
        /// con el resultado de tipo TEntity que coincide con el filtro, o null si no se encuentra.</returns>

        Task<TEntity> GetByCondition(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity);

        /// <summary>
        /// Remove method
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<bool> Remove(TType Id);

        /// <summary>
        /// Un filtro de tipo Expression que define una condición (por ejemplo, buscar 
        /// * libros con un nombre específico)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<bool> Exists(Expression<Func<TEntity, bool>> filter);





    }
}
