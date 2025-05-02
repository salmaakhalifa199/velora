using velora.core.Data;
using velora.core.Entities;
using velora.core.Entities.IdentityEntities;
using velora.repository.Interfaces;
using velora.repository.Repositories;

namespace Store.Repository.Interfaces
{
    public interface IUnitWork
    {
        IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;
        //IGenericRepository<Person, string> PersonRepository();
        Task<int> CompleteAsync();
    }
}