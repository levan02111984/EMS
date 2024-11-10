using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EMS.Data;
using System.Diagnostics.Eventing.Reader;

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

        //Insert DbSet
        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity); // Mark EntityState as Modified
        }

        //Update DbSet
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity); // Mark EntityState as Modified
        }

        //Delete DbSet
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


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed == false)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
            

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
