using Contracts.Common.Interfaces;
using Contracts.Identity;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;
using Product.API.Reponsitories;
using Product.API.Reponsitories.Interfaces;
using Shared.Configurations;

namespace Product.API.Extensions
{
	public static class ServiceExtensions
	{
		internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtSettings = configuration.GetSection(key: nameof(JwtSettings)).Get<JwtSettings>();
			services.AddSingleton(jwtSettings);
			return services;
		}
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();
			services.Configure<RouteOptions>(options =>
			{
				options.LowercaseUrls = true;
			});
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.ConfigureProductDbContext(configuration);
			services.AddInfrastructureServices();
			services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
			services.AddJwtAuthentication();

			return services;

		}

		internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
		{
			var jwtSettings = services.GetOptions<JwtSettings>(nameof(JwtSettings));
			if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
			{
				throw new ArgumentNullException(nameof(jwtSettings), "JWT Key is not configured properly.");
			}

			var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key));

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = signingKey,
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero,
				RequireExpirationTime = false
			};
			services.AddAuthentication(o =>
			{
				o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = tokenValidationParameters;
			});

			return services;
		}

		private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			var conntectionString = configuration.GetConnectionString("DefaultConnectionString");
			var builder = new MySqlConnectionStringBuilder(conntectionString);

			services.AddDbContext<ProductContext>(m => m.UseMySql(builder.ConnectionString,
			   ServerVersion.AutoDetect(builder.ConnectionString), e =>
			   {
				   e.MigrationsAssembly("Product.API");
				   e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
			   }));

			return services;
		}
		private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
		{
			return services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
				.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
				.AddScoped<IProductRepository, ProductRepository>()
			;
		}
	}
}
