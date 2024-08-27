namespace Systems.Menu.Runtime
{
	public interface IMenuData
	{
		// tag for menu data

		public static IMenuData Default => new DefaultMenuData();
	}

	public struct DefaultMenuData : IMenuData
	{
	}
}
