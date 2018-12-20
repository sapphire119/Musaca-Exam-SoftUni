using Microsoft.EntityFrameworkCore;
using MusacaWebApp.Models;

namespace MusacaWebApp.Data
{
	public class MusacaDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Receipt> Receipts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(ConncetionConfig.ConnectionString)
					.UseLazyLoadingProxies();
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Order>(entity =>
			{
				entity.HasOne(e => e.Product)
					.WithMany(p => p.Orders);

				entity.HasOne(e => e.Cashier)
					.WithMany(u => u.Orders);

				entity.HasOne(e => e.Receipt)
					.WithMany(r => r.Orders);
			});

			builder.Entity<Receipt>(entity =>
			{
				entity.HasOne(e => e.Cashier)
					.WithMany(u => u.Receipts);
			});
		}
	}
}
