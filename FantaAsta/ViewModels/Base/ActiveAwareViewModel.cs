using System;
using Prism;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	/// <summary>
	/// Implementa un view model di base che viene notificato quando la view corrispondente viene attivata o disattivata
	/// </summary>
	public abstract class ActiveAwareViewModel : BaseViewModel, IActiveAware
	{
		#region Properties

		public virtual bool IsActive { get; set; }

		#endregion

		#region Events

		public event EventHandler IsActiveChanged;

		#endregion

		protected ActiveAwareViewModel(Lega lega) : base(lega)
		{ }

		#region Protected methods

		protected void RaiseIsActiveChanged(System.EventArgs e)
		{
			IsActiveChanged?.Invoke(this, e);
		}

		#endregion
	}
}
