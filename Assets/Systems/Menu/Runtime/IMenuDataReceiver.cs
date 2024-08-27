namespace Systems.Menu.Runtime
{
	public interface IMenuDataReceiver<in TData> where TData : IMenuData
	{
		public void SetData(TData data);
	}
}
