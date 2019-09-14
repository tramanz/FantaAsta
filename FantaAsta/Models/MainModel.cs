using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FantaAsta.Models
{
	public class MainModel
	{
		private readonly Random m_random = new Random();

		public ObservableCollection<Giocatore> ListaPortieri { get; private set; }
		public ObservableCollection<Giocatore> ListaDifensori { get; private set; }
		public ObservableCollection<Giocatore> ListaCentrocampisti { get; private set; }
		public ObservableCollection<Giocatore> ListaAttaccanti { get; private set; }

		public List<Squadra> Squadre { get; private set; }

		public MainModel()
		{
			CaricaListe();
			CaricaSquadre();
		}

		private void CaricaListe()
		{
			ListaPortieri = new ObservableCollection<Giocatore>();
			ListaDifensori = new ObservableCollection<Giocatore>();
			ListaCentrocampisti = new ObservableCollection<Giocatore>();
			ListaAttaccanti = new ObservableCollection<Giocatore>();

			using (var reader = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources", "Quotazioni_Fantacalcio.csv")))
			{
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					var values = line.Split(';');

					var giocatore = new Giocatore(Convert.ToInt32(values[0]), (Ruoli)Enum.Parse(typeof(Ruoli), values[1]), values[2], values[3], Convert.ToDouble(values[4]));
					switch (giocatore.Ruolo)
					{
						case Ruoli.P:
							ListaPortieri.Add(giocatore);
							break;
						case Ruoli.D:
							ListaDifensori.Add(giocatore);

							break;
						case Ruoli.C:
							ListaCentrocampisti.Add(giocatore);
							break;
						case Ruoli.A:
							ListaAttaccanti.Add(giocatore);
							break;
						default:
							break;
					}
				}
			}
		}

		private void CaricaSquadre()
		{
			Squadre = new List<Squadra>
			{
				new Squadra("Trama"),
				new Squadra("Busi"),
				new Squadra("Mirco"),
				new Squadra("Tommi"),
				new Squadra("Mac"),
				new Squadra("Sabbi"),
				new Squadra("Sandro"),
				new Squadra("Scudi"),
				new Squadra("Caso"),
				new Squadra("Ciucci")
			};

			Squadre = Squadre.OrderBy(s => s.Nome).ToList();
		}

		public Giocatore EstraiGiocatore(Ruoli ruolo)
		{
			switch (ruolo)
			{
				case Ruoli.P:
					return ListaPortieri[m_random.Next(0, ListaPortieri.Count - 1)];
				case Ruoli.D:
					return ListaDifensori[m_random.Next(0, ListaDifensori.Count - 1)];
				case Ruoli.C:
					return ListaCentrocampisti[m_random.Next(0, ListaCentrocampisti.Count - 1)];
				case Ruoli.A:
					return ListaAttaccanti[m_random.Next(0, ListaAttaccanti.Count - 1)];
				default:
					return null;
			}
		}

		public bool AggiungiGiocatore(Squadra squadra, Giocatore giocatore, double prezzo)
		{
			if (squadra.Budget - prezzo > 0)
			{
				switch (giocatore.Ruolo)
				{
					case Ruoli.P:
						if (squadra.Giocatori.Where(g => g.Ruolo == Ruoli.P).Count() == 3)
						{
							return false;
						}
						else
						{
							giocatore.Prezzo = prezzo;

							squadra.Giocatori.Add(giocatore);
							squadra.Budget -= prezzo;

							ListaPortieri.Remove(giocatore);
							return true;
						}
					case Ruoli.D:
						if (squadra.Giocatori.Where(g => g.Ruolo == Ruoli.D).Count() == 8)
						{
							return false;
						}
						else
						{
							giocatore.Prezzo = prezzo;

							squadra.Giocatori.Add(giocatore);
							squadra.Budget -= prezzo;

							ListaDifensori.Remove(giocatore);
							return true;
						}
					case Ruoli.C:
						if (squadra.Giocatori.Where(g => g.Ruolo == Ruoli.C).Count() == 8)
						{
							return false;
						}
						else
						{
							giocatore.Prezzo = prezzo;

							squadra.Giocatori.Add(giocatore);
							squadra.Budget -= prezzo;

							ListaCentrocampisti.Remove(giocatore);
							return true;
						}
					case Ruoli.A:
						if (squadra.Giocatori.Where(g => g.Ruolo == Ruoli.A).Count() == 6)
						{
							return false;
						}
						else
						{
							giocatore.Prezzo = prezzo;

							squadra.Giocatori.Add(giocatore);
							squadra.Budget -= prezzo;

							ListaAttaccanti.Remove(giocatore);
							return true;
						}
					default:
						return false;
				}
			}
			else
			{
				return false;
			}
		}

		public void RimuoviGiocatore(Squadra squadra, Giocatore giocatore)
		{
			if (squadra.Giocatori.Contains(giocatore))
			{
				squadra.Giocatori.Remove(giocatore);
				squadra.Budget += giocatore.Prezzo;

				giocatore.Prezzo = 0;

				switch (giocatore.Ruolo)
				{
					case Ruoli.P:
						ListaPortieri.Add(giocatore);
						ListaPortieri.OrderByDescending(g => g.Quotazione);
						break;
					case Ruoli.D:
						ListaDifensori.Add(giocatore);
						ListaDifensori.OrderByDescending(g => g.Quotazione);
						break;
					case Ruoli.C:
						ListaCentrocampisti.Add(giocatore);
						ListaCentrocampisti.OrderByDescending(g => g.Quotazione);
						break;
					case Ruoli.A:
						ListaAttaccanti.Add(giocatore);
						ListaAttaccanti.OrderByDescending(g => g.Quotazione);
						break;
					default:
						break;
				}
			}
		}
	}

	public enum Ruoli
	{
		P,
		D,
		C,
		A
	}
}
