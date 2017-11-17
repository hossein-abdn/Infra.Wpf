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
    public class GetCountAsyncBusiness<TEntity> : BusinessBase<Task<int>> where TEntity : class
    {
        private DbSet<TEntity> _set;

        private Expression<Func<TEntity, bool>> _predicate;

        private CancellationToken _cancellationToken;

        public GetCountAsyncBusiness(DbSet<TEntity> set)
        {
            _set = set;
            OnExecute = () => GetCountAsyncExecute();
        }

        public GetCountAsyncBusiness(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate) : this(set)
        {
            _predicate = predicate;
            OnExecute = () => GetCountAsyncPredicateExecute();
        }

        public GetCountAsyncBusiness(DbSet<TEntity> set, string predicate) : this(set)
        {
            _predicate = DynamicLinq.ConvertToExpression<TEntity>(predicate);
            OnExecute = () => GetCountAsyncPredicateExecute();
        }

        public GetCountAsyncBusiness(DbSet<TEntity> set, CancellationToken cancellationToken) : this(set)
        {
            _cancellationToken = cancellationToken;
            OnExecute = () => GetCountAsyncTokenExecute();
        }

        public GetCountAsyncBusiness(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) : this(set, cancellationToken)
        {
            _predicate = predicate;
            OnExecute = () => GetCountAsyncTokenPredicateExecute();
        }

        public GetCountAsyncBusiness(DbSet<TEntity> set, string predicate, CancellationToken cancellationToken) : this(set, cancellationToken)
        {
            _predicate = DynamicLinq.ConvertToExpression<TEntity>(predicate);
            OnExecute = () => GetCountAsyncTokenPredicateExecute();
        }

        private bool GetCountAsyncExecute()
        {
            Result.Data = _set.CountAsync();
            Result.Message = new BusinessMessage(" اطلاعات", "تعداد با موفقیت محاسبه شد.", Controls.MessageType.Information);

            return true;
        }

        private bool GetCountAsyncPredicateExecute()
        {
            Result.Data = _set.CountAsync(_predicate);

            Result.Message = new BusinessMessage(" اطلاعات", "تعداد با موفقیت محاسبه شد.", Controls.MessageType.Information);
            return true;
        }

        private bool GetCountAsyncTokenExecute()
        {
            Result.Data = _set.CountAsync(_cancellationToken);
            Result.Message = new BusinessMessage(" اطلاعات", "تعداد با موفقیت محاسبه شد.", Controls.MessageType.Information);

            return true;
        }

        private bool GetCountAsyncTokenPredicateExecute()
        {
            Result.Data = _set.CountAsync(_predicate, _cancellationToken);

            Result.Message = new BusinessMessage(" اطلاعات", "تعداد با موفقیت محاسبه شد.", Controls.MessageType.Information);
            return true;
        }
    }
}
