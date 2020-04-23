using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Enxaquecapp.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Enxaquecapp.Data
{
    public class GenericRepository<T> where T : Entity
    {
        private ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return await _context.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateRangeAsync(IEnumerable<T> entity)
        {
            _context.Set<T>().UpdateRange(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteRangeAsync(IEnumerable<T> entity)
        {
            _context.Set<T>().RemoveRange(entity);
            return _context.SaveChangesAsync();
        }

        public IQueryable<T> GetQueryable()
            => _context.Set<T>().AsQueryable();

        public Task<T> GetByIdAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            var queryable = _context.Set<T>().AsQueryable();

            if (includes != null)
                queryable = includes(queryable);

            return queryable.SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}