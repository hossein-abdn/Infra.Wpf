using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class FindByIdBusiness<TEntity> : BusinessBase<TEntity> where TEntity : class
    {
        private object _id;

        private DbSet<TEntity> _set;

        public FindByIdBusiness(Logger logger = null) : base(logger)
        {
        }

        public void Config(DbSet<TEntity> set, object id)
        {
            _id = id;
            _set = set;

            OnExecute = () => FindByIdExecute();
        }

        private bool FindByIdExecute()
        {
            Result.Data = _set.Find(_id);
            Result.Message = new BusinessMessage(" اطلاعات", "اطلاعات با موفقیت دریافت شد.", Controls.MessageType.Information);

            return true;
        }
    }
}
