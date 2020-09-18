using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Prism.Events;
using FantaAsta.Constants;
using FantaAsta.Enums;
using FantaAsta.Events;
using FantaAsta.Utilities;

namespace FantaAsta.Models
{
	[DataContract(Name = "FantaLega", Namespace = "")]
	public class Lega
	{
		#region Private fields

		private readonly Random m_random = new Random();

		private readonly IEventAggregator m_eventAggregator;

		#endregion

		#region Properties

		[DataMember(Name = "FantaSquadre")]
		public List<FantaSquadra> FantaSquadre { get; set; }

		[DataMember(Name = "PercorsoFileLista")]
		public string PercorsoFileLista { get; set; }

		public List<Squadra> Squadre { get; private set; }

		public List<Giocatore> Lista { get; private set; }

		public List<Giocatore> Svincolati { get; private set; }

		public Opzioni Opzioni { get; private set; }

		public bool ListaPresente { get; private set; }

		public bool ModalitaAstaInvernaleAttiva { get; private set; }

		public double QuotazioneMedia { get; private set; }

		#endregion

		public Lega(IEventAggregator eventAggregator)
		{
			m_eventAggregator = eventAggregator;
			m_eventAggregator.GetEvent<OpzioniModificateEvent>().Subscribe(OnOpzioniModificate);

			CaricaOpzioni();

			CaricaSquadre();

			CaricaLista();

			CaricaFantaSquadre();

			DisattivaModalitaAstaInvernale();
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
			// *************************************************************************************************************************************************************************************************
			// *                                                                                                                                                                                               *
			// * La probabilità di un giocatore di essere estratto (a meno di un fattore correttivo per i giocatori meno quotati, dovuto alla divisione intera) è:                                             *
			// *        1                                                                                                                                                                                      *
			// * p = ---------,                                                                                                                                                                                *
			// *           k                                                                                                                                                                                   *
			// *      N * 5                                                                                                                                                                                    *
			// * dove N è il numero di giocatori nella lista del ruolo specificato e k = 0, 1, ..., 4 è l'indice del gruppo in cui è presente il giocatore.                                                    *
			// * I giocatori in lista vengono ordinati in modo decrescente e divisi in 5 gruppi di cardinalità M = [N/5] (ad eccezione dell'ultimo, pari a M = N - 4*[N/5]).                                   *
			// * In questo modo si rende più probabile l'estrazione di un giocatore più quotato rispetto ad uno meno quotato, mantenendo al contempo la probabilità uniforme all'interno di uno stesso gruppo. *
			// *                                                                                                                                                                                               *
			// *************************************************************************************************************************************************************************************************

			// Ricavo dalla lista i giocatori del ruolo specificato e ordino per quotazione decrescente
			List<Giocatore> listaRuolo = GeneraListaPerRuolo(Svincolati, ruolo).Where(g => !g.Scartato).OrderByDescending(g => g.Quotazione).ToList();

			// Ricavo il numero N di giocatori totali
			int N = listaRuolo.Count;

			// Se non sono presenti giocatori del ruolo specificato, aggiorno lo stato 'scartato' di tutti e ritorno null
			if (N == 0)
			{
				foreach (Giocatore giocatore in GeneraListaPerRuolo(Svincolati, ruolo))
				{
					giocatore.Scartato = false;
				}

				return null;
			}

			// Se in lista sono rimasti meno di 5 giocatori del ruolo specificato, ritorno un giocatore con probabilità uniforme
			if (N < 5)
			{
				return listaRuolo[m_random.Next(0, N - 1)];
			}

			// Inizializzo le variabili di supporto per l'estrazione
			int[] estrazioni = new int[5];
			int k = 0;
			bool flag = true;

			// Continuo ad estrarre un indice di gruppo finché la condizione d'uscita non è soddisfatta
			while (flag)
			{
				// Estraggo un indice di gruppo
				k = m_random.Next(0, 4);

				// Aggiorno il numero di volte che l'indice è stato estratto e la condizione d'uscita
				flag = ++estrazioni[k] < k + 1;
			}

			// Ricavo il numero di giocatori per intervallo (se l'intervallo estratto è l'ultimo, aggiungo il resto della divisione intera)
			int M = (int)Math.Floor((double)(N / 5));
			if (k == 4)
			{
				M += N % M;
			}

			// Ricavo l'intervallo estratto
			List<Giocatore> listaIntervallo = listaRuolo.GetRange(k * M, M);

			// Estraggo un giocatore con probabilità uniforme dall'intervallo estratto
			return listaIntervallo[m_random.Next(0, M - 1)];
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
			_ = Svincolati.Remove(giocatore);

			// Segnalo alla view l'operazione
			m_eventAggregator.GetEvent<GiocatoreAggiuntoEvent>().Publish(new GiocatoreAggiuntoEventArgs(giocatore, squadra, prezzo));

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
				m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Publish(new GiocatoreRimossoEventArgs(giocatore, squadra, prezzo));
			}
		}

