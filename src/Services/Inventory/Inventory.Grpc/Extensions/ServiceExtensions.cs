using Infrastructure.Extensions;
using Inventory.Grpc.Repositores;
using Inventory.Grpc.Repositores.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Grpc.Extensions;

public static class ServiceExtensions
{
	public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
	{
		var databaseSettings = configuration.GetSection(key: nameof(MongoDbSettings))
			.Get<MongoDbSettings>();
		services.AddSingleton(databaseSettings);

		return services;
	}
	private static string getMongoConnectionString(this IServiceCollection services)
	{
		var settings = services.GetOptions<MongoDbSettings>(nameof(MongoDbSettings));
		if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
			throw new ArgumentNullException(paramName: "DatabaseSettings is not configured.");

		var databaseName = settings.DatabaseName;
		var mongoDbConnectionString = settings.ConnectionString + "/" + databaseName + "?authSource=admin";

		return mongoDbConnectionString;
	}
	public static void ConfigureMondoDbClient(this IServiceCollection services)
	{
		services.AddSingleton<IMongoClient>(new MongoClient(getMongoConnectionString(services)))
			.AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
	}
	public static void AddInfrastructureServices(this IServiceCollection services)
	{
		services.AddScoped<IInventoryRepository, InventoryRepository>();
	}
}
