namespace MusacaWebApp.ViewModels.Receipts
{
	using MusacaWebApp.ViewModels.Orders;
	using System;
	using System.Collections.Generic;

	public class DetailsViewModel
	{
		public Guid ReceiptId { get; set; }

		public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();

		public decimal TotalPrice { get; set; }

		public string IssueDate { get; set; }

		public string CashierName { get; set; }
	}
}
