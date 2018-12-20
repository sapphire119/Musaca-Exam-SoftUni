namespace MusacaWebApp.Controllers
{
	using MusacaWebApp.Controllers.Base;
	using MusacaWebApp.Models;
	using MusacaWebApp.Models.Enums;
	using MusacaWebApp.ViewModels.Receipts;
	using MusacaWebApp.ViewModels.Users;
	using SIS.HTTP.Responses;
	using SIS.MvcFramework;
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	public class ReceiptsController : BaseController
	{
		[Authorize]
		[HttpPost]
		public IHttpResponse Cashout()
		{
			var currentUser = this.Context.Users.FirstOrDefault(u => u.Username == this.User.Username);
			if (currentUser == null)
			{
				throw new System.Exception("Invalid action on action Cashout");
			}

			var ordersForCurrentUser = this.Context.Orders.Where(o => o.CashierId == currentUser.Id &&
			o.Status == Models.Enums.StatusType.Active).ToList();

			if (!ordersForCurrentUser.Any())
			{
				return this.BadRequestErrorWithView("You have no products to add.");
			}

			var receipt = new Receipt
			{
				CashierId = currentUser.Id,
				IssuedOn = DateTime.UtcNow,
			};

			foreach (var order in ordersForCurrentUser)
			{
				order.Status = Models.Enums.StatusType.Completed;

				receipt.Orders.Add(order);
			}

			this.Context.Receipts.Add(receipt);

			try
			{
				this.Context.SaveChanges();
			}
			catch (Exception)
			{
				return this.ServerError($"Could not save data for {this.GetType().Name}");
			}

			return this.Redirect("/Receipts/Details?receiptId=" + receipt.Id);
		}

		[Authorize]
		public IHttpResponse Details(string receiptId)
		{
			var receiptIdToGuid = Guid.Parse(receiptId.ToUpper());
			var currentReceipt = this.Context.Receipts.FirstOrDefault(u => u.Id == receiptIdToGuid);
			if (currentReceipt == null)
			{
				return this.Redirect("/Home/Index");
			}
			
			var model = new DetailsViewModel
			{
				ReceiptId = currentReceipt.Id,
				CashierName = currentReceipt.Cashier.Username,
				IssueDate = currentReceipt.IssuedOn.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture),
				TotalPrice = currentReceipt.Orders.Sum(o => o.Quantity * o.Product.Price)
			};

			foreach (var order in currentReceipt.Orders)
			{
				model.Orders.Add(new ViewModels.Orders.OrderViewModel
				{
					Id = order.Id,
					Name = order.Product.Name,
					Price = order.Product.Price,
					Quantity = order.Quantity
				});
			}

			return this.View(model);
		}

		[Authorize(nameof(UserRole.Admin))]
		public IHttpResponse All()
		{
			if (this.User.Username == null || this.User.Role != "Admin")
			{
				throw new Exception("Invalid action on Profile");
			}

			var allreceipts = this.Context.Receipts.ToList();

			var model = new List<AllViewModel>();

			foreach (var receipt in allreceipts)
			{
				model.Add(new AllViewModel
				{
					Cashier = receipt.Cashier.Username,
					Id = receipt.Id.ToString(),
					IssueDate = receipt.IssuedOn.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture),
					Total = Math.Round(receipt.Orders.Sum(o => o.Quantity * o.Product.Price), 2)
				});
			}

			return this.View(model.ToArray());
		}
	}
}