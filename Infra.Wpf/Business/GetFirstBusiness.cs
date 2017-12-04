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
    public class GetFirstBusiness<TEntity> : BusinessBase<TEntity> where TEntity : class
    {
        private DbSet<TEntity> _set;

        private Expression<Func<TEntity, bool>> _predicate;

        private string _orderBy;

        private string _include;

        public GetFirstBusiness()
        {
        }

        public void Config(DbSet<TEntity> set, string orderBy = null, string include = null)
        {
            _set = set;
            _orderBy = orderBy;
            _include = include;
            OnExecute = () => GetFirstExecute();
        }

        public void Config(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null)
        {
            Config(set, orderBy, include);
            _predicate = predicate;
            OnExecute = () => GetFirstExecute();
        }

        public void Config(DbSet<TEntity> set, string predicate, object[] values, string orderBy = null, string include = null)
        {
            Config(set, orderBy, include);
            _predicate = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            OnExecute = () => GetFirstExecute();
        }

        private bool GetFirstExecute()
        {
            IQueryable<TEntity> query = GenerateQuery();

            Result.Data = query.FirstOrDefault();
            Result.Message = new BusinessMessage(" اطلاعات", "اطلاعات با موفقیت دریافت شد.", Controls.MessageType.Information);

            return true;
        }

        private IQueryable<TEntity> GenerateQuery()
        {
            IQueryable<TEntity> query = _set;

            if (_predicate != null)
                query = query.Where(_predicate);

            if (_include != null)
            {
                var includeList = _include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in includeList)
                    query = query.Include(item);
            }

            if (!string.IsNullOrEmpty(_orderBy))
                query = query.OrderBy(_orderBy);

            return query;
        }
    }
}
