using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Constants;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class RoseModule : IModule
	{
		#region Public methods

		public void OnInitialized(IContainerProvider containerProvider)
		{
			IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

			_ = regionManager.RegisterViewWithRegion(CommonConstants.MAIN_REGION, typeof(RoseView));
			_ = regionManager.RegisterViewWithRegion(CommonConstants.CONTENT_REGION, typeof(RoseView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<RoseView>();
		}

		#endregion
	}
}
