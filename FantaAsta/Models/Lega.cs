using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FantaAsta.Enums;
using FantaAsta.EventArgs;
using FantaAsta.Utilities;

namespace FantaAsta.Models
{
	[DataContract(Name = "FantaLega", Namespace = "")]
	public class Lega
	{
		#region Private fields

		private readonly Random m_random = new Random();

		#endregion

		#region Properties

		public List<Giocatore> Lista { get; private set; }

		[DataMember(Name = "FantaSquadre")]
		public List<FantaSquadra> FantaSquadre { get; set; }

		public bool IsAstaInvernale { get; private set; }

		#endregion

		#region Events

		public event EventHandler<GiocatoreEventArgs> GiocatoreAggiunto;

		public event EventHandler<GiocatoreEventArgs> GiocatoreRimosso;

		public event EventHandler ModalitàAstaInvernale;

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
		/// Metodo per rimuovere un giocatore dalla rosa di una fantasquadra
		/// </summary>
		/// <param name="squadra">La fantasquadra da cui rimuovere il giocatore</param>
		/// <param name="giocatore">Il giocatore da rimuovere</param>
		/// <param name="prezzo">Il prezzo di vendita</param>
		public void RimuoviGiocatore(FantaSquadra squadra, Giocatore giocatore, double prezzo)
		{
			// Se il giocatore è nella rosa della fantasquadra
			if (squadra.Giocatori.Contains(giocatore))
			{
				// Rimuovo il giocatore
				squadra.RimuoviGiocatore(giocatore, prezzo);

				// Azzero il prezzo d'acquisto del giocatore
				giocatore.Prezzo = 0;

				// Riaggiungo il giocatore alla lista
				Lista.Add(giocatore);

				// Segnalo alla view l'operazione
				GiocatoreRimosso?.Invoke(this, new GiocatoreEventArgs(giocatore, squadra));
			}
		}

		/// <summary>
		/// Metodo per salvare i dati riguardanti le fantasquadre
		/// </summary>
		public void SalvaSquadre()
		{
			if (!Directory.Exists(Constants.DATA_DIRECTORY_PATH))
			{
				Directory.CreateDirectory(Constants.DATA_DIRECTORY_PATH);
			}

			if (File.Exists(Constants.DATA_FILE_PATH))
			{
				File.Delete(Constants.DATA_FILE_PATH);
			}

			DataContractSerializer dcs = new DataContractSerializer(typeof(Lega));

			using (FileStream fs = new FileStream(Constants.DATA_FILE_PATH, FileMode.OpenOrCreate))
			using (XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateTextWriter(fs, Encoding.UTF8))
			{
				dcs.WriteObject(xdw, this);
			}
		}

		public void AvviaAstaInvernale()
		{
			IsAstaInvernale = true;

			foreach(FantaSquadra squadra in FantaSquadre)
			{
				squadra.Budget += 100;
			}

			ModalitàAstaInvernale?.Invoke(this, System.EventArgs.Empty);
		}

		public void TerminaAstaInvernale()
		{
			IsAstaInvernale = false;

			foreach (FantaSquadra squadra in FantaSquadre)
			{
				squadra.Budget -= 100;
			}

			ModalitàAstaInvernale?.Invoke(this, System.EventArgs.Empty);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Metodo per caricare la lista dei giocatori
		/// </summary>
		private void CaricaListe()
		{
			Lista = new List<Giocatore>();

			using (var reader = new StreamReader(Constants.RAW_DATA_FILE_PATH))
			{
				while (!reader.EndOfStream)
				{
					string[] data = reader.ReadLine().Split(';');

					Lista.Add(new Giocatore(Convert.ToInt32(data[0]), (Ruoli)Enum.Parse(typeof(Ruoli), data[1]), data[2], data[3], Convert.ToDouble(data[4])));
				}
			}
		}

		/// <summary>
		/// Metodo per caricare le fantasquadre
		/// </summary>
		private void CaricaFantaSquadre()
		{
			if (File.Exists(Constants.DATA_FILE_PATH))
			{
				CaricaSquadraDaFile();
			}
			else
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
		}

		/// <summary>
		/// Metodo per caricare le fantasquadre dai dati salvati
		/// </summary>
		private void CaricaSquadraDaFile()
		{
			FantaSquadre = new List<FantaSquadra>();

			using (FileStream fs = new FileStream(Constants.DATA_FILE_PATH, FileMode.Open))
			{
				DataContractSerializer ser = new DataContractSerializer(typeof(Lega));

				XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

				FantaSquadre = new List<FantaSquadra>(((Lega)ser.ReadObject(reader)).FantaSquadre);
			}
		}

		/// <summary>
		/// Metodo per generare la lista dei giocatori del ruolo specificato a partire da un'altra lista
		/// </summary>
		/// <param name="lista">La lista da cui estrarre la lista dei giocatori per ruolo</param>
		/// <param name="ruolo">Il ruolo specificato</param>
		/// <returns>La lista dei giocaori del ruolo specificato</returns>
		private IEnumerable<Giocatore> GeneraListaPerRuolo(IEnumerable<Giocatore> lista, Ruoli ruolo)
		{
			return lista.Where(g => g.Ruolo == ruolo);
		}

		#endregion
	}
}
