using Systems.Menu.Runtime;

namespace Systems.Menu.Example.Runtime
{
	public readonly struct MockMenuData : IMenuData
	{
		// all fields go here
		public readonly string Name;

		public MockMenuData(string name) => Name = name;
	}
}
