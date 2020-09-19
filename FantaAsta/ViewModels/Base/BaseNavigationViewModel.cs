using System.Threading;
using Prism.Events;
using Prism.Regions;
using FantaAsta.Models;
using FantaAsta.Utilities.Navigation;

namespace FantaAsta.ViewModels
{
	public abstract class BaseNavigationViewModel : NavigationAwareViewModel
	{
		#region Protected fields

		protected readonly SynchronizationContext m_syncContext;

		protected readonly Asta m_asta;

		#endregion

		protected BaseNavigationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, Asta asta) : base(regionManager, eventAggregator)
		{
			m_syncContext = SynchronizationContext.Current;

			m_asta = asta;
		}
	}
}