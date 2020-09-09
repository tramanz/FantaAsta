using System.Windows;
using System.Windows.Controls;
using FantaAsta.ViewModels;

namespace FantaAsta.Views
{
	/// <summary>
	/// Logica di interazione per PrezzoView.xaml
	/// </summary>
	public partial class PrezzoView : UserControl
	{
		public PrezzoView()
		{
			InitializeComponent();
		}

		#region Private methods

		private void OnDataContextChanged(object _, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != null)
			{
				(DataContext as PrezzoViewModel).SelectPrezzoTextBox += OnSelectNameTextBox;
			}
		}

		private void OnSelectNameTextBox(object sender, System.EventArgs e)
		{
			_ = prezzoTxtBox.Focus();
			prezzoTxtBox.SelectAll();
		}

		#endregion
	}
}
