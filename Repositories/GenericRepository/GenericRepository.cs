using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly MarketDBContext _dbContext;

        public GenericRepository(MarketDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> All()
        {
            return _dbContext.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            return _dbContext.Set<TEntity>().Add(entity).Entity;
        }
        public void AddRange(TEntity[] entities)
        {
           _dbContext.Set<TEntity>().AddRange(entities);
        }

        public TEntity Update(TEntity entity)
        {
            return _dbContext.Set<TEntity>().Update(entity).Entity;
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(TEntity[] entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }
    }

}
