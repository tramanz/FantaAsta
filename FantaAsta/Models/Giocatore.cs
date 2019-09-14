namespace FantaAsta.Models
{
    public class Giocatore
    {
		public int ID { get; }
		public Ruoli Ruolo { get; }
		public string Nome { get; }
		public string Squadra { get; }
		public double Quotazione { get; }
		public double Prezzo { get; set; }

		public Giocatore(int id, Ruoli ruolo, string nome, string squadra, double quotazione)
		{
			ID = id;
			Ruolo = ruolo;
			Nome = nome;
			Squadra = squadra;
			Quotazione = quotazione;
		}
    }
}
