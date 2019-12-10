using Prism.Commands;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class SelezioneViewModel
	{
		#region Private fields

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

		public SelezioneViewModel(Lega lega)
		{
			m_lega = lega;

			AstaEstivaCommand = new DelegateCommand(AvviaAstaEstiva);
			AstaInvernaleCommand = new DelegateCommand(AvviaAstaInvernale);
			GestisciRoseCommand = new DelegateCommand(GestisciRose);
			SvuotaRoseCommand = new DelegateCommand(SvuotaRose);
			ImportaListaCommand = new DelegateCommand(ImportaLista);
		}

		#region Private methods

		#region Commands

		private void AvviaAstaEstiva()
		{

		}

		private void AvviaAstaInvernale()
		{

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
