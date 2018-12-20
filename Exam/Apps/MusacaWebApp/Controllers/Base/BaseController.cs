namespace MusacaWebApp.Controllers.Base
{
	using MusacaWebApp.Data;
	using SIS.MvcFramework;

	public class BaseController : Controller
	{
		public BaseController()
		{
			this.Context = new MusacaDbContext();
		}

		protected MusacaDbContext Context { get; set; }
	}
}
