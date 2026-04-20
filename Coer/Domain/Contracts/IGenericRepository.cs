using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{

    public interface IGenericRepository<TEntity, TKey> 
    {
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specifications);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(ISpecifications<TEntity> specifications);       
        Task AddAsync(TEntity entity);
        Task<int> CountAsync();
        void Update(TEntity entity);

        void Remove(TEntity entity);

    }
}
