using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using FantaAsta.EventArgs;
using FantaAsta.Models;
using FantaAsta.Views;

namespace FantaAsta.ViewModels
{
	public class RoseViewModel : BindableBase, INavigationAware, IActiveAware
	{
		#region Private fields

		private readonly IRegionManager m_regionManager;

		private readonly IDialogService m_dialogService;

		private readonly Lega m_lega;

		private bool m_isActive;

		// Variabili per gestire la visualizzazione del tasto "Indietro"
		private bool m_isStandalone;

		#endregion

		#region Properties

		public List<FantaSquadraViewModel> Squadre { get; }

		public bool IsStandalone
		{
			get { return m_isStandalone; }
			set { SetProperty(ref m_isStandalone, value); }
		}

		#region IActiveAware

		public bool IsActive
		{
			get { return m_isActive; }
			set { m_isActive = value; IsStandalone = false; }
		}

		#endregion

		#region Commands

		public DelegateCommand<FantaSquadraViewModel> ModificaCommand { get; private set; }

		public DelegateCommand IndietroCommand { get; }

		#endregion

		#endregion

		#region Events

		#region IActiveAware

		public event EventHandler IsActiveChanged { add { } remove { } }

		#endregion

		#endregion

		public RoseViewModel(IRegionManager regionManager, IDialogService dialogService, Lega lega)
		{
			m_regionManager = regionManager;
			m_dialogService = dialogService;

			m_lega = lega;

			Squadre = new List<FantaSquadraViewModel>();
			foreach (FantaSquadra squadra in m_lega.FantaSquadre)
			{
				Squadre.Add(new FantaSquadraViewModel(squadra));
			}

			m_lega.GiocatoreAggiunto += OnGiocatoreAggiunto;
			m_lega.GiocatoreRimosso += OnGiocatoreRimosso;
			m_lega.ModalitàAstaInvernale += OnModalitàAstaInvernale;
			m_lega.RoseResettate += OnRoseResettate;
			m_lega.ListaImportata += OnListaImportata;

			ModificaCommand = new DelegateCommand<FantaSquadraViewModel>(Modifica, AbilitaModifica);
			IndietroCommand = new DelegateCommand(NavigateToSelezione);
		}

		#region Public methods

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			IsStandalone = ((string)navigationContext.Parameters["Modalità"]).Equals("Gestione rose", StringComparison.OrdinalIgnoreCase);
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{ }

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		#endregion

		#region Private methods

		#region Event handlers

		private void OnGiocatoreAggiunto(object sender, GiocatoreAggiuntoEventArgs e)
		{
			Squadre.Where(s => s.FantaSquadra.Equals(e.FantaSquadra)).Single().AggiungiGiocatore(e.Giocatore);
		}

		private void OnGiocatoreRimosso(object sender, GiocatoreRimossoEventArgs e)
		{
			Squadre.Where(s => s.FantaSquadra.Equals(e.FantaSquadra)).Single().RimuoviGiocatore(e.Giocatore);
		}

		private void OnModalitàAstaInvernale(object sender, System.EventArgs e)
		{
			foreach (FantaSquadraViewModel squadraVm in Squadre)
			{
				squadraVm.AggiornaBudget();
			}
		}

		private void OnRoseResettate(object sender, System.EventArgs e)
		{
			foreach (FantaSquadraViewModel squadraVm in Squadre)
			{
				squadraVm.AggiornaBudget();
				squadraVm.AggiornaRosa();
			}
		}

		private void OnListaImportata(object sender, System.EventArgs e)
		{
			ModificaCommand?.RaiseCanExecuteChanged();
		}

		#endregion

		#region Commands

		private void Modifica(FantaSquadraViewModel squadraVM)
		{
			FantaSquadra fantaSquadra = m_lega.FantaSquadre.Where(s => s.Equals(squadraVM.FantaSquadra)).Single();
			m_dialogService.Show("Modifica", new DialogParameters { { "squadra", fantaSquadra } }, null);
		}

		private bool AbilitaModifica(FantaSquadraViewModel squadraVM)
		{
			return m_lega.ListaPresente;
		}

		private void NavigateToSelezione()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(SelezioneView));
		}

		#endregion

		#endregion
	}

	public class FantaSquadraViewModel : BindableBase
	{
		#region Private fields
		
		private ObservableCollection<Giocatore> m_giocatori;

		private string m_budget;

		#endregion

		#region Public fields

		public FantaSquadra FantaSquadra { get; }

		public string Nome { get; }

		public ObservableCollection<Giocatore> Giocatori
		{
			get { return m_giocatori; }
			set { SetProperty(ref m_giocatori, value); }
		}

		public string Budget
		{
			get { return m_budget; }
			set { SetProperty(ref m_budget, value); }
		}

		#endregion

		public FantaSquadraViewModel(FantaSquadra fantaSquadra)
		{
			FantaSquadra = fantaSquadra;
			Nome = FantaSquadra.Nome;
			Giocatori = new ObservableCollection<Giocatore>(FantaSquadra.Giocatori);
			Budget = FantaSquadra.Budget.ToString();
		}

		#region Public methods

		public void AggiungiGiocatore(Giocatore giocatore)
		{
			if (!Giocatori.Contains(giocatore))
			{
				Giocatori.Add(giocatore);
				Budget = FantaSquadra.Budget.ToString();
			}
		}

		public void RimuoviGiocatore(Giocatore giocatore)
		{
			if (Giocatori.Contains(giocatore))
			{
				Giocatori.Remove(giocatore);
				Budget = FantaSquadra.Budget.ToString();
			}
		}

		public void AggiornaBudget()
		{
			Budget = FantaSquadra.Budget.ToString();
		}

		public void AggiornaRosa()
		{
			Giocatori = new ObservableCollection<Giocatore>(FantaSquadra.Giocatori);
		}

		#endregion
	}
}