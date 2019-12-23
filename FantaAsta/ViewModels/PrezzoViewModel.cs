using System;
using Prism.Commands;
using Prism.Services.Dialogs;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class PrezzoViewModel : DialogAwareViewModel
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private FantaSquadra m_squadra;

		private Giocatore m_giocatore;

		private Movimenti m_movimento;

		private double m_prezzo;

		#endregion

		#region Properties

		public double Prezzo
		{
			get { return m_prezzo; }
			set { SetProperty(ref m_prezzo, value); Buttons[0]?.Command.RaiseCanExecuteChanged(); }
		}

		#endregion

		#region Events

		public event EventHandler SelectNameTextBox;

		#endregion

		public PrezzoViewModel(IDialogService dialogService, Lega lega) : base(lega)
		{
			m_dialogService = dialogService;
		}

		#region Public methods

		public override void OnDialogOpened(IDialogParameters parameters)
		{
			m_giocatore = parameters.GetValue<Giocatore>("Giocatore");
			m_squadra = parameters.GetValue<FantaSquadra>("FantaSquadra");
			m_movimento = parameters.GetValue<Movimenti>("Movimento");

			base.OnDialogOpened(parameters);
		}

		#endregion

		#region Protected methods

		protected override void InizializzaTitolo()
		{
			Title = $"Inserisci il prezzo di {m_movimento.ToString().ToLower()}";
		}

		protected override void InizializzaBottoni()
		{
			Buttons.Add(new DialogButton("Conferma", new DelegateCommand(Conferma, AbilitaConferma)));
			Buttons.Add(new DialogButton("Annulla", new DelegateCommand(Annulla)));
		}

		#endregion

		#region Private methods

		private void Conferma()
		{
			if (double.IsNaN(Prezzo))
			{
				m_dialogService.ShowMessage("Inserire un prezzo", MessageType.Warning);

				SelectNameTextBox?.Invoke(this, System.EventArgs.Empty);
			}
			else
			{
				if (m_movimento == Movimenti.Acquisto)
				{
					AcquistaGiocatore();
				}
				else if (m_movimento == Movimenti.Vendita)
				{
					VendiGiocatore();
				}

				Annulla();
			}
		}
		private bool AbilitaConferma()
		{
			return !double.IsNaN(Prezzo);
		}

		private void Annulla()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.OK));
		}

		private void AcquistaGiocatore()
		{
			if (Prezzo < m_giocatore.Quotazione)
			{
				m_dialogService.ShowMessage("Il prezzo di acquisto non può essere inferiore alla quotazione del giocatore.", MessageType.Warning);

				SelectNameTextBox?.Invoke(this, System.EventArgs.Empty);
			}
			else
			{
				bool result = m_lega.AggiungiGiocatore(m_squadra, m_giocatore, Prezzo);

				string msg = result ? "Giocatore aggiunto" : "Il giocatore non può essere aggiunto";
				MessageType type = result ? MessageType.Notification : MessageType.Error;

				m_dialogService.ShowMessage(msg, type);
			}
		}

		private void VendiGiocatore()
		{
			if (Prezzo <= 0)
			{
				m_dialogService.ShowMessage("Il prezzo di vendita non può essere minore o uguale a 0.", MessageType.Error);

				SelectNameTextBox?.Invoke(this, System.EventArgs.Empty);
			}
			else
			{
				m_lega.RimuoviGiocatore(m_squadra, m_giocatore, Prezzo);

				m_dialogService.ShowMessage("Giocatore rimosso", MessageType.Notification);
			}
		}

		#endregion
	}
}