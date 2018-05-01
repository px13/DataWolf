using System;
using System.Collections.Generic;

namespace DataWolf
{
	public class ObjektInfo
	{
		public String znamienko;
		public String text;
		public List<BodkaInfo> vstupy;
		public List<BodkaInfo> vystupy;
		public bool clkRstSignal;
		
		public ObjektInfo(String nZnamienko, String nText, List<BodkaInfo> nVstupy, List<BodkaInfo> nVystupy,
		                  bool nClkRstSignal = false)
		{
			znamienko = nZnamienko;
			text = nText;
			vstupy = nVstupy;
			vystupy = nVystupy;
			clkRstSignal = nClkRstSignal;
		}
		
		public int pocetVstupov()
		{
			return vstupy == null ? 0 : vstupy.Count;
		}
		
		public int pocetVystupov()
		{
			return vystupy == null ? 0 : vystupy.Count;
		}
	}
	
	public class Info
	{
		public String meno;
		public int bitSirka;
		public int bitDlzka;
		
		protected String typ;
		
		public Info(String nMeno, int nBitSirka, int nBitDlzka)
		{
			meno = nMeno;
			bitSirka = nBitSirka;
			bitDlzka = nBitDlzka;
		}
			
		public String generujDWKod()
		{
			String vysl = typ + " " + meno + " ";
			vysl += "# " + bitSirka + " " + bitDlzka + " ";
			vysl += "\r\n";
			return vysl;
		}
	}
	
	public class RegisterInfo : Info
	{
		public RegisterInfo(String nMeno, int nBitSirka, int nBitDlzka)
			:base(nMeno, nBitSirka, nBitDlzka)
		{
			typ = "register";
		}
	}
	
	public class BodkaInfo : Info
	{
		public bool bitSirkaZmena;
		public bool bitDlzkaZmena;
		
		public BodkaInfo(String nMeno, int nBitSirka, bool nBitSirkaZmena, int nBitDlzka, bool nBitDlzkaZmena)
			:base(nMeno, nBitSirka, nBitDlzka)
		{
			bitSirkaZmena = nBitSirkaZmena;
			bitDlzkaZmena = nBitDlzkaZmena;
		}
		
		public static BodkaInfo INT(String nMeno)
		{
			return new BodkaInfo(nMeno, 1, false, 32, true);
		}
		
		public static BodkaInfo BOOL(String nMeno)
		{
			return new BodkaInfo(nMeno, 1, false, 1, false);
		}
		
		public String generujDWKod(String io)
		{
			typ = io;
			return generujDWKod();
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			BodkaInfo other = obj as BodkaInfo;
				if (other == null)
					return false;
						return this.meno == other.meno && this.bitSirka == other.bitSirka && this.bitDlzka == other.bitDlzka && this.typ == other.typ && this.bitSirkaZmena == other.bitSirkaZmena && this.bitDlzkaZmena == other.bitDlzkaZmena;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * bitSirkaZmena.GetHashCode();
				hashCode += 1000000009 * bitDlzkaZmena.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(BodkaInfo lhs, BodkaInfo rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(BodkaInfo lhs, BodkaInfo rhs) {
			return !(lhs == rhs);
		}

		#endregion
		
	}
	
}
