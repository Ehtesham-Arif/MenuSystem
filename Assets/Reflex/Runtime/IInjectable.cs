using Reflex.Core;

namespace Reflex.Runtime
{
	public interface IInjectable
	{
		public void OnInject(Container container);
	}
}
