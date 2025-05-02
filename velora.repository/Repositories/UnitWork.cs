using System;
using System.Collections;
using Store.Repository.Interfaces;
using velora.core.Data.Contexts;
using velora.core.Entities.IdentityEntities;
using velora.repository.Interfaces;
using velora.repository.Repositories;


namespace velora.Repository.Repositories
{
    public class UnitWork : IUnitWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;

        public UnitWork(StoreContext context)
        {
            _context = context;
        }

        async Task<int> IUnitWork.CompleteAsync()
        => await _context.SaveChangesAsync();

        IGenericRepository<TEntity, Tkey> IUnitWork.Repository<TEntity, Tkey>()
        {
            if(_repositories is null) 
                _repositories = new Hashtable();
            var entityKey = $"{typeof(TEntity).Name}_{typeof(Tkey).Name}";
            if (!_repositories.ContainsKey(entityKey))
            {
                var repositoryType = typeof(GenericRepository<,>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity),typeof(Tkey)), _context);
                _repositories.Add(entityKey, repositoryInstance);
            }
            return (IGenericRepository<TEntity, Tkey>) _repositories[entityKey]; 
        }
      
    }
}