		/// <summary>
		/// Metodo per salvare i dati dell'applicazione
		/// </summary>
		public void Salva()
		{
			SalvaOpzioni();

			SalvaSquadre();
		}

		/// <summary>
		/// Metodo per cambiare la modalità d'asta
		/// </summary>
		public void CambiaModalitaAsta()
		{
			AggiornaModalitaAsta(!ModalitaAstaInvernaleAttiva);
		}

		/// <summary>
		/// Metodo per disattivare la modalità asta invernale
		/// </summary>
		public void DisattivaModalitaAstaInvernale()
		{
			AggiornaModalitaAsta(false);
		}

		/// <summary>
		/// Metodo per attivare la modalità asta invernale
		/// </summary>
		public void AttivaModalitaAstaInvernale()
		{
			AggiornaModalitaAsta(true);
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
				squadra.Budget = Opzioni.BudgetIniziale;
				squadra.Giocatori.Clear();
			}

			m_eventAggregator.GetEvent<RoseResettateEvent>().Publish();
		}

		/// <summary>
		/// Metodo per avvia l'import di una lista di giocatori
		/// </summary>
		public void AvviaImportaLista()
		{
			m_eventAggregator.GetEvent<ApriFileDialogEvent>().Publish();
		}

		/// <summary>
		/// Metodo per aggiungere una fantasquadra alla lega
		/// </summary>
		/// <param name="nome">Il nome della fantasquadra</param>
		public bool AggiungiSquadra(string nome)
		{
			if (FantaSquadre.Select(s => s.Nome).Contains(nome))
				return false;

			FantaSquadra squadra = new FantaSquadra(nome, Opzioni.BudgetIniziale);

			FantaSquadre.Add(squadra);
			_ = FantaSquadre.OrderBy(s => s.Nome);

			m_eventAggregator.GetEvent<FantaSquadraAggiuntaEvent>().Publish(new FantaSquadraEventArgs(squadra));

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

					m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Publish(new GiocatoreRimossoEventArgs(giocatore, squadra, giocatore.Prezzo));
				}

				_ = FantaSquadre.Remove(squadra);

				m_eventAggregator.GetEvent<FantaSquadraRimossaEvent>().Publish(new FantaSquadraEventArgs(squadra));
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

		/// <summary>
		/// Metodo per verificare se un giocatore è stato acquistato da una fantasquadra e aggiornare il suo stato
		/// </summary>
		/// <param name="giocatore">Il giocatore da controllare</param>
		public void ControllaAcquistoGiocatore(Giocatore giocatore)
		{
			if (giocatore != null)
			{
				giocatore.Scartato = !FantaSquadre.Any(s => s.Giocatori.Contains(giocatore));
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Metodo per salvare i dati riguardanti le preferenze
		/// </summary>
		private void SalvaOpzioni()
		{
			if (!Directory.Exists(CommonConstants.DATA_DIRECTORY_PATH))
			{
				_ = Directory.CreateDirectory(CommonConstants.DATA_DIRECTORY_PATH);
			}

			if (File.Exists(CommonConstants.SETTINGS_FILE_PATH))
			{
				File.Delete(CommonConstants.SETTINGS_FILE_PATH);
			}

			XMLSerializer.Serialize(CommonConstants.SETTINGS_FILE_PATH, Opzioni);
		}

		/// <summary>
		/// Metodo per salvare i dati riguardanti le fantasquadre
		/// </summary>
		private void SalvaSquadre()
		{
			if (!Directory.Exists(CommonConstants.DATA_DIRECTORY_PATH))
			{
				_ = Directory.CreateDirectory(CommonConstants.DATA_DIRECTORY_PATH);
			}

			if (File.Exists(CommonConstants.DATA_FILE_PATH))
			{
				File.Delete(CommonConstants.DATA_FILE_PATH);
			}

			XMLSerializer.Serialize(CommonConstants.DATA_FILE_PATH, this);
		}

		/// <summary>
		/// Metodo per caricare la lista di giocatori
		/// </summary>
		private void CaricaLista()
		{
			if (File.Exists(CommonConstants.DATA_FILE_PATH))
			{
				PercorsoFileLista = ((Lega)XMLSerializer.Deserialize(CommonConstants.DATA_FILE_PATH, typeof(Lega))).PercorsoFileLista;
			}

			_ = CaricaListaDaFile(PercorsoFileLista, true);
		}

		/// <summary>
		/// Metodo per caricare la lista dei giocatori da un file .csv
		/// </summary>
		/// <param name="filePath">Percorso del file .csv della lista</param>
		/// <param name="isLoadingAtStart">Indica se il caricamento è partito da un richiesta automatica all'avvio oppure dall'utentemo</param>
		/// <returns>True se l'operazione è andata a buon fine, false altrimenti</returns>
		private bool CaricaListaDaFile(string filePath, bool isLoadingAtStart)
		{
			if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
			{
				return false;
			}

			if (!Directory.Exists(CommonConstants.DATA_DIRECTORY_PATH))
			{
				_ = Directory.CreateDirectory(CommonConstants.DATA_DIRECTORY_PATH);
			}

			Lista = new List<Giocatore>();
			Svincolati = new List<Giocatore>();

			if (!isLoadingAtStart)
			{
				try
				{
					PercorsoFileLista = Path.Combine(CommonConstants.DATA_DIRECTORY_PATH, $"Quotazioni ({DateTime.Now:dd_MM_yyyy}).csv");
					File.Copy(filePath, PercorsoFileLista, true);
				}
				catch
				{
					return false;
				}
			}

			using (StreamReader reader = new StreamReader(PercorsoFileLista))
			{
				Giocatore giocatore;
				while (!reader.EndOfStream)
				{
					string[] data = reader.ReadLine().Split(';');

					giocatore = new Giocatore(Convert.ToInt32(data[0]), (Ruoli)Enum.Parse(typeof(Ruoli), data[1]), data[2], Squadre.Single(s => s.Nome.Equals(data[3], StringComparison.OrdinalIgnoreCase)), Convert.ToDouble(data[4]));

					Lista.Add(giocatore);
					Svincolati.Add(giocatore);
				}
			}

			ListaPresente = true;

			AggiornaQuotazioneMedia();

			AggiornaStatoGiocatoriInRosa();

			m_eventAggregator.GetEvent<ListaImportataEvent>().Publish();

			return true;
		}

		/// <summary>
		/// Metodo per caricare le fantasquadre
		/// </summary>
		private void CaricaFantaSquadre()
		{
			FantaSquadre = new List<FantaSquadra>();

			if (File.Exists(CommonConstants.DATA_FILE_PATH))
			{
				CaricaFantaSquadreDaFile();
			}
		}

		/// <summary>
		/// Metodo per caricare le fantasquadre dai dati salvati
		/// </summary>
		private void CaricaFantaSquadreDaFile()
		{
			if (File.Exists(CommonConstants.DATA_FILE_PATH))
			{
				FantaSquadre = ((Lega)XMLSerializer.Deserialize(CommonConstants.DATA_FILE_PATH, typeof(Lega))).FantaSquadre;
			}

			AggiornaStatoGiocatoriInRosa();
		}

		/// <summary>
		/// Metodo per caricare le squadre da file .csv nelle risorse
		/// </summary>
		private void CaricaSquadre()
		{
			Squadre = new List<Squadra>();

			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FantaAsta.Resources.Data.Squadre.csv"))
			using (StreamReader reader = new StreamReader(stream))
			{
				while (!reader.EndOfStream)
				{
					string[] data = reader.ReadLine().Split(';');

					Squadre.Add(new Squadra(data[0], data[1], data[2]));
				}
			}
		}

		/// <summary>
		/// Metodo per caricare le preferenze
		/// </summary>
		private void CaricaOpzioni()
		{
			Opzioni = File.Exists(CommonConstants.SETTINGS_FILE_PATH) ? 
				XMLSerializer.Deserialize(CommonConstants.SETTINGS_FILE_PATH, typeof(Opzioni)) as Opzioni : 
				new Opzioni();
		}

		/// <summary>
		/// Metodo che controlla se i giocatori nelle rose sono presenti in lista e aggiorna la lista degli svincolati
		/// </summary>
		private void AggiornaStatoGiocatoriInRosa()
		{
			if (FantaSquadre != null)
			{
				List<Giocatore> listaSupporto; Giocatore giocatoreSupporto;

				foreach (FantaSquadra squadra in FantaSquadre)
				{
					listaSupporto = new List<Giocatore>();

					foreach (Giocatore giocatore in squadra.Giocatori)
					{
						// Se il giocatore è in lista, prendo l'istanza nella lista
						// (per mantenere un'unica istanza del giocatore tra lista e rose),
						// altrimenti lascio l'istanza attuale
						giocatoreSupporto = Lista.Contains(giocatore) ? Lista.Find(g => g.Equals(giocatore)) : giocatore;
						giocatoreSupporto.InLista = Lista.Contains(giocatore);
						giocatoreSupporto.Prezzo = giocatore.Prezzo;

						// Rimuovo il giocatore dalla lista degli svincolati
						if (Svincolati.Contains(giocatoreSupporto))
						{
							_ = Svincolati.Remove(giocatoreSupporto);
						}

						listaSupporto.Add(giocatoreSupporto);
					}

					squadra.Giocatori = listaSupporto;
				}

				m_eventAggregator.GetEvent<RoseResettateEvent>().Publish();
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

				QuotazioneMedia = (sum / Lista.Count) + 2;
			}
		}

		/// <summary>
		/// Metodo per aggiornare la modalità d'asta corrente secondo il parametro passato in ingresso
		/// </summary>
		/// <param name="attivaAstaInvernale">Indica se attivare (true) o disattivare (false) la modalità d'asta invernale</param>
		private void AggiornaModalitaAsta(bool attivaAstaInvernale)
		{
			if (ModalitaAstaInvernaleAttiva != attivaAstaInvernale)
			{
				ModalitaAstaInvernaleAttiva = attivaAstaInvernale;

				foreach (FantaSquadra squadra in FantaSquadre)
				{
					squadra.Budget += attivaAstaInvernale ? Opzioni.BudgetAggiuntivo : -Opzioni.BudgetAggiuntivo;
				}

				m_eventAggregator.GetEvent<ModalitàAstaCambiataEvent>().Publish();
			}
		}

		/// <summary>
		/// Metodo eseguito quando le opzioni vengono modificate
		/// </summary>
		private void OnOpzioniModificate()
		{
			SvuotaRose();
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