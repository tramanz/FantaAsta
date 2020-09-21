using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Prism.Commands;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
using FantaAsta.Constants;
using FantaAsta.Models;
using FantaAsta.Views;

namespace FantaAsta.Modules
{
	public class MainModule : IModule
	{
		#region Private fields

		private IDialogService m_dialogService;

		private Asta m_asta;

		#endregion

		#region Public methods

		public void OnInitialized(IContainerProvider containerProvider)
		{
			m_asta = containerProvider.Resolve<Asta>();

			m_dialogService = containerProvider.Resolve<IDialogService>();
			
			IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
			_ = regionManager.RegisterViewWithRegion(CommonConstants.MAIN_REGION, typeof(MainView));
			foreach (MenuItem menuItem in InizializzaMenu())
			{
				_ = regionManager.AddToRegion(CommonConstants.MENU_REGION, menuItem);
			}
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MainView>();
		}

		#endregion

		#region Private methods

		private List<MenuItem> InizializzaMenu()
		{
			// File menu
			MenuItem fileMenuItem = new MenuItem { Header = "File", TabIndex = 0 };
			MenuItem saveMenuItem = new MenuItem { Header = "Salva", Command = new DelegateCommand(Salva), TabIndex = 0 };
			MenuItem exitMenuItem = new MenuItem { Header = "Esci", Command = new DelegateCommand(ChiudiApplicazione), TabIndex = 0 };

			_ = fileMenuItem.Items.Add(saveMenuItem);
			_ = fileMenuItem.Items.Add(exitMenuItem);

			// Opzioni
			MenuItem optionsMenuItem = new MenuItem { Header = "Preferenze", Command = new DelegateCommand(ApriPreferenze), TabIndex = 1 };

			// About
			MenuItem aboutMenuItem = new MenuItem { Header = "Info", Command = new DelegateCommand(ApriAbout), TabIndex = 2 };

			return new List<MenuItem>
			{
				fileMenuItem,
				optionsMenuItem,
				aboutMenuItem
			};
		}

		private void ApriPreferenze()
		{
			m_dialogService.ShowDialog(CommonConstants.PREFERENZE_DIALOG, new DialogParameters(), null);
		}

		private void ApriAbout()
		{
			m_dialogService.ShowDialog(CommonConstants.ABOUT_DIALOG, new DialogParameters(), null);
		}

		private void Salva()
		{
			m_asta.SalvaDati();
		}

		private void ChiudiApplicazione()
		{
			SystemCommands.CloseWindowCommand.Execute(null, Application.Current.MainWindow);
		}

		#endregion
	}
}
