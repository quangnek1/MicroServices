using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Log.Information(messageTemplate: "Starting Product API...");

try
{
	builder.Host.UseSerilog(Serilogger.Configure);

	builder.Host.AddAppConfigurations();

	// Add services to the container.
	builder.Services.AddInfrastructure(builder.Configuration);

	var app = builder.Build();
	app.UseInfrastructure();

	app.MigrateDatabase<ProductContext>((context, _) =>
	{
		ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
	})
		.Run();

}
catch (Exception ex)
{
	string type = ex.GetType().Name;
	if (type.Equals("StopTheHostException", StringComparison.Ordinal))	throw;

	Log.Fatal(ex, messageTemplate: "Unhandled exception");
}
finally
{
	Log.Information(messageTemplate: "Shut down Product API complete");
	Log.CloseAndFlush();
}

