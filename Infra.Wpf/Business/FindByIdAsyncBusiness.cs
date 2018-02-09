using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class FindByIdAsyncBusiness<TEntity> : BusinessBase<Task<TEntity>> where TEntity : class
    {
        private object _id;

        private DbSet<TEntity> _set;

        private CancellationToken _cancellationToken;

        public FindByIdAsyncBusiness(Logger logger = null) : base(logger)
        {
        }

        public void Config(DbSet<TEntity> set, object id)
        {
            _id = id;
            _set = set;
            OnExecute = () => FindByIdAsyncExecute();
        }

        public void Config(DbSet<TEntity> set, object id, CancellationToken cancellationToken)
        {
            Config(set, id);
            _cancellationToken = cancellationToken;
            OnExecute = () => FindByIdTokenExecute();
        }

        private bool FindByIdAsyncExecute()
        {
            Result.Data = _set.FindAsync(_id);
            Result.Message = new BusinessMessage(" اطلاعات", "اطلاعات با موفقیت دریافت شد.", Controls.MessageType.Information);

            return true;
        }

        private bool FindByIdTokenExecute()
        {
            Result.Data = _set.FindAsync(_cancellationToken, _id);
            Result.Message = new BusinessMessage(" اطلاعات", "اطلاعات با موفقیت دریافت شد.", Controls.MessageType.Information);

            return true;
        }
    }
}
