using FantaAsta.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace FantaAsta.Modules
{
	public class AstaModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{
			containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion("ContentRegion", typeof(AstaView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			
		}
	}
}
