using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FantaAsta.Enums;
using FantaAsta.EventArgs;

namespace FantaAsta.Models
{
	public class Lega
	{
		#region Private fields

		private readonly Random m_random = new Random();

		#endregion

		#region Properties

		public List<Giocatore> Lista { get; private set; }

		public List<FantaSquadra> FantaSquadre { get; private set; }

		#endregion

		#region Events

		public event EventHandler<GiocatoreEventArgs> GiocatoreAggiunto;

		public event EventHandler<GiocatoreEventArgs> GiocatoreRimosso;

		#endregion

		public Lega()
		{
			CaricaListe();

			CaricaFantaSquadre();
		}

		#region Public methods

		/// <summary>
		/// Metodo per estrarre i giocatori di un ruolo specificato dalla lista.
		/// La probabilità di estrarre giocatori più quotati rispetto a quelli meno quotati è di 5:1.
		/// </summary>
		/// <param name="ruolo">Il ruolo specificato.</param>
		/// <returns>Il giocatore estratto.</returns>
		public Giocatore EstraiGiocatore(Ruoli ruolo)
		{
			// Ricavo dalla lista completa i giocatori del ruolo specificato e ordino per quotazione decrescente
			List<Giocatore> listaRuolo = GeneraListaPerRuolo(Lista, ruolo).OrderByDescending(g => g.Quotazione).ToList();

			// Se non sono rimasti giocatori del ruolo speciicato, ritorno null
			if (Lista.Count == 0)
			{
				return null;
			}

			// Se in lista sono rimasti meno di 5 giocatori del ruolo specificato, ritorno un giocatore con probabilità uniforme
			if (listaRuolo.Count < 5)
			{
				return listaRuolo[m_random.Next(0, listaRuolo.Count - 1)];
			}

			// Ricavo il numero di giocatori totali e il numero di giocatori per intervallo
			int numTot = listaRuolo.Count;
			int numPerInt = (int)Math.Floor((double)(numTot / 5));

			// Costruisco la lista degli indici per dividere i giocatori del ruolo specificato in 5 intervalli
			List<Tuple<int, int>> listaIndInt = new List<Tuple<int, int>>
			{
				new Tuple<int, int>(0, numPerInt - 1),
				new Tuple<int, int>(numPerInt,  2 * numPerInt - 1),
				new Tuple<int, int>(2 * numPerInt, 3 * numPerInt - 1),
				new Tuple<int, int>(3 * numPerInt, 4 * numPerInt - 1),
				new Tuple<int, int>(4 * numPerInt, numTot - 1)
			};

			// Costruisco la struttura per memorizzare il numero di volte che un intervallo viene estratto
			List<Tuple<Tuple<int, int>, int>> listaOccInt = new List<Tuple<Tuple<int, int>, int>>
			{
				new Tuple<Tuple<int, int>, int>(listaIndInt[0], 0),
				new Tuple<Tuple<int, int>, int>(listaIndInt[1], 0),
				new Tuple<Tuple<int, int>, int>(listaIndInt[2], 0),
				new Tuple<Tuple<int, int>, int>(listaIndInt[3], 0),
				new Tuple<Tuple<int, int>, int>(listaIndInt[4], 0)
			};

			// Continuo ad estrarre un indice, corrispondente ad un intervallo, finché:
			// - il primo intervallo non capita una volta, oppure
			// - il secondo non capita due volte, oppure
			// - il terzo non capita tre volte, oppure
			// - il quarto non capita quattro volte, oppure
			// - il quinto non capita cinque volte
			int random; int indInt = 0;
			while (listaOccInt[0].Item2 < 1 && listaOccInt[1].Item2 < 2 && listaOccInt[2].Item2 < 3 && listaOccInt[3].Item2 < 4 && listaOccInt[4].Item2 < 5)
			{
				// Estraggo un indice da quelli della lista completa
				random = m_random.Next(0, numTot - 1);

				// Ricavo l'intervallo in cui ricade l'indice e aggiorno le sue occorrenze
				indInt = listaIndInt.IndexOf(listaIndInt.Find(i => random >= i.Item1 && random <= i.Item2));
				listaOccInt[indInt] = new Tuple<Tuple<int, int>, int>(listaIndInt[indInt], listaOccInt[indInt].Item2 + 1);
			}

			// Ricavo l'intervallo estratto
			Tuple<int, int> intSel = listaIndInt[indInt];

			// Ricavo i giocatori nell'intervallo estratto dalla lista
			List<Giocatore> listaRuoloInt = listaRuolo.GetRange(intSel.Item1, intSel.Item2 - intSel.Item1 + 1);

			// Ritorno un giocatore dall'intervallo con probabilità uniforme
			return listaRuoloInt[m_random.Next(0, listaRuoloInt.Count - 1)];
		}

