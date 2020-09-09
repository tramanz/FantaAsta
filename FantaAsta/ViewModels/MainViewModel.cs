using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using FantaAsta.Views;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class MainViewModel : BaseNavigationViewModel
	{
		#region Properties

		public DelegateCommand IndietroCommand { get; }
		public DelegateCommand SalvaCommand { get; }

		#endregion

		public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, Lega lega) : base(regionManager, eventAggregator, lega)
		{
			IndietroCommand = new DelegateCommand(NavigateToSelezione);
			SalvaCommand = new DelegateCommand(Salva);
		}

		#region Private methods

		private void NavigateToSelezione()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(SelezioneView));
		}

		private void Salva()
		{
			Mouse.OverrideCursor = Cursors.Wait;

			m_lega.SalvaSquadre();

			Mouse.OverrideCursor = Cursors.Arrow;
		}

		#endregion
	}
}