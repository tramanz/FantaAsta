using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
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

		private Geometry m_icon;

		private SolidColorBrush m_iconColor;

		private string m_title;

		private ObservableCollection<DialogButton> m_buttons;

		#endregion

		#region Properties 

		public Geometry Icon
		{
			get { return m_icon; }
			protected set { SetProperty(ref m_icon, value); }
		}

		public SolidColorBrush IconColor
		{
			get { return m_iconColor; }
			protected set { SetProperty(ref m_iconColor, value); }
		}

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
			InizializzaIcona(parameters);
			InizializzaTitolo(parameters);
			InizializzaBottoni(parameters);
		}

		#endregion

		#region Protected methods

		protected abstract void InizializzaIcona(IDialogParameters parameters);

		protected abstract void InizializzaTitolo(IDialogParameters parameters);

		protected abstract void InizializzaBottoni(IDialogParameters parameters);

		protected void RaiseRequestClose(IDialogResult result)
		{
			RequestClose(result);
		}

		#endregion
	}
}