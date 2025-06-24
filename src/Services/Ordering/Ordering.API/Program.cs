using Common.Logging;
using Serilog;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Serilogger.Configure);
Log.Information(messageTemplate: "Starting Ordering API...");

try
{
	// Add services to the container.
	builder.Services.AddInfrastructureServices(builder.Configuration);

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

