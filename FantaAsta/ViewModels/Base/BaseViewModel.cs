using System.Threading;
using Prism.Events;
using Prism.Mvvm;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public abstract class BaseViewModel : BindableBase
	{
		#region Protected fields

		protected readonly IEventAggregator m_eventAggregator;

		protected readonly SynchronizationContext m_syncContext;

		protected readonly Lega m_lega;

		#endregion

		protected BaseViewModel(IEventAggregator eventAggregator, Lega lega)
		{
			m_eventAggregator = eventAggregator;

			m_syncContext = SynchronizationContext.Current;

			m_lega = lega;
		}
	}
}