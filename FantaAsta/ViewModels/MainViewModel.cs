using System;
using Prism.Commands;
using Prism.Regions;
using FantaAsta.Views;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class MainViewModel : INavigationAware
	{
		#region Private fields

		private readonly IRegionManager m_regionManager;

		private readonly Lega m_lega;

		#endregion

		#region Properties

		#region Commands

		public DelegateCommand IndietroCommand { get; }

		#endregion

		#endregion

		public MainViewModel(IRegionManager regionManager, Lega lega)
		{
			m_regionManager = regionManager;

			m_lega = lega;

			IndietroCommand = new DelegateCommand(NavigateToSelezione);
		}

		#region Public methods

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			string parameter = navigationContext.Parameters["Modalità"] as string;

			if (parameter.Equals("Asta invernale", StringComparison.OrdinalIgnoreCase))
			{
				m_lega.AvviaAstaInvernale();
			}
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			if (m_lega.IsAstaInvernale)
			{
				m_lega.TerminaAstaInvernale();
			}
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
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
