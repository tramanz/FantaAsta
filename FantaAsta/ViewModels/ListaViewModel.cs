using System.Collections.ObjectModel;
using System.Linq;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.EventArgs;

namespace FantaAsta.ViewModels
{
	public class ListaViewModel : BaseViewModel
	{
		#region Private fields

		private ObservableCollection<Giocatore> m_portieri;
		private ObservableCollection<Giocatore> m_difensori;
		private ObservableCollection<Giocatore> m_centrocampisti;
		private ObservableCollection<Giocatore> m_attaccanti;

		#endregion

		#region Public fields

		public ObservableCollection<Giocatore> Portieri
		{
			get { return m_portieri; }
			private set { SetProperty(ref m_portieri, value); }
		}
		public ObservableCollection<Giocatore> Difensori
		{
			get { return m_difensori; }
			private set { SetProperty(ref m_difensori, value); }
		}
		public ObservableCollection<Giocatore> Centrocampisti
		{
			get { return m_centrocampisti; }
			private set { SetProperty(ref m_centrocampisti, value); }
		}
		public ObservableCollection<Giocatore> Attaccanti
		{
			get { return m_attaccanti; }
			private set { SetProperty(ref m_attaccanti, value); }
		}

		#endregion

		public ListaViewModel(Lega lega) : base(lega)
		{
			InizializzaListe();

			m_lega.GiocatoreAggiunto += OnGiocatoreAggiunto;
			m_lega.GiocatoreRimosso += OnGiocatoreRimosso;
			m_lega.RoseResettate += OnRoseResettate;
			m_lega.ListaImportata += OnListaImportata;
		}

		#region Private methods

		private void OnGiocatoreAggiunto(object sender, GiocatoreAggiuntoEventArgs e)
		{
			SelezionaListaDaRuolo(e.Giocatore.Ruolo).Remove(e.Giocatore);
		}

		private void OnGiocatoreRimosso(object sender, GiocatoreRimossoEventArgs e)
		{
			if (e.Giocatore.InLista)
			{
				ObservableCollection<Giocatore> lista = SelezionaListaDaRuolo(e.Giocatore.Ruolo);
				lista = new ObservableCollection<Giocatore>(m_lega.Svincolati.OrderByDescending(g => g.Quotazione).ThenBy(g => g.Nome));
			}
		}

		private void OnRoseResettate(object sender, System.EventArgs e)
		{
			InizializzaListe();
		}

		private void OnListaImportata(object sender, System.EventArgs e)
		{
			InizializzaListe();
		}

		private void InizializzaListe()
		{
			if (m_lega.ListaPresente)
			{
				Portieri = new ObservableCollection<Giocatore>(m_lega.Svincolati.Where(g => g.Ruolo == Ruoli.P).OrderByDescending(g => g.Quotazione).ThenBy(g => g.Nome));
				Difensori = new ObservableCollection<Giocatore>(m_lega.Svincolati.Where(g => g.Ruolo == Ruoli.D).OrderByDescending(g => g.Quotazione).ThenBy(g => g.Nome));
				Centrocampisti = new ObservableCollection<Giocatore>(m_lega.Svincolati.Where(g => g.Ruolo == Ruoli.C).OrderByDescending(g => g.Quotazione).ThenBy(g => g.Nome));
				Attaccanti = new ObservableCollection<Giocatore>(m_lega.Svincolati.Where(g => g.Ruolo == Ruoli.A).OrderByDescending(g => g.Quotazione).ThenBy(g => g.Nome));
			}
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