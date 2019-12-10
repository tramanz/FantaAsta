﻿using Prism.Commands;
using Prism.Regions;
using FantaAsta.Models;
using FantaAsta.Views;

namespace FantaAsta.ViewModels
{
	public class SelezioneViewModel
	{
		#region Private fields

		private readonly IRegionManager m_regionManager;
		private readonly Lega m_lega;

		#endregion

		#region Properties

		#region Commands

		public DelegateCommand AstaEstivaCommand { get; }
		public DelegateCommand AstaInvernaleCommand { get; }
		public DelegateCommand GestisciRoseCommand { get; }
		public DelegateCommand SvuotaRoseCommand { get; }
		public DelegateCommand ImportaListaCommand { get; }

		#endregion

		#endregion

		public SelezioneViewModel(IRegionManager regionManager, Lega lega)
		{
			m_regionManager = regionManager;

			m_lega = lega;

			AstaEstivaCommand = new DelegateCommand(AvviaAstaEstiva);
			AstaInvernaleCommand = new DelegateCommand(AvviaAstaInvernale);
			GestisciRoseCommand = new DelegateCommand(GestisciRose);
			SvuotaRoseCommand = new DelegateCommand(SvuotaRose);
			ImportaListaCommand = new DelegateCommand(ImportaLista);
		}

		#region Private methods

		private void NavigateToMain()
		{
			m_regionManager.RequestNavigate("MainRegion", nameof(MainView));
		}

		#region Commands

		private void AvviaAstaEstiva()
		{
			NavigateToMain();
		}

		private void AvviaAstaInvernale()
		{
			NavigateToMain();
		}

		private void GestisciRose()
		{

		}

		private void SvuotaRose()
		{

		}

		private void ImportaLista()
		{

		}

		#endregion

		#endregion
	}
}
