using Infra.Wpf.Business;
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
        BusinessResult<int> SaveChange();

        Task<BusinessResult<int>> SaveChangeAsync();

        Task<BusinessResult<int>> SaveChangeAsync(CancellationToken cancellationToken);

        void RejectChange();
    }
}
