using System.Collections.Generic;
using FantaAsta.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace FantaAsta.ViewModels
{
	public class RoseViewModel : BindableBase
	{
		private readonly IDialogService m_dialogService;
		private readonly MainModel m_mainModel;

		public List<Squadra> Squadre => m_mainModel.Squadre;

		public RoseViewModel(IDialogService dialogService, MainModel mainModel)
		{
			m_dialogService = dialogService;
			m_mainModel = mainModel;

			ModificaCommand = new DelegateCommand<string>(Modifica);
		}

		public DelegateCommand<string> ModificaCommand { get; private set; }
		private void Modifica(string param)
		{
			m_dialogService.Show("Modifica", new DialogParameters($"squadra={param as string}"), null);
		}
	}
}
