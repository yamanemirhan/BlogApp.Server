﻿using System.Linq.Expressions;

namespace BlogApp.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);
        Task<bool> DeleteByIdAsync(int id);
        Task SaveChangesAsync();
    }

}
