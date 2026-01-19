using System.ComponentModel.DataAnnotations;
using System.Net;
using Infrastructure.Common.Models;
using Inventory.Product.API.Services;
using Inventory.Product.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Inventory;

namespace Inventory.Product.API.Controllers
{
	[ApiController]
	[Route(template: "api/[controller]")]
	public class InventoryController : ControllerBase
	{
		private readonly IInventoryService _inventoryService;
		public InventoryController(IInventoryService inventoryService)
		{
			_inventoryService = inventoryService;
		}

		/// <summary>
		/// api/inventory/items/{itemNo}
		/// </summary>
		/// <param name="itemNo"></param>
		/// <returns></returns>
		[Route(template: "items/{itemNo}", Name = "GetAllByItemNo")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), statusCode: (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNo([Required] string itemNo)
		{
			var result = await _inventoryService.GetAllByItemNoAsync(itemNo);
			return Ok(result);
		}

		/// <summary>
		/// api/inventory/items/{itemNo}/paging
		/// </summary>
		/// <param name="itemNo"></param>
		/// <returns></returns>
		[Route("items/{itemNo}/paging", Name = "GetAllByItemNoPaging")]
		[HttpGet]
		[ProducesResponseType(typeof(PagedList<InventoryEntryDto>), statusCode: (int)HttpStatusCode.OK)]
		public async Task<ActionResult<PagedList<InventoryEntryDto>>> GetAllByItemNoPaging([Required] string itemNo,
			[FromQuery] GetInventoryPagingQuery query)
		{
			query.SetItemNo(itemNo);
			var result = await _inventoryService.GetAllByItemNoPagingAsync(query);
			return Ok(result);
		}

		/// <summary>
		/// api/inventory/{id}
		/// </summary>
		/// <param name="itemNo"></param>
		/// <returns></returns>
		[Route(template: "{id}", Name = "GetInventoryById")]
		[HttpGet]
		[ProducesResponseType(typeof(InventoryEntryDto), statusCode: (int)HttpStatusCode.OK)]
		public async Task<ActionResult<InventoryEntryDto>> GetInventoryById([Required] string id)
		{
			var result = await _inventoryService.GetByIdAsync(id);
			return Ok(result);

		}

		/// <summary>
		/// api/inventory/purchase/{itemNo}
		/// </summary>
		/// <param name="itemNo"></param>
		/// <returns></returns>
		[HttpPost(template: "purchase/{itemNo}", Name = "PurchaseOrder")]
		[ProducesResponseType(typeof(InventoryEntryDto), statusCode: (int)HttpStatusCode.OK)]
		public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder(
			[Required] string itemNo,
			[FromBody] PurchaseProductDto model)
		{
			var result = await _inventoryService.PurchaseItemAsync(itemNo, model);
			return Ok(result);

		}

		/// <summary>
		/// api/inventory/purchase/{itemNo}
		/// </summary>
		/// <param name="itemNo"></param>
		/// <returns></returns>
		[HttpDelete(template: "{id}", Name = "DeleteById")]
		[ProducesResponseType(typeof(InventoryEntryDto), statusCode: (int)HttpStatusCode.OK)]
		public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder([Required] string id)
		{
			var entity = await _inventoryService.GetByIdAsync(id);
			if (entity == null) return NotFound();

			await _inventoryService.DeleteAsync(id);
			return NoContent();

		}
	}
}
