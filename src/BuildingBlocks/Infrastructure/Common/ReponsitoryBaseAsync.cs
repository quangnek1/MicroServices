using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common
{
	public class ReponsitoryBaseAsync<T, K, TContext> : IReponsitoryBaseAsync<T, K, TContext> where T : EntityBase<K> where TContext : DbContext
	{
		private readonly TContext _context;
		private readonly IUnitOfWork<TContext> _unitOfWork;
		public ReponsitoryBaseAsync(TContext context, IUnitOfWork<TContext> unitOfWork)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}
		public Task<IDbContextTransaction> BeginTransactionAsync()
		{
			throw new NotImplementedException();
		}

		public Task<K> CreateAsync(T entity)
		{
			throw new NotImplementedException();
		}

		public Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(T entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteListAsync(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}

		public Task EndTransactionAsync()
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> FindAll(bool trackChanges = false)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> FindByCondition(Expression<Func<T, object>> condition, bool trackChanges = false)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public Task<T?> GetByIdAsync(K id)
		{
			throw new NotImplementedException();
		}

		public Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public Task RollBackTransactionAsync()
		{
			throw new NotImplementedException();
		}

		public Task<int> SaveChangeAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(T entity)
		{
			throw new NotImplementedException();
		}

		public Task UpdateListAsync(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}
	}
}
