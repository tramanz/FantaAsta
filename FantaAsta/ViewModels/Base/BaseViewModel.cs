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

		protected readonly Asta m_asta;

		#endregion

		protected BaseViewModel(IEventAggregator eventAggregator, Asta asta)
		{
			m_eventAggregator = eventAggregator;

			m_syncContext = SynchronizationContext.Current;

			m_asta = asta;
		}
	}
}