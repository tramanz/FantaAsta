using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Constants;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class ListaModule : IModule
	{
		#region Public methods

		public void OnInitialized(IContainerProvider containerProvider)
		{
			_ = containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(CommonConstants.CONTENT_REGION, typeof(ListaView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{ }

		#endregion
	}
}
