using DataAccess;
using DataAccess.Models;
using Infra.Wpf.Business;
using Infra.Wpf.Common;
using Infra.Wpf.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControls
{
    public class TransactionGroupRepository : Repository<TransactionGroup>
    {
        internal TransactionGroupRepository(DbContext context, Logger logger = null, bool logOnException = true) : base(context, logger)
        {
        }

        public override List<TransactionGroup> GetAll(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null, bool distinct = false)
        {
            if (values == null)
                values = new object[] { RecordStatus.Exist };
            else
            {
                Array.Resize(ref values, 2);
                values[1] = values[0];
                values[0] = RecordStatus.Exist;
            }

            if (string.IsNullOrEmpty(predicate))
                predicate = "RecordStatusId == @0";
            else
                predicate = predicate + " AND RecordStatusId == @0";

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "OrderItem";
            else
                orderBy = "OrderItem," + orderBy;

            var searchResult = base.GetAll(predicate, values);

            var parentIds = searchResult.Select(x => x.ParentPath?.Split(',')).Where(x => x != null).SelectMany(x => x).Select(Int32.Parse).Union(searchResult.Select(x => x.TransactionGroupId));

            return base.GetAll(x => parentIds.Contains(x.TransactionGroupId), orderBy, take, skip, include);
        }
    }
}
