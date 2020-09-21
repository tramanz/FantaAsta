using System.Windows;
using System.Windows.Controls;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using FantaAsta.Constants;
using FantaAsta.Models;
using FantaAsta.Modules;
using FantaAsta.Utilities.Regions;
using FantaAsta.Utilities.Dialogs;
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
			containerRegistry.RegisterDialog<MessageView, MessageViewModel>(CommonConstants.MESSAGE_DIALOG);
			containerRegistry.RegisterDialog<ModificaView, ModificaViewModel>(CommonConstants.MODIFICA_DIALOG);
			containerRegistry.RegisterDialog<PrezzoView, PrezzoViewModel>(CommonConstants.PREZZO_DIALOG);
			containerRegistry.RegisterDialog<AggiungiSquadraView, AggiungiSquadraViewModel>(CommonConstants.AGGIUNGI_DIALOG);
			containerRegistry.RegisterDialog<AssegnaView, AssegnaViewModel>(CommonConstants.ASSEGNA_DIALOG);
			containerRegistry.RegisterDialog<PreferenzeView, PreferenzeViewModel>(CommonConstants.PREFERENZE_DIALOG);
			containerRegistry.RegisterDialog<AboutView, AboutViewModel>(CommonConstants.ABOUT_DIALOG);

			containerRegistry.RegisterDialogWindow<DialogWindow>();

			_ = containerRegistry.RegisterSingleton<Asta>();
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			_ = moduleCatalog.AddModule<AstaModule>();
			_ = moduleCatalog.AddModule<RoseModule>();
			_ = moduleCatalog.AddModule<ListaModule>();
			_ = moduleCatalog.AddModule<StoricoModule>();
			_ = moduleCatalog.AddModule<MainModule>();
			_ = moduleCatalog.AddModule<SelezioneModule>();
		}

		protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
		{
			base.ConfigureRegionAdapterMappings(regionAdapterMappings);

			regionAdapterMappings.RegisterMapping(typeof(TabControl), Container.Resolve<TabControlRegionAdapter>());
			regionAdapterMappings.RegisterMapping(typeof(Menu), Container.Resolve<MenuRegionAdapter>());
		}
	}
}
