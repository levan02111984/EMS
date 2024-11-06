using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EMS.Data;

namespace EMS.Repositories
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;

        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>(); // Will be used later
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity); // Mark EntityState as Modified
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity); // Mark EntityState as Modified
        }

        public async Task DeleteAsync(object Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            if (entity != null)
            {
                {
                    _dbSet.Remove(entity);
                }
            }
        }

        //Do Save DbContext into the Database
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
