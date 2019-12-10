﻿using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class MainModule : IModule
	{
		public MainModule()
		{ }

		#region Public methods

		public void OnInitialized(IContainerProvider containerProvider)
		{
			containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion("MainRegion", typeof(MainView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MainView>();
		}

		#endregion
	}
}
