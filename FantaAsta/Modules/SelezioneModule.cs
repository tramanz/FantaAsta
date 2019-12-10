using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class SelezioneModule : IModule
	{
		#region Private fields

		private readonly IRegionManager m_regionManager;

		#endregion

		public SelezioneModule(IRegionManager regionManager)
		{
			m_regionManager = regionManager;
		}

		#region Public methods

		#region IModule

		public void OnInitialized(IContainerProvider containerProvider)
		{
			m_regionManager.RegisterViewWithRegion("MainRegion", typeof(SelezioneView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<SelezioneView>();
		}

		#endregion

		#endregion
	}
}
