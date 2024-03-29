﻿using System;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class AggiungiSquadraViewModel : BaseDialogViewModel
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private string m_nome;

		#endregion

		#region Properties

		public string Nome
		{
			get { return m_nome; }
			set { _ = SetProperty(ref m_nome, value); Buttons[0]?.Command.RaiseCanExecuteChanged(); }
		}

		#endregion

		#region Events

		public event EventHandler SelectNameTextBox;

		#endregion

		public AggiungiSquadraViewModel(IEventAggregator eventAggregator, IDialogService dialogService, Asta asta) : base(eventAggregator, asta)
		{
			m_dialogService = dialogService;
		}

		#region Protected methods

		protected override void InizializzaIcona(IDialogParameters parameters)
		{ }

		protected override void InizializzaTitolo(IDialogParameters parameters)
		{
			Title = "Inserisci il nome della squadra";
		}

		protected override void InizializzaBottoni(IDialogParameters parameters)
		{
			Buttons.Add(new DialogButton("Aggiungi", new DelegateCommand(Aggiungi, AbilitaAggiungi)));
			Buttons.Add(new DialogButton("Chiudi", new DelegateCommand(Chiudi)));
		}

		#endregion

		#region Private methods

		private void Aggiungi()
		{
			bool result = m_asta.AggiungiSquadra(Nome);

			if (result)
			{
				_ = m_dialogService.ShowMessage("Squadra aggiunta", MessageType.Notification);
			}
			else
			{
				_ = m_dialogService.ShowMessage("Non è possibile aggiungere una squadra con lo stesso nome di una già esistente", MessageType.Error);
			}

			SelectNameTextBox?.Invoke(this, EventArgs.Empty);
		}
		private bool AbilitaAggiungi()
		{
			return !string.IsNullOrEmpty(Nome);
		}

		private void Chiudi()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.OK));
		}

		#endregion
	}
}