﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Prism.Commands;
using Prism.Services.Dialogs;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.ViewModels;

namespace FantaAsta.Utilities.Dialogs
{
	public class MessageViewModel : DialogAwareViewModel
	{
		#region Private fields

		private MessageType m_messageType;

		private string m_message;

		private Geometry m_icon;

		#endregion

		#region Properties

		public string Message
		{
			get { return m_message; }
			protected set { SetProperty(ref m_message, value); }
		}

		public Geometry Icon
		{
			get { return m_icon; }
			protected set { SetProperty(ref m_icon, value); }
		}

		#endregion

		public MessageViewModel(Lega lega) : base(lega)
		{

		}

		#region Public methods

		public override void OnDialogOpened(IDialogParameters parameters)
		{
			Message = parameters.GetValue<string>("Message");
			m_messageType = parameters.GetValue<MessageType>("Type");
			switch (m_messageType)
			{
				case MessageType.Error:
					{
						Icon = (Geometry)Application.Current.TryFindResource("CancelIcon");
						break;
					}
				case MessageType.Notification:
					{
						Icon = (Geometry)Application.Current.TryFindResource("CheckedIcon");
						break;
					}
				case MessageType.Warning:
					{
						Icon = (Geometry)Application.Current.TryFindResource("ExclamationMarkIcon");
						break;
					}
				default:
					break;
			}

			base.OnDialogOpened(parameters);
		}

		#endregion

		#region Protected methods

		protected override void InizializzaTitolo()
		{
			switch (m_messageType)
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

		protected override void InizializzaBottoni()
		{
			switch (m_messageType)
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
