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
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

    }
}
