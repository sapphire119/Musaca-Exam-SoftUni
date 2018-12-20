namespace MusacaWebApp
{
	using SIS.MvcFramework;
	using System;

	public class Program
	{
		public static void Main()
		{
			WebHost.Start(new StartUp());
		}
	}
}
