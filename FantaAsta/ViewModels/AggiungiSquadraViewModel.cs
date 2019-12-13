using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace FantaAsta.ViewModels
{
	public class AggiungiSquadraViewModel : BindableBase, IDialogAware
	{
		#region Private fields

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
		public DelegateCommand AnnullaCommand { get; }

		#endregion

		#endregion

		#region Events

		public event Action<IDialogResult> RequestClose;

		#endregion

		public AggiungiSquadraViewModel()
		{
			AggiungiCommand = new DelegateCommand(Aggiungi, AbilitaAggiungi);
			AnnullaCommand = new DelegateCommand(Annulla);
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
			RequestClose?.Invoke(new AggiungiSquadraResult(ButtonResult.Yes, Nome));
		}

		private bool AbilitaAggiungi()
		{
			return !string.IsNullOrEmpty(Nome);
		}

		private void Annulla()
		{
			RequestClose?.Invoke(new AggiungiSquadraResult(ButtonResult.No, string.Empty));
		}

		#endregion

		#endregion
	}

	public class AggiungiSquadraResult : DialogResult
	{
		#region Properties

		public string Nome { get; }

		#endregion

		public AggiungiSquadraResult(ButtonResult result, string nome) : base (result)
		{
			Nome = nome;
		}
	}
}
