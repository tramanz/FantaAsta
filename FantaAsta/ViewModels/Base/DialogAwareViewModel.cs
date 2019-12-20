using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Prism.Services.Dialogs;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;
using System.Windows;

namespace FantaAsta.ViewModels
{
	/// <summary>
	/// Implementa un view model di base la cui view corrispondente è associato ad una finestra di dialogo
	/// </summary>
	public abstract class DialogAwareViewModel : BaseViewModel, IDialogAware
	{
		#region Private fields

		private DialogType m_dialogType;

		private string m_title;

		private string m_message;

		private Geometry m_icon;

		private ObservableCollection<DialogButton> m_buttons;

		#endregion

		#region Properties 

		public DialogType Type
		{
			get { return m_dialogType; }
			protected set { SetProperty(ref m_dialogType, value); }
		}

		public string Title 
		{ 
			get { return m_title; }
			protected set { SetProperty(ref m_title, value); }
		}

		public string Message
		{
			get { return m_message; }
			protected set { SetProperty(ref m_message, value); }
		}

		public Geometry Icon
		{
			get { return m_icon; }
			protected set { SetProperty(ref m_icon, value); }
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

		protected DialogAwareViewModel(Lega lega) : base(lega)
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
			Type = parameters.GetValue<DialogType>("Type");

			if (Type == DialogType.Message)
			{
				Message = parameters.GetValue<string>("Message");
				switch (parameters.GetValue<MessageIcon>("Icon"))
				{
					case MessageIcon.Error:
						break;
					case MessageIcon.Notification:
						break;
					case MessageIcon.Warning:
						{
							Icon = (Geometry)Application.Current.TryFindResource("WarningIcon");
							break;
						}
					default:
						break;
				}
			}

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