using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EStoreX.Infrastructure.Repository
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TModel> _db;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _db = context.Set<TModel>();
        }

        public async Task<TModel> AddAsync(TModel entity)
        {
            await _db.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            _db.Remove(entity);
            int res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await _db.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync(params Expression<Func<TModel, object>>[] includes)
        {
            IQueryable<TModel> query = _db;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<TModel?> GetByIdAsync(Guid id)
        {
            return await _db.FindAsync(id);
        }

        public async Task<TModel?> GetByIdAsync(Guid id, params Expression<Func<TModel, object>>[] includes)
        {
            IQueryable<TModel> query = _db;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public Task<TModel> UpdateAsync(TModel entity)
        {
            _db.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return Task.FromResult(entity);
        }
    }
}
