namespace Ordering.Domain.Exceptions
{
	public class EntityNotFoundException : ApplicationException
	{
		public EntityNotFoundException(string entity, object key): base($"Entity '{entity}' with key '{key}' was not found.")
		{
		}
	}
}
