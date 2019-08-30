using Microsoft.EntityFrameworkCore;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.GenericRepository
{ 
	public interface IGenericRepository<TEntity> where TEntity : class
	{
        DbSet<TEntity> All();

		TEntity Add(TEntity entity);

        void AddRange(TEntity[] entities);

        void RemoveRange(TEntity[] entities);

        TEntity Update(TEntity entity);

		void Delete(TEntity entity);
	}
}




