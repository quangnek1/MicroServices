using Infrastructure.Middlewares;
using Ocelot.Middleware;
using OcelotApiGw.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Information(messageTemplate: $"Starting {builder.Environment.EnvironmentName} up");


try
{
	// Add services to the container.
	builder.Host.AddAppConfigurations();
	builder.Services.AddConfigurationSettings(builder.Configuration);

	builder.Services.AddControllers();
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();
	builder.Services.ConfigureOcelot(builder.Configuration);
	builder.Services.ConfigureCors(builder.Configuration);

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: $"{builder.Environment.EnvironmentName} v1"));
	}

	app.UseCors("CorsPolicy");
	app.UseMiddleware<ErrorWrappingMiddleware>();
	app.UseAuthentication();

	//	app.UseHttpsRedirection();

	app.UseAuthorization();
	app.UseEndpoints(endpoints =>
	{
		endpoints.MapGet(pattern: "/", async context =>
		{
			await context.Response.WriteAsync("Ocelot API Gateway is running...");
		});
	});

	app.MapControllers();

	await app.UseOcelot();

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
	Log.Information(messageTemplate: $"Shut down  {builder.Environment.EnvironmentName} complete");
	Log.CloseAndFlush();
}

