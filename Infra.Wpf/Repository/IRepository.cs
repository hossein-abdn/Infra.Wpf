using Infra.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Wpf.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ILogInfo LogInfo { get; set; }

        bool Add(TEntity entity);

        bool Any();

        bool Any(string predicate, object[] values = null);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(CancellationToken cancellationToken);

        Task<bool> AnyAsync(string predicate, object[] values = null);

        Task<bool> AnyAsync(CancellationToken cancellationToken, string predicate, object[] values = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> AnyAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate);

        bool Contains(TEntity item);

        Task<bool> ContainsAsync(TEntity item);

        Task<bool> ContainsAsync(CancellationToken cancellationToken, TEntity item);

        TEntity FindById(params object[] ids);

        Task<TEntity> FindByIdAsync(params object[] ids);

        Task<TEntity> FindByIdAsync(CancellationToken cancellationToken, params object[] ids);

        List<TEntity> GetAll(string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        List<TEntity> GetAll(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null,bool distinct = false);

        List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        Task<List<TEntity>> GetAllAsync(string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        Task<List<TEntity>> GetAllAsync(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken, string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        int GetCount();

        int GetCount(string predicate, object[] values = null);

        int GetCount(Expression<Func<TEntity, bool>> predicate);

        Task<int> GetCountAsync();

        Task<int> GetCountAsync(string predicate, object[] values = null);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> GetCountAsync(CancellationToken cancellationToken);

        Task<int> GetCountAsync(CancellationToken cancellationToken, string predicate, object[] values = null);

        Task<int> GetCountAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate);

        TEntity GetFirst(string orderBy = null, string include = null);

        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null);

        TEntity GetFirst(string predicate, object[] values = null, string orderBy = null, string include = null);

        Task<TEntity> GetFirstAsync(string orderBy = null, string include = null);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null);

        Task<TEntity> GetFirstAsync(string predicate, object[] values = null, string orderBy = null, string include = null);

        Task<TEntity> GetFirstAsync(CancellationToken cancellationToken, string orderBy = null, string include = null);

        Task<TEntity> GetFirstAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null);

        Task<TEntity> GetFirstAsync(CancellationToken cancellationToken, string predicate, object[] values = null, string orderBy = null, string include = null);

        bool Remove(TEntity entity);

        bool Remove(object id);

        bool Update(TEntity entity);

        DateTime GetDateTime();

        List<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false);

        List<TResult> Select<TResult>(string predicate, Expression<Func<TEntity, TResult>> selector, object[] values = null, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false);

        List<TResult> Select<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false);

        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false);

        Task<List<TResult>> SelectAsync<TResult>(string predicate, Expression<Func<TEntity, TResult>> selector, object[] values = null, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false);

        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false);

        Task<List<TResult>> SelectAsync<TResult>(CancellationToken cancellationToken, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false);

        Task<List<TResult>> SelectAsync<TResult>(CancellationToken cancellationToken, string predicate, Expression<Func<TEntity, TResult>> selector, object[] values = null, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false);

        Task<List<TResult>> SelectAsync<TResult>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false);
    }
}
