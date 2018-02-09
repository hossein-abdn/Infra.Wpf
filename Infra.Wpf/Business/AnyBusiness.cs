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
    public class AnyBusiness<TEntity> : BusinessBase<bool> where TEntity : class
    {
        private TEntity _entity;

        private DbSet<TEntity> _set;

        private Expression<Func<TEntity, bool>> _predicate;

        public AnyBusiness(Logger logger) : base(logger)
        {
        }

        public void Config(DbSet<TEntity> set)
        {
            _set = set;
            OnExecute = () => AnyExecute();
        }

        public void Config(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate)
        {
            Config(set);
            _predicate = predicate;
            OnExecute = () => AnyExecute();
        }

        public void Config(DbSet<TEntity> set, string predicate, object[] values)
        {
            Config(set);
            _predicate = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            OnExecute = () => AnyExecute();
        }

        private bool AnyExecute()
        {
            if (_predicate == null)
                Result.Data = _set.Any();
            else
                Result.Data = _set.Any(_predicate);

            Result.Message = new BusinessMessage("ثبت اطلاعات", "عملیات با موفقیت انجام شد.", Controls.MessageType.Information);

            return true;
        }
    }
}
