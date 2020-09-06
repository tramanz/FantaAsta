using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Prism.Commands;
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
			set { SetProperty(ref m_squadre, value); }
		}

		public string SquadraSelezionata
		{
			get { return m_squadraSelezionata; }
			set { SetProperty(ref m_squadraSelezionata, value); Buttons[0]?.Command.RaiseCanExecuteChanged(); }
		}

		public string Prezzo
		{
			get { return m_prezzoString; }
			set
			{
				SetProperty(ref m_prezzoString, value);
				m_prezzo = double.TryParse(m_prezzoString, NumberStyles.AllowDecimalPoint, null, out double number) ? number : double.NaN;
				Buttons[0]?.Command.RaiseCanExecuteChanged();
			}
		}

		#endregion

		#region Events

		public event EventHandler SelectPrezzoTextBox;

		#endregion

		public AssegnaViewModel(IDialogService dialogService, Lega lega) : base(lega)
		{
			m_dialogService = dialogService;
		}

		#region Public methods

		public override void OnDialogOpened(IDialogParameters parameters)
		{
			base.OnDialogOpened(parameters);
			
			m_giocatore = parameters.GetValue<Giocatore>("Giocatore");
			Prezzo = m_giocatore.Quotazione.ToString();

			Squadre = new ObservableCollection<string>(m_lega?.FantaSquadre.Select(s => s.Nome).OrderBy(s => s));
		}

		#endregion

		#region Protected methods

		protected override void InizializzaIcona(IDialogParameters parameters)
		{ }

		protected override void InizializzaTitolo(IDialogParameters parameters)
		{
			Title = $"Chi si aggiudica {parameters.GetValue<Giocatore>("Giocatore").Nome}?";
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
			FantaSquadra fantaSquadra = m_lega?.FantaSquadre.Single(s => s.Nome.Equals(SquadraSelezionata));

			if (double.IsNaN(m_prezzo))
			{
				m_dialogService.ShowMessage("Inserire un prezzo.", MessageType.Error);

				SelectPrezzoTextBox?.Invoke(this, System.EventArgs.Empty);
			}
			else if (m_prezzo < m_giocatore.Quotazione)
			{
				m_dialogService.ShowMessage("Il prezzo di acquisto non può essere inferiore alla quotazione del giocatore.", MessageType.Error);

				SelectPrezzoTextBox?.Invoke(this, System.EventArgs.Empty);
			}
			else if (m_prezzo > fantaSquadra.Budget)
			{
				m_dialogService.ShowMessage("Budget non disponibile.", MessageType.Error);

				SelectPrezzoTextBox?.Invoke(this, System.EventArgs.Empty);
			}
			else
			{
				bool result = m_lega.AggiungiGiocatore(fantaSquadra, m_giocatore, m_prezzo);

				if (!result)
				{
					m_dialogService.ShowMessage("Il giocatore non può essere aggiunto", MessageType.Error);
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