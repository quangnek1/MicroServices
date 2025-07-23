using Common.Logging;
using Contracts.Common.Interfaces;
using Contracts.Messages;
using Infrastructure.Common;
using Infrastructure.Messages;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Serilogger.Configure);
Log.Information(messageTemplate: "Starting Ordering API...");

try
{
	// Add services to the container.
	builder.Host.AddAppConfigurations();
	builder.Services.AddConfigurationSettings(configuration: builder.Configuration);
	builder.Services.AddApplicationServices();
	builder.Services.AddInfrastructureServices(configuration: builder.Configuration);
	builder.Services.AddScoped<IMessageProducer, RabbitMQProducer>();
	builder.Services.AddScoped<ISerializeServices, SerializeServices>();
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

	// Initialise and seed the database
	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;
		var orderContextSeed = services.GetRequiredService<OrderContextSeed>();
		await orderContextSeed.InitialiseAsync();
		await orderContextSeed.SeedAsync();
	}

	app.UseHttpsRedirection();

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
	Log.Information(messageTemplate: "Shut down Ordering API complete");
	Log.CloseAndFlush();
}

