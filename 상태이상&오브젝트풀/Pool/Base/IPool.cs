/// <summary>
/// Represents a collection that pools objects of T.
/// </summary>
/// <typeparam name="T">Specifies the type of elements in the pool.</typeparam>

namespace EZPool
{
	public interface IPool<T>
	{
		void Prewarm(int num);
		T Pop();
		void Push(T member);
	}
}