using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class ListaModule : IModule
	{
		#region Public methods

		public void OnInitialized(IContainerProvider containerProvider)
		{
			containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion("ContentRegion", typeof(ListaView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{ }

		#endregion
	}
}
