using Data.Layer.Contexts;
using Data.Layer.Entities;
using Repository.Layer.Interfaces;
using System.Collections;

namespace Repository.Layer
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var EntityKey = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(typeof(TEntity).Name))
            {
                var RepoType = typeof(GenericRepository<,>);
                var RepoInstance = Activator.CreateInstance(RepoType.MakeGenericType(typeof(TEntity), typeof(TKey)), _context);
                _repositories.Add(typeof(TEntity).Name, RepoInstance);
            }
            return (IGenericRepository<TEntity, TKey>)_repositories[EntityKey];
        }
    }
}
