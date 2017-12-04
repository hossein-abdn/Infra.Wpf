using Infra.Wpf.Business;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Wpf.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        protected DbContext _context;

        private bool disposed = false;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public Repository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_context);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _context.Dispose();
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
            foreach (var entry in _context.ChangeTracker.Entries())
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
                result.Data = _context.SaveChanges();
                result.Message = new BusinessMessage("انجام عملیات", "عملیات با موفقیت انجام شد.", Controls.MessageType.Information);
                return result;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.Message = new BusinessMessage("خطا", "در سامانه خطایی رخ داده است.");
                return result;
            }
        }

        public Task<int> SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
