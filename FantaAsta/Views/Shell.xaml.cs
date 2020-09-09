using System.Windows;
using System.Windows.Input;
using Prism.Services.Dialogs;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.Views
{
	/// <summary>
	/// Logica di interazione per Shell.xaml
	/// </summary>
	public partial class Shell : Window
	{
		#region Private fields

		private readonly Lega m_lega;
		private readonly IDialogService m_dialogService;

		#endregion

		public Shell(IDialogService dialogService, Lega lega)
		{
			InitializeComponent();

			m_lega = lega;
			m_dialogService = dialogService;

			_ = CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow, CanCloseWindow));
			_ = CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanMaximizeWindow));
			_ = CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
		}

		#region Private methods

		private void CloseWindow(object target, ExecutedRoutedEventArgs e)
		{
			m_lega.DisattivaModalitaAstaInvernale();

			ButtonResult result = m_dialogService.ShowMessage("Vuoi salvare le modifiche prima di chiudere?", MessageType.Warning);

			if (result == ButtonResult.Yes)
			{
				m_lega.SalvaSquadre();
			}

			SystemCommands.CloseWindow(this);
		}
		private void CanCloseWindow(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void MaximizeWindow(object target, ExecutedRoutedEventArgs e)
		{
			ChangeWindowSize();
			e.Handled = true;
		}
		private void CanMaximizeWindow(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void MinimizeWindow(object target, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MinimizeWindow(this);
			e.Handled = true;
		}
		private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OnTitleBarClicked(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				ChangeWindowSize();
			}
			else
			{
				DragMove();
			}
		}

		private void ChangeWindowSize()
		{
			switch (WindowState)
			{
				case WindowState.Normal:
					{
						WindowState = WindowState.Maximized;
						break;
					}
				case WindowState.Maximized:
					{
						WindowState = WindowState.Normal;
						break;
					}
				case WindowState.Minimized:
				default:
					break;
			}
		}

		#endregion
	}
}
