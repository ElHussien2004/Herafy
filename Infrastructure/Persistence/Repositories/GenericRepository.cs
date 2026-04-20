using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey>(HerafyDbContext _dbContext)
        : IGenericRepository<TEntity, TKey>
        where TEntity : class
    {
        public async Task AddAsync(TEntity entity)
            => await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<int> CountAsync()=>await _dbContext.Set<TEntity>().CountAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specifications)
        {

            return await  QueryGenerator.CreateQuery(_dbContext.Set<TEntity>(), specifications).ToListAsync();
          
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity> specifications)
        {
            return await QueryGenerator.CreateQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        }
           
        
        public void Remove(TEntity entity)
            => _dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity)
            => _dbContext.Set<TEntity>().Update(entity);

    }
}