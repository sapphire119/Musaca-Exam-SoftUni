namespace MusacaWebApp.ViewModels.Home
{
	using MusacaWebApp.ViewModels.Orders;
	using System.Collections.Generic;

	public class IndexViewModel
	{
		public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
		public decimal TotalPrice { get; set; }
	}
}
