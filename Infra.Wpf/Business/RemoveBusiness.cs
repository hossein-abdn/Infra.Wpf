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

        public RemoveBusiness(Logger logger = null) : base(logger)
        {
        }

        public void Config(DbSet<TEntity> set, TEntity entity)
        {
            _entity = entity;
            _set = set;

            OnExecute = () => RemoveExecute();
        }

        public void Config(DbSet<TEntity> set, object id)
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
