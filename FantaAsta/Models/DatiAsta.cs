using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FantaAsta.Utilities;

namespace FantaAsta.Models
{
	[DataContract(Name = "FantaAstaData", Namespace = "")]
	public class DatiAsta : ICloneable
	{
		#region Properties

		[DataMember(Name = "FantaSquadre")]
		public List<FantaSquadra> FantaSquadre { get; set; }

		[DataMember(Name = "PercorsoFileLista")]
		public string PercorsoFileLista { get; set; }

		#endregion

		public DatiAsta()
		{
			FantaSquadre = new List<FantaSquadra>();
			PercorsoFileLista = string.Empty;
		}

		#region Public methods

		public override bool Equals(object obj)
		{
			if (obj is DatiAsta other)
			{
				bool res = true;

				res &= (string.IsNullOrEmpty(PercorsoFileLista) && string.IsNullOrEmpty(other.PercorsoFileLista)) || 
					   (!string.IsNullOrEmpty(PercorsoFileLista) && !string.IsNullOrEmpty(other.PercorsoFileLista) && PercorsoFileLista.Equals(other.PercorsoFileLista));
				
				res &= FantaSquadre.Count == other.FantaSquadre.Count;
				if (res)
				{
					foreach (FantaSquadra fantaSquadra in FantaSquadre)
					{
						res &= other.FantaSquadre.Contains(fantaSquadra);
					}
				}

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
			return new DatiAsta
			{
				FantaSquadre = FantaSquadre.Clone(),
				PercorsoFileLista = PercorsoFileLista
			};
		}

		#endregion
	}
}
