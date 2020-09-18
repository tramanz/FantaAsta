using System.Windows;
using System.Windows.Input;
using Prism.Services.Dialogs;

namespace FantaAsta.Utilities.Dialogs
{
	/// <summary>
	/// Interaction logic for DialogWindow.xaml
	/// </summary>
	public partial class DialogWindow : Window, IDialogWindow
	{
		#region Properties

		public IDialogResult Result { get; set; }

		#endregion

		public DialogWindow()
		{
			InitializeComponent();
		}

		#region Protected methods

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);

			DragMove();
		}

		#endregion
	}
}
