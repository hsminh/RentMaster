namespace RentMaster.Core.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FindByIdAsync(Guid id);
    Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}