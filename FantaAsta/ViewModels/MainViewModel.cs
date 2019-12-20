using System;
using Prism.Commands;
using Prism.Regions;
using FantaAsta.Views;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class MainViewModel : NavigationAwareViewModel
	{
		#region Properties

		public DelegateCommand IndietroCommand { get; }

		#endregion

		public MainViewModel(IRegionManager regionManager, Lega lega) : base(regionManager, lega)
		{
			IndietroCommand = new DelegateCommand(NavigateToSelezione);
		}

		#region Public methods

		public override void OnNavigatedTo(NavigationContext navigationContext)
		{
			string parameter = navigationContext.Parameters["Modalità"] as string;

			if (parameter.Equals("Asta invernale", StringComparison.OrdinalIgnoreCase))
			{
				m_lega.AvviaAstaInvernale();
			}
		}

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			if (m_lega.IsAstaInvernale)
			{
				m_lega.TerminaAstaInvernale();
			}
		}

		#endregion

		#region Private methods

		private void NavigateToSelezione()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(SelezioneView));
		}

		#endregion
	}
}