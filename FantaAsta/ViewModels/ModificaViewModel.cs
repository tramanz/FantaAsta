using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Services.Dialogs;
using FantaAsta.Models;
using FantaAsta.EventArgs;

namespace FantaAsta.ViewModels
{
	public class ModificaViewModel : DialogAwareViewModel
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private string m_title;

		private FantaSquadra m_squadra;

		private ObservableCollection<Giocatore> m_svincolati;
		private ObservableCollection<Giocatore> m_rosa;

		private Giocatore m_giocatoreSelezionato;
		private Giocatore m_svincolatoSelezionato;

		#endregion

		#region Properties

		public override string Title
		{
			get { return m_title; }
			set { SetProperty(ref m_title, value); }
		}

		public ObservableCollection<Giocatore> Svincolati
		{
			get { return m_svincolati; }
			set { SetProperty(ref m_svincolati, value); }
		}
		public ObservableCollection<Giocatore> Rosa
		{
			get { return m_rosa; }
			set { SetProperty(ref m_rosa, value); }
		}

		public Giocatore GiocatoreSelezionato
		{
			get { return m_giocatoreSelezionato; }
			set { SetProperty(ref m_giocatoreSelezionato, value); RimuoviCommand?.RaiseCanExecuteChanged(); }
		}
		public Giocatore SvincolatoSelezionato
		{
			get { return m_svincolatoSelezionato; }
			set { SetProperty(ref m_svincolatoSelezionato, value); AggiungiCommand?.RaiseCanExecuteChanged(); }
		}

		public DelegateCommand AggiungiCommand { get; }
		public DelegateCommand RimuoviCommand { get; }
		public DelegateCommand ChiudiCommand { get; }

		#endregion

		public ModificaViewModel(IDialogService dialogService, Lega lega) : base(lega)
		{
			m_dialogService = dialogService;

			AggiungiCommand = new DelegateCommand(Aggiungi, AbilitaAggiungi);
			RimuoviCommand = new DelegateCommand(Rimuovi, AbilitaRimuovi);
			ChiudiCommand = new DelegateCommand(Chiudi);
		}

		#region Public methods

		public override void OnDialogClosed()
		{
			m_lega.GiocatoreAggiunto -= OnGiocatoreAggiunto;
			m_lega.GiocatoreRimosso -= OnGiocatoreRimosso;
		}

		public override void OnDialogOpened(IDialogParameters parameters)
		{
			m_squadra = parameters.GetValue<FantaSquadra>("squadra");

			Title = $"Modifica la rosa di {m_squadra.Nome}";

			Rosa = new ObservableCollection<Giocatore>(m_squadra.Giocatori.OrderBy(g => g.Ruolo).ThenBy(g => g.Nome));
			Svincolati = new ObservableCollection<Giocatore>(m_lega.Lista.OrderBy(g => g.Nome));

			m_lega.GiocatoreAggiunto += OnGiocatoreAggiunto;
			m_lega.GiocatoreRimosso += OnGiocatoreRimosso;
		}

		#endregion

		#region Private methods

		#region Event handlers

		private void OnGiocatoreAggiunto(object sender, GiocatoreAggiuntoEventArgs e)
		{
			if (e.FantaSquadra.Equals(m_squadra) && !Rosa.Contains(e.Giocatore) && Svincolati.Contains(e.Giocatore))
			{
				Rosa.Add(e.Giocatore);
				Rosa = new ObservableCollection<Giocatore>(Rosa.OrderBy(g => g.Ruolo).ThenBy(g => g.Nome));
				Svincolati.Remove(e.Giocatore);
			}
		}

		private void OnGiocatoreRimosso(object sender, GiocatoreRimossoEventArgs e)
		{
			if (e.FantaSquadra.Equals(m_squadra) && Rosa.Contains(e.Giocatore) && !Svincolati.Contains(e.Giocatore))
			{
				Rosa.Remove(e.Giocatore);
				Svincolati.Add(e.Giocatore);
				Svincolati = new ObservableCollection<Giocatore>(Svincolati.OrderBy(g => g.Nome));
			}
		}

		#endregion

		#region Commands

		private void Aggiungi()
		{
			if (m_lega.FantaSquadre.Select(s => s.Giocatori).Where(g => g.Contains(SvincolatoSelezionato)).Count() > 0)
			{
				MessageBox.Show("Il giocatore selezionato è già assegnato ad una fantasquadra.", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				m_dialogService.ShowDialog("Prezzo", new DialogParameters
				{
					{ "Movimento", "acquisto" },
					{ "FantaSquadra", m_squadra},
					{ "Giocatore", SvincolatoSelezionato }
				}, null);
			}
		}
		private bool AbilitaAggiungi()
		{
			return SvincolatoSelezionato != null;
		}

		private void Rimuovi()
		{
			if (!m_squadra.Giocatori.Contains(GiocatoreSelezionato))
			{
				MessageBox.Show("Il giocatore selezionato non è presente nella rosa della fantasquadra.", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				m_dialogService.ShowDialog("Prezzo", new DialogParameters
				{
					{ "Movimento", "vendita" },
					{ "FantaSquadra", m_squadra},
					{ "Giocatore", GiocatoreSelezionato }
				}, null);
			}
		}
		private bool AbilitaRimuovi()
		{
			return GiocatoreSelezionato != null;
		}

		private void Chiudi()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.OK));
		}

		#endregion

		#endregion
	}
}