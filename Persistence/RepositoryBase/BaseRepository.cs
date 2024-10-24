
using BiblioTech.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.RepositoryBase
{
    public abstract class BaseRepository<TEntity, TType> : IRepository<TEntity, TType> where TEntity : class
    {

        private readonly BiblioTechDb _dbContext;
        private DbSet<TEntity> _dbSet;
        public BaseRepository(BiblioTechDb dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task<bool> Create(TEntity entity)
        {
            bool result = false;
            try
            {
                _dbSet.Add(entity);
                await _dbContext.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                // Manejar la excepción (puedes registrar el error o lanzar otra excepción personalizada)
                //Log.Error(ex.Message);
                result = false;
            }

            return result;
        }



        public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> filter)
        {
            bool result = false;    
            try
            {
                result = await _dbSet.AnyAsync(filter);

            }
            catch (Exception ex)
            {
                // Manejar la excepción
                // Log.Error(ex.Message);
                result = false;
            }
            return result;
        }



        public virtual async Task<List<TEntity>> GetAll()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                // Log.Error(ex.Message);
                return new List<TEntity>(); // Retornar una lista vacía en caso de error
            }
        }


        public virtual async Task<TEntity> GetByCondition(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                // Log.Error(ex.Message);
                return null; // Retornar null en caso de error
            }
        }


        public virtual async Task<bool> Remove(TType Id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(Id);
                if (entity == null) return false;

                _dbSet.Remove(entity);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                // Log.Error(ex.Message);
                return false;
            }
        }


        public virtual async Task<bool> Update(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                // Log.Error(ex.Message);
                return false;
            }
        }

        protected string GetEntityName()
        {
            return typeof(TEntity).Name;
        }


    }
}
