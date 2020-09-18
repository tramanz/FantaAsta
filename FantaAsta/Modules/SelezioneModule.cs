using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Constants;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class SelezioneModule : IModule
	{
		#region Public methods

		public void OnInitialized(IContainerProvider containerProvider)
		{
			IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

			_ = regionManager.RegisterViewWithRegion(CommonConstants.MAIN_REGION, typeof(SelezioneView));

			regionManager.RequestNavigate(CommonConstants.MAIN_REGION, nameof(SelezioneView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<SelezioneView>();
		}

		#endregion
	}
}
