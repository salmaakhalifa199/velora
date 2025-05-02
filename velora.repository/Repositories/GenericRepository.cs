using Microsoft.EntityFrameworkCore;
using velora.core.Data;
using velora.repository.Interfaces;
using velora.repository.Specifications;
using velora.core.Data.Contexts;
using System.Linq.Expressions;
using velora.core.Entities;



namespace velora.repository.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreContext _dbContext;
        public GenericRepository(StoreContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();

        }
        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }
        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> Spec)
        {
            return await ApplySpecification(Spec).ToListAsync();
        }
        public async Task<TEntity> GetByIdWithSpecAsync(ISpecifications<TEntity> Spec)
        {
            return await ApplySpecification(Spec).FirstOrDefaultAsync();
        }
        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity> Spec)
        {
            return SpecificationEvaluator<TEntity, TKey>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), Spec);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }
        public async Task<int> CountAsync(ISpecifications<TEntity> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

  

    }
}

