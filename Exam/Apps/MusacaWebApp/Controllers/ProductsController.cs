namespace MusacaWebApp.Controllers
{
	using MusacaWebApp.Controllers.Base;
	using MusacaWebApp.Models;
	using MusacaWebApp.Models.Enums;
	using MusacaWebApp.ViewModels.Products;
	using SIS.HTTP.Responses;
	using SIS.MvcFramework;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	public class ProductsController : BaseController
	{
		private const int BarcodeRequiredLength = 12;

		[Authorize]
		public IHttpResponse All()
		{
			var products = this.Context.Products.ToList();

			var model = new List<AllViewModel>();

			foreach (var product in products)
			{
				model.Add(new AllViewModel
				{
					Name = product.Name,
					Barcode = product.Barcode,
					ImageUrl = WebUtility.UrlDecode(product.Picture),
					Price = Math.Round(product.Price)
				});
			}

			;
			
			return this.View(model.ToArray());
		}

		[Authorize(nameof(UserRole.Admin))]
		public IHttpResponse Create()
		{
			return this.View();
		}

		[Authorize(nameof(UserRole.Admin))]
		[HttpPost]
		public IHttpResponse Create(CreateInputModel model)
		{
			if (model.Barcode.Length != BarcodeRequiredLength)
			{
				return this.BadRequestErrorWithView("Invalid barcode length");
			}

			var firstHalfOfBarcode = model.Barcode.Substring(0, 6);
			var secondHalfOfBarcode = model.Barcode.Substring(6);

			var isFirstHalfAnInt = int.TryParse(firstHalfOfBarcode, out var firstHalfNumber);
			var isSecondHalAnInt = int.TryParse(secondHalfOfBarcode, out var secondHalf);

			if (!isFirstHalfAnInt || !isSecondHalAnInt)
			{
				return this.BadRequestErrorWithView("Barcode must only be valid integer numbers!");
			}

			var isValidPrice = decimal.TryParse(model.Price, out var price);
			if (!isValidPrice)
			{
				return this.BadRequestErrorWithView("Invalid Price format");
			}


			var product = new Product
			{
				Barcode = model.Barcode,
				Name = model.Name,
				Picture = string.IsNullOrWhiteSpace(model.ImageUrl) ? string.Empty : WebUtility.UrlEncode(model.ImageUrl),
				Price = price
			};

			this.Context.Products.Add(product);

			try
			{
				this.Context.SaveChanges();
			}
			catch (Exception)
			{
				return this.ServerError("Coudnot save data in db");
			}

			return this.Redirect("/Home/Index");
		}
	}
}
