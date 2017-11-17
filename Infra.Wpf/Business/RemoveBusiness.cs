using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class RemoveBusiness<TEntity> : BusinessBase<bool> where TEntity : class
    {
        private TEntity _entity;

        private DbSet<TEntity> _set;


        public RemoveBusiness(DbSet<TEntity> set, TEntity entity, Logger logger = null) : base(logger)
        {
            _entity = entity;
            _set = set;

            OnExecute = () => RemoveExecute();
        }

        public RemoveBusiness(DbSet<TEntity> set, object id, Logger logger = null) : base(logger)
        {
            _set = set;
            _entity = _set.Find(id);

            OnExecute = () => RemoveExecute();
        }

        private bool RemoveExecute()
        {
            _set.Remove(_entity);

            Result.Data = true;
            Result.Message = new BusinessMessage("ثبت اطلاعات", "اطلاعات با موفقیت حذف شد.", Controls.MessageType.Information);

            return true;
        }
    }
}
