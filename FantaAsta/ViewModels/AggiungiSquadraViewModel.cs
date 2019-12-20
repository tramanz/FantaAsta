using System;
using System.Windows;
using Prism.Commands;
using Prism.Services.Dialogs;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class AggiungiSquadraViewModel : DialogAwareViewModel
	{
		#region Private fields

		private string m_nome;

		#endregion

		#region Properties

		public string Nome
		{
			get { return m_nome; }
			set { SetProperty(ref m_nome, value); Buttons[0]?.Command.RaiseCanExecuteChanged(); }
		}

		#endregion

		#region Events

		public event EventHandler SelectNameTextBox;

		#endregion

		public AggiungiSquadraViewModel(Lega lega) : base(lega)
		{ }

		#region Protected methods

		protected override void InizializzaTitolo()
		{
			Title = "Inserisci il nome della squadra";
		}

		protected override void InizializzaBottoni()
		{
			Buttons.Add(new DialogButton("Aggiungi", new DelegateCommand(Aggiungi, AbilitaAggiungi)));
			Buttons.Add(new DialogButton("Chiudi", new DelegateCommand(Chiudi)));
		}

		#endregion

		#region Private methods

		private void Aggiungi()
		{
			bool result = m_lega.AggiungiSquadra(Nome);

			if (result)
			{
				MessageBox.Show("Squadra aggiunta", "OPERAZIONE COMPLETATA", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
			{
				MessageBox.Show("Non è possibile aggiungere una squadra con lo stesso nome di una già esistente", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			SelectNameTextBox?.Invoke(this, System.EventArgs.Empty);
		}
		private bool AbilitaAggiungi()
		{
			return !string.IsNullOrEmpty(Nome);
		}

		private void Chiudi()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.OK));
		}

		#endregion
	}
}