using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using FantaAsta.EventArgs;
using FantaAsta.Models;
using System.Collections.Generic;

namespace FantaAsta.ViewModels
{
	public class RoseViewModel : BindableBase
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private readonly Lega m_lega;

		#endregion

		#region Public fields

		public List<FantaSquadraViewModel> Squadre { get; }

		#region Commands

		public DelegateCommand<FantaSquadraViewModel> ModificaCommand { get; private set; }

		#endregion

		#endregion

		public RoseViewModel(IDialogService dialogService, Lega lega)
		{
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

			ModificaCommand = new DelegateCommand<FantaSquadraViewModel>(Modifica);
		}

		#region Private methods

		#region Event handlers

		private void OnGiocatoreAggiunto(object sender, GiocatoreEventArgs e)
		{
			Squadre.Where(s => s.FantaSquadra.Equals(e.FantaSquadra)).Single().AggiungiGiocatore(e.Giocatore);
		}

		private void OnGiocatoreRimosso(object sender, GiocatoreEventArgs e)
		{
			Squadre.Where(s => s.FantaSquadra.Equals(e.FantaSquadra)).Single().RimuoviGiocatore(e.Giocatore);
		}

		private void OnModalitàAstaInvernale(object sender, System.EventArgs e)
		{
			for (int i = 0; i < Squadre.Count; i++)
			{
				Squadre[i].Budget = m_lega.FantaSquadre[i].Budget.ToString();
			}
		}

		#endregion

		#region Commands

		private void Modifica(FantaSquadraViewModel squadraVM)
		{
			FantaSquadra fantaSquadra = m_lega.FantaSquadre.Where(s => s.Equals(squadraVM.FantaSquadra)).Single();
			m_dialogService.Show("Modifica", new DialogParameters { { "squadra", fantaSquadra } }, null);
		}

		#endregion

		#endregion
	}

	public class FantaSquadraViewModel : BindableBase
	{
		#region Private fields

		private string m_budget;

		#endregion

		#region Public fields

		public FantaSquadra FantaSquadra { get; }

		public string Nome { get; }

		public ObservableCollection<Giocatore> Giocatori { get; }

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

		#endregion
	}

}
