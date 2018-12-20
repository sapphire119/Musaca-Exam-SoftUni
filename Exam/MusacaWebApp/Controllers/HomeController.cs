namespace MusacaWebApp.Controllers
{
	using MusacaWebApp.Controllers.Base;
	using MusacaWebApp.ViewModels.Home;
	using SIS.HTTP.Responses;
	using System;
	using System.Linq;

	public class HomeController : BaseController
	{
		public IHttpResponse Index()
		{
			var model = new IndexViewModel();

			if (this.User.IsLoggedIn)
			{
				var currentUser = this.Context.Users.FirstOrDefault(u => u.Username == this.User.Username);
				if (currentUser == null)
				{
					throw new System.Exception("Invalid operation");
				}

				var activeOrders = this.Context.Orders.Where(o => o.Status == Models.Enums.StatusType.Active &&
				o.CashierId == currentUser.Id).ToList();


				foreach (var activeOrder in activeOrders)
				{
					model.Orders.Add(new ViewModels.Orders.OrderViewModel
					{
						Id = activeOrder.Id,
						Name = activeOrder.Product.Name,
						Price = activeOrder.Product.Price,
						Quantity = activeOrder.Quantity
					});
				}

				model.TotalPrice = Math.Round(activeOrders.Sum(o => o.Quantity * o.Product.Price), 2);
			}

			return this.View(model);
		}
	}
}
