namespace Product.API.Extensions
{
	public static class ApplicationExtensions
	{
		public static void UseInfrastructure(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseRouting();
			//	app.UseHttpsRedirection(); For production, consider enabling HTTPS redirection

			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
