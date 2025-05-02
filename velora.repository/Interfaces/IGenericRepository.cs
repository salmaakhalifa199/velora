using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using velora.core.Data;
using velora.core.Entities;
using velora.repository.Specifications;

namespace velora.repository.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey id);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> Spec);
        Task<TEntity> GetByIdWithSpecAsync(ISpecifications<TEntity> Spec);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> CountAsync(ISpecifications<TEntity> spec);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    }

}
