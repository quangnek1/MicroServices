using Microsoft.EntityFrameworkCore;

namespace Product.API.Extensions
{
	public static class HostExtensions
	{
		public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var configuration = services.GetRequiredService<IConfiguration>();
				var logger = services.GetRequiredService<ILogger<TContext>>();
				var context = services.GetService<TContext>();

				try
				{
					logger.LogInformation("Migrating mysql database");
					ExecuteMigtations(context);
					logger.LogInformation("Migrated mysql database");
					InvokeSeeder(seeder, context, services);
				}
				catch (Exception)
				{

					throw;
				}
			}
			return host;
		}
		public static void ExecuteMigtations<TContext>(TContext context) where TContext : DbContext
		{
			context.Database.Migrate();
		}
		private static void InvokeSeeder<Tcontext>(Action<Tcontext, IServiceProvider> seeder, Tcontext context, IServiceProvider services) where Tcontext : DbContext
		{
			seeder(context,services);
		}
	}
}
