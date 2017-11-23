using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infra.Wpf.Business;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Infra.Wpf.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext _context;

        private DbSet<TEntity> _set;

        public Repository(DbContext context)
        {
            _context = context;
        }

        protected DbSet<TEntity> Set
        {
            get { return _set ?? (_set = _context.Set<TEntity>()); }
        }

        public virtual BusinessResult<bool> Add(TEntity entity)
        {
            var addBusiness = new AddBusiness<TEntity>(Set, entity);
            addBusiness.Execute();

            return addBusiness.Result;
        }

        public virtual BusinessResult<bool> Any()
        {
            var anyBusiness = new AnyBusiness<TEntity>(Set);
            anyBusiness.Execute();

            return anyBusiness.Result;
        }

        public virtual BusinessResult<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            var anyBusiness = new AnyBusiness<TEntity>(Set, predicate);
            anyBusiness.Execute();

            return anyBusiness.Result;
        }

        public virtual BusinessResult<bool> Any(string predicate)
        {
            var anyBusiness = new AnyBusiness<TEntity>(Set, predicate);
            anyBusiness.Execute();

            return anyBusiness.Result;
        }

        public virtual BusinessResult<TEntity> FindById(object id)
        {
            var findByIdBusiness = new FindByIdBusiness<TEntity>(Set, id);
            findByIdBusiness.Execute();

            return findByIdBusiness.Result;
        }

        public virtual BusinessResult<Task<TEntity>> FindByIdAsync(object id)
        {
            var findByIdAsyncBusiness = new FindByIdAsyncBusiness<TEntity>(Set, id);
            findByIdAsyncBusiness.Execute();

            return findByIdAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<TEntity>> FindByIdAsync(CancellationToken cancellationToken, object id)
        {
            var findByIdAsyncBusiness = new FindByIdAsyncBusiness<TEntity>(Set, id, cancellationToken);
            findByIdAsyncBusiness.Execute();

            return findByIdAsyncBusiness.Result;
        }

        public virtual BusinessResult<List<TEntity>> GetAll(string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            var getAllBusiness = new GetAllBusiness<TEntity>(Set, orderBy, take, skip, include);
            getAllBusiness.Execute();

            return getAllBusiness.Result;
        }

        public virtual BusinessResult<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            var getAllBusiness = new GetAllBusiness<TEntity>(Set, predicate, orderBy, take, skip, include);
            getAllBusiness.Execute();

            return getAllBusiness.Result;
        }

        public virtual BusinessResult<List<TEntity>> GetAll(string predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            var getAllBusiness = new GetAllBusiness<TEntity>(Set, predicate, orderBy, take, skip, include);
            getAllBusiness.Execute();

            return getAllBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            var getAllAsyncBusiness = new GetAllAsyncBusiness<TEntity>(Set, orderBy, take, skip, include);
            getAllAsyncBusiness.Execute();

            return getAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            var getAllAsyncBusiness = new GetAllAsyncBusiness<TEntity>(Set, predicate, orderBy, take, skip, include);
            getAllAsyncBusiness.Execute();

            return getAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(string predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            var getAllAsyncBusiness = new GetAllAsyncBusiness<TEntity>(Set, predicate, orderBy, take, skip, include);
            getAllAsyncBusiness.Execute();

            return getAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            var getAllAsyncBusiness = new GetAllAsyncBusiness<TEntity>(Set, cancellationToken, orderBy, take, skip, include);
            getAllAsyncBusiness.Execute();

            return getAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            var getAllAsyncBusiness = new GetAllAsyncBusiness<TEntity>(Set, predicate, cancellationToken, orderBy, take, skip, include);
            getAllAsyncBusiness.Execute();

            return getAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken, string predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            var getAllAsyncBusiness = new GetAllAsyncBusiness<TEntity>(Set, predicate, cancellationToken, orderBy, take, skip, include);
            getAllAsyncBusiness.Execute();

            return getAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<int> GetCount()
        {
            var getCountBusiness = new GetCountBusiness<TEntity>(Set);
            getCountBusiness.Execute();

            return getCountBusiness.Result;
        }

        public virtual BusinessResult<int> GetCount(string predicate)
        {
            var getCountBusiness = new GetCountBusiness<TEntity>(Set, predicate);
            getCountBusiness.Execute();

            return getCountBusiness.Result;
        }

        public virtual BusinessResult<int> GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            var getCountBusiness = new GetCountBusiness<TEntity>(Set, predicate);
            getCountBusiness.Execute();

            return getCountBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync()
        {
            var getCountAsyncBusiness = new GetCountAsyncBusiness<TEntity>(Set);
            getCountAsyncBusiness.Execute();

            return getCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(string predicate)
        {
            var getCountAsyncBusiness = new GetCountAsyncBusiness<TEntity>(Set, predicate);
            getCountAsyncBusiness.Execute();

            return getCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var getCountAsyncBusiness = new GetCountAsyncBusiness<TEntity>(Set, predicate);
            getCountAsyncBusiness.Execute();

            return getCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(CancellationToken cancellationToken)
        {
            var getCountAsyncBusiness = new GetCountAsyncBusiness<TEntity>(Set, cancellationToken);
            getCountAsyncBusiness.Execute();

            return getCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(CancellationToken cancellationToken, string predicate)
        {
            var getCountAsyncBusiness = new GetCountAsyncBusiness<TEntity>(Set, predicate, cancellationToken);
            getCountAsyncBusiness.Execute();

            return getCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
        {
            var getCountAsyncBusiness = new GetCountAsyncBusiness<TEntity>(Set, predicate, cancellationToken);
            getCountAsyncBusiness.Execute();

            return getCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<TEntity> GetFirst(string orderBy = null, string include = null)
        {
            var getFirstBusiness = new GetFirstBusiness<TEntity>(Set, orderBy, include);
            getFirstBusiness.Execute();

            return getFirstBusiness.Result;
        }

        public virtual BusinessResult<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null)
        {
            var getFirstBusiness = new GetFirstBusiness<TEntity>(Set, predicate, orderBy, include);
            getFirstBusiness.Execute();

            return getFirstBusiness.Result;
        }

        public virtual BusinessResult<TEntity> GetFirst(string predicate, string orderBy = null, string include = null)
        {
            var getFirstBusiness = new GetFirstBusiness<TEntity>(Set, predicate, orderBy, include);
            getFirstBusiness.Execute();

            return getFirstBusiness.Result;
        }

        public virtual BusinessResult<bool> Remove(object id)
        {
            var removeBusiness = new RemoveBusiness<TEntity>(Set, id);
            removeBusiness.Execute();

            return removeBusiness.Result;
        }

        public virtual BusinessResult<bool> Remove(TEntity entity)
        {
            var removeBusiness = new RemoveBusiness<TEntity>(Set, entity);
            removeBusiness.Execute();

            return removeBusiness.Result;
        }

        public virtual BusinessResult<bool> Update(TEntity entity)
        {
            var updateBusiness = new UpdateBusiness<TEntity>(_context, Set, entity);
            updateBusiness.Execute();

            return updateBusiness.Result;
        }
    }
}