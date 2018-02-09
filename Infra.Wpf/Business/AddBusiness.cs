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

        private DbContext _context;

        public AddBusiness(Logger logger, bool logOnException) : base(logger, logOnException)
        {
        }

        public void Config(DbContext context, DbSet<TEntity> set, TEntity entity)
        {
            _entity = entity;
            _set = set;
            _context = context;
            OnExecute = () => AddExecute();
        }

        private bool AddExecute()
        {
            _set.Add(_entity);

            Result.Data = true;
            Result.Message = new BusinessMessage("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد.", Controls.MessageType.Information);

            LogInfo = new LogInfo()
            {
                CallSite = typeof(TEntity).Name + ".AddBusiness",
                LogType = LogType.Add,
                UserId = 1,
                Entry = _context?.Entry(_entity)
            };
            return true;
        }
    }
}
