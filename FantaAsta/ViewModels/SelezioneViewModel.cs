using System;
using System.Windows.Input;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.Views;
using FantaAsta.Utilities.Dialogs;

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

		public SelezioneViewModel(IRegionManager regionManager, IDialogService dialogService, Lega lega) : base(regionManager, lega)
		{
			m_dialogService = dialogService;

			m_lega.ApriFileDialog += OnApriFileDialog;
			m_lega.ListaImportata += OnListaImportata;
			m_lega.FantaSquadraAggiunta += AggiornaComandoAvviaAsta;
			m_lega.FantaSquadraRimossa += AggiornaComandoAvviaAsta;

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

		private void OnApriFileDialog(object sender, System.EventArgs e)
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
					m_dialogService.ShowMessage("Errore durante l'import della lista", MessageType.Error);
				}
			}
		}

		private void OnListaImportata(object sender, System.EventArgs e)
		{
			Mouse.OverrideCursor = Cursors.Arrow;

			m_dialogService.ShowMessage("Lista importata con successo", MessageType.Notification);

			AggiornaComandoAvviaAsta(null, null);
		}

		private void AggiornaComandoAvviaAsta(object sender, System.EventArgs e)
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

			m_dialogService.ShowMessage("Rose resettate", MessageType.Notification);
		}

		private void AggiungiSquadra()
		{
			m_dialogService.ShowDialog("Aggiungi", new DialogParameters { { "Type", DialogType.Popup } }, (res) => AggiornaComandoAvviaAsta(null, null));
		}

		private void ImportaLista()
		{
			m_lega.AvviaImportaLista();
		}

		#endregion

		#endregion
	}
}
