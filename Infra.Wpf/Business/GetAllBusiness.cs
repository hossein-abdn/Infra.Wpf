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
    public class GetAllBusiness<TEntity> : BusinessBase<List<TEntity>> where TEntity : class
    {
        private DbSet<TEntity> _set;

        private Expression<Func<TEntity, bool>> _predicate;

        private string _orderBy;

        private int? _take;

        private int? _skip;

        private string _include;

        public GetAllBusiness()
        {
        }

        public void Config(DbSet<TEntity> set, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            _set = set;
            _orderBy = orderBy;
            _take = take;
            _skip = skip;
            _include = include;
            OnExecute = () => GetAllExecute();
        }

        public void Config(DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            Config(set, orderBy, take, skip, include);
            _predicate = predicate;
            OnExecute = () => GetAllExecute();
        }

        public void Config(DbSet<TEntity> set, string predicate, object[] values, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            Config(set, orderBy, take, skip, include);
            _predicate = DynamicLinq.ConvertToExpression<TEntity>(predicate, values);
            OnExecute = () => GetAllExecute();
        }

        private bool GetAllExecute()
        {
            IQueryable<TEntity> query = GenerateQuery();

            Result.Data = query.ToList();
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

            if (_skip.HasValue)
                query = query.Skip(_skip.Value);

            if (_take.HasValue)
                query = query.Take(_take.Value);

            return query;
        }
    }
}
