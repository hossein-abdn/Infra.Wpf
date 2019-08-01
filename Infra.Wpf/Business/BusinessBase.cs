using Infra.Wpf.Common;
using Infra.Wpf.Security;
using System;
using System.Threading;

namespace Infra.Wpf.Business
{
    public class BusinessBase
    {
        #region Properties

        protected ILogger Logger { get; set; }

        private bool logOnException { get; set; }

        public ILogInfo LogInfo { get; set; }

        public BusinessResult Result { get; set; }

        public bool ThrowException { get; set; }

        public Func<bool> OnBeforeExecute { get; set; }

        public Func<bool> OnExecute { get; set; }

        public Func<bool> OnAfterExecute { get; set; }

        #endregion

        #region Methods

        public BusinessBase(ILogger logger = null, bool logOnException = true)
        {
            Result = new BusinessResult();
            this.Logger = logger;
            ThrowException = false;
            this.logOnException = logOnException;
            OnBeforeExecute = () => true;
        }

        public BusinessBase(Func<bool> onExecute, ILogger logger = null, bool logOnException = true) : this(logger, logOnException)
        {
            OnExecute = onExecute;
        }

        public BusinessBase(Func<bool> onBeforeExecute, Func<bool> onExecute, ILogger logger = null, bool logOnException = true) : this(onExecute, logger, logOnException)
        {
            OnBeforeExecute = onBeforeExecute;
        }

        public BusinessBase(Func<bool> onBeforExecute, Func<bool> onExecute, Func<bool> onAfterExecute, ILogger logger = null, bool logOnException = true) : this(onBeforExecute, onExecute, logger, logOnException)
        {
            OnAfterExecute = onAfterExecute;
        }

        public virtual void Execute()
        {
            if (Result == null)
                Result = new BusinessResult();

            try
            {
                if (OnExecute == null)
                    throw new ArgumentNullException("OnExecute");

                if (OnBeforeExecute != null)
                {
                    Result.IsOnBeforExecute = OnBeforeExecute();
                    if (Result.IsOnBeforExecute == false)
                        return;
                }

                if (OnExecute != null)
                {
                    Result.IsOnExecute = OnExecute();
                    if (Logger != null && logOnException == false && LogInfo != null)
                    {
                        Logger.LogList.Add(LogInfo);
                        LogInfo = null;
                    }

                    if (Result.IsOnExecute == false)
                        return;
                }

                if (OnAfterExecute != null)
                    Result.IsOnAfterExecute = OnAfterExecute();
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.Log(ex, this.GetType().FullName, (Thread.CurrentPrincipal.Identity as Identity).Id);
                    LogInfo = null;
                }

                Result.Exception = ex;
                Result.Message = new BusinessMessage("خطا", "در سامانه خطایی رخ داده است.");

                if (ThrowException)
                    throw ex;
            }
        }

        #endregion
    }

    public class BusinessBase<T> : BusinessBase
    {
        public BusinessResult<T> Result { get; set; }

        public BusinessBase(ILogger logger = null, bool logOnException = true) : base(logger, logOnException)
        {
            Result = new BusinessResult<T>();
        }

        public BusinessBase(Func<bool> onExecute, ILogger logger = null, bool logOnException = true) : this(logger, logOnException)
        {
            OnExecute = onExecute;
        }

        public BusinessBase(Func<bool> onBeforeExecute, Func<bool> onExecute, ILogger logger = null, bool logOnException = true) : this(onExecute, logger, logOnException)
        {
            OnBeforeExecute = onBeforeExecute;
        }

        public BusinessBase(Func<bool> onBeforExecute, Func<bool> onExecute, Func<bool> onAfterExecute, ILogger logger = null, bool logOnException = true) : this(onBeforExecute, onExecute, logger, logOnException)
        {
            OnAfterExecute = onAfterExecute;
        }

        public override void Execute()
        {
            if (Result == null)
                Result = new BusinessResult<T>();

            base.Execute();

            Result.IsOnBeforExecute = base.Result.IsOnBeforExecute;
            Result.IsOnExecute = base.Result.IsOnExecute;
            Result.IsOnAfterExecute = base.Result.IsOnAfterExecute;
            if (Result.HasException == false && base.Result.HasException)
            {
                Result.Exception = base.Result.Exception;
                Result.Message = base.Result.Message;
            }
        }
    }
}
