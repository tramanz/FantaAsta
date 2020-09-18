﻿using System.Runtime.Serialization;
using FantaAsta.Constants;

namespace FantaAsta.Models
{
	[DataContract(Name = "FantaLegaSettings", Namespace = "")]
	public class Opzioni
	{
		#region Properties

		[DataMember(Name = "BudgetIniziale")]
		public double BudgetIniziale { get; set; }

		#endregion

		public Opzioni()
		{
			SetDefaults();
		}

		#region Public methods

		public override bool Equals(object obj)
		{
			if (obj is Opzioni other)
			{
				bool res = true;

				res &= BudgetIniziale == other.BudgetIniziale;

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

		public void Copia(ref Opzioni destinazione)
		{
			if (destinazione == null)
			{
				destinazione = new Opzioni();
			}

			destinazione.BudgetIniziale = BudgetIniziale;
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
			BudgetIniziale = CommonConstants.BUDGET_ESTIVO_DEFAULT;
		}

		#endregion
	}
}
