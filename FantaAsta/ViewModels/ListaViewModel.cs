using System.Collections.ObjectModel;
using Prism.Mvvm;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class ListaViewModel : BindableBase
	{
		private readonly MainModel m_mainModel;

		public ObservableCollection<Giocatore> Portieri => m_mainModel.ListaPortieri;
		public ObservableCollection<Giocatore> Difensori => m_mainModel.ListaDifensori;
		public ObservableCollection<Giocatore> Centrocampisti => m_mainModel.ListaCentrocampisti;
		public ObservableCollection<Giocatore> Attaccanti => m_mainModel.ListaAttaccanti;

		public ListaViewModel(MainModel mainModel)
		{
			m_mainModel = mainModel;
		}
	}
}
