using Common.Logging;
using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Serilogger.Configure);
Log.Information(messageTemplate: "Starting Customer API...");

try
{
	// Add services to the container.

	builder.Services.AddControllers();
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	var conntectionString = builder.Configuration.GetConnectionString(name: "DefaultConnectionString");
	builder.Services.AddDbContext<CustomerContext>(optionsAction: options => options.UseNpgsql(conntectionString));

	builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
		.AddScoped(serviceType: typeof(IRepositoryBaseAsync<,,>), implementationType: typeof(RepositoryBaseAsync<,,>))
		.AddScoped(serviceType: typeof(IUnitOfWork<>), implementationType: typeof(UnitOfWork<>))
		.AddScoped<ICustomerServices, CustomerServices>();

	var app = builder.Build();

	app.MapGet(pattern: "/", handler: () => "Welcome to Customer API");
	app.MapGet(pattern: "/api/customers", 
		handler: async (ICustomerRepository customerServices) => await customerServices.GetCustomersAsync());

	app.MapGet(pattern: "/api/customers/{userName}",
	handler: async (string userName, ICustomerRepository customerServices) => await customerServices.GetCustomerByUserNameAsync(userName));
	//app.MapPost(pattern: "/", handler: () => "Welcome to Customer API");
	//app.MapPut(pattern: "/", handler: () => "Welcome to Customer API");
	//app.MapDelete(pattern: "/", handler: () => "Welcome to Customer API");

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();

	app.SeedCustomerData()
		.Run();

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

