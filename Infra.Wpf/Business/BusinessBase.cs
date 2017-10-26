using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public abstract class BusinessBase<T>
    {
        #region Properties

        private Logger logger { get; set; }

        public BusinessResult<T> Result { get; set; }

        public bool Continue { get; set; }

        public bool HasException
        {
            get
            {
                return Result.Exception != null;
            }
        }

        #endregion

        #region Methods

        protected abstract ILogInfo OnBeforeExecute();

        protected virtual ILogInfo OnExecute()
        {
            return null;
        }

        protected virtual ILogInfo OnAfterExecute()
        {
            return null;
        }

        public BusinessBase(Logger logger = null)
        {
            Result = new BusinessResult<T>();
            Continue = true;
            this.logger = logger;
        }

        public void Execute(bool throwException = true)
        {
            if (Result == null)
                Result = new BusinessResult<T>();

            try
            {
                var logInfo = OnBeforeExecute();
                if (logger != null && logger.LogOnException == false && logInfo != null)
                    logger.Log(logInfo);

                if (Continue == false)
                    return;

                logInfo = OnExecute();
                if (logger != null && logger.LogOnException == false && logInfo != null)
                    logger.Log(logInfo);

                if (Continue == false)
                    return;

                logInfo = OnAfterExecute();
                if (logger != null && logger.LogOnException == false && logInfo != null)
                    logger.Log(logInfo);

            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Log(ex);

                Result.Exception = ex;
                Continue = false;
                Result.Message = new BusinessMessage("خطا", "در سامانه خطایی رخ داده است.");

                if (throwException)
                    throw ex;
            }
        }

        #endregion
    }
}
