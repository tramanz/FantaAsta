using System.Windows;
using System.Windows.Controls;
using FantaAsta.ViewModels;

namespace FantaAsta.Views
{
	/// <summary>
	/// Interaction logic for AggiungiSquadraView.xaml
	/// </summary>
	public partial class AggiungiSquadraView : UserControl
	{
		public AggiungiSquadraView()
		{
			InitializeComponent();
		}

		#region Private methods

		private void OnDataContextChanged(object _, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != null)
			{
				(DataContext as AggiungiSquadraViewModel).SelectNameTextBox += OnSelectNameTextBox;
			}
		}

		private void OnSelectNameTextBox(object sender, System.EventArgs e)
		{
			_ = nomeTxtBox.Focus();
			nomeTxtBox.SelectAll();
		}

		#endregion
	}
}
