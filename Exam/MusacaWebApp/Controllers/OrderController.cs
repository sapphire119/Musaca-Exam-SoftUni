namespace MusacaWebApp.Controllers
{
	using MusacaWebApp.Controllers.Base;
	using MusacaWebApp.ViewModels.Orders;
	using System.Linq;
	using SIS.HTTP.Responses;
	using SIS.MvcFramework;
	using MusacaWebApp.ViewModels.Home;
	using System;
	using MusacaWebApp.Models;

	public class OrdersController : BaseController
	{
		private const int BarcodeRequiredLength = 12;

		[Authorize]
		[HttpPost]
		public IHttpResponse Create(CreateInputModel inputModel)
		{
			if (inputModel.Barcode.Length != BarcodeRequiredLength)
			{
				return this.BadRequestErrorWithView("Invalid barcode length");
			}

			var firstHalfOfBarcode = inputModel.Barcode.Substring(0, 6);
			var secondHalfOfBarcode = inputModel.Barcode.Substring(6);

			var isFirstHalfAnInt = int.TryParse(firstHalfOfBarcode, out var firstHalfNumber);
			var isSecondHalAnInt = int.TryParse(secondHalfOfBarcode, out var secondHalf);

			if (!isFirstHalfAnInt || !isSecondHalAnInt)
			{
				return this.BadRequestErrorWithView("Barcode must only be valid integer numbers!");
			}

			var isQuantityInt = int.TryParse(inputModel.Qunatity, out var quantity);
			if (!isQuantityInt)
			{
				return this.BadRequestErrorWithView("Quantity must only be valid integer numbers!");
			}

			var product = this.Context.Products.FirstOrDefault(p => p.Barcode == inputModel.Barcode);
			if (product == null)
			{
				return this.BadRequestErrorWithView("Product does not exist!");
			}

			var currentUser = this.Context.Users.FirstOrDefault(u => u.Username == this.User.Username);
			if (currentUser == null)
			{
				throw new Exception("Invalid action");
			}

			var currentOrder = new Order
			{
				Cashier = currentUser,
				Product = product,
				Quantity = quantity,
				Status = Models.Enums.StatusType.Active,
			};

			this.Context.Orders.Add(currentOrder);

			try
			{
				this.Context.SaveChanges();
			}
			catch (Exception)
			{
				return this.ServerError("Coudn't save data into Db Context");
			}
			//if (!Request.Session.ContainsParameter("products"))
			//{
			//	var model = new IndexViewModel();

			//	model.Orders.Add(new OrderViewModel
			//	{
			//		Id = product.Id,
			//		Name = product.Name,
			//		Price = Math.Round(product.Price, 2),
			//		Quantity = quantity
			//	});

			//	model.TotalPrice = Math.Round(model.Orders.Sum(o => o.Price), 2);

			//	this.Request.Session.AddParameter("products", model);
			//}
			//else
			//{
			//	var products = (IndexViewModel)this.Request.Session.GetParameter("products");

			//	var currentOrder = new OrderViewModel
			//	{
			//		Id = product.Id,
			//		Name = product.Name,
			//		Price = Math.Round(product.Price, 2),
			//		Quantity = quantity
			//	};

			//	products.Orders.Add(currentOrder);

			//	products.TotalPrice = Math.Round(products.Orders.Sum(o => o.Price * (decimal)o.Quantity), 2);

			//	this.Request.Session.ClearParameters();

			//	this.Request.Session.AddParameter("products", products);
			//}

			return this.Redirect("/Home/Index");
		}


	}
}
