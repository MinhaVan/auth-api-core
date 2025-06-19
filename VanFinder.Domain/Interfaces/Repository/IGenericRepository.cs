using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VanFinder.Domain.Interfaces.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> SaveChangesAsync();
}