using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Wpf.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChange();

        Task<int> SaveChangeAsync();

        Task<int> SaveChangeAsync(CancellationToken cancellationToken);

        void RejectChange();
    }
}
