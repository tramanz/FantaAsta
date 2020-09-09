using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class AstaModule : IModule
	{
		#region Public methods

		public void OnInitialized(IContainerProvider containerProvider)
		{
			_ = containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion("ContentRegion", typeof(AstaView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{ }

		#endregion
	}
}
