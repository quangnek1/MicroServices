using Common.Logging;
using Inventory.Product.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Serilogger.Configure);
Log.Information(messageTemplate: "Starting Inventory.Product.API...");

try
{
	// Add services to the container.
	builder.Services.AddConfigurationSettings(builder.Configuration);
	builder.Services.AddControllers();
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();
	builder.Services.Configure<RouteOptions>(option => option.LowercaseUrls = true);
	builder.Services.AddInfrastructureServices();
	builder.Services.ConfigureMondoDbClient();

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapDefaultControllerRoute();

	app.MigrateDatabase().Run();
	//	app.Run();

}
catch (Exception ex)
{
	string type = ex.GetType().Name;
	if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

	Log.Fatal(ex, messageTemplate: "Unhandled exception");
}
finally
{
	Log.Information(messageTemplate: "Shut down Product API complete");
	Log.CloseAndFlush();
}

