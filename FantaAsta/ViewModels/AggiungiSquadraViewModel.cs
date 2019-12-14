using System;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class AggiungiSquadraViewModel : BindableBase, IDialogAware
	{
		#region Private fields

		private readonly Lega m_lega;

		private string m_nome;

		#endregion

		#region Properties

		public string Nome
		{
			get { return m_nome; }
			set { SetProperty(ref m_nome, value); AggiungiCommand?.RaiseCanExecuteChanged(); }
		}

		public string Title => "Aggiungi squadra";

		#region Commands

		public DelegateCommand AggiungiCommand { get; }
		public DelegateCommand ChiudiCommand { get; }

		#endregion

		#endregion

		#region Events

		public event Action<IDialogResult> RequestClose;

		public event EventHandler SelectNameTextBox;

		#endregion

		public AggiungiSquadraViewModel(Lega lega)
		{
			m_lega = lega;

			AggiungiCommand = new DelegateCommand(Aggiungi, AbilitaAggiungi);
			ChiudiCommand = new DelegateCommand(Chiudi);
		}

		#region Public methods

		public bool CanCloseDialog()
		{
			return true;
		}

		public void OnDialogClosed()
		{ }

		public void OnDialogOpened(IDialogParameters parameters)
		{ }

		#endregion

		#region Private methods

		#region Commands

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
			RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
		}

		#endregion

		#endregion
	}
}
