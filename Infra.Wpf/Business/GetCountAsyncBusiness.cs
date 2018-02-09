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

        public GetCountAsyncBusiness(Logger logger) : base(logger)
        {
        }

        public void Config(DbSet<TEntity> set)
        {
            _set = set;
            OnExecute = () => GetCountAsyncExecute();
        }

        public void Config(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate)
        {
            Config(set);
            _predicate = predicate;
            OnExecute = () => GetCountAsyncPredicateExecute();
        }

        public void Config(DbSet<TEntity> set, string predicate, object[] values)
        {
            Config(set);
            _predicate = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            OnExecute = () => GetCountAsyncPredicateExecute();
        }

        public void Config(DbSet<TEntity> set, CancellationToken cancellationToken)
        {
            Config(set);
            _cancellationToken = cancellationToken;
            OnExecute = () => GetCountAsyncTokenExecute();
        }

        public void Config(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            Config(set, cancellationToken);
            _predicate = predicate;
            OnExecute = () => GetCountAsyncTokenPredicateExecute();
        }

        public void Config(DbSet<TEntity> set, string predicate, object[] values, CancellationToken cancellationToken)
        {
            Config(set, cancellationToken);
            _predicate = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
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
