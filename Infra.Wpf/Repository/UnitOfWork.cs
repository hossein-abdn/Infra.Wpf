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

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public Repository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
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

        public int SaveChange()
        {
            return _context.SaveChanges();
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
