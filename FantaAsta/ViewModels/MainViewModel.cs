using Prism.Regions;
using FantaAsta.Views;

namespace FantaAsta.ViewModels
{
	public class MainViewModel
	{
		private readonly IRegionManager m_regionManager;

		public MainViewModel(IRegionManager regionManager)
		{
			m_regionManager = regionManager;
		}

		private void NavigateToSelezione()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(SelezioneView));
		}
	}
}
