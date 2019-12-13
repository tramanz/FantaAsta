using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using FantaAsta.Models;
using FantaAsta.Views;

namespace FantaAsta.ViewModels
{
	public class SelezioneViewModel
	{
		#region Private fields

		private readonly SynchronizationContext m_syncContext;

		private readonly IRegionManager m_regionManager;

		private readonly IDialogService m_dialogService;

		private readonly Lega m_lega;

		#endregion

		#region Properties

		#region Commands

		public DelegateCommand AstaEstivaCommand { get; }
		public DelegateCommand AstaInvernaleCommand { get; }
		public DelegateCommand GestisciRoseCommand { get; }
		public DelegateCommand SvuotaRoseCommand { get; }
		public DelegateCommand AggiungiSquadraCommand { get; }
		public DelegateCommand ImportaListaCommand { get; }

		#endregion

		#endregion

		public SelezioneViewModel(IRegionManager regionManager, IDialogService dialogService, Lega lega)
		{
			m_syncContext = SynchronizationContext.Current;

			m_regionManager = regionManager;
			m_dialogService = dialogService;

			m_lega = lega;
			m_lega.ApriFileDialog += OnApriFileDialog;
			m_lega.ListaImportata += OnListaImportata;

			AstaEstivaCommand = new DelegateCommand(AvviaAstaEstiva, AbilitaAvviaAsta);
			AstaInvernaleCommand = new DelegateCommand(AvviaAstaInvernale, AbilitaAvviaAsta);
			GestisciRoseCommand = new DelegateCommand(GestisciRose);
			SvuotaRoseCommand = new DelegateCommand(SvuotaRose);
			AggiungiSquadraCommand = new DelegateCommand(AggiungiSquadra);
			ImportaListaCommand = new DelegateCommand(ImportaLista);
		}

		#region Private methods

		private void NavigateToMain(NavigationParameters parameters)
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(MainView), parameters);
		}

		private void NavigateToGestioneRose()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(RoseView), new NavigationParameters { { "Modalità", "Gestione rose" } });
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
				m_syncContext.Send(new SendOrPostCallback((obj) => Mouse.OverrideCursor = Cursors.Wait), null);

				result = m_lega.ImportaLista(fd.FileName);

				if (result.HasValue && !result.Value)
				{
					m_syncContext.Send(new SendOrPostCallback((obj) =>
					{
						MessageBox.Show(Application.Current.MainWindow, "Errore durante l'import della lista", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
					}), null);
				}
			}
		}

		private void OnListaImportata(object sender, System.EventArgs e)
		{
			m_syncContext.Send(new SendOrPostCallback((obj) =>
			{
				Mouse.OverrideCursor = Cursors.Arrow;

				MessageBox.Show(Application.Current.MainWindow, "Lista importata con successo", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
			}), null);

			AstaEstivaCommand?.RaiseCanExecuteChanged();
			AstaInvernaleCommand?.RaiseCanExecuteChanged();
		}

		#endregion

		#region Commands

		private void AvviaAstaEstiva()
		{
			NavigateToMain(new NavigationParameters { { "Modalità", "Asta estiva" } });
		}

		private void AvviaAstaInvernale()
		{
			NavigateToMain(new NavigationParameters { { "Modalità", "Asta invernale" } });
		}

		private bool AbilitaAvviaAsta()
		{
			return m_lega.ListaPresente;
		}

		private void GestisciRose()
		{
			NavigateToGestioneRose();
		}

		private void SvuotaRose()
		{
			m_lega.SvuotaRose();

			MessageBox.Show("Rose resettate", "OPERAZIONE COMPLETATA", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void AggiungiSquadra()
		{
			m_dialogService.ShowDialog("Aggiungi", null, (r1) =>
			{
				if (r1 is AggiungiSquadraResult r2)
				{
					if (r2.Result == ButtonResult.Yes)
					{
						bool r3 = m_lega.AggiungiSquadra(r2.Nome);

						if (r3)
						{
							MessageBox.Show("Squadra aggiunta", "OPERAZIONE COMPLETATA", MessageBoxButton.OK, MessageBoxImage.Information);
						}
						else
						{
							MessageBox.Show("Non è possibile aggiungere una squadra con lo stesso nome di una già esistente", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
				}
			});
		}

		private void ImportaLista()
		{
			if (m_lega.FantaSquadre.Where(s => s.Giocatori.Count() > 0).Count() > 0)
			{
				MessageBoxResult result = MessageBoxResult.None;
				m_syncContext.Send(new SendOrPostCallback((obj) =>
				{
					result = MessageBox.Show(Application.Current.MainWindow, "L'import di una nuova lista richiede di resettare le rose. Continuare?", "ATTENZIONE", MessageBoxButton.YesNo, MessageBoxImage.Question);
				}), null);

				if (result == MessageBoxResult.Yes)
				{
					m_lega.AvviaImportaLista();
				}
			}
			else
			{ 
				m_lega.AvviaImportaLista();
			}
		}

		#endregion

		#endregion
	}
}
