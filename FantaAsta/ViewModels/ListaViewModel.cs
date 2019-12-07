using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.EventArgs;

namespace FantaAsta.ViewModels
{
	public class ListaViewModel : BindableBase
	{
		#region Private fields

		private readonly Lega m_mainModel;

		#endregion

		#region Public fields

		public ObservableCollection<Giocatore> Portieri { get; }
		public ObservableCollection<Giocatore> Difensori { get; }
		public ObservableCollection<Giocatore> Centrocampisti { get; }
		public ObservableCollection<Giocatore> Attaccanti { get; }

		#endregion

		public ListaViewModel(Lega mainModel)
		{
			m_mainModel = mainModel;

			Portieri = new ObservableCollection<Giocatore>(m_mainModel.Lista.Where(g => g.Ruolo == Ruoli.P));
			Difensori = new ObservableCollection<Giocatore>(m_mainModel.Lista.Where(g => g.Ruolo == Ruoli.D));
			Centrocampisti = new ObservableCollection<Giocatore>(m_mainModel.Lista.Where(g => g.Ruolo == Ruoli.C));
			Attaccanti = new ObservableCollection<Giocatore>(m_mainModel.Lista.Where(g => g.Ruolo == Ruoli.A));

			m_mainModel.GiocatoreAggiunto += OnGiocatoreAggiunto;
			m_mainModel.GiocatoreRimosso += OnGiocatoreRimosso;
		}

		#region Private methods

		private void OnGiocatoreAggiunto(object sender, GiocatoreEventArgs e)
		{
			SelezionaListaDaRuolo(e.Giocatore.Ruolo).Remove(e.Giocatore);
		}

		private void OnGiocatoreRimosso(object sender, GiocatoreEventArgs e)
		{
			SelezionaListaDaRuolo(e.Giocatore.Ruolo).Add(e.Giocatore);
		}

		private ObservableCollection<Giocatore> SelezionaListaDaRuolo(Ruoli ruolo)
		{
			switch (ruolo)
			{
				case Ruoli.P:
					{
						return Portieri;
					}
				case Ruoli.D:
					{
						return Difensori;
					}
				case Ruoli.C:
					{
						return Centrocampisti;
					}
				case Ruoli.A:
					{
						return Attaccanti;
					}
				default:
					{
						return null;
					}
			}
		}

		#endregion
	}
}
