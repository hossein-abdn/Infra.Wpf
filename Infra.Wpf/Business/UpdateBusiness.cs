using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class UpdateBusiness<TEntity> : BusinessBase<bool> where TEntity : class
    {
        private TEntity _entity;

        private DbSet<TEntity> _set;

        private DbContext _context;

        public UpdateBusiness(Logger logger, bool logOnException) : base(logger, logOnException)
        {
        }

        public void Config(DbContext context, DbSet<TEntity> set, TEntity entity)
        {
            _context = context;
            _entity = entity;
            _set = set;
            OnExecute = () => UpdateExecute();
        }

        private bool UpdateExecute()
        {
            var entry = _context.Entry(_entity);
            if (entry.State == EntityState.Detached)
            {
                _set.Attach(_entity);
                entry = _context.Entry(_entity);
            }
            entry.State = EntityState.Modified;

            Result.Data = true;
            Result.Message = new BusinessMessage("ثبت اطلاعات", "اطلاعات با موفقیت به روزرسانی شد.", Controls.MessageType.Information);

            LogInfo = new LogInfo()
            {
                CallSite = typeof(TEntity).Name + ".UpdateBusiness",
                LogType = LogType.Update,
                UserId = 1,
                Entry = _context?.Entry(_entity)
            };

            return true;
        }
    }
}
