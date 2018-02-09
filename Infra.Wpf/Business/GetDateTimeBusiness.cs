using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class GetDateTimeBusiness : BusinessBase<DateTime>
    {
        private DbContext _context;

        public GetDateTimeBusiness(Logger logger) : base(logger)
        {
        }

        public void Config(DbContext context)
        {
            _context = context;
            OnExecute = () => GetDateTimeExecute();
        }

        private bool GetDateTimeExecute()
        {
            Result.Data = _context.Database.SqlQuery<DateTime>("select getdate();").First();
            Result.Message = new BusinessMessage(" اطلاعات", "اطلاعات با موفقیت دریافت شد.", Controls.MessageType.Information);

            return true;
        }
    }
}
