namespace MusacaWebApp.Models
{
	using MusacaWebApp.Models.BaseModel;
	using System;
	using System.Collections.Generic;

	public class Receipt : BaseModel<Guid>
	{
		public DateTime IssuedOn { get; set; }

		public int CashierId { get; set; }

		public virtual User Cashier { get; set; }

		public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
	}
}
