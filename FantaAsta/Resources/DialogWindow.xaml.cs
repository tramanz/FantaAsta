using System.Windows;
using System.Windows.Input;
using Prism.Services.Dialogs;

namespace FantaAsta.Resources
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

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);

			DragMove();
		}

		#endregion
	}
}
