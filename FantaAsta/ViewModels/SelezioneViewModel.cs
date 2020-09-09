using System;
using System.Windows.Input;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using FantaAsta.Constants;
using FantaAsta.Enums;
using FantaAsta.Events;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;
using FantaAsta.Views;

namespace FantaAsta.ViewModels
{
	public class SelezioneViewModel : BaseNavigationViewModel
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		#endregion

		#region Properties

		#region Commands

		public DelegateCommand AvviaAstaCommand { get; }
		public DelegateCommand GestisciRoseCommand { get; }
		public DelegateCommand SvuotaRoseCommand { get; }
		public DelegateCommand AggiungiSquadraCommand { get; }
		public DelegateCommand ImportaListaCommand { get; }

		#endregion

		#endregion

		public SelezioneViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService, Lega lega) : base(regionManager, eventAggregator, lega)
		{
			m_dialogService = dialogService;

			_ = m_eventAggregator.GetEvent<ApriFileDialogEvent>().Subscribe(OnApriFileDialog);
			_ = m_eventAggregator.GetEvent<ListaImportataEvent>().Subscribe(OnListaImportata);
			_ = m_eventAggregator.GetEvent<FantaSquadraAggiuntaEvent>().Subscribe(AggiornaComandoAvviaAsta);
			_ = m_eventAggregator.GetEvent<FantaSquadraRimossaEvent>().Subscribe(AggiornaComandoAvviaAsta);

			AvviaAstaCommand = new DelegateCommand(AvviaAsta, AbilitaAvviaAsta);
			GestisciRoseCommand = new DelegateCommand(GestisciRose);
			SvuotaRoseCommand = new DelegateCommand(SvuotaRose);
			AggiungiSquadraCommand = new DelegateCommand(AggiungiSquadra);
			ImportaListaCommand = new DelegateCommand(ImportaLista);
		}

		#region Private methods

		private void NavigateToMain()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(MainView));
		}

		private void NavigateToGestioneRose()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(RoseView));
		}

		#region Event handlers

		private void OnApriFileDialog()
		{
			OpenFileDialog fd = new OpenFileDialog
			{
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				Filter = "CSV Files (*.csv)|*.csv"
			};

			bool? result = fd.ShowDialog();

			if (result.HasValue && result.Value)
			{
				Mouse.OverrideCursor = Cursors.Wait;

				result = m_lega.ImportaLista(fd.FileName);

				if (result.HasValue && !result.Value)
				{
					_ = m_dialogService.ShowMessage("Errore durante l'import della lista", MessageType.Error);
				}
			}
		}

		private void OnListaImportata()
		{
			Mouse.OverrideCursor = Cursors.Arrow;

			_ = m_dialogService.ShowMessage("Lista importata con successo", MessageType.Notification);

			AggiornaComandoAvviaAsta(null);
		}

		private void AggiornaComandoAvviaAsta(FantaSquadraEventArgs args)
		{
			AvviaAstaCommand?.RaiseCanExecuteChanged();
		}

		#endregion

		#region Commands

		private void AvviaAsta()
		{
			NavigateToMain();
		}
		private bool AbilitaAvviaAsta()
		{
			return m_lega.ListaPresente && m_lega.FantaSquadre.Count > 0;
		}

		private void GestisciRose()
		{
			NavigateToGestioneRose();
		}

		private void SvuotaRose()
		{
			m_lega.SvuotaRose();

			_ = m_dialogService.ShowMessage("Rose resettate", MessageType.Notification);
		}

		private void AggiungiSquadra()
		{
			m_dialogService.ShowDialog(CommonConstants.AGGIUNGI_DIALOG, new DialogParameters { { "Type", DialogType.Popup } }, (res) => AggiornaComandoAvviaAsta(null));
		}

		private void ImportaLista()
		{
			m_lega.AvviaImportaLista();
		}

		#endregion

		#endregion
	}
}