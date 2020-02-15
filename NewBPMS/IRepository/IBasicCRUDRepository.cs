using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NewBPMS.IRepository
{
    public interface IBasicCRUDRepository<T> where T : class
    {
        IEnumerable<T> EntityItems { get; }

        DbSet<T> ContextSet { get; }
        /// <summary>
        /// 通过Lamda表达式获取实体
        /// </summary>
        /// <param name="predicate">Lamda表达式（p=>p.Id==Id）</param>
        /// <returns></returns>
        T GetEntity(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 通过Lamda表达式获取DbSet
        /// </summary>
        /// <param name="predicate">Lamda表达式（p=>p.Id==Id）</param>
        /// <returns></returns>
        IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate);

        Task CreateAsync(T dbEntity);

        T QueryById(Guid Id);
        Task<T> QueryByIdAsync(Guid Id);

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <param name="orderBy">排序字段</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        Task<Tuple<List<T>, int>> PageListAsync<TOrderBy>(Expression<Func<T, TOrderBy>> orderBy, int pageIndex, int pageSize);

        Task EditAsync(T dbEntity);

        Task DeleteAsync(T dbEntity);

        Task<T> DeleteAsync(Guid Id);

        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回实体对象集合
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <param name="queryExpression">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>

        /// <returns>实体集合</returns>
        ///例子:var linqVar = _bridgeRepository.QueryEntity<Bridge>(p => p.Name.Contains(nameQuery));
        IEnumerable<TEntity> QueryEntity<TEntity>
            (Expression<Func<TEntity, bool>> queryExpression)
            where TEntity : class;
    }
}
