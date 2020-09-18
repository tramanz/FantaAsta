using System.Collections.Specialized;
using System.Windows.Controls;
using Prism.Regions;

namespace FantaAsta.Utilities.Regions
{
	public class MenuRegionAdapter : RegionAdapterBase<Menu>
	{
		public MenuRegionAdapter(IRegionBehaviorFactory regionBehaviourFactory) : base(regionBehaviourFactory)
		{ }

		#region Protected methods

		protected override void Adapt(IRegion region, Menu regionTarget)
		{
			region.Views.CollectionChanged += (s, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					foreach (MenuItem element in e.NewItems)
					{
						_ = regionTarget.Items.Add(element);
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					foreach (MenuItem element in e.OldItems)
					{
						regionTarget.Items.Remove(element);
					}
				}
			};
		}

		protected override IRegion CreateRegion()
		{
			return new AllActiveRegion();
		}

		#endregion
	}
}
