﻿using System;
using System.Globalization;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class PrezzoViewModel : BaseDialogViewModel
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private FantaSquadra m_squadra;

		private Giocatore m_giocatore;

		private Movimenti m_movimento;

		private string m_prezzoString;
		private double m_prezzo;

		#endregion

		#region Properties

		public string Prezzo
		{
			get { return m_prezzoString; }
			set
			{
				_ = SetProperty(ref m_prezzoString, value);
				m_prezzo = double.TryParse(m_prezzoString, NumberStyles.AllowDecimalPoint, null, out double number) ? number : double.NaN;
				Buttons[0]?.Command.RaiseCanExecuteChanged();
			}
		}

		#endregion

		#region Events

		public event EventHandler SelectPrezzoTextBox;

		#endregion

		public PrezzoViewModel(IEventAggregator eventAggregator, IDialogService dialogService, Asta asta) : base(eventAggregator, asta)
		{
			m_dialogService = dialogService;
		}

		#region Public methods

		public override void OnDialogOpened(IDialogParameters parameters)
		{
			m_giocatore = parameters.GetValue<Giocatore>(typeof(Giocatore).ToString()); ;
			m_squadra = parameters.GetValue<FantaSquadra>(typeof(FantaSquadra).ToString());
			m_movimento = parameters.GetValue<Movimenti>(typeof(Movimenti).ToString());

			base.OnDialogOpened(parameters);

			Prezzo = m_giocatore.Quotazione.ToString();
		}

		#endregion

		#region Protected methods

		protected override void InizializzaIcona(IDialogParameters parameters)
		{ }

		protected override void InizializzaTitolo(IDialogParameters parameters)
		{
			Title = $"Inserisci il prezzo di {parameters.GetValue<Movimenti>(typeof(Movimenti).ToString()).ToString().ToLower()}";
		}

		protected override void InizializzaBottoni(IDialogParameters parameters)
		{
			Buttons.Add(new DialogButton("Conferma", new DelegateCommand(Conferma, AbilitaConferma)));
			Buttons.Add(new DialogButton("Annulla", new DelegateCommand(Annulla)));
		}

		#endregion

		#region Private methods

		private void Conferma()
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
		private bool AbilitaConferma()
		{
			return !double.IsNaN(m_prezzo);
		}

		private void Annulla()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.OK));
		}

		private void AcquistaGiocatore()
		{
			double puntataMinima = m_asta.Preferenze.PuntataMinima == PuntataMinima.Quotazione ? m_giocatore.Quotazione : 1;
			if (m_prezzo < puntataMinima)
			{
				_ = m_dialogService.ShowMessage("Il prezzo di acquisto non può essere inferiore alla puntata minima consentita", MessageType.Error);

				SelectPrezzoTextBox?.Invoke(this, EventArgs.Empty);
			}
			else if (m_prezzo > m_squadra.Budget)
			{
				_ = m_dialogService.ShowMessage("La fantasquadra non dispone del budget necessario", MessageType.Error);

				SelectPrezzoTextBox?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				bool result = m_asta.AggiungiGiocatore(m_squadra, m_giocatore, m_prezzo);

				if (!result)
				{
					_ = m_dialogService.ShowMessage("Il giocatore non può essere aggiunto", MessageType.Error);
				}
			}
		}

		private void VendiGiocatore()
		{
			if (m_prezzo <= 0)
			{
				_ = m_dialogService.ShowMessage("Il prezzo di vendita non può essere minore o uguale a 0", MessageType.Error);

				SelectPrezzoTextBox?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				m_asta.RimuoviGiocatore(m_squadra, m_giocatore, m_prezzo);
			}
		}

		#endregion
	}
}