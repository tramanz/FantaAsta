using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class RoseModule : IModule
	{
		#region Public methods

		public void OnInitialized(IContainerProvider containerProvider)
		{
			IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

			regionManager.RegisterViewWithRegion("MainRegion", typeof(RoseView));
			regionManager.RegisterViewWithRegion("ContentRegion", typeof(RoseView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<RoseView>();
		}

		#endregion
	}
}