		/// <summary>
		/// Metodo per aggiungere un giocatore acquistato ad una fantasquadra.
		/// </summary>
		/// <param name="squadra">La fantasquadra dove aggiungere il giocatore.</param>
		/// <param name="giocatore">Il giocatore da aggiungere.</param>
		/// <param name="prezzo">Il prezzo di acquisto del giocatore.</param>
		/// <returns>True se l'operazione può essere effettuata, false altrimenti.</returns>
		public bool AggiungiGiocatore(FantaSquadra squadra, Giocatore giocatore, double prezzo)
		{
			// Definisco il numero massimo di giocatori per il ruolo del giocatore
			int maximum = giocatore.Ruolo == Ruoli.P ? 3 : giocatore.Ruolo == Ruoli.D ? 8 : giocatore.Ruolo == Ruoli.C ? 8 : 6;

			// Se la fantasquadra non ha abbastanza soldi o non ha spazio in rosa, l'operazione non si può effettuare
			if (squadra.Budget - prezzo <= 0 || GeneraListaPerRuolo(squadra.Giocatori, giocatore.Ruolo).Count() == maximum)
				return false;

			// Memorizzo il prezzo d'acquisto del giocatore
			giocatore.Prezzo = prezzo;

			// Aggiungo il giocatore alla fantasquadra
			squadra.AggiungiGiocatore(giocatore);

			// Rimuovo il giocatore dalla lista
			Lista.Remove(giocatore);

			// Segnalo alla view l'operazione
			GiocatoreAggiunto?.Invoke(this, new GiocatoreEventArgs(giocatore, squadra));

			return true;
		}

		/// <summary>
		/// Metodo per rimuovere un giocatore dalla rosa di una fantasquadra.
		/// </summary>
		/// <param name="squadra">La fantasquadra da cui rimuovere il giocatore.</param>
		/// <param name="giocatore">Il giocatore da rimuovere.</param>
		public void RimuoviGiocatore(FantaSquadra squadra, Giocatore giocatore)
		{
			// Se il giocatore è nella rosa della fantasquadra
			if (squadra.Giocatori.Contains(giocatore))
			{
				// Rimuovo il giocatore
				squadra.RimuoviGiocatore(giocatore);

				// Azzero il prezzo d'acquisto del giocatore
				giocatore.Prezzo = 0;

				// Riaggiungo il giocatore alla lista
				Lista.Add(giocatore);

				// Segnalo alla view l'operazione
				GiocatoreRimosso?.Invoke(this, new GiocatoreEventArgs(giocatore, squadra));
			}
		}

		#endregion

		#region Private methods

		private void CaricaListe()
		{
			Lista = new List<Giocatore>();

			using (var reader = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources", "Quotazioni_Fantacalcio.csv")))
			{
				while (!reader.EndOfStream)
				{
					string[] data = reader.ReadLine().Split(';');

					Lista.Add(new Giocatore(Convert.ToInt32(data[0]), (Ruoli)Enum.Parse(typeof(Ruoli), data[1]), data[2], data[3], Convert.ToDouble(data[4])));
				}
			}
		}

		private void CaricaFantaSquadre()
		{
			FantaSquadre = new List<FantaSquadra>
			{
				new FantaSquadra("Trama"),
				new FantaSquadra("Busi"),
				new FantaSquadra("Mirco"),
				new FantaSquadra("Tommi"),
				new FantaSquadra("Mac"),
				new FantaSquadra("Sabbi"),
				new FantaSquadra("Sandro"),
				new FantaSquadra("Scudi"),
				new FantaSquadra("Caso"),
				new FantaSquadra("Ciucci")
			}.OrderBy(s => s.Nome).ToList();
		}

		private IEnumerable<Giocatore> GeneraListaPerRuolo(IEnumerable<Giocatore> lista, Ruoli ruolo)
		{
			return lista.Where(g => g.Ruolo == ruolo);
		}

		#endregion
	}
}
