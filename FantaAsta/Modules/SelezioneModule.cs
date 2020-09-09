using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class SelezioneModule : IModule
	{
		#region Public methods

		public void OnInitialized(IContainerProvider containerProvider)
		{
			IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

			_ = regionManager.RegisterViewWithRegion("MainRegion", typeof(SelezioneView));

			regionManager.RequestNavigate("MainRegion", nameof(SelezioneView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<SelezioneView>();
		}

		#endregion
	}
}
