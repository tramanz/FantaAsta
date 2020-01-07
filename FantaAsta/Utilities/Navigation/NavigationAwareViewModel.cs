using Prism.Mvvm;
using Prism.Regions;

namespace FantaAsta.Utilities.Navigation
{
	/// <summary>
	/// Implementa un view model di base la cui view è disponibile per la navigazione
	/// </summary>
	public abstract class NavigationAwareViewModel : BindableBase, INavigationAware
	{
		#region Protected fields

		protected readonly IRegionManager m_regionManager;

		#endregion

		protected NavigationAwareViewModel(IRegionManager regionManager)
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