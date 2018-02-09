using Infra.Wpf.Business;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Wpf.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;

        protected DbContext Context { get; set; }

        protected Logger Logger { get; set; }

        public UnitOfWork(DbContext context)
        {
            this.Context = context;
        }

        public UnitOfWork(DbContext context, Logger logger) : this(context)
        {
            Logger = logger;
        }

        public Repository<TEntity> GetRepository<TEntity>(bool logOnException = true) where TEntity : class
        {
            return new Repository<TEntity>(Context, Logger, logOnException);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    Context.Dispose();
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void RejectChange()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public BusinessResult<int> SaveChange()
        {
            var result = new BusinessResult<int>();

            try
            {
                if (Logger != null)
                {
                    foreach (var item in Logger.LogList)
                    {
                        if (item.LogType == LogType.Delete)
                            GenerateLogMessage(item);
                    }
                }

                result.Data = Context.SaveChanges();
                result.Message = new BusinessMessage("انجام عملیات", "عملیات با موفقیت انجام شد.", Controls.MessageType.Information);

                if (Logger != null)
                {
                    foreach (var item in Logger.LogList)
                    {
                        if (item.LogType == LogType.Add || item.LogType == LogType.Update)
                            GenerateLogMessage(item);
                    }

                    Logger.LogPendingList();
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                Logger?.Log(ex, this.GetType().FullName, 0);
                Logger?.LogList?.Clear();
                result.Message = new BusinessMessage("خطا", "در سامانه خطایی رخ داده است.");
                return result;
            }
        }

        public Task<int> SaveChangeAsync()
        {
            return Context.SaveChangesAsync();
        }

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        private void GenerateLogMessage(ILogInfo logInfo)
        {
            if (logInfo.Entry == null)
                return;

            StringBuilder message = new StringBuilder();

            if (logInfo.LogType == LogType.Add)
            {
                message.Append(logInfo.Entry.Entity.GetType().Name + ": ");

                foreach (var prop in logInfo.Entry.CurrentValues.PropertyNames)
                {
                    message.Append(prop + "=");
                    message.Append(logInfo.Entry.CurrentValues[prop]?.ToString() ?? "Null");
                    message.Append(", ");
                }
                if (message.Length > 0)
                    message = message.Remove(message.Length - 2, 2);

                logInfo.Message = message.ToString();
            }
            else if (logInfo.LogType == LogType.Update)
            {
                message.Append(logInfo.Entry.Entity.GetType().Name + ": ");

                var objectStateEntry = ((IObjectContextAdapter) this).ObjectContext.ObjectStateManager.GetObjectStateEntry(logInfo.Entry.Entity);
                if (objectStateEntry.EntityKey.EntityKeyValues.Count() > 0)
                {
                    message.Append(objectStateEntry.EntityKey.EntityKeyValues[0].Key + "=");
                    message.Append(objectStateEntry.EntityKey.EntityKeyValues[0].Value.ToString() + ", ");
                }

                foreach (var prop in logInfo.Entry.OriginalValues.PropertyNames)
                {
                    var originalValue = logInfo.Entry.OriginalValues[prop];
                    var currentValue = logInfo.Entry.CurrentValues[prop];
                    message.AppendFormat("{0}->{1}", originalValue?.ToString() ?? "Null", currentValue?.ToString() ?? "Null");
                }
            }
            else if (logInfo.LogType == LogType.Delete)
            {
                message.Append(logInfo.Entry.Entity.GetType().Name + ": ");

                foreach (var prop in logInfo.Entry.CurrentValues.PropertyNames)
                {
                    message.Append(prop + "=");
                    message.Append(logInfo.Entry.OriginalValues[prop]?.ToString() ?? "Null");
                    message.Append(", ");
                }
                if (message.Length > 0)
                    message = message.Remove(message.Length - 2, 2);

                logInfo.Message = message.ToString();
            }
        }
    }
}

