﻿using System.Collections.ObjectModel;
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

		public ListaViewModel(Lega mainModel)
		{
			m_mainModel = mainModel;

			InizializzaListe();

			m_mainModel.GiocatoreAggiunto += OnGiocatoreAggiunto;
			m_mainModel.GiocatoreRimosso += OnGiocatoreRimosso;
			m_mainModel.RoseResettate += OnRoseResettate;
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

		private void OnRoseResettate(object sender, System.EventArgs e)
		{
			InizializzaListe();
		}

		private void InizializzaListe()
		{
			Portieri = new ObservableCollection<Giocatore>(m_mainModel.Lista.Where(g => g.Ruolo == Ruoli.P));
			Difensori = new ObservableCollection<Giocatore>(m_mainModel.Lista.Where(g => g.Ruolo == Ruoli.D));
			Centrocampisti = new ObservableCollection<Giocatore>(m_mainModel.Lista.Where(g => g.Ruolo == Ruoli.C));
			Attaccanti = new ObservableCollection<Giocatore>(m_mainModel.Lista.Where(g => g.Ruolo == Ruoli.A));
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
