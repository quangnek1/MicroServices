﻿namespace Shared.Seedwork
{
	public class ApiErrorResult<T> : ApiResult<T>
	{
		public ApiErrorResult() : this("Something wrong happened. Please try again later.")
		{

		}
		public ApiErrorResult(string message) : base(false,message)
		{

		}

		public ApiErrorResult(List<string> errors) : base(isSucceeded: false)
		{
			Errors = errors;
		}

		public List<string> Errors { get; set; }
	}
}
