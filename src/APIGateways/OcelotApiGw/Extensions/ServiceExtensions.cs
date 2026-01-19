using Contracts.Identity;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Shared.Configurations;

namespace OcelotApiGw.Extensions
{
	public static class ServiceExtensions
	{
		internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtSettings = configuration.GetSection(key: nameof(JwtSettings)).Get<JwtSettings>();
			services.AddSingleton(jwtSettings);

			return services;
		}
		public static void ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddOcelot(configuration);
			services.AddTransient<ITokenService, TokenService>();
			services.AddJwtAuthentication();
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


		public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
		{
			var origins = configuration["AllowOrigins"];
			services.AddCors(option =>
			{
				option.AddPolicy("CorsPolicy", buider =>
				{
					buider.WithOrigins(origins: origins)
					.AllowAnyHeader()
					.AllowAnyMethod();
				});
			});
		}
	}
}
