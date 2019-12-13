using FantaAsta.Enums;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace FantaAsta.Views
{
	/// <summary>
	/// Logica di interazione per Shell.xaml
	/// </summary>
	public partial class Shell : Window
	{
		public Shell()
		{
			InitializeComponent();

			CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow, CanCloseWindow));
			CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanMaximizeWindow));
			CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
		}

		#region Private methods

		private void CloseWindow(object target, ExecutedRoutedEventArgs e)
		{
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
				case WindowState.Maximized:
					WindowState = WindowState.Normal;
					break;
				case WindowState.Normal:
					WindowState = WindowState.Maximized;
					break;
			}
		}

		#endregion
	}
}
