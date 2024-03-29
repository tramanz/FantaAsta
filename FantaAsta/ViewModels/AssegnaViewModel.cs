﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class AssegnaViewModel : BaseDialogViewModel
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private ObservableCollection<string> m_squadre;

		private Giocatore m_giocatore;

		private string m_squadraSelezionata;

		private string m_prezzoString;
		private double m_prezzo;

		#endregion

		#region Properties

		public ObservableCollection<string> Squadre
		{
			get { return m_squadre; }
			set { _ = SetProperty(ref m_squadre, value); }
		}

		public string SquadraSelezionata
		{
			get { return m_squadraSelezionata; }
			set { _ = SetProperty(ref m_squadraSelezionata, value); Buttons[0]?.Command.RaiseCanExecuteChanged(); }
		}

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

		public AssegnaViewModel(IEventAggregator eventAggregator, IDialogService dialogService, Asta asta) : base(eventAggregator, asta)
		{
			m_dialogService = dialogService;
		}

		#region Public methods

		public override void OnDialogOpened(IDialogParameters parameters)
		{
			base.OnDialogOpened(parameters);
			
			m_giocatore = parameters.GetValue<Giocatore>(typeof(Giocatore).ToString());
			Prezzo = m_giocatore.Quotazione.ToString();

			Squadre = new ObservableCollection<string>(m_asta?.DatiAsta.FantaSquadre.Select(s => s.Nome).OrderBy(s => s));
		}

		#endregion

		#region Protected methods

		protected override void InizializzaIcona(IDialogParameters parameters)
		{ }

		protected override void InizializzaTitolo(IDialogParameters parameters)
		{
			Title = $"Chi si aggiudica {parameters.GetValue<Giocatore>(typeof(Giocatore).ToString()).Nome}?";
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
			FantaSquadra fantaSquadra = m_asta?.DatiAsta.FantaSquadre.Single(s => s.Nome.Equals(SquadraSelezionata));
			double puntataMinima = m_asta.Preferenze.PuntataMinima == PuntataMinima.Quotazione ? m_giocatore.Quotazione : 1;

			if (m_prezzo < puntataMinima)
			{
				_ = m_dialogService.ShowMessage("Il prezzo di acquisto non può essere inferiore alla puntata minima consentita", MessageType.Error);

				SelectPrezzoTextBox?.Invoke(this, EventArgs.Empty);
			}
			else if (m_prezzo > fantaSquadra.Budget)
			{
				_ = m_dialogService.ShowMessage("La fantasquadra non dispone del budget necessario", MessageType.Error);

				SelectPrezzoTextBox?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				bool result = m_asta.AggiungiGiocatore(fantaSquadra, m_giocatore, m_prezzo);

				if (!result)
				{
					_ = m_dialogService.ShowMessage("Il giocatore non può essere aggiunto", MessageType.Error);
				}

				Annulla();
			}
		}
		private bool AbilitaConferma()
		{
			return !string.IsNullOrEmpty(SquadraSelezionata) && !double.IsNaN(m_prezzo);
		}

		private void Annulla()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.OK));
		}

		#endregion
	}
}