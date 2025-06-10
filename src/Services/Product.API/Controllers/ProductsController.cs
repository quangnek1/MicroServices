using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.Entities;
using Product.API.Reponsitories.Interfaces;
using Shared.DTOs.Product;

namespace Product.API.Controllers
{
	[ApiController]
	[Route(template: "api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;
		public ProductsController(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		#region CRUD
		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			var products = await _productRepository.GetProducts();
			var result = _mapper.Map<IEnumerable<ProductDto>>(products);
			return Ok(result);
		}
		[HttpGet(template: "{id:long}")]
		public async Task<IActionResult> GetProduct([Required] long id)
		{
			var product = await _productRepository.GetProduct(id);
			if (product == null) return NotFound();

			var result = _mapper.Map<ProductDto>(product);
			return Ok(result);
		}
		[HttpPost]
		public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
		{
			var productEntity = await _productRepository.GetProductByNo(productDto.No);
			if (productEntity != null) return BadRequest(error:$"Product No: {productDto.No} is existed");

			var product = _mapper.Map<CatalogProduct>(productDto);
			await _productRepository.CreateProduct(product);
			await _productRepository.SaveChangeAsync();
			var result = _mapper.Map<ProductDto>(product);
			return Ok(result);
		}
		[HttpPut(template: "{id:long}")]
		public async Task<IActionResult> UpdateProduct([Required] long id, [FromBody] UpdateProductDto productDto)
		{
			var product = await _productRepository.GetProduct(id);
			if (product == null) return NotFound();

			var updateProduct = _mapper.Map(source: productDto, destination: product);
			await _productRepository.UpdateProduct(updateProduct);
			await _productRepository.SaveChangeAsync();
			var result = _mapper.Map<ProductDto>(product);
			return Ok(result);
		}
		[HttpDelete(template: "{id:long}")]
		public async Task<IActionResult> DeleteProduct([Required] long id)
		{
			var product = await _productRepository.GetProduct(id);
			if (product == null) return NotFound();

			await _productRepository.DeleteProduct(id);
			await _productRepository.SaveChangeAsync();
			return NoContent();
		}
		#endregion

		#region Additional Resources
		[HttpGet(template:"get-product-by-no/{productNo}")]
		public async Task<IActionResult> GetProductByNo([Required] string productNo)
		{
			var product = await _productRepository.GetProductByNo(productNo);
			if (product == null) return NotFound();

			var result = _mapper.Map<ProductDto>(product);
			return Ok(result);
		}
		#endregion
	}
}
