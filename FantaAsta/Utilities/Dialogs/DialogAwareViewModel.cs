using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace FantaAsta.Utilities.Dialogs
{
	/// <summary>
	/// Implementa un view model di base la cui view corrispondente è associato ad una finestra di dialogo
	/// </summary>
	public abstract class DialogAwareViewModel : BindableBase, IDialogAware
	{
		#region Private fields

		private string m_title;

		private ObservableCollection<DialogButton> m_buttons;

		#endregion

		#region Properties 

		public string Title 
		{ 
			get { return m_title; }
			protected set { SetProperty(ref m_title, value); }
		}

		public ObservableCollection<DialogButton> Buttons
		{
			get { return m_buttons; }
			protected set { SetProperty(ref m_buttons, value); }
		}

		#endregion

		#region Events

		public event Action<IDialogResult> RequestClose;

		#endregion

		protected DialogAwareViewModel()
		{
			Buttons = new ObservableCollection<DialogButton>();
		}

		#region Public methods

		public virtual bool CanCloseDialog()
		{ return true; }

		public virtual void OnDialogClosed()
		{ }

		public virtual void OnDialogOpened(IDialogParameters parameters)
		{
			InizializzaTitolo();
			InizializzaBottoni();
		}

		#endregion

		#region Protected methods

		protected abstract void InizializzaTitolo();

		protected abstract void InizializzaBottoni();

		protected void RaiseRequestClose(IDialogResult result)
		{
			RequestClose(result);
		}

		#endregion
	}
}