using Infra.Wpf.Business;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Wpf.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        BusinessResult<int> GetCount();

        BusinessResult<int> GetCount(string predicate, object[] values = null);

        BusinessResult<int> GetCount(Expression<Func<TEntity, bool>> predicate);

        BusinessResult<Task<int>> GetCountAsync();

        BusinessResult<Task<int>> GetCountAsync(string predicate, object[] values = null);

        BusinessResult<Task<int>> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        BusinessResult<Task<int>> GetCountAsync(CancellationToken cancellationToken);

        BusinessResult<Task<int>> GetCountAsync(CancellationToken cancellationToken, string predicate, object[] values = null);

        BusinessResult<Task<int>> GetCountAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate);

        BusinessResult<List<TEntity>> GetAll(string orderBy = null, int? take = null, int? skip = null, string include = null);

        BusinessResult<List<TEntity>> GetAll(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null);

        BusinessResult<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null);

        BusinessResult<Task<List<TEntity>>> GetAllAsync(string orderBy = null, int? take = null, int? skip = null, string include = null);

        BusinessResult<Task<List<TEntity>>> GetAllAsync(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null);

        BusinessResult<Task<List<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null);

        BusinessResult<Task<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken, string orderBy = null, int? take = null, int? skip = null, string include = null);

        BusinessResult<Task<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken, string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null);

        BusinessResult<Task<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null);

        BusinessResult<List<TResult>> Select<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        BusinessResult<Task<List<TResult>>> SelectAsync<TResult>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false);

        BusinessResult<TEntity> GetFirst(string orderBy = null, string include = null);

        BusinessResult<TEntity> GetFirst(string predicate, object[] values = null, string orderBy = null, string include = null);

        BusinessResult<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null);

        BusinessResult<bool> Any();

        BusinessResult<bool> Any(string predicate, object[] values = null);

        BusinessResult<bool> Any(Expression<Func<TEntity, bool>> predicate);

        BusinessResult<TEntity> FindById(object id);

        BusinessResult<Task<TEntity>> FindByIdAsync(object id);

        BusinessResult<Task<TEntity>> FindByIdAsync(CancellationToken cancellationToken, object id);

        BusinessResult<bool> Add(TEntity entity);

        BusinessResult<bool> Update(TEntity entity);

        BusinessResult<bool> Remove(TEntity entity);

        BusinessResult<bool> Remove(object id);

        BusinessResult<DateTime> GetDateTime();
    }
}
