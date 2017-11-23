using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class BusinessBase<T>
    {
        #region Properties

        private Logger logger { get; set; }

        public ILogInfo LogInfo { get; set; }

        public BusinessResult<T> Result { get; set; }

        public bool HasException
        {
            get
            {
                return Result.Exception != null;
            }
        }

        public bool ThrowException { get; set; }

        public Func<bool> OnBeforeExecute { get; set; }

        public Func<bool> OnExecute { get; set; }

        public Action OnAfterExecute { get; set; }

        #endregion

        #region Methods

        public BusinessBase(Logger logger = null)
        {
            Result = new BusinessResult<T>();
            this.logger = logger;
            ThrowException = false;
            OnBeforeExecute = () => true;
        }

        public BusinessBase(Func<bool> onExecute, Logger logger = null) : this(logger)
        {
            OnExecute = onExecute;
        }

        public BusinessBase(Func<bool> onBeforeExecute, Func<bool> onExecute, Logger logger = null) : this(onExecute, logger)
        {
            OnBeforeExecute = onBeforeExecute;
        }

        public BusinessBase(Func<bool> onBeforExecute, Func<bool> onExecute, Action onAfterExecute, Logger logger = null) : this(onBeforExecute, onExecute, logger)
        {
            OnAfterExecute = onAfterExecute;
        }

        public void Execute()
        {
            if (Result == null)
                Result = new BusinessResult<T>();

            try
            {
                if (OnExecute == null)
                    throw new ArgumentNullException("OnExecute");

                if (OnBeforeExecute != null)
                {
                    var beforResult = OnBeforeExecute();
                    if (logger != null && logger.LogOnException == false && LogInfo != null)
                    {
                        logger.Log(LogInfo);
                        LogInfo = null;
                    }

                    if (beforResult == false)
                        return;
                }

                if (OnExecute != null)
                {
                    var result = OnExecute();
                    if (logger != null && logger.LogOnException == false && LogInfo != null)
                    {
                        logger.Log(LogInfo);
                        LogInfo = null;
                    }

                    if (result == false)
                        return;
                }

                if (OnAfterExecute != null)
                {
                    OnAfterExecute();
                    if (logger != null && logger.LogOnException == false && LogInfo != null)
                    {
                        logger.Log(LogInfo);
                        LogInfo = null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Log(ex);

                Result.Exception = ex;
                Result.Message = new BusinessMessage("خطا", "در سامانه خطایی رخ داده است.");

                if (ThrowException)
                    throw ex;
            }
        }

        #endregion
    }
}
