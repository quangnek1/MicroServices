﻿using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;

namespace Product.API.Persistence
{
	public class ProductContext : DbContext
	{
		public ProductContext(DbContextOptions<ProductContext> options) : base(options)
		{
		}
		public DbSet<CatalogProduct> Products { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<CatalogProduct>().HasIndex(x => x.No)
				.IsUnique();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			var modified = ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

			foreach (var item in modified)
			{
				switch (item.State)
				{
					case EntityState.Added:
						if (item.Entity is IDateTracking addedEntity)
						{
							addedEntity.CreatedDate = DateTime.UtcNow;
							item.State = EntityState.Added;
						}
						break;
					case EntityState.Modified:
						Entry(item.Entity).Property("Id").IsModified = false;
						if (item.Entity is IDateTracking modifiedEntity)
						{
							modifiedEntity.LastModifiedDate = DateTime.UtcNow;
							item.State = EntityState.Modified;
						}
						break;
					case EntityState.Deleted:
						break;

					default:
						break;
				}
			}
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
