using System;
using System.Linq.Expressions;

namespace EMS.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        //Retrieves a single instance of type T based on the provided ID.It returns null if no entity is found, which is indicated by T?.
        Task<T?> GetByIdAsync(object id);

        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
        
        Task SaveAsync(); // Committing all changes from context into the database
        void Dispose(); // Clear unsued resources
    }
}

