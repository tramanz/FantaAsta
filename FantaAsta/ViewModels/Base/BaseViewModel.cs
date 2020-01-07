using System.Threading;
using Prism.Mvvm;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public abstract class BaseViewModel : BindableBase
	{
		#region Protected fields

		protected readonly SynchronizationContext m_syncContext;

		protected readonly Lega m_lega;

		#endregion

		protected BaseViewModel(Lega lega)
		{
			m_syncContext = SynchronizationContext.Current;

			m_lega = lega;
		}
	}
}