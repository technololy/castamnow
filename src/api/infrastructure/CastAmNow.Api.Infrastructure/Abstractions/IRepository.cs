
using CastAmNow.Core.Models;

namespace CastAmNow.Api.Infrastructure.Abstractions
{
    public interface IRepository<T>
    {
        T Get(object? id);

        IEnumerable<T> GetAll<TId>(Query<TId>? searchQuery = default, PaginationFilter? paginationFilter = default);

        T Add(T entity);

        void Attach(T entity);

        T Delete(object? id);

        T Update(T entity);

        Task<bool> Commit();

        long Count(Func<IQueryable<T>, IQueryable<T>>? filter = null);

        T Get(Func<IQueryable<T>, IQueryable<T>>? filter = null);

        IQueryable<T> GetAllWithFilter(Func<IQueryable<T>, IQueryable<T>>? filter = null);
    }
}
