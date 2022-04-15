/// <summary>
/// Represents a factory.
/// </summary>
/// <typeparam name="T">Specifies the type to create.</typeparam>
/// 
namespace EZPool
{
	public interface IFactory<T>
	{
		T Create();
	}
}