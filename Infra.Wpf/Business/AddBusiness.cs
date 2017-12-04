using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class AddBusiness<TEntity> : BusinessBase<bool> where TEntity : class
    {
        private TEntity _entity;

        private DbSet<TEntity> _set;

        public AddBusiness(Logger logger = null) : base(logger)
        {
        }

        public void Config(DbSet<TEntity> set, TEntity entity)
        {
            _entity = entity;
            _set = set;
            OnExecute = () => AddExecute();
        }

        private bool AddExecute()
        {
            _set.Add(_entity);

            Result.Data = true;
            Result.Message = new BusinessMessage("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد.", Controls.MessageType.Information);

            return true;
        }
    }
}
