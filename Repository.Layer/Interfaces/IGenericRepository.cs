using Data.Layer.Entities;
using Repository.Layer.Specification;

namespace Repository.Layer.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Task<IEnumerable<TEntity>> GetAll();
        public Task<IEnumerable<TEntity>> GetAllWithSpecs(ISpecification<TEntity> spec);
        public Task<TEntity> GetById(TKey id);
        public Task<TEntity> GetByIdWithSpecs(ISpecification<TEntity> spec);
        public Task Add(TEntity entity);
        public Task Update(TEntity entity);
        public Task Delete(TEntity entity);
    }
}
