﻿using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Reponsitories.Interfaces;

namespace Product.API.Reponsitories
{
	public class ProductRepository : RepositoryBaseAsync<CatalogProduct, long, ProductContext>, IProductRepository
	{
		public ProductRepository(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork) : base(dbContext, unitOfWork)
		{

		}
		public Task<CatalogProduct> GetProduct(long id) => GetByIdAsync(id);
		public Task<CatalogProduct> GetProductByNo(string productNo) => FindByCondition(x => x.No.Equals(productNo)).SingleOrDefaultAsync();

		public async Task<IEnumerable<CatalogProduct>> GetProducts() => await FindAll().ToListAsync();
		public Task CreateProduct(CatalogProduct product) => CreateAsync(product);
		public Task UpdateProduct(CatalogProduct product) => UpdateAsync(product);
		public async Task DeleteProduct(long id)
		{
			var product = await GetProduct(id);
			if (product != null) DeleteAsync(product);
		}
	}
}
