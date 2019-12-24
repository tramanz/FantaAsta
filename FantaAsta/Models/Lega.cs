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

		[DataMember(Name = "FantaSquadre")]
		public List<FantaSquadra> FantaSquadre { get; set; }

		[DataMember(Name = "PercorsoFileLista")]
		public string PercorsoFileLista { get; set; }

		public List<Giocatore> Lista { get; private set; }

		public List<Giocatore> Svincolati { get; private set; }

		public bool ListaPresente { get; private set; }

		public bool IsAstaInvernale { get; private set; }

		public double QuotazioneMedia { get; private set; }

		#endregion

		#region Events

		public event EventHandler<GiocatoreAggiuntoEventArgs> GiocatoreAggiunto;
		public event EventHandler<GiocatoreRimossoEventArgs> GiocatoreRimosso;
		public event EventHandler<FantaSquadraEventArgs> FantaSquadraAggiunta;
		public event EventHandler<FantaSquadraEventArgs> FantaSquadraRimossa;
		public event EventHandler RoseResettate;
		public event EventHandler ModalitàAstaInvernale;
		public event EventHandler ApriFileDialog;
		public event EventHandler ListaImportata;

		#endregion

		public Lega()
		{
			CaricaLista();

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
			// La probabilità di un giocatore di essere estratto (a meno di un fattore correttivo per i giocatori meno quotati, dovuto alla divisione intera) è:
			//           1
			// p = --------------,
			//           (k - 1)
			//      N * 5
			// dove N è il numero di giocatori nella lista del ruolo specificato e k = 1, 2, ..., 5 è l'indice del gruppo in cui è presente il giocatore.
			// I giocatori in lista vengono ordinati in modo decrescente e divisi in 5 gruppi di cardinalità K = [N/5] (ad eccezione dell'ultimo, pari a K = N - 4*[N/5]).
			// In questo modo si rende più probabile l'estrazione di un giocatore più quotato rispetto ad uno meno quotato,
			// mantenendo al contempo la probabilità uniforme all'interno di uno stesso gruppo.

			// Ricavo dalla lista i giocatori del ruolo specificato e ordino per quotazione decrescente
			List<Giocatore> listaRuolo = GeneraListaPerRuolo(Svincolati, ruolo).OrderByDescending(g => g.Quotazione).ThenBy(g => g.Nome).ToList();

			// Ricavo il numero N di giocatori totali e il numero K di giocatori per intervallo
			int N = listaRuolo.Count;
			int K = (int)Math.Floor((double)(N / 5));

			// Se non sono presenti giocatori del ruolo specificato, ritorno null
			if (N == 0)
				return null;

			// Se in lista sono rimasti meno di 5 giocatori del ruolo specificato, ritorno un giocatore con probabilità uniforme
			if (N < 5)
				return listaRuolo[m_random.Next(0, N - 1)];

			// Costruisco la lista degli intervalli
			List<Intervallo> estrazioni = new List<Intervallo>
			{
				new Intervallo(0, K - 1),
				new Intervallo(K,  2 * K - 1),
				new Intervallo(2 * K, 3 * K - 1),
				new Intervallo(3 * K, 4 * K - 1),
				new Intervallo(4 * K, N - 1)
			};

			// Continuo ad estrarre un indice finché la condizione d'uscita non è soddisfatta
			bool flag = true;
			int n; int k = 0;
			while (flag)
			{
				// Estraggo un indice dalla lista
				n = m_random.Next(0, N - 1);

				// Ricavo l'indice dell'intervallo in cui ricade l'indice estratto
				k = estrazioni.IndexOf(estrazioni.Find(i => n >= i.Inf && n <= i.Sup));

				// Aggiorno il numero di volte che l'intervallo è stato estratto
				estrazioni[k].Estrazioni++;

				// Aggiorno la condizione d'uscita
				flag = estrazioni[k].Estrazioni < k + 1;
			}

			// Ricavo l'intervallo estratto
			List<Giocatore> intervallo = listaRuolo.GetRange(estrazioni[k].Inf, estrazioni[k].Dim);

			// Estraggo con probabilità uniforme un indice dall'intervallo estratto
			k = m_random.Next(0, estrazioni[k].Dim - 1);

			// Ritorno il giocatore corrispondente all'indice estratto
			return intervallo[k];
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
			if (squadra.Budget - prezzo < 0 || GeneraListaPerRuolo(squadra.Giocatori, giocatore.Ruolo).Count() == maximum)
				return false;

			// Memorizzo il prezzo d'acquisto del giocatore
			giocatore.Prezzo = prezzo;

			// Aggiungo il giocatore alla fantasquadra
			squadra.AggiungiGiocatore(giocatore);

			// Rimuovo il giocatore dalla lista degli svincolati
			Svincolati.Remove(giocatore);

			// Segnalo alla view l'operazione
			GiocatoreAggiunto?.Invoke(this, new GiocatoreAggiuntoEventArgs(giocatore, squadra, prezzo));

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

				// Riaggiungo il giocatore alla lista degli svincolati se è nella lista completa
				if (giocatore.InLista)
				{
					Svincolati.Add(giocatore);
				}

				// Segnalo alla view l'operazione
				GiocatoreRimosso?.Invoke(this, new GiocatoreRimossoEventArgs(giocatore, squadra, prezzo));
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
			using (XmlWriter xdw = XmlWriter.Create(fs))
			{
				dcs.WriteObject(xdw, this);
			}
		}

		/// <summary>
		/// Metodo per avviare la modalità asta invernale aggiungendo 100 fantamilioni al budget di ogni squadra
		/// </summary>
		public void AvviaAstaInvernale()
		{
			IsAstaInvernale = true;

			foreach (FantaSquadra squadra in FantaSquadre)
			{
				squadra.Budget += Constants.BUDGET_INVERNALE;
			}

			ModalitàAstaInvernale?.Invoke(this, System.EventArgs.Empty);
		}

		/// <summary>
		/// Metodo per terminare la modalità asta invernale rimuovendo 100 fantamilioni dal budget di ogni squadra
		/// </summary>
		public void TerminaAstaInvernale()
		{
			IsAstaInvernale = false;

			foreach (FantaSquadra squadra in FantaSquadre)
			{
				squadra.Budget -= Constants.BUDGET_INVERNALE;
			}

			ModalitàAstaInvernale?.Invoke(this, System.EventArgs.Empty);
		}

		/// <summary>
		/// Metodo per resettare le rose
		/// </summary>
		public void SvuotaRose()
		{
			foreach (FantaSquadra squadra in FantaSquadre)
			{
				foreach (Giocatore giocatore in squadra.Giocatori)
				{
					giocatore.Prezzo = 0;

					if (Lista.Contains(giocatore))
					{
						Svincolati.Add(giocatore);
					}
				}

				squadra.ValoreMedio = 0;
				squadra.Budget = Constants.BUDGET_ESTIVO;
				squadra.Giocatori.Clear();
			}

			RoseResettate?.Invoke(this, System.EventArgs.Empty);
		}

		/// <summary>
		/// Metodo per avvia l'import di una lista di giocatori
		/// </summary>
		public void AvviaImportaLista()
		{
			ApriFileDialog?.Invoke(this, System.EventArgs.Empty);
		}

		/// <summary>
		/// Metodo per aggiungere una fantasquadra alla lega
		/// </summary>
		/// <param name="nome">Il nome della fantasquadra</param>
		public bool AggiungiSquadra(string nome)
		{
			if (FantaSquadre.Select(s => s.Nome).Contains(nome))
				return false;

			FantaSquadra squadra = new FantaSquadra(nome);

			FantaSquadre.Add(squadra);
			FantaSquadre.OrderBy(s => s.Nome);

			FantaSquadraAggiunta?.Invoke(this, new FantaSquadraEventArgs(squadra));

			return true;
		}

		/// <summary>
		/// Metodo per rimuovere una fantasquadra dalla lega
		/// </summary>
		/// <param name="nome">Il nome della fantasquadra</param>
		public void RimuoviSquadra(string nome)
		{
			FantaSquadra squadra = FantaSquadre.Where(s => s.Nome.Equals(nome)).SingleOrDefault();

			if (squadra != null)
			{
				foreach (Giocatore giocatore in squadra.Giocatori)
				{
					giocatore.Prezzo = 0;

					Lista.Add(giocatore);

					GiocatoreRimosso?.Invoke(this, new GiocatoreRimossoEventArgs(giocatore, squadra, giocatore.Prezzo));
				}

				FantaSquadre.Remove(squadra);

				FantaSquadraRimossa?.Invoke(this, new FantaSquadraEventArgs(squadra));
			}
		}

		/// <summary>
		/// Metodo per importare una lista di giocatori
		/// </summary>
		/// <param name="filePath">Il percorso del file .csv da cui ricavare la lista</param>
		/// <returns>True se l'operazione è andata a buon fine, false altrimenti</returns>
		public bool ImportaLista(string filePath)
		{
			return CaricaListaDaFile(filePath, false);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Metodo per caricare la lista di giocatori
		/// </summary>
		private void CaricaLista()
		{
			if (File.Exists(Constants.DATA_FILE_PATH))
			{
				using (FileStream fs = new FileStream(Constants.DATA_FILE_PATH, FileMode.Open))
				{
					DataContractSerializer ser = new DataContractSerializer(typeof(Lega));

					XmlReader reader = XmlReader.Create(fs);

					PercorsoFileLista = ((Lega)ser.ReadObject(reader)).PercorsoFileLista;
				}
			}

			CaricaListaDaFile(PercorsoFileLista, true);
		}

		/// <summary>
		/// Metodo per caricare la lista dei giocatori da un file .csv
		/// </summary>
		/// <param name="filePath">Percorso del file .csv della lista</param>
		/// <param name="isLoadingAtStart">Indica se il caricamento è partito da un richiesta automatica all'avvio oppure dall'utentemo</param>
		/// <returns>True se l'operazione è andata a buon fine, false altrimenti</returns>
		private bool CaricaListaDaFile(string filePath, bool isLoadingAtStart)
		{
			Lista = new List<Giocatore>();
			Svincolati = new List<Giocatore>();

			if (!Directory.Exists(Constants.DATA_DIRECTORY_PATH))
			{
				Directory.CreateDirectory(Constants.DATA_DIRECTORY_PATH);
			}

			if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
			{
				return false;
			}

			if (!isLoadingAtStart)
			{
				try
				{
					PercorsoFileLista = Path.Combine(Constants.DATA_DIRECTORY_PATH, $"Quotazioni ({DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}).csv");
					File.Copy(filePath, PercorsoFileLista, true);
				}
				catch
				{
					return false;
				}
			}

			using (var reader = new StreamReader(PercorsoFileLista))
			{
				Giocatore giocatore;
				while (!reader.EndOfStream)
				{
					string[] data = reader.ReadLine().Split(';');

					giocatore = new Giocatore(Convert.ToInt32(data[0]), (Ruoli)Enum.Parse(typeof(Ruoli), data[1]), data[2], data[3], Convert.ToDouble(data[4]));

					Lista.Add(giocatore);
					Svincolati.Add(giocatore);
				}
			}

			ListaPresente = true;

			AggiornaQuotazioneMedia();

			AggiornaStatoGiocatoriInRosa();

			ListaImportata?.Invoke(this, System.EventArgs.Empty);

			return true;
		}

		/// <summary>
		/// Metodo per caricare le fantasquadre
		/// </summary>
		private void CaricaFantaSquadre()
		{
			FantaSquadre = new List<FantaSquadra>();

			if (File.Exists(Constants.DATA_FILE_PATH))
			{
				CaricaFantaSquadreDaFile();
			}
		}

		/// <summary>
		/// Metodo per caricare le fantasquadre dai dati salvati
		/// </summary>
		private void CaricaFantaSquadreDaFile()
		{
			using (FileStream fs = new FileStream(Constants.DATA_FILE_PATH, FileMode.Open))
			{
				DataContractSerializer ser = new DataContractSerializer(typeof(Lega));

				XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

				FantaSquadre = new List<FantaSquadra>(((Lega)ser.ReadObject(reader)).FantaSquadre);
			}

			AggiornaStatoGiocatoriInRosa();
		}

		/// <summary>
		/// Metodo che controlla se i giocatori nelle rose sono presenti in lista e aggiorna la lista degli svincolati
		/// </summary>
		private void AggiornaStatoGiocatoriInRosa()
		{
			if (FantaSquadre != null)
			{
				foreach (FantaSquadra squadra in FantaSquadre)
				{
					foreach (Giocatore giocatore in squadra.Giocatori)
					{
						giocatore.InLista = Lista.Contains(giocatore);

						if (Svincolati.Contains(giocatore))
						{
							Svincolati.Remove(giocatore);
						}
					}
				}

				RoseResettate?.Invoke(this, System.EventArgs.Empty);
			}
		}

		/// <summary>
		/// Metodo per calcolare la quotazione media di tutti i giocatori in lista
		/// </summary>
		private void AggiornaQuotazioneMedia()
		{
			if (Lista != null)
			{
				double sum = 0;
				foreach (Giocatore giocatore in Lista)
				{
					sum += giocatore.Quotazione;
				}

				QuotazioneMedia = sum / Lista.Count + 2;
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

		/// <summary>
		/// Classe di supporto per identificare un intervallo numerico e il numero di volte che viene estratto
		/// </summary>
		private class Intervallo
		{
			#region Properties

			public int Inf { get; }

			public int Sup { get; }

			public int Dim { get; }

			public int Estrazioni { get; set; }

			#endregion

			public Intervallo(int inf, int sup)
			{
				Inf = inf;
				Sup = sup;
				Dim = sup - inf + 1;
				Estrazioni = 0;
			}
		}
	}
}
