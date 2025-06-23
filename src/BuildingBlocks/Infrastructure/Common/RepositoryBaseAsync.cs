using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common
{
	public class RepositoryBaseAsync<T, K, TContext> : RepositoryQueryBase<T, K, TContext>,

		IRepositoryBaseAsync<T, K, TContext> where T : EntityBase<K> where TContext : DbContext
	{
		private readonly TContext _context;
		private readonly IUnitOfWork<TContext> _unitOfWork;
		public RepositoryBaseAsync(TContext context, IUnitOfWork<TContext> unitOfWork) : base(context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}
		public Task<IDbContextTransaction> BeginTransactionAsync() => _context.Database.BeginTransactionAsync();

		public async Task<K> CreateAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			return entity.Id;
		}

		public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
		{
			await _context.Set<T>().AddRangeAsync(entities);
			return entities.Select(e => e.Id).ToList();
		}
		public Task UpdateAsync(T entity)
		{
			if (_context.Entry(entity).State == EntityState.Unchanged) return Task.CompletedTask;
			T exist = _context.Set<T>().Find(entity.Id);
			_context.Entry(exist).CurrentValues.SetValues(entity);
			return Task.CompletedTask;
		}
		public Task UpdateListAsync(IEnumerable<T> entities) => _context.Set<T>().AddRangeAsync(entities);
		public Task DeleteAsync(T entity)
		{
			_context.Set<T>().Remove(entity);
			return Task.CompletedTask;
		}
		public Task DeleteListAsync(IEnumerable<T> entities)
		{
			_context.Set<T>().RemoveRange(entities);
			return Task.CompletedTask;
		}
		public async Task EndTransactionAsync()
		{
			await _context.Database.CommitTransactionAsync();
		}
		public async Task RollBackTransactionAsync() => await _context.Database.RollbackTransactionAsync();

		public Task<int> SaveChangeAsync() => _unitOfWork.CommitAsync();

	}
}
