﻿using AutoMapper;

namespace Ordering.Application.Common.Mappings
{
	public interface IMapFrom<T>
	{
		void Mapping(Profile profile) =>
			profile.CreateMap(sourceType: typeof(T), destinationType: GetType());
	}
}
