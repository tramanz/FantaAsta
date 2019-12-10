using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class MainModule : IModule
	{
		#region Private fields

		private readonly IRegionManager m_regionManager;

		#endregion

		public MainModule(IRegionManager regionManager)
		{
			m_regionManager = regionManager;
		}

		#region Public methods

		#region IModule

		public void OnInitialized(IContainerProvider containerProvider)
		{
			m_regionManager.RegisterViewWithRegion("MainRegion", typeof(MainView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MainView>();
		}

		#endregion

		#endregion
	}
}
