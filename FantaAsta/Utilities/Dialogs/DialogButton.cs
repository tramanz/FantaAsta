using Prism.Commands;

namespace FantaAsta.Utilities.Dialogs
{
	public class DialogButton
	{
		#region Properties

		public string Content { get; }

		public DelegateCommand Command { get; }

		#endregion

		public DialogButton(string content, DelegateCommand command)
		{
			Content = content;
			Command = command;
		}
	}
}