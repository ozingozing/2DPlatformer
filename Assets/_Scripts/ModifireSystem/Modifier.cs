
namespace Ozing.ModifireSystem
{
	public class Modifier
	{
	}

	public abstract class Modifier<T> : Modifier
	{
		public abstract T ModifyValue(T value);
	}
}