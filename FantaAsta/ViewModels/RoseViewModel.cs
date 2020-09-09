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

		public RoseViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService, Lega lega) : base(regionManager, eventAggregator, lega)
		{
			m_dialogService = dialogService;

			_ = m_eventAggregator.GetEvent<GiocatoreAggiuntoEvent>().Subscribe(OnGiocatoreAggiunto);
			_ = m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Subscribe(OnGiocatoreRimosso);
			_ = m_eventAggregator.GetEvent<FantaSquadraAggiuntaEvent>().Subscribe(OnFantaSquadraAggiunta);
			_ = m_eventAggregator.GetEvent<FantaSquadraRimossaEvent>().Subscribe(OnFantaSquadraRimossa);
			_ = m_eventAggregator.GetEvent<ModalitàAstaCambiataEvent>().Subscribe(OnModalitàAstaCambiata);
			_ = m_eventAggregator.GetEvent<RoseResettateEvent>().Subscribe(OnRoseResettate);
			_ = m_eventAggregator.GetEvent<ListaImportataEvent>().Subscribe(OnListaImportata);

			Media = m_lega.QuotazioneMedia;
			Squadre = m_lega.FantaSquadre.Count > 0 ?
				new ObservableCollection<FantaSquadraViewModel>(m_lega.FantaSquadre.Select(s => new FantaSquadraViewModel(s)).OrderBy(s => s.FantaSquadra.Nome)) :
				new ObservableCollection<FantaSquadraViewModel>();

			SalvaCommand = new DelegateCommand(Salva);
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
			Squadre.Where(s => s.FantaSquadra.Equals(args.FantaSquadra)).Single().AggiungiGiocatore(args.Giocatore);
		}

		private void OnGiocatoreRimosso(GiocatoreRimossoEventArgs args)
		{
			Squadre.Where(s => s.FantaSquadra.Equals(args.FantaSquadra)).Single().RimuoviGiocatore(args.Giocatore);
		}

		private void OnFantaSquadraAggiunta(FantaSquadraEventArgs args)
		{
			Squadre.Add(new FantaSquadraViewModel(args.FantaSquadra));
			Squadre = new ObservableCollection<FantaSquadraViewModel>(Squadre.OrderBy(s => s.Nome));
		}

		private void OnFantaSquadraRimossa(FantaSquadraEventArgs args)
		{
			FantaSquadraViewModel squadraVM = Squadre.Where(s => s.Nome.Equals(args.FantaSquadra.Nome, StringComparison.Ordinal)).SingleOrDefault();

			if (squadraVM != null)
			{
				_ = Squadre.Remove(squadraVM);
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
		}

		private void OnListaImportata()
		{
			ModificaCommand?.RaiseCanExecuteChanged();

			Media = m_lega.QuotazioneMedia;
		}

		#endregion

		#region Commands

		private void Elimina(FantaSquadraViewModel squadraVM)
		{
			ButtonResult res = m_dialogService.ShowMessage("Sei sicuro di voler eliminare la fanta squadra?", MessageType.Warning);

			if (res == ButtonResult.Yes)
			{
				m_lega.RimuoviSquadra(squadraVM.Nome);

				_ = m_dialogService.ShowMessage("Squadra eliminata", MessageType.Notification);
			}
		}

		private void Modifica(FantaSquadraViewModel squadraVM)
		{
			FantaSquadra fantaSquadra = m_lega.FantaSquadre.Where(s => s.Equals(squadraVM.FantaSquadra)).Single();
					
			m_dialogService.ShowDialog(CommonConstants.MODIFICA_DIALOG, new DialogParameters 
			{
				{ "Type", DialogType.Popup },
				{ "squadra", fantaSquadra } 
			}, null);
		}
		private bool AbilitaModifica(FantaSquadraViewModel squadraVM)
		{
			return m_lega.ListaPresente;
		}

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
			Giocatori = new ObservableCollection<Giocatore>(FantaSquadra.Giocatori.OrderBy(g => g.Ruolo).ThenByDescending(g => g.Prezzo));
		}

		public void AggiornaValore()
		{
			Valore = FantaSquadra.ValoreMedio;
		}

		#endregion
	}
}