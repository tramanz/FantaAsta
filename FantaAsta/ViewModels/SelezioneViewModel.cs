﻿using System;
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

		public SelezioneViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService, Asta asta) : base(regionManager, eventAggregator, asta)
		{
			m_dialogService = dialogService;

			_ = m_eventAggregator.GetEvent<ApriFileDialogEvent>().Subscribe(OnApriFileDialog);
			_ = m_eventAggregator.GetEvent<ListaImportataEvent>().Subscribe(OnListaImportata);

			AvviaAstaCommand = new DelegateCommand(AvviaAsta);
			GestisciRoseCommand = new DelegateCommand(GestisciRose);
			SvuotaRoseCommand = new DelegateCommand(SvuotaRose);
			AggiungiSquadraCommand = new DelegateCommand(AggiungiSquadra);
			ImportaListaCommand = new DelegateCommand(ImportaLista);
		}

		#region Private methods

		private void NavigateToMain()
		{
			m_regionManager.RequestNavigate(CommonConstants.MAIN_REGION, nameof(MainView));
		}

		private void NavigateToGestioneRose()
		{
			m_regionManager.RequestNavigate(CommonConstants.MAIN_REGION, nameof(RoseView));
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

				result = m_asta.ImportaLista(fd.FileName);

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
		}

		#endregion

		#region Commands

		private void AvviaAsta()
		{
			if (!m_asta.Preferenze.PreferenzeImpostate)
			{
				MostraPreferenze();
			}

			if (m_asta.Preferenze.PreferenzeImpostate)
			{
				if (!m_asta.ListaPresente || m_asta.DatiAsta.FantaSquadre.Count == 0)
				{
					m_dialogService.ShowMessage("Per avviare l'asta importare la lista e aggiungere almeno una fantasquadra", MessageType.Error);
				}
				else
				{
					NavigateToMain();
				}
			}
		}

		private void GestisciRose()
		{
			if (!m_asta.Preferenze.PreferenzeImpostate)
			{
				MostraPreferenze();
			}

			if (m_asta.Preferenze.PreferenzeImpostate)
			{
				NavigateToGestioneRose();
			}
		}

		private void SvuotaRose()
		{
			m_asta.SvuotaRose();

			_ = m_dialogService.ShowMessage("Rose resettate", MessageType.Notification);
		}

		private void AggiungiSquadra()
		{
			m_dialogService.ShowDialog(CommonConstants.AGGIUNGI_DIALOG, new DialogParameters(), null);
		}

		private void ImportaLista()
		{
			m_asta.AvviaImportaLista();
		}

		private void MostraPreferenze()
		{
			_ = m_dialogService.ShowMessage("Le preferenze non sono ancora state impostate", MessageType.Error);

			m_dialogService.ShowDialog(CommonConstants.PREFERENZE_DIALOG, new DialogParameters(), null);
		}

		#endregion

		#endregion
	}
}