using Prism.Services.Dialogs;
using FantaAsta.Enums;

namespace FantaAsta.Utilities.Dialogs
{
	public static class IDialogServiceExtensions
	{
		public static ButtonResult ShowMessage(this IDialogService dialogService, string message, MessageType type, bool isModal = true)
		{
			DialogParameters parameters = new DialogParameters
			{
				{ "Message", message },
				{ "Type", type }
			};

			ButtonResult result = ButtonResult.None;

			if (isModal)
			{
				dialogService.ShowDialog("Message", parameters, (IDialogResult res) => result = res.Result);
			}
			else
			{
				dialogService.Show("Message", parameters, (IDialogResult res) => result = res.Result);
			}

			return result;
		}
	}
}
