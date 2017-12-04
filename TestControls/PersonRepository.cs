using Infra.Wpf.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Wpf.Business;
using System.Linq.Expressions;
using DataAccess.Models;
using Infra.Wpf.Common.Helpers;
using DataAccess;

namespace TestControls
{
    public class PersonRepository : Repository<Person>
    {
        internal PersonRepository(DbContext context) : base(context)
        {
        }

        public override BusinessResult<List<Person>> GetAll(string predicate, object[] values, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null)
        {
            if (string.IsNullOrEmpty(predicate))
                predicate = "RecordStatusId == @0";
            else
                predicate = predicate + "AND RecordStatusId == @0";

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "OrderItem desc";
            else
                orderBy = "OrderItem desc," + orderBy;

            Expression<Func<Person, bool>> expression = Infra.Wpf.Common.Helpers.DynamicExpression.ParseLambda<Person, bool>(predicate, new object[] { RecordStatus.Exist });

            return base.GetAll(expression, orderBy);
            //return base.GetAll(predicate, orderBy, take, skip, include);
        }
    }
}
