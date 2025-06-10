using System.Reflection;
using AutoMapper;

namespace Infrastructure.Mappings
{
	public static class AutoMapperExtension
	{
		public static IMappingExpression<TSource, TDestination> IgnoreAllNonExising<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression){
			var flags = BindingFlags.Public | BindingFlags.Instance;
			var sourcetype = typeof(TSource);
			var destinationProperties = typeof(TDestination).GetProperties(flags);

			foreach (var property in destinationProperties)
			{
				if (sourcetype.GetProperty(property.Name, flags) == null)
				{
					expression.ForMember(property.Name, opt => opt.Ignore());
				}
			}

			return expression;
		}
	}
}
