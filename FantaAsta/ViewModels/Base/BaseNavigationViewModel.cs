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

		protected readonly Lega m_lega;

		#endregion

		protected BaseNavigationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, Lega lega) : base(regionManager, eventAggregator)
		{
			m_syncContext = SynchronizationContext.Current;

			m_lega = lega;
		}
	}
}