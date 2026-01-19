using Shared.Enums.Inventory;

namespace Shared.DTOs.Inventory
{
	public class PurchaseProductDto
	{
		public EDocumentType DocumentType => EDocumentType.Purchase;
		public string? ItemNo { get; set; }
		public string? DocumentNo { get; set; }// "PO-2022-07-XXXX"
		public string? ExternalDocumentNo { get; set; }
		public int Quantity { get; set; }
	}
}
