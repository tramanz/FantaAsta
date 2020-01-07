using Prism.Regions;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public abstract class BaseNavigationViewModel : BaseViewModel, INavigationAware
	{
		#region Protected fields
		
		protected readonly IRegionManager m_regionManager;

		#endregion

		protected BaseNavigationViewModel(IRegionManager regionManager, Lega lega) : base(lega)
		{
			m_regionManager = regionManager;
		}

		#region Public methods

		public virtual bool IsNavigationTarget(NavigationContext navigationContext)
		{ return true; }

		public virtual void OnNavigatedFrom(NavigationContext navigationContext)
		{ }

		public virtual void OnNavigatedTo(NavigationContext navigationContext)
		{ }

		#endregion
	}
}