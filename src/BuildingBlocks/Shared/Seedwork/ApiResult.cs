﻿namespace Shared.Seedwork
{
	public class ApiResult<T>
	{
		public ApiResult() { }
		public ApiResult(bool isSucceeded, string message = null)
		{
			Message = message;
			isSucceeded = isSucceeded;
		}
		public ApiResult(bool isSucceeded, T data, string message = null)
		{
			Data = data;
			IsSucceeded = isSucceeded;
			Message = message;
		}

		public bool IsSucceeded { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }
	}
}
