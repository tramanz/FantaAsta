using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using FantaAsta.Constants;
using FantaAsta.Enums;
using FantaAsta.Events;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class ModificaViewModel : BaseDialogViewModel
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private FantaSquadra m_squadra;

		private ObservableCollection<Giocatore> m_svincolati;
		private ObservableCollection<Giocatore> m_rosa;

		private Giocatore m_giocatoreSelezionato;
		private Giocatore m_svincolatoSelezionato;

		#endregion

		#region Properties

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

		#endregion

		public ModificaViewModel(IDialogService dialogService, IEventAggregator eventAggregator, Lega lega) : base(eventAggregator, lega)
		{
			m_dialogService = dialogService;

			AggiungiCommand = new DelegateCommand(Aggiungi, AbilitaAggiungi);
			RimuoviCommand = new DelegateCommand(Rimuovi, AbilitaRimuovi);
		}

		#region Public methods

		public override void OnDialogClosed()
		{
			m_eventAggregator.GetEvent<GiocatoreAggiuntoEvent>().Unsubscribe(OnGiocatoreAggiunto);
			m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Unsubscribe(OnGiocatoreRimosso);
		}

		public override void OnDialogOpened(IDialogParameters parameters)
		{
			m_squadra = parameters.GetValue<FantaSquadra>("squadra");

			Rosa = new ObservableCollection<Giocatore>(m_squadra.Giocatori.OrderBy(g => g.Ruolo).ThenByDescending(g => g.Prezzo).ThenBy(g => g.Nome));
			Svincolati = new ObservableCollection<Giocatore>(m_lega.Svincolati.OrderBy(g => g.Nome));

			m_eventAggregator.GetEvent<GiocatoreAggiuntoEvent>().Subscribe(OnGiocatoreAggiunto);
			m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Subscribe(OnGiocatoreRimosso);

			base.OnDialogOpened(parameters);
		}

		#endregion

		#region Protected methods

		protected override void InizializzaIcona(IDialogParameters parameters)
		{ }

		protected override void InizializzaTitolo(IDialogParameters parameters)
		{
			Title = $"Modifica la rosa di {parameters.GetValue<FantaSquadra>("squadra").Nome}";
		}

		protected override void InizializzaBottoni(IDialogParameters parameters)
		{
			Buttons.Add(new DialogButton("Chiudi", new DelegateCommand(Chiudi)));
		}

		#endregion

		#region Private methods

		#region Event handlers

		private void OnGiocatoreAggiunto(GiocatoreAggiuntoEventArgs args)
		{
			if (args.FantaSquadra.Equals(m_squadra) && !Rosa.Contains(args.Giocatore) && Svincolati.Contains(args.Giocatore))
			{
				Rosa.Add(args.Giocatore);
				Rosa = new ObservableCollection<Giocatore>(Rosa.OrderBy(g => g.Ruolo).ThenByDescending(g => g.Prezzo).ThenBy(g => g.Nome));
				Svincolati.Remove(args.Giocatore);
			}
		}

		private void OnGiocatoreRimosso(GiocatoreRimossoEventArgs args)
		{
			if (args.FantaSquadra.Equals(m_squadra) && Rosa.Contains(args.Giocatore) && !Svincolati.Contains(args.Giocatore))
			{
				Rosa.Remove(args.Giocatore);
				Svincolati = new ObservableCollection<Giocatore>(m_lega.Svincolati.OrderBy(g => g.Nome));
			}
		}

		#endregion

		#region Commands

		private void Aggiungi()
		{
			if (m_lega.FantaSquadre.Select(s => s.Giocatori).Where(g => g.Contains(SvincolatoSelezionato)).Count() > 0)
			{
				m_dialogService.ShowMessage("Il giocatore selezionato è già assegnato ad una fantasquadra.", MessageType.Warning);
			}
			else
			{
				m_dialogService.ShowDialog(CommonConstants.PREZZO_DIALOG, new DialogParameters
				{
					{ "Type", DialogType.Popup },
					{ "Movimento", Movimenti.Acquisto },
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
				m_dialogService.ShowMessage("Il giocatore selezionato non è presente nella rosa della fantasquadra.", MessageType.Warning);
			}
			else
			{
				m_dialogService.ShowDialog(CommonConstants.PREZZO_DIALOG, new DialogParameters
				{
					{ "Movimento", Movimenti.Vendita },
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