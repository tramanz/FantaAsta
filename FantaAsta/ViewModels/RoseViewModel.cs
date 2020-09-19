using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using FantaAsta.Constants;
using FantaAsta.Enums;
using FantaAsta.Events;
using FantaAsta.Models;
using FantaAsta.Views;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class RoseViewModel : BaseNavigationViewModel
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private ObservableCollection<FantaSquadraViewModel> m_squadre;

		private bool m_isStandalone;

		private double m_media;

		#endregion

		#region Properties

		public ObservableCollection<FantaSquadraViewModel> Squadre
		{
			get { return m_squadre; }
			private set { _ = SetProperty(ref m_squadre, value); }
		}

		public bool IsStandalone
		{
			get { return m_isStandalone; }
			set { _ = SetProperty(ref m_isStandalone, value); }
		}

		public double Media
		{
			get { return m_media; }
			set { _ = SetProperty(ref m_media, value); }
		}


		#region Commands

		public DelegateCommand IndietroCommand { get; }
		public DelegateCommand SalvaCommand { get; }

		public DelegateCommand<FantaSquadraViewModel> ModificaCommand { get; }
		public DelegateCommand<FantaSquadraViewModel> EliminaCommand { get; }

		#endregion

		#endregion

		public RoseViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService, Asta asta) : base(regionManager, eventAggregator, asta)
		{
			m_dialogService = dialogService;

			_ = m_eventAggregator.GetEvent<GiocatoreAggiuntoEvent>().Subscribe(OnGiocatoreAggiunto);
			_ = m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Subscribe(OnGiocatoreRimosso);
			_ = m_eventAggregator.GetEvent<FantaSquadraAggiuntaEvent>().Subscribe(OnFantaSquadraAggiunta);
			_ = m_eventAggregator.GetEvent<FantaSquadraRimossaEvent>().Subscribe(OnFantaSquadraRimossa);
			_ = m_eventAggregator.GetEvent<ModalitàAstaCambiataEvent>().Subscribe(OnModalitàAstaCambiata);
			_ = m_eventAggregator.GetEvent<RoseResettateEvent>().Subscribe(OnRoseResettate);
			_ = m_eventAggregator.GetEvent<ListaImportataEvent>().Subscribe(OnListaImportata);
			_ = m_eventAggregator.GetEvent<FantaSquadreSalvateEvent>().Subscribe(OnFantaSquadreSalvate);

			Media = m_asta.QuotazioneMedia;
			Squadre = new ObservableCollection<FantaSquadraViewModel>(m_asta.DatiAsta.FantaSquadre.Select(s => new FantaSquadraViewModel(s)).OrderBy(s => s.FantaSquadra.Nome));

			SalvaCommand = new DelegateCommand(Salva, AbilitaSalva);
			IndietroCommand = new DelegateCommand(NavigateToSelezione);
			ModificaCommand = new DelegateCommand<FantaSquadraViewModel>(Modifica, AbilitaModifica);
			EliminaCommand = new DelegateCommand<FantaSquadraViewModel>(Elimina);
		}

		#region Public methods

		public override void OnNavigatedTo(NavigationContext navigationContext)
		{
			IsStandalone = true;
		}

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			IsStandalone = false;
		}

		#endregion

		#region Private methods

		#region Event handlers

		private void OnGiocatoreAggiunto(GiocatoreAggiuntoEventArgs args)
		{
			Squadre.Single(s => s.FantaSquadra.Equals(args.FantaSquadra)).AggiungiGiocatore(args.Giocatore);

			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnGiocatoreRimosso(GiocatoreRimossoEventArgs args)
		{
			Squadre.Single(s => s.FantaSquadra.Equals(args.FantaSquadra)).RimuoviGiocatore(args.Giocatore);

			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnFantaSquadraAggiunta(FantaSquadraEventArgs args)
		{
			Squadre.Add(new FantaSquadraViewModel(args.FantaSquadra));
			Squadre = new ObservableCollection<FantaSquadraViewModel>(Squadre.OrderBy(s => s.Nome));

			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnFantaSquadraRimossa(FantaSquadraEventArgs args)
		{
			FantaSquadraViewModel squadraVM = Squadre.SingleOrDefault(s => s.Nome.Equals(args.FantaSquadra.Nome, StringComparison.Ordinal));
			if (squadraVM != null)
			{
				_ = Squadre.Remove(squadraVM);
				
				SalvaCommand?.RaiseCanExecuteChanged();
			}
		}

		private void OnModalitàAstaCambiata()
		{
			foreach (FantaSquadraViewModel squadraVm in Squadre)
			{
				squadraVm.AggiornaBudget();
			}
		}

		private void OnRoseResettate()
		{
			foreach (FantaSquadraViewModel squadraVm in Squadre)
			{
				squadraVm.AggiornaBudget();
				squadraVm.AggiornaRosa();
				squadraVm.AggiornaValore();
			}

			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnListaImportata()
		{
			Media = m_asta.QuotazioneMedia;

			ModificaCommand?.RaiseCanExecuteChanged();
			SalvaCommand?.RaiseCanExecuteChanged();
		}

		private void OnFantaSquadreSalvate()
		{
			SalvaCommand?.RaiseCanExecuteChanged();
		}

		#endregion

		#region Commands

		private void Elimina(FantaSquadraViewModel squadraVM)
		{
			ButtonResult res = m_dialogService.ShowMessage("Sei sicuro di voler eliminare la fanta squadra?", MessageType.Warning);

			if (res == ButtonResult.Yes)
			{
				m_asta.RimuoviSquadra(squadraVM.Nome);

				_ = m_dialogService.ShowMessage("Squadra eliminata", MessageType.Notification);
			}
		}

		private void Modifica(FantaSquadraViewModel squadraVM)
		{
			m_dialogService.ShowDialog(CommonConstants.MODIFICA_DIALOG, new DialogParameters { { typeof(FantaSquadra).ToString(), m_asta.DatiAsta.FantaSquadre.Single(s => s.Equals(squadraVM.FantaSquadra)) } }, null);
		}
		private bool AbilitaModifica(FantaSquadraViewModel squadraVM)
		{
			return m_asta.ListaPresente;
		}

		private void NavigateToSelezione()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(SelezioneView));
		}

		private void Salva()
		{
			Mouse.OverrideCursor = Cursors.Wait;

			m_asta.SalvaDati();

			Mouse.OverrideCursor = Cursors.Arrow;
		}
		private bool AbilitaSalva()
		{
			return m_asta.AbilitaSalvataggio();
		}

		#endregion

		#endregion
	}

	public class FantaSquadraViewModel : BindableBase
	{
		#region Private fields

		private ObservableCollection<Giocatore> m_giocatori;

		private string m_budget;

		private double m_valore;

		#endregion

		#region Public fields

		public FantaSquadra FantaSquadra { get; }

		public string Nome { get; }

		public ObservableCollection<Giocatore> Giocatori
		{
			get { return m_giocatori; }
			set { _ = SetProperty(ref m_giocatori, value); }
		}

		public string Budget
		{
			get { return m_budget; }
			set { _ = SetProperty(ref m_budget, value); }
		}

		public double Valore
		{
			get { return m_valore; }
			set { _ = SetProperty(ref m_valore, value); }
		}

		#endregion

		public FantaSquadraViewModel(FantaSquadra fantaSquadra)
		{
			FantaSquadra = fantaSquadra;
			Nome = FantaSquadra.Nome;
			AggiornaBudget();
			AggiornaRosa();
			AggiornaValore();
		}

		#region Public methods

		public void AggiungiGiocatore(Giocatore giocatore)
		{
			if (!Giocatori.Contains(giocatore))
			{
				Giocatori.Add(giocatore);
				AggiornaRosa();
				AggiornaBudget();
				AggiornaValore();
			}
		}

		public void RimuoviGiocatore(Giocatore giocatore)
		{
			if (Giocatori.Contains(giocatore))
			{
				_ = Giocatori.Remove(giocatore);
				AggiornaBudget();
				AggiornaValore();
			}
		}

		public void AggiornaBudget()
		{
			Budget = FantaSquadra.Budget.ToString();
		}

		public void AggiornaRosa()
		{
			Giocatori = new ObservableCollection<Giocatore>(FantaSquadra.Rosa.OrderBy(g => g.Ruolo).ThenByDescending(g => g.Prezzo));
		}

		public void AggiornaValore()
		{
			Valore = FantaSquadra.ValoreMedio;
		}

		#endregion
	}
}