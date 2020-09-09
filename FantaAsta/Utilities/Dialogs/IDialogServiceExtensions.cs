using Prism.Services.Dialogs;
using FantaAsta.Constants;
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
				dialogService.ShowDialog(CommonConstants.MESSAGE_DIALOG, parameters, (IDialogResult res) => result = res.Result);
			}
			else
			{
				dialogService.Show(CommonConstants.MESSAGE_DIALOG, parameters, (IDialogResult res) => result = res.Result);
			}

			return result;
		}
	}
}
