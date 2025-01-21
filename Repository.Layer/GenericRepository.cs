using Data.Layer.Contexts;
using Data.Layer.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Layer.Interfaces;
using Repository.Layer.Specification;

namespace Repository.Layer
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecs(ISpecification<TEntity> specs)
        {
            return await SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>().AsQueryable(), specs).ToListAsync();
        }

        public async Task<TEntity> GetById(TKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetByIdWithSpecs(ISpecification<TEntity> specs)
        {
            return await SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>().AsQueryable(), specs).FirstOrDefaultAsync();
        }

        public async Task Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
