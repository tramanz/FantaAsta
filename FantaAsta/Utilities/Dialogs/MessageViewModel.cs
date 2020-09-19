using System.Windows;
using System.Windows.Media;
using Prism.Commands;
using Prism.Services.Dialogs;
using FantaAsta.Enums;

namespace FantaAsta.Utilities.Dialogs
{
	public class MessageViewModel : DialogAwareViewModel
	{
		#region Constants

		private static readonly SolidColorBrush RED_BRUSH = new SolidColorBrush(Colors.DarkRed);
		private static readonly SolidColorBrush GOLD_BRUSH = new SolidColorBrush(Colors.DarkGoldenrod);
		private static readonly SolidColorBrush GREEN_BRUSH = new SolidColorBrush(Colors.DarkGreen);

		#endregion

		#region Private fields

		private string m_message;

		#endregion

		#region Properties

		public string Message
		{
			get { return m_message; }
			protected set { _ = SetProperty(ref m_message, value); }
		}

		#endregion

		#region Public methods

		public override void OnDialogOpened(IDialogParameters parameters)
		{
			base.OnDialogOpened(parameters);

			Message = parameters.GetValue<string>(typeof(string).ToString());
		}

		#endregion

		#region Protected methods

		protected override void InizializzaIcona(IDialogParameters parameters)
		{
			MessageType messageType = parameters.GetValue<MessageType>(typeof(MessageType).ToString());
			switch (messageType)
			{
				case MessageType.Error:
					{
						Icon = (Geometry)Application.Current.TryFindResource("CancelIcon");
						IconColor = RED_BRUSH;
						break;
					}
				case MessageType.Notification:
					{
						Icon = (Geometry)Application.Current.TryFindResource("CheckedIcon");
						IconColor = GREEN_BRUSH;
						break;
					}
				case MessageType.Warning:
					{
						Icon = (Geometry)Application.Current.TryFindResource("ExclamationMarkIcon");
						IconColor = GOLD_BRUSH;
						break;
					}
				default:
					break;
			}
		}

		protected override void InizializzaTitolo(IDialogParameters parameters)
		{
			MessageType messageType = parameters.GetValue<MessageType>(typeof(MessageType).ToString());
			switch (messageType)
			{
				case MessageType.Error:
					{
						Title = "ERRORE";
						break;
					}
				case MessageType.Notification:
					{
						Title = "INFORMAZIONE";
						break;
					}
				case MessageType.Warning:
					{
						Title = "ATTENZIONE";
						break;
					}
				default:
					break;
			}
		}

		protected override void InizializzaBottoni(IDialogParameters parameters)
		{
			MessageType messageType = parameters.GetValue<MessageType>(typeof(MessageType).ToString());
			switch (messageType)
			{
				case MessageType.Error:
				case MessageType.Notification:
					{
						Buttons.Add(new DialogButton("OK", new DelegateCommand(ChiudiOK)));
						break;
					}
				case MessageType.Warning:
					{
						Buttons.Add(new DialogButton("SÌ", new DelegateCommand(ChiudiSI)));
						Buttons.Add(new DialogButton("NO", new DelegateCommand(ChiudiNO)));
						break;
					}
				default:
					break;
			}
		}

		#endregion

		#region Private methods

		private void ChiudiOK()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.OK));
		}

		private void ChiudiSI()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.Yes));
		}

		private void ChiudiNO()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.No));
		}

		#endregion
	}
}