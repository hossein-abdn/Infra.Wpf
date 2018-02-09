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
        protected DbContext Context { get; private set; }

        protected AddBusiness<TEntity> AddBusiness { get; set; }

        protected AnyBusiness<TEntity> AnyBusiness { get; set; }

        protected FindByIdAsyncBusiness<TEntity> FindByIdAsyncBusiness { get; set; }

        protected FindByIdBusiness<TEntity> FindByIdBusiness { get; set; }

        protected GetAllAsyncBusiness<TEntity> GetAllAsyncBusiness { get; set; }

        protected GetAllBusiness<TEntity> GetAllBusiness { get; set; }

        protected GetCountAsyncBusiness<TEntity> GetCountAsyncBusiness { get; set; }

        protected GetCountBusiness<TEntity> GetCountBusiness { get; set; }

        protected GetFirstBusiness<TEntity> GetFirstBusiness { get; set; }

        protected RemoveBusiness<TEntity> RemoveBusiness { get; set; }

        protected UpdateBusiness<TEntity> UpdateBusiness { get; set; }

        protected Logger Logger { get; set; }

        private bool logOnException { get; set; }

        private DbSet<TEntity> _set;

        protected DbSet<TEntity> Set
        {
            get { return _set ?? (_set = Context.Set<TEntity>()); }
        }

        public Repository(DbContext context, Logger logger = null, bool logOnException = true)
        {
            Context = context;
            this.Logger = logger;
            this.logOnException = logOnException;

            AddBusiness = new AddBusiness<TEntity>(logger, logOnException);
            AnyBusiness = new AnyBusiness<TEntity>(logger);
            FindByIdAsyncBusiness = new FindByIdAsyncBusiness<TEntity>(logger);
            FindByIdBusiness = new FindByIdBusiness<TEntity>(logger);
            GetAllAsyncBusiness = new GetAllAsyncBusiness<TEntity>(logger);
            GetAllBusiness = new GetAllBusiness<TEntity>(logger);
            GetCountAsyncBusiness = new GetCountAsyncBusiness<TEntity>(logger);
            GetCountBusiness = new GetCountBusiness<TEntity>(logger);
            GetFirstBusiness = new GetFirstBusiness<TEntity>(logger);
            RemoveBusiness = new RemoveBusiness<TEntity>(logger, logOnException);
            UpdateBusiness = new UpdateBusiness<TEntity>(logger, logOnException);
        }

        public virtual BusinessResult<bool> Add(TEntity entity)
        {
            AddBusiness.Config(Context, Set, entity);
            AddBusiness.Execute();

            return AddBusiness.Result;
        }

        public virtual BusinessResult<bool> Any()
        {
            AnyBusiness.Config(Set);
            AnyBusiness.Execute();

            return AnyBusiness.Result;
        }

        public virtual BusinessResult<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            AnyBusiness.Config(Set, predicate);
            AnyBusiness.Execute();

            return AnyBusiness.Result;
        }

        public virtual BusinessResult<bool> Any(string predicate, object[] values = null)
        {
            AnyBusiness.Config(Set, predicate, values);
            AnyBusiness.Execute();

            return AnyBusiness.Result;
        }

        public virtual BusinessResult<TEntity> FindById(object id)
        {
            FindByIdBusiness.Config(Set, id);
            FindByIdBusiness.Execute();

            return FindByIdBusiness.Result;
        }

        public virtual BusinessResult<Task<TEntity>> FindByIdAsync(object id)
        {
            FindByIdAsyncBusiness.Config(Set, id);
            FindByIdAsyncBusiness.Execute();

            return FindByIdAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<TEntity>> FindByIdAsync(CancellationToken cancellationToken, object id)
        {
            FindByIdAsyncBusiness.Config(Set, id, cancellationToken);
            FindByIdAsyncBusiness.Execute();

            return FindByIdAsyncBusiness.Result;
        }

        public virtual BusinessResult<List<TEntity>> GetAll(string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            GetAllBusiness.Config(Set, orderBy, take, skip, include);
            GetAllBusiness.Execute();

            return GetAllBusiness.Result;
        }

        public virtual BusinessResult<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            GetAllBusiness.Config(Set, predicate, orderBy, take, skip, include);
            GetAllBusiness.Execute();

            return GetAllBusiness.Result;
        }

        public virtual BusinessResult<List<TEntity>> GetAll(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            GetAllBusiness.Config(Set, predicate, values, orderBy, take, skip, include);
            GetAllBusiness.Execute();

            return GetAllBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            GetAllAsyncBusiness.Config(Set, orderBy, take, skip, include);
            GetAllAsyncBusiness.Execute();

            return GetAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            GetAllAsyncBusiness.Config(Set, predicate, orderBy, take, skip, include);
            GetAllAsyncBusiness.Execute();

            return GetAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            GetAllAsyncBusiness.Config(Set, predicate, values, orderBy, take, skip, include);
            GetAllAsyncBusiness.Execute();

            return GetAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            GetAllAsyncBusiness.Config(Set, cancellationToken, orderBy, take, skip, include);
            GetAllAsyncBusiness.Execute();

            return GetAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            GetAllAsyncBusiness.Config(Set, predicate, cancellationToken, orderBy, take, skip, include);
            GetAllAsyncBusiness.Execute();

            return GetAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken, string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            GetAllAsyncBusiness.Config(Set, predicate, values, cancellationToken, orderBy, take, skip, include);
            GetAllAsyncBusiness.Execute();

            return GetAllAsyncBusiness.Result;
        }

        public virtual BusinessResult<int> GetCount()
        {
            GetCountBusiness.Config(Set);
            GetCountBusiness.Execute();

            return GetCountBusiness.Result;
        }

        public virtual BusinessResult<int> GetCount(string predicate, object[] values = null)
        {
            GetCountBusiness.Config(Set, predicate, values);
            GetCountBusiness.Execute();

            return GetCountBusiness.Result;
        }

        public virtual BusinessResult<int> GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            GetCountBusiness.Config(Set, predicate);
            GetCountBusiness.Execute();

            return GetCountBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync()
        {
            GetCountAsyncBusiness.Config(Set);
            GetCountAsyncBusiness.Execute();

            return GetCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(string predicate, object[] values = null)
        {
            GetCountAsyncBusiness.Config(Set, predicate, values);
            GetCountAsyncBusiness.Execute();

            return GetCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            GetCountAsyncBusiness.Config(Set, predicate);
            GetCountAsyncBusiness.Execute();

            return GetCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(CancellationToken cancellationToken)
        {
            GetCountAsyncBusiness.Config(Set, cancellationToken);
            GetCountAsyncBusiness.Execute();

            return GetCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(CancellationToken cancellationToken, string predicate, object[] values = null)
        {
            GetCountAsyncBusiness.Config(Set, predicate, values, cancellationToken);
            GetCountAsyncBusiness.Execute();

            return GetCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<Task<int>> GetCountAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
        {
            GetCountAsyncBusiness.Config(Set, predicate, cancellationToken);
            GetCountAsyncBusiness.Execute();

            return GetCountAsyncBusiness.Result;
        }

        public virtual BusinessResult<TEntity> GetFirst(string orderBy = null, string include = null)
        {
            GetFirstBusiness.Config(Set, orderBy, include);
            GetFirstBusiness.Execute();

            return GetFirstBusiness.Result;
        }

        public virtual BusinessResult<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate, string orderBy = null, string include = null)
        {
            GetFirstBusiness.Config(Set, predicate, orderBy, include);
            GetFirstBusiness.Execute();

            return GetFirstBusiness.Result;
        }

        public virtual BusinessResult<TEntity> GetFirst(string predicate, object[] values = null, string orderBy = null, string include = null)
        {
            GetFirstBusiness.Config(Set, predicate, values, orderBy, include);
            GetFirstBusiness.Execute();

            return GetFirstBusiness.Result;
        }

        public virtual BusinessResult<bool> Remove(object id)
        {
            RemoveBusiness.Config(Context, Set, id);
            RemoveBusiness.Execute();

            return RemoveBusiness.Result;
        }

        public virtual BusinessResult<bool> Remove(TEntity entity)
        {
            RemoveBusiness.Config(Context, Set, entity);
            RemoveBusiness.Execute();

            return RemoveBusiness.Result;
        }

        public virtual BusinessResult<bool> Update(TEntity entity)
        {
            UpdateBusiness.Config(Context, Set, entity);
            UpdateBusiness.Execute();

            return UpdateBusiness.Result;
        }
    }
}