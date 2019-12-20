using Prism.Regions;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	/// <summary>
	/// Implementa un view model di base la cui view corrispondente è associata ad una regione che prevede la navigazione tra le view
	/// </summary>
	public abstract class NavigationAwareViewModel : BaseViewModel, INavigationAware
	{
		#region Protected fields

		protected readonly IRegionManager m_regionManager;

		#endregion

		protected NavigationAwareViewModel(IRegionManager regionManager, Lega lega) : base (lega)
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