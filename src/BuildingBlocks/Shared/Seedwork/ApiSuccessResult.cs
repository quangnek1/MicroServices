namespace Shared.Seedwork
{
	public class ApiSuccessResult<T> : ApiResult<T>
	{
		public ApiSuccessResult(T data) : base(isSucceeded: true, data, message: "Success")
		{

		}
		public ApiSuccessResult(T data, string message) : base(isSucceeded: true, data, message)
		{
		}
	}
}
