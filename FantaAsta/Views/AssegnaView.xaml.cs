using System.Windows;
using System.Windows.Controls;
using FantaAsta.ViewModels;

namespace FantaAsta.Views
{
	/// <summary>
	/// Logica di interazione per AssegnaView.xaml
	/// </summary>
	public partial class AssegnaView : UserControl
	{
		public AssegnaView()
		{
			InitializeComponent();
		}

		#region Private methods

		private void OnDataContextChanged(object _, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != null)
			{
				(DataContext as AssegnaViewModel).SelectPrezzoTextBox += OnSelectPrezzoTextBox;
			}
		}

		private void OnSelectPrezzoTextBox(object sender, System.EventArgs e)
		{
			prezzoTxtBox.Focus();
			prezzoTxtBox.SelectAll();
		}

		#endregion
	}
}