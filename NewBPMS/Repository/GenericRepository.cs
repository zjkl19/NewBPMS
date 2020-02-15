using Microsoft.EntityFrameworkCore;
using NewBPMS.Data;
using NewBPMS.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NewBPMS.Repository
{
    public abstract class GenericRepository<T> : IBasicCRUDRepository<T> where T : class
    {
        protected ApplicationDbContext context;

        public GenericRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public virtual DbSet<T> ContextSet => context.Set<T>();


        /// <summary>
        /// 通过Lamda表达式获取实体
        /// </summary>
        /// <param name="predicate">Lamda表达式（p=>p.Id==Id）</param>
        /// <returns></returns>
        public virtual T GetEntity(Expression<Func<T, bool>> predicate)
        {
            
            return context.Set<T>().AsNoTracking().SingleOrDefault(predicate);
        }

        public virtual IEnumerable<T> EntityItems => context.Set<T>().AsQueryable();

        public virtual IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = context.Set<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;

        }
        public virtual Task CreateAsync(T dbEntity)
        {
            context.Set<T>().Add(dbEntity);
            return context.SaveChangesAsync();
        }
        public async virtual Task<T> QueryByIdAsync(Guid Id)
        {
            return await context.Set<T>().FindAsync(Id);
        }

        public virtual T QueryById(Guid Id)
        {
            return context.Set<T>().Find(Id);
        }

        public virtual async Task<Tuple<List<T>, int>> PageListAsync<TOrderBy>(Expression<Func<T, TOrderBy>> orderBy, int pageIndex, int pageSize)
        {
            var query = context.Set<T>().AsQueryable();
            var count = await query.CountAsync();

            var linqVar = await query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Tuple<List<T>, int>(linqVar, count);
        }

        public virtual Task EditAsync(T dbEntity)
        {
            context.Entry<T>(dbEntity).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }

        public virtual Task DeleteAsync(T dbEntity)
        {
            context.Remove(dbEntity);

            return context.SaveChangesAsync();

        }

        public async Task<T> DeleteAsync(Guid Id)
        {
            var dbEntry = await context.Set<T>().FindAsync(Id);
            if (dbEntry != null)
            {
                context.Set<T>().Remove(dbEntry);
                await context.SaveChangesAsync();
            }
            return dbEntry;
        }

        public IEnumerable<TEntity> QueryEntity<TEntity>
            (Expression<Func<TEntity, bool>> queryExpression)
            where TEntity : class
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (queryExpression != null)
            {
                query = query.Where(queryExpression);
            }

            return query.Cast<TEntity>().AsNoTracking();

        }
    }
}
