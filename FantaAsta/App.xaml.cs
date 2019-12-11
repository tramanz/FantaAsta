﻿using System.Windows;
using System.Windows.Controls;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using FantaAsta.Models;
using FantaAsta.Modules;
using FantaAsta.Regions;
using FantaAsta.ViewModels;
using FantaAsta.Views;

namespace FantaAsta
{
	/// <summary>
	/// Logica di interazione per App.xaml
	/// </summary>
	public partial class App : PrismApplication
	{
		protected override Window CreateShell()
		{
			return Container.Resolve<Shell>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterDialog<ModificaView, ModificaViewModel>("Modifica");

			containerRegistry.RegisterSingleton<Lega>();
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			moduleCatalog.AddModule<AstaModule>();
			moduleCatalog.AddModule<RoseModule>();
			moduleCatalog.AddModule<ListaModule>();
			moduleCatalog.AddModule<SelezioneModule>();
			moduleCatalog.AddModule<MainModule>();
		}

		protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
		{
			base.ConfigureRegionAdapterMappings(regionAdapterMappings);

			regionAdapterMappings.RegisterMapping(typeof(TabControl), Container.Resolve<TabControlRegionAdapter>());
		}

		protected override void OnExit(ExitEventArgs e)
		{
			Lega lega = Container.Resolve<Lega>();

			if (lega.IsAstaInvernale)
			{
				lega.TerminaAstaInvernale();
			}

			lega.SalvaSquadre();

			base.OnExit(e);
		}
	}
}
