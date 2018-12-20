namespace MusacaWebApp.Models
{
	using MusacaWebApp.Models.BaseModel;
	using System.Collections.Generic;

	public class Product : BaseModel<int>
	{
		public string Name { get; set; }

		public decimal Price { get; set; }

		public string Barcode { get; set; }

		public string Picture { get; set; }

		public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
	}
}
