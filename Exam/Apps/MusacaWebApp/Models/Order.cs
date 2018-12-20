namespace MusacaWebApp.Models
{
	using MusacaWebApp.Models.BaseModel;
	using MusacaWebApp.Models.Enums;
	using System;

	public class Order : BaseModel<int>
	{
		public StatusType Status { get; set; }

		public int ProductId { get; set; }

		public virtual Product Product { get; set; }

		public int Quantity { get; set; }

		public int CashierId { get; set; }

		public virtual User Cashier { get; set; }

		public Guid? ReceiptId { get; set; }

		public virtual Receipt Receipt { get; set; }
	}
}
