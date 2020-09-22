using System;
using System.Runtime.Serialization;
using FantaAsta.Constants;
using FantaAsta.Enums;

namespace FantaAsta.Models
{
	[DataContract(Name = "FantaAstaSettings", Namespace = "")]
	public class Preferenze : ICloneable
	{
		#region Properties

		[DataMember(Name = "PreferenzeImpostate")]
		public bool PreferenzeImpostate { get; set; }

		[DataMember(Name = "BudgetIniziale")]
		public double BudgetIniziale { get; set; }

		[DataMember(Name = "BudgetAggiuntivo")]
		public double BudgetAggiuntivo { get; set; }

		[DataMember(Name = "PuntataMinima")]
		public PuntataMinima PuntataMinima { get; set; }

		#endregion

		public Preferenze()
		{
			SetDefaults();
		}

		#region Public methods

		public override bool Equals(object obj)
		{
			if (obj is Preferenze other)
			{
				bool res = true;

				res &= BudgetIniziale == other.BudgetIniziale;
				res &= BudgetAggiuntivo == other.BudgetAggiuntivo;
				res &= PuntataMinima == other.PuntataMinima;

				return res;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public object Clone()
		{
			return new Preferenze()
			{
				PreferenzeImpostate = PreferenzeImpostate,
				BudgetIniziale = BudgetIniziale,
				BudgetAggiuntivo = BudgetAggiuntivo,
				PuntataMinima = PuntataMinima
			};
		}

		#endregion

		#region Private methods

		[OnDeserializing]
		private void OnDeserializing(StreamingContext streamingContext)
		{
			SetDefaults();
		}

		private void SetDefaults()
		{
			PreferenzeImpostate = false;
			BudgetIniziale = CommonConstants.BUDGET_INIZIALE_DEFAULT;
			BudgetAggiuntivo = CommonConstants.BUDGET_AGGIUNTIVO_DEFAULT;
			PuntataMinima = PuntataMinima.Quotazione;
		}

		#endregion
	}
}
