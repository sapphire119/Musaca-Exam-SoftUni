﻿namespace MusacaWebApp
{
	using SIS.MvcFramework;
	using SIS.MvcFramework.Logger;
	using SIS.MvcFramework.Services;

	public class StartUp : IMvcApplication
	{
		public MvcFrameworkSettings Configure()
		{
			return new MvcFrameworkSettings();
		}

		public void ConfigureServices(IServiceCollection collection)
		{
			collection.AddService<IHashService, HashService>();
			collection.AddService<IUserCookieService, UserCookieService>();
			collection.AddService<ILogger>(() => new FileLogger($"log.txt"));

			/*
             * In ASP.NET Core
            collection.AddTransient<IUserCookieService, UserCookieService>();
            collection.AddScoped<IUserCookieService, UserCookieService>();
            collection.AddSingleton<IUserCookieService, UserCookieService>();
             */
		}
	}
}
