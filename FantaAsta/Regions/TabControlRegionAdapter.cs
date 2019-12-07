using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Prism.Regions;

namespace FantaAsta.Regions
{
	public class TabControlRegionAdapter : RegionAdapterBase<TabControl>
	{
		public TabControlRegionAdapter(IRegionBehaviorFactory regionBehaviourFactory) : base(regionBehaviourFactory)
		{ }

		protected override void Adapt(IRegion region, TabControl regionTarget)
		{
			region.Views.CollectionChanged += (s, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					foreach (FrameworkElement element in e.NewItems)
					{
						regionTarget.Items.Add(new TabItem { Content = element, Header = element.Name });
					}
				}
			};
		}

		protected override IRegion CreateRegion()
		{
			return new AllActiveRegion();
		}
	}
}
