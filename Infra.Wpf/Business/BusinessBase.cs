﻿using Infra.Wpf.Common;
using Infra.Wpf.Security;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        protected Func<bool> OnBeforeExecute { get; set; }

        protected Func<bool> OnExecute { get; set; }

        protected Func<bool> OnAfterExecute { get; set; }

        protected Func<Task<bool>> OnBeforeExecuteAsync { get; set; }

        protected Func<Task<bool>> OnExecuteAsync { get; set; }

        protected Func<Task<bool>> OnAfterExecuteAsync { get; set; }

        #endregion

        #region Methods

        public BusinessBase(ILogger logger = null, bool logOnException = true)
        {
            Result = new BusinessResult();
            this.Logger = logger;
            ThrowException = false;
            this.logOnException = logOnException;
            OnBeforeExecute = () => true;
            OnBeforeExecuteAsync = () => Task.Run(() => true);
        }

        public virtual void Execute()
        {
            Execute(OnExecute);
        }

        public virtual void Execute(Func<bool> onExecute)
        {
            this.OnExecute = onExecute;

            if (Result == null)
                Result = new BusinessResult();

            try
            {
                if (this.OnExecute == null)
                    throw new ArgumentNullException("OnExecute");

                if (OnBeforeExecute != null)
                {
                    Result.IsOnBeforExecute = OnBeforeExecute();
                    if (Result.IsOnBeforExecute == false)
                        return;
                }

                if (this.OnExecute != null)
                {
                    Result.IsOnExecute = this.OnExecute();
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
                    Logger.Log(ex, this.GetType().FullName, (Thread.CurrentPrincipal.Identity as Identity)?.Id ?? 0);
                    LogInfo = null;
                }

                Result.Exception = ex;
                Result.Message = new BusinessMessage("خطا", "در سامانه خطایی رخ داده است.");

                if (ThrowException)
                    throw ex;
            }
        }

        public virtual void Execute(Func<bool> onBeforeExecute, Func<bool> onExecute, Func<bool> onAfterExecute)
        {
            this.OnBeforeExecute = onBeforeExecute;
            this.OnAfterExecute = onAfterExecute;
            Execute(onExecute);
        }

        public virtual async Task ExecuteAsync()
        {
            await ExecuteAsync(OnExecuteAsync);
        }

        public virtual async Task ExecuteAsync(Func<Task<bool>> onExecuteAsync)
        {
            this.OnExecuteAsync = onExecuteAsync;

            if (Result == null)
                Result = new BusinessResult();

            try
            {
                if (OnExecute == null)
                    throw new ArgumentNullException("OnExecute");

                if (OnBeforeExecute != null)
                {
                    Result.IsOnBeforExecute = await OnBeforeExecuteAsync();
                    if (Result.IsOnBeforExecute == false)
                        return;
                }

                if (OnExecute != null)
                {
                    Result.IsOnExecute = await this.OnExecuteAsync();
                    if (Logger != null && logOnException == false && LogInfo != null)
                    {
                        Logger.LogList.Add(LogInfo);
                        LogInfo = null;
                    }

                    if (Result.IsOnExecute == false)
                        return;
                }

                if (OnAfterExecute != null)
                    Result.IsOnAfterExecute = await OnAfterExecuteAsync();
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.Log(ex, this.GetType().FullName, (Thread.CurrentPrincipal.Identity as Identity)?.Id ?? 0);
                    LogInfo = null;
                }

                Result.Exception = ex;
                Result.Message = new BusinessMessage("خطا", "در سامانه خطایی رخ داده است.");

                if (ThrowException)
                    throw ex;
            }
        }

        public virtual async Task ExecuteAsync(Func<Task<bool>> onBeforeExecuteAsync, Func<Task<bool>> onExecuteAsync, Func<Task<bool>> onAfterExecuteAsync)
        {
            this.OnBeforeExecuteAsync = onBeforeExecuteAsync;
            this.OnAfterExecuteAsync = onAfterExecuteAsync;
            await ExecuteAsync(onExecuteAsync);
        }

        #endregion
    }

    public class BusinessBase<T> : BusinessBase
    {
        public new BusinessResult<T> Result { get; set; }

        public BusinessBase(ILogger logger = null, bool logOnException = true) : base(logger, logOnException)
        {
            Result = new BusinessResult<T>();
        }

        public override void Execute()
        {
            Execute(OnExecute);
        }

        public override void Execute(Func<bool> onExecute)
        {
            if (Result == null)
                Result = new BusinessResult<T>();

            base.Execute(onExecute);

            Result.IsOnBeforExecute = base.Result.IsOnBeforExecute;
            Result.IsOnExecute = base.Result.IsOnExecute;
            Result.IsOnAfterExecute = base.Result.IsOnAfterExecute;
            if (Result.HasException == false && base.Result.HasException)
            {
                Result.Exception = base.Result.Exception;
                Result.Message = base.Result.Message;
            }
        }

        public override void Execute(Func<bool> onBeforeExecute, Func<bool> onExecute, Func<bool> onAfterExecute)
        {
            this.OnBeforeExecute = onBeforeExecute;
            this.OnAfterExecute = onAfterExecute;
            Execute(onExecute);
        }

        public override async Task ExecuteAsync()
        {
            await ExecuteAsync(OnExecuteAsync);
        }

        public override async Task ExecuteAsync(Func<Task<bool>> onExecuteAsync)
        {
            if (Result == null)
                Result = new BusinessResult<T>();

            await base.ExecuteAsync(onExecuteAsync);

            Result.IsOnBeforExecute = base.Result.IsOnBeforExecute;
            Result.IsOnExecute = base.Result.IsOnExecute;
            Result.IsOnAfterExecute = base.Result.IsOnAfterExecute;
            if (Result.HasException == false && base.Result.HasException)
            {
                Result.Exception = base.Result.Exception;
                Result.Message = base.Result.Message;
            }
        }

        public override async Task ExecuteAsync(Func<Task<bool>> onBeforeExecuteAsync, Func<Task<bool>> onExecuteAsync, Func<Task<bool>> onAfterExecuteAsync)
        {
            base.OnBeforeExecuteAsync = onBeforeExecuteAsync;
            base.OnAfterExecuteAsync = onAfterExecuteAsync;
            await ExecuteAsync(onExecuteAsync);
        }
    }
}
