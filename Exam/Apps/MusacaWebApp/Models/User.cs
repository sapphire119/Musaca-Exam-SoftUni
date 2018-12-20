namespace MusacaWebApp.Models
{
	using MusacaWebApp.Models.BaseModel;
	using MusacaWebApp.Models.Enums;
	using System.Collections.Generic;

	public class User : BaseModel<int>
	{
		public string Username { get; set; }

		public string Password { get; set; }

		public string Email { get; set; }

		public UserRole Role { get; set; }

		public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

		public virtual ICollection<Receipt> Receipts { get; set; } = new HashSet<Receipt>();
	}
}