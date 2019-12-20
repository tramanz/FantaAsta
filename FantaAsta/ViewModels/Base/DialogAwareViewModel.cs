using System;
using FantaAsta.Models;
using Prism.Services.Dialogs;

namespace FantaAsta.ViewModels
{
	/// <summary>
	/// Implementa un view model di base la cui view corrispondente è associato ad una finestra di dialogo
	/// </summary>
	public abstract class DialogAwareViewModel : BaseViewModel, IDialogAware
	{
		#region Properties 

		public virtual string Title { get; set; }

		#endregion

		#region Events

		public event Action<IDialogResult> RequestClose;

		#endregion

		protected DialogAwareViewModel(Lega lega) : base(lega)
		{ }

		#region Public methods

		public virtual bool CanCloseDialog()
		{ return true; }

		public virtual void OnDialogClosed()
		{ }

		public virtual void OnDialogOpened(IDialogParameters parameters)
		{ }

		#endregion

		#region Protected methods

		protected void RaiseRequestClose(IDialogResult result)
		{
			RequestClose(result);
		}

		#endregion
	}
}