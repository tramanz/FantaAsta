using System;
using System.IO;
using System.Text;
using System.Windows;
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
			return Container.Resolve<MainView>();
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
		}

		protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
		{
			base.ConfigureRegionAdapterMappings(regionAdapterMappings);

			regionAdapterMappings.RegisterMapping(typeof(TabControl), Container.Resolve<TabControlRegionAdapter>());
		}

		protected override void OnExit(ExitEventArgs e)
		{
			//string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FantaRose2019-2020.csv");
			//StringBuilder output = new StringBuilder();

			//string s = string.Empty;
			//foreach (var squadra in m_mainModel.Squadre)
			//{
			//	s += $"{squadra.Nome};;;;";
			//}
			//output.AppendLine(s);

			//for (int i = 0; i < 25; i++)
			//{
			//	s = string.Empty;
			//	foreach (var squadra in m_mainModel.Squadre)
			//	{
			//		if (i < squadra.Giocatori.Count)
			//		{
			//			s += $"{squadra.Giocatori[i].Ruolo};{squadra.Giocatori[i].Nome};{squadra.Giocatori[i].Squadra};{squadra.Giocatori[i].Prezzo};";
			//		}
			//		else
			//		{
			//			s += "/;/;/;/;";
			//		}
			//	}
			//	output.AppendLine(s);
			//}

			//if (File.Exists(filePath))
			//{
			//	File.Delete(filePath);
			//}

			//File.WriteAllText(filePath, output.ToString());

			base.OnExit(e);
		}
	}
}
