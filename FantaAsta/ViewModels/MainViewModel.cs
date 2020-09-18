using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using FantaAsta.Events;
using FantaAsta.Models;
using FantaAsta.Views;

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
			_ = m_eventAggregator.GetEvent<GiocatoreAggiuntoEvent>().Subscribe(OnGiocatoreAggiunto);
			_ = m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Subscribe(OnGiocatoreRimosso);
			_ = m_eventAggregator.GetEvent<FantaSquadraAggiuntaEvent>().Subscribe(OnFantaSquadraAggiunta);
			_ = m_eventAggregator.GetEvent<FantaSquadraRimossaEvent>().Subscribe(OnFantaSquadraRimossa);
			_ = m_eventAggregator.GetEvent<RoseResettateEvent>().Subscribe(OnRoseResettate);
			_ = m_eventAggregator.GetEvent<ListaImportataEvent>().Subscribe(OnListaImportata);
			_ = m_eventAggregator.GetEvent<FantaSquadreSalvateEvent>().Subscribe(OnFantaSquadreSalvate);

			IndietroCommand = new DelegateCommand(NavigateToSelezione);
			SalvaCommand = new DelegateCommand(Salva, AbilitaSalva);
		}

		#region Private methods

		#region Event handlers

		private void OnGiocatoreAggiunto(GiocatoreAggiuntoEventArgs args)
		{
			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnGiocatoreRimosso(GiocatoreRimossoEventArgs args)
		{
			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnFantaSquadraAggiunta(FantaSquadraEventArgs args)
		{
			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnFantaSquadraRimossa(FantaSquadraEventArgs args)
		{
			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnRoseResettate()
		{
			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnListaImportata()
		{
			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnFantaSquadreSalvate()
		{
			SalvaCommand?.RaiseCanExecuteChanged();
		}

		#endregion

		#region Commands

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
		private bool AbilitaSalva()
		{
			return m_lega.AbilitaSalvataggio();
		}

		#endregion

		#endregion
	}
}