using System.Collections.ObjectModel;
using System.Linq;
using Prism.Events;
using FantaAsta.Enums;
using FantaAsta.Models;
using FantaAsta.Events;

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
			private set { _ = SetProperty(ref m_portieri, value); }
		}
		public ObservableCollection<Giocatore> Difensori
		{
			get { return m_difensori; }
			private set { _ = SetProperty(ref m_difensori, value); }
		}
		public ObservableCollection<Giocatore> Centrocampisti
		{
			get { return m_centrocampisti; }
			private set { _ = SetProperty(ref m_centrocampisti, value); }
		}
		public ObservableCollection<Giocatore> Attaccanti
		{
			get { return m_attaccanti; }
			private set { _ = SetProperty(ref m_attaccanti, value); }
		}

		#endregion

		public ListaViewModel(IEventAggregator eventAggregator, Lega lega) : base(eventAggregator, lega)
		{
			InizializzaListe();

			_ = m_eventAggregator.GetEvent<GiocatoreAggiuntoEvent>().Subscribe(OnGiocatoreAggiunto);
			_ = m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Subscribe(OnGiocatoreRimosso);
			_ = m_eventAggregator.GetEvent<RoseResettateEvent>().Subscribe(OnRoseResettate);
			_ = m_eventAggregator.GetEvent<ListaImportataEvent>().Subscribe(OnListaImportata);
		}

		#region Private methods

		private void OnGiocatoreAggiunto(GiocatoreAggiuntoEventArgs args)
		{
			_ = SelezionaListaDaRuolo(args.Giocatore.Ruolo).Remove(args.Giocatore);
		}

		private void OnGiocatoreRimosso(GiocatoreRimossoEventArgs args)
		{
			if (args.Giocatore.InLista)
			{
				ObservableCollection<Giocatore> lista = SelezionaListaDaRuolo(args.Giocatore.Ruolo);
				lista = new ObservableCollection<Giocatore>(m_lega.Svincolati.OrderByDescending(g => g.Quotazione).ThenBy(g => g.Nome));
			}
		}

		private void OnRoseResettate()
		{
			InizializzaListe();
		}

		private void OnListaImportata()
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