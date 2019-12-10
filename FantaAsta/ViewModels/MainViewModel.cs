using Prism.Commands;
using Prism.Regions;
using FantaAsta.Views;

namespace FantaAsta.ViewModels
{
	public class MainViewModel
	{
		private readonly IRegionManager m_regionManager;

		public DelegateCommand IndietroCommand { get; }

		public MainViewModel(IRegionManager regionManager)
		{
			m_regionManager = regionManager;

			IndietroCommand = new DelegateCommand(NavigateToSelezione);
		}

		private void NavigateToSelezione()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(SelezioneView));
		}
	}
}
