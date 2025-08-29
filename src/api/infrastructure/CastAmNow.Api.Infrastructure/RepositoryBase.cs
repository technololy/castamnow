using CastAmNow.Api.Infrastructure.Abstractions;
using CastAmNow.Core.Models;
using CastAmNow.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CastAmNow.Api.Infrastructure
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey, TDBContext>(TDBContext context) : IRepository<TEntity> where TDBContext : DbContext where TEntity : BaseEntity<TPrimaryKey>
    {
        public virtual TEntity Add(TEntity entity)
        {
            context.Add(entity);

            return entity;
        }
        public void Attach(TEntity entity)
        {
            context.Attach(entity);
        }

        public virtual async Task<bool> Commit()
        {
            if (context.ChangeTracker.HasChanges())
            {
                try
                {
                    var isSaved = await context.SaveChangesAsync();
                    return isSaved > 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }
            return false;
        }

        public virtual TEntity Delete(object? id)
        {
            var oldEntity = context.Set<TEntity>().Find(id);
            if (oldEntity != null)
            {
                oldEntity.IsDeleted = true;
                oldEntity.UpdatedAt = DateTimeOffset.UtcNow;
                context.Entry(oldEntity).CurrentValues.SetValues(oldEntity);
                context.Entry(oldEntity).State = EntityState.Modified;
            }

            return oldEntity ?? default!;
        }

        public virtual TEntity Get(object? id)
        {
            var entity = context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                if (entity.IsDeleted)
                {
                    return default!;
                }
            }
            return entity ?? default!;
        }

        public virtual TEntity Update(TEntity entity)
        {
            var oldEntity = context.Set<TEntity>().Find(entity.Id);
            if (oldEntity != null)
            {
                entity.UpdatedAt = DateTimeOffset.UtcNow;
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                context.Entry(oldEntity).State = EntityState.Modified;
            }

            return entity;
        }

        public virtual long Count(Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();

            if (filter != null)
            {
                query = filter(query);
            }

            return query.Count(entity => !entity.IsDeleted);
        }

        public abstract IEnumerable<TEntity> GetAll<TId>(Query<TId>? searchQuery = default, PaginationFilter? paginationFilter = default);

        public virtual TEntity? Get(Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();

            if (filter != null)
            {
                query = filter(query);
            }

            return query.FirstOrDefault(); // or SingleOrDefault(), First(), etc. as needed
        }

        public virtual IQueryable<TEntity> GetAllWithFilter(Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();

            if (filter != null)
            {
                query = filter(query);
            }

            return query;
        }
    }

}
