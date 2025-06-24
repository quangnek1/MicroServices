namespace Contracts.Common.Interfaces
{
	public interface ISerializeServices
	{
		string Serialize<T>(T obj);	
		string Serialize<T>(T obj, Type type);
		T Deserialize<T>(string text);
	}
}
