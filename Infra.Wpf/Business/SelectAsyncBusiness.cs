using Infra.Wpf.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class SelectAsyncBusiness<TEntity, TResult> : BusinessBase<Task<List<TResult>>> where TEntity : class
    {
        private DbSet<TEntity> _set;

        private Expression<Func<TEntity, bool>> _predicate;

        private Expression<Func<TEntity, TResult>> _selector;

        private string _orderBy;

        private int? _take;

        private int? _skip;

        private string _include;

        private bool _distinct;

        private CancellationToken _cancellationToken;

        public SelectAsyncBusiness(Logger logger) : base(logger)
        {
        }

        public void Config(DbSet<TEntity> set, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            _set = set;
            _selector = selector;
            _orderBy = orderBy;
            _take = take;
            _skip = skip;
            _include = include;
            _distinct = distinct;
            OnExecute = () => SelectAsyncExecute();
        }

        public void Config(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            Config(set, selector, orderBy, take, skip, include, distinct);
            _predicate = predicate;
            OnExecute = () => SelectAsyncExecute();
        }

        public void Config(DbSet<TEntity> set, CancellationToken cancellationToken, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            Config(set, selector, orderBy, take, skip, include, distinct);
            _cancellationToken = cancellationToken;
            OnExecute = () => SelectAsyncExecute();
        }

        public void Config(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, Expression<Func<TEntity, TResult>> selector, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            Config(set, predicate, selector, orderBy, take, skip, include, distinct);
            _cancellationToken = cancellationToken;
            OnExecute = () => SelectAsyncExecute();
        }

        private bool SelectAsyncExecute()
        {
            IQueryable<TResult> query = GenerateQuery();

            Result.Data = query.ToListAsync();
            Result.Message = new BusinessMessage(" اطلاعات", "اطلاعات با موفقیت دریافت شد.", Controls.MessageType.Information);

            return true;
        }

        private IQueryable<TResult> GenerateQuery()
        {
            IQueryable<TEntity> query = _set;
            IQueryable<TResult> result = null;
            if (_predicate != null)
                result = query.Where(_predicate).Select(_selector);

            if (_distinct)
                result = result.Distinct();

            if (_include != null)
            {
                var includeList = _include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in includeList)
                    result = result.Include(item);
            }

            if (!string.IsNullOrEmpty(_orderBy))
                result = result.OrderBy(_orderBy);

            if (_skip.HasValue)
                result = result.Skip(_skip.Value);

            if (_take.HasValue)
                result = result.Take(_take.Value);

            return result;
        }
    }
}
