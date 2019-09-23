using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Infra.Wpf.Business;
using Infra.Wpf.Common;
using Infra.Wpf.Security;
using Infra.Wpf.Common.Helpers;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace Infra.Wpf.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext Context { get; set; }

        private ILogger logger { get; set; }

        private DbSet<TEntity> _set;

        private DbSet<TEntity> set
        {
            get { return _set ?? (_set = Context.Set<TEntity>()); }
        }

        public ILogInfo LogInfo { get; set; }

        public Repository(DbContext context, ILogger logger = null)
        {
            this.Context = context;
            this.logger = logger;
        }

        public virtual bool Add(TEntity entity)
        {
            set.Add(entity);

            LogInfo = new LogInfo()
            {
                CallSite = typeof(TEntity).Name + ".Add",
                LogType = LogType.Add,
                UserId = (Thread.CurrentPrincipal.Identity as Identity).Id,
                Entry = Context?.Entry(entity)
            };

            return true;
        }

        public virtual bool Any()
        {
            return Any(null);
        }

        public virtual bool Any(string predicate, object[] values = null)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return Any(dynamicQuery);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                return set.Any();
            else
                return set.Any(predicate);
        }

        public virtual async Task<bool> AnyAsync()
        {
            return await AnyAsync(null);
        }

        public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return await AnyAsync(cancellationToken, null);
        }

        public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken, string predicate, object[] values = null)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await AnyAsync(cancellationToken, dynamicQuery);
        }

        public virtual async Task<bool> AnyAsync(string predicate, object[] values = null)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await AnyAsync(dynamicQuery);
        }

        public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                return await set.AnyAsync(cancellationToken);
            else
                return await set.AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                return await set.AnyAsync();
            else
                return await set.AnyAsync(predicate);
        }

        public virtual bool Contains(TEntity item)
        {
            return set.Contains(item);
        }

        public virtual async Task<bool> ContainsAsync(TEntity item)
        {
            return await set.ContainsAsync(item);
        }

        public virtual async Task<bool> ContainsAsync(CancellationToken cancellationToken, TEntity item)
        {
            return await set.ContainsAsync(item, cancellationToken);
        }

        public virtual TEntity FindById(params object[] ids)
        {
            return set.Find(ids);
        }

        public virtual async Task<TEntity> FindByIdAsync(params object[] ids)
        {
            return await set.FindAsync(ids);
        }

        public virtual async Task<TEntity> FindByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            return await set.FindAsync(cancellationToken, ids);
        }

        private IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>> predicate, string orderBy, int? take, int? skip, string include, bool distinct)
        {
            IQueryable<TEntity> query = set;

            if (predicate != null)
                query = query.Where(predicate);

            if (distinct)
                query = query.Distinct();

            if (include != null)
            {
                var includeList = include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in includeList)
                    query = query.Include(item);
            }

            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderBy(orderBy);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);
            return query;
        }

        public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            IQueryable<TEntity> query = GenerateQuery(predicate, orderBy, take, skip, include, distinct);
            return query.ToList();
        }

        public virtual List<TEntity> GetAll(string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            Expression<Func<TEntity, bool>> predicate = null;
            return GetAll(predicate, orderBy, take, skip, include, distinct);
        }

        public virtual List<TEntity> GetAll(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return GetAll(dynamicQuery, orderBy, take, skip, include, distinct);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            Expression<Func<TEntity, bool>> predicate = null;
            return await GetAllAsync(predicate, orderBy, take, skip, include, distinct);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            IQueryable<TEntity> query = GenerateQuery(predicate, orderBy, take, skip, include, distinct);
            return await query.ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAllAsync(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await GetAllAsync(dynamicQuery, orderBy, take, skip, include, distinct);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            Expression<Func<TEntity, bool>> predicate = null;
            return await GetAllAsync(cancellationToken, predicate, orderBy, take, skip, include, distinct);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            IQueryable<TEntity> query = GenerateQuery(predicate, orderBy, take, skip, include, distinct);
            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken, string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await GetAllAsync(cancellationToken, dynamicQuery, orderBy, take, skip, include, distinct);
        }

        public virtual int GetCount()
        {
            return set.Count();
        }

        public virtual int GetCount(string predicate, object[] values = null)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return GetCount(dynamicQuery);
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            return set.Count(predicate);
        }

        public virtual async Task<int> GetCountAsync()
        {
            return await set.CountAsync();
        }

        public virtual async Task<int> GetCountAsync(string predicate, object[] values = null)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await GetCountAsync(dynamicQuery);
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await set.CountAsync(predicate);
        }

        public virtual async Task<int> GetCountAsync(CancellationToken cancellationToken)
        {

            return await set.CountAsync(cancellationToken);
        }

        public virtual async Task<int> GetCountAsync(CancellationToken cancellationToken, string predicate, object[] values = null)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await GetCountAsync(cancellationToken, dynamicQuery);
        }

        public virtual async Task<int> GetCountAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
        {
            return await set.CountAsync(predicate, cancellationToken);
        }

        public virtual TEntity GetFirst(string orderBy = null, string include = null)
        {
            return GetFirst(null, orderBy, include);
        }

        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null)
        {
            var query = GenerateQuery(predicate, orderBy, null, null, include, false);
            return query.FirstOrDefault();
        }

        public virtual TEntity GetFirst(string predicate, object[] values = null, string orderBy = null, string include = null)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return GetFirst(dynamicQuery, orderBy, include);
        }

        public virtual async Task<TEntity> GetFirstAsync(string orderBy = null, string include = null)
        {
            return await GetFirstAsync(null, orderBy, include);
        }

        public virtual Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null)
        {
            var query = GenerateQuery(predicate, orderBy, null, null, include, false);
            return query.FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetFirstAsync(string predicate, object[] values = null, string orderBy = null, string include = null)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await GetFirstAsync(dynamicQuery, orderBy, include);
        }

        public virtual async Task<TEntity> GetFirstAsync(CancellationToken cancellationToken, string orderBy = null, string include = null)
        {
            return await GetFirstAsync(cancellationToken, null, orderBy, include);
        }

        public virtual Task<TEntity> GetFirstAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null)
        {
            var query = GenerateQuery(predicate, orderBy, null, null, include, false);
            return query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetFirstAsync(CancellationToken cancellationToken, string predicate, object[] values = null, string orderBy = null, string include = null)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await GetFirstAsync(cancellationToken, dynamicQuery, orderBy, include);
        }

        public virtual bool Remove(object id)
        {
            var entity = FindById(id);
            return Remove(entity);
        }

        public virtual bool Remove(TEntity entity)
        {
            set.Remove(entity);

            LogInfo = new LogInfo()
            {
                CallSite = typeof(TEntity).Name + ".Remove",
                LogType = LogType.Delete,
                UserId = (Thread.CurrentPrincipal.Identity as Identity).Id,
                Entry = Context.Entry(entity)
            };

            return true;
        }

        public virtual bool Update(TEntity entity)
        {
            PropertyInfo prop = null;
            foreach (var item in typeof(TEntity).GetProperties())
            {
                if (item.GetCustomAttribute<KeyAttribute>() != null)
                {
                    prop = item;
                    break;
                }
            }
            var orginalModel = GetFirst(DynamicLinq.ConvertToExpression<TEntity>(prop.Name + "=" + prop.GetValue(entity), null));
            var entry = Context.Entry(orginalModel);

            entry.CurrentValues.SetValues(entity);
            entry.State = EntityState.Modified;

            LogInfo = new LogInfo()
            {
                CallSite = typeof(TEntity).Name + ".Update",
                LogType = LogType.Update,
                UserId = (Thread.CurrentPrincipal.Identity as Identity).Id,
                Entry = entry
            };

            return true;
        }

        public DateTime GetDateTime()
        {
            return Context.Database.SqlQuery<DateTime>("select getdate();").First();
        }

        public List<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false)
        {
            return Select(null, selector, orderBy, take, skip, include, distinct);
        }

        public List<TResult> Select<TResult>(string predicate, Expression<Func<TEntity, TResult>> selector, object[] values = null, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return Select(dynamicQuery, selector, orderBy, take, skip, include, distinct);
        }

        public List<TResult> Select<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false)
        {
            IQueryable<TResult> query = SelectGenerateQuery(predicate, selector, orderBy, take, skip, include, distinct);
            return query.ToList();
        }

        public async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false)
        {
            return await SelectAsync(null, selector, orderBy, take, skip, include, distinct);
        }

        public async Task<List<TResult>> SelectAsync<TResult>(string predicate, Expression<Func<TEntity, TResult>> selector, object[] values = null, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await SelectAsync(dynamicQuery, selector, orderBy, take, skip, include, distinct);
        }

        public async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false)
        {
            IQueryable<TResult> query = SelectGenerateQuery(predicate, selector, orderBy, take, skip, include, distinct);
            return await query.ToListAsync();
        }

        public async Task<List<TResult>> SelectAsync<TResult>(CancellationToken cancellationToken, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false)
        {
            return await SelectAsync(cancellationToken, null, selector, orderBy, take, skip, include, distinct);
        }

        public async Task<List<TResult>> SelectAsync<TResult>(CancellationToken cancellationToken, string predicate, Expression<Func<TEntity, TResult>> selector, object[] values = null, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false)
        {
            var dynamicQuery = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            return await SelectAsync(cancellationToken, dynamicQuery, selector, orderBy, take, skip, include, distinct);
        }

        public async Task<List<TResult>> SelectAsync<TResult>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null, bool distinct = false)
        {
            IQueryable<TResult> query = SelectGenerateQuery(predicate, selector, orderBy, take, skip, include, distinct);
            return await query.ToListAsync(cancellationToken);
        }

        private IQueryable<TResult> SelectGenerateQuery<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy, int? take, int? skip, string include, bool distinct)
        {
            IQueryable<TEntity> query = set;
            IQueryable<TResult> result = null;
            if (predicate != null)
                result = query.Where(predicate).Select(selector);

            if (distinct)
                result = result.Distinct();

            if (include != null)
            {
                var includeList = include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in includeList)
                    result = result.Include(item);
            }

            if (!string.IsNullOrEmpty(orderBy))
                result = result.OrderBy(orderBy);

            if (skip.HasValue)
                result = result.Skip(skip.Value);

            if (take.HasValue)
                result = result.Take(take.Value);

            return result;
        }
    }
}