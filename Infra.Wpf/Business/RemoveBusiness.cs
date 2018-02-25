using Infra.Wpf.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class RemoveBusiness<TEntity> : BusinessBase<bool> where TEntity : class
    {
        private TEntity _entity;

        private DbSet<TEntity> _set;

        private DbContext _context;

        public RemoveBusiness(Logger logger, bool logOnException) : base(logger, logOnException)
        {
        }

        public void Config(DbContext context, DbSet<TEntity> set, TEntity entity)
        {
            _context = context;
            _entity = entity;
            _set = set;

            OnExecute = () => RemoveExecute();
        }

        public void Config(DbContext context, DbSet<TEntity> set, object id)
        {
            _context = context;
            _set = set;
            _entity = _set.Find(id);

            OnExecute = () => RemoveExecute();
        }

        private bool RemoveExecute()
        {
            _set.Remove(_entity);

            Result.Data = true;
            Result.Message = new BusinessMessage("ثبت اطلاعات", "اطلاعات با موفقیت حذف شد.", Controls.MessageType.Information);

            LogInfo = new LogInfo()
            {
                CallSite = typeof(TEntity).Name + ".RemoveBusiness",
                LogType = LogType.Delete,
                UserId = (Thread.CurrentPrincipal.Identity as Identity).Id,
                Entry = _context?.Entry(_entity)
            };

            return true;
        }
    }
}
