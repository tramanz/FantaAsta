using System.Windows;
using System.Windows.Input;
using Prism.Services.Dialogs;

namespace FantaAsta.Utilities
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
	}
}
