using Basket.API;
using Basket.API.Extensions;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Log.Information(messageTemplate: $"Starting {builder.Environment.EnvironmentName} up");

try
{
	// Add services to the container.
	builder.Host.UseSerilog(Serilogger.Configure);
	builder.Host.AddAppConfigurations();
	builder.Services.AddConfigurationSettings(builder.Configuration);
	builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

	builder.Services.ConfigureServices();
	builder.Services.ConfigureRedis(builder.Configuration);
	builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

	//Configure Mass Transit
	builder.Services.ConfigureMassTransit();

	builder.Services.AddControllers();
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	//app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();

	app.Run();

}
catch (Exception ex)
{
	string type = ex.GetType().Name;
	if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

	Log.Fatal(ex, messageTemplate: "Unhandled exception");
}
finally
{
	Log.Information(messageTemplate: "Shut down Customer API complete");
	Log.CloseAndFlush();
}

