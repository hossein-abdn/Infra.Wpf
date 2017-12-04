using Infra.Wpf.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class GetCountBusiness<TEntity> : BusinessBase<int> where TEntity : class
    {
        private DbSet<TEntity> _set;

        private Expression<Func<TEntity, bool>> _predicate;

        public GetCountBusiness()
        {
        }

        public void Config(DbSet<TEntity> set)
        {
            _set = set;
            OnExecute = () => GetCountExecute();
        }

        public void Config(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate)
        {
            Config(set);
            _predicate = predicate;
            OnExecute = () => GetCountPredicateExecute();
        }

        public void Config(DbSet<TEntity> set, string predicate, object[] values)
        {
            Config(set);
            _predicate = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            OnExecute = () => GetCountPredicateExecute();
        }

        private bool GetCountExecute()
        {
            Result.Data = _set.Count();
            Result.Message = new BusinessMessage(" اطلاعات", "تعداد با موفقیت محاسبه شد.", Controls.MessageType.Information);

            return true;
        }

        private bool GetCountPredicateExecute()
        {
            Result.Data = _set.Count(_predicate);

            Result.Message = new BusinessMessage(" اطلاعات", "تعداد با موفقیت محاسبه شد.", Controls.MessageType.Information);
            return true;
        }
    }
}
