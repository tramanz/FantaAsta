using System.Threading;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public abstract class BaseDialogViewModel : DialogAwareViewModel
	{
		#region Protected fields

		protected readonly SynchronizationContext m_syncContext;

		protected readonly Lega m_lega;

		#endregion

		protected BaseDialogViewModel(Lega lega)
		{
			m_syncContext = SynchronizationContext.Current;
			
			m_lega = lega;
		}
	}
}