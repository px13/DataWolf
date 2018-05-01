using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace DataWolf
{
	public class Modul
	{
		public String meno;
		public Dictionary<String, Objekt> objekty;
		public List<BodkaInfo> vstupy;
		public List<BodkaInfo> vystupy;
		public List<RegisterInfo> registre;
		public List<IfBlok> ifBloky;
		
		List<CiaraBodka> poleBodiek;
		List<List<String>> poleIndexov;
		
		public Modul(String nMeno, Dictionary<String, Objekt> nObjekty,
		               List<BodkaInfo> nVstupy, List<BodkaInfo> nVystupy,
		               List<RegisterInfo> nRegistre,
		              List<CiaraBodka> nPoleBodiek = null, List<List<String>> nPoleIndexov = null)
		{
			meno = nMeno;
			objekty = nObjekty;
			vstupy = nVstupy;
			vystupy = nVystupy;
			registre = nRegistre;
			ifBloky = new List<IfBlok>();
			foreach (Objekt o in objekty.Values)
			{
				o.setM(this);
				if (o is IfBlok)
				{
					ifBloky.Add((IfBlok) o);
				}
			}
			poleBodiek = nPoleBodiek;
			poleIndexov = nPoleIndexov;
		}
		
		public String getOpId(String op)
		{
			for (int i = 0 ; i < 1000 ; i++)
			{
				if (!objekty.ContainsKey(op+i))
				{
					return op+i;
				}
			}
			return "PREKROCENY_LIMIT_JEDNEJ_OPERACIE";
		}
		
		public void objektNoveId(String id, String nId)
		{
			Objekt o = objekty[id];
			objekty.Remove(id);
			objekty[nId] = o;
		}
		
		public void nastavGraf()
		{
			if (poleBodiek == null) poleBodiek = new List<CiaraBodka>();
			if (poleIndexov == null) poleIndexov = new List<List<String>>();
			foreach (List<String> ciara in poleIndexov)
			{
				int idBodky = int.Parse(ciara[0]);
				String idObjektu = ciara[1];
				Ciara c = new Ciara(objekty[idObjektu].bodky[idBodky], null, new List<CiaraBodka>());
				objekty[idObjektu].ciary[idBodky] = c;
				objekty[idObjektu].bodky[idBodky].stav = Brushes.LawnGreen;
				for (int i = 2 ; i < ciara.Count - 2 ; i++)
				{
					c.cesta.Add(poleBodiek[int.Parse(ciara[i])]);
					poleBodiek[int.Parse(ciara[i])].ciary.Add(c);
				}
				idBodky = int.Parse(ciara[ciara.Count-2]);
				idObjektu = ciara[ciara.Count-1];
				c.bodkaVystup = objekty[idObjektu].bodky[idBodky];
			}
			foreach (Objekt o in objekty.Values)
			{
				priradObjektIfBloku(o);
			}
			foreach (CiaraBodka mb in poleBodiek)
			{
				priradCiarovuBodkuIfBloku(mb);
			}
			poleBodiek = null;
			poleIndexov = null;
		}
		
		public void priradCiarovuBodkuIfBloku(CiaraBodka mb)
		{
			IfBlok blok = null;
			for (int i = 0; i < ifBloky.Count ; i++)
			{
				IfBlok tempBlok = ifBloky[i];
				if (tempBlok.jeVBloku(mb.getX(), mb.getY()))
				{
					if (blok == null)
					{
						blok = tempBlok;
					}
					else if (mb.getX() - blok.getX() > mb.getX() - tempBlok.getX())
					{
						blok = tempBlok;
					}
				}
			}
			if (blok != null)
			{
				if (!blok.ciaroveBodky.Contains(mb))
				{
					blok.ciaroveBodky.Add(mb);
					mb.ifBlok = blok;
				}
			}
			else if (mb.ifBlok != null)
			{
				mb.ifBlok.ciaroveBodky.Remove(mb);
			}
		}
		
		public void priradObjektIfBloku(Objekt o)
		{
			IfBlok blok = null;
			for (int i = 0; i < ifBloky.Count ; i++)
			{
				IfBlok tempBlok = ifBloky[i];
				if (tempBlok.jeVBloku(o.getX(), o.getY()))
				{
					if (blok == null)
					{
						blok = tempBlok;
					}
					else if (o.getX() - blok.getX() > o.getX() - tempBlok.getX())
					{
						blok = tempBlok;
					}
				}
			}
			if (blok != null)
			{
				if (!blok.objekty.Contains(o))
				{
					blok.objekty.Add(o);
					o.ifBlok = blok;
				}
			}
			else if (o.ifBlok != null)
			{
				o.ifBlok.objekty.Remove(o);
			}
		}
		
		bool rozdielneBodkaInfoList(List<BodkaInfo> a, List<BodkaInfo> b)
		{
			if (a.Count != b.Count) return true;
			for (int i = 0 ; i < a.Count ; i++)
			{
				if (!a[i].Equals(b[i]))
				{
					return true;
				}
			}
			return false;
		}
		
		public bool aktualizujVstupy(List<BodkaInfo> nVstupy)
		{
			if (rozdielneBodkaInfoList(vstupy, nVstupy))
			{
				vstupy = nVstupy;
				Projekt.DATABAZA[meno].vstupy = vstupy;
				return true;
			}
			return false;
		}
		
		public bool aktualizujVystupy(List<BodkaInfo> nVystupy)
		{
			if (rozdielneBodkaInfoList(vystupy, nVystupy))
			{
				vystupy = nVystupy;
				Projekt.DATABAZA[meno].vystupy = vystupy;
				return true;
			}
			return false;
		}
		
		public void zmazObjekt(String objektIndex)
		{
			foreach (Objekt o in objekty.Values)
			{
				for (int j = 0 ; j < o.ciary.Length ; j++)
				{
					if (o.ciary[j] == null) continue;
					if (o.ciary[j].bodkaVystup.obj.id == objekty[objektIndex].id)
					{
						o.ciary[j] = null;
					}
				}
			}
			if (objekty[objektIndex] is IfBlok)
			{
				IfBlok blok = (IfBlok)objekty[objektIndex];
				foreach (Objekt o in blok.objekty)
				{
					o.ifBlok = blok.ifBlok;
				}
				foreach (CiaraBodka mb in blok.ciaroveBodky)
				{
					mb.ifBlok = blok.ifBlok;
				}
				if (blok.ifBlok != null)
				{
					blok.ifBlok.objekty.AddRange(blok.objekty);
					blok.ifBlok.ciaroveBodky.AddRange(blok.ciaroveBodky);
				}
				blok.objekty = null;
				blok.ciaroveBodky = null;
				ifBloky.Remove(blok);
			}
			objekty.Remove(objektIndex);
		}
		
		public String generujDWKod()
		{
			String vysl = "def " + meno;
			vysl += "\r\n[\r\n";
			foreach (BodkaInfo b in vstupy)
			{
				vysl += b.generujDWKod("input");
			}
			foreach (BodkaInfo b in vystupy)
			{
				vysl += b.generujDWKod("output");
			}
			foreach (RegisterInfo r in registre)
			{
				vysl += r.generujDWKod();
			}
			poleBodiek = new List<CiaraBodka>();
			poleIndexov = new List<List<String>>();
			foreach (Objekt o in objekty.Values)
			{
				vysl += o.generujDWKod();
				if (o.ciary == null) continue;
				foreach (Ciara c in o.ciary)
				{
					if (c == null || c.cesta == null) continue;
					poleIndexov.Add(new List<String>());
					poleIndexov[poleIndexov.Count-1].Add(Convert.ToString(c.bodkaVstup.id));
					poleIndexov[poleIndexov.Count-1].Add(c.bodkaVstup.obj.id);
					
					foreach (CiaraBodka mb in c.cesta)
					{
						int ix = poleBodiek.IndexOf(mb);
						if (ix < 0)
						{
							poleBodiek.Add(mb);
							ix = poleBodiek.Count - 1;
						}
						poleIndexov[poleIndexov.Count-1].Add(Convert.ToString(ix));
					}
					poleIndexov[poleIndexov.Count-1].Add(Convert.ToString(c.bodkaVystup.id));
					poleIndexov[poleIndexov.Count-1].Add(c.bodkaVystup.obj.id);
				}
			}
			vysl += "# ";
			foreach (CiaraBodka mb in poleBodiek)
			{
				vysl += mb.getX() + " " + mb.getY() + " ";
			}
			vysl += "\r\n";
			foreach (List<String> ciara in poleIndexov)
			{
				vysl += "# ";
				foreach (String str in ciara)
				{
					vysl += str + " ";
				}
				vysl += "\r\n";
			}
			vysl += "]\r\n";
			poleBodiek = null;
			poleIndexov = null;
			return vysl;
		}
		
		public Dictionary<String, String> generujLucidKod(String cesta)
		{
			Dictionary<String, String> moduly = new Dictionary<String, String>();
			Dictionary<String, String> clkModuly = new Dictionary<String, String>();
			Queue<Struktura> fronta = new Queue<Struktura>();
			Struktura korenProgramu = new Struktura("KorenProgramu");
			foreach (Objekt o in objekty.Values)
			{
				if (o is PouzitieModulu)
				{
					if (Projekt.DATABAZA[o.op].clkRstSignal)
					{
						clkModuly[o.id] = o.op;
					}
				}
				else if (o.vlastnyModul())
				{
					moduly[o.id] = o.menoModulu();
				}
				if (o.ifBlok == null || !(o is Register || o is IoOperacia || o is IfBlok))
				{
					korenProgramu.deti.Add(new Struktura(o.id));
				}
				else
				{
					fronta.Enqueue(new Struktura(o.id));
				}
			}
			while (fronta.Count > 0)
			{
				Struktura s = fronta.Dequeue();
				if (!korenProgramu.pridajDieta(objekty[s.id].ifBlok.id, s))
				{
					fronta.Enqueue(s);
				}
			}
			System.IO.File.WriteAllText(@cesta+"\\"+meno+".luc", generujModul()
			                           + generujIo() + generujRegistreAClkModuly(clkModuly)
			                           + generujModuly(moduly) + generujProgram(korenProgramu));
			return moduly;
		}
		
		String generujModul()
		{
			String modul = "module " + meno + " (\r\n";
			if (Projekt.DATABAZA[meno].clkRstSignal)
			{
				modul += Objekt.tabulator(2) + "input clk,\r\n";
				modul += Objekt.tabulator(2) + "input rst,\r\n";
			}
			return modul;
		}
		
		String generujIo()
		{
			String io = "";
			foreach (BodkaInfo info in vstupy)
			{
				io += Objekt.tabulator(2) + "input " + info.meno;
				if (info.bitSirka > 1)
				{
					io += "[" + info.bitSirka + "]";
				}
				io += "[" + info.bitDlzka + "],\r\n";
			}
			for (int i = 0; i < vystupy.Count; i++) {
				BodkaInfo info = vystupy[i];
				io += Objekt.tabulator(2) + "output " + info.meno;
				if (info.bitSirka > 1) {
					io += "[" + info.bitSirka + "]";
				}
				io += "[" + info.bitDlzka + "]";
				if (i+1 < vystupy.Count)
				{
					io += ",";
				}
				io += "\r\n";
			}
			io += Objekt.tabulator(1) + ") {\r\n\r\n";
			return io;
		}
		
		String generujRegistreAClkModuly(Dictionary<String, String> moduly)
		{
			if (!Projekt.DATABAZA[meno].clkRstSignal || (registre.Count == 0 && moduly.Count == 0))
			{
				return "";
			}
			String vysl = "";
			vysl += Objekt.tabulator(1) + ".clk(clk) {\r\n" + Objekt.tabulator(2) + ".rst(rst) {\r\n";
			for (int i = 0 ; i < registre.Count ; i++)
			{
				vysl += Objekt.tabulator(3) + "dff " + registre[i].meno;
				if (registre[i].bitSirka > 1)
				{
					vysl += "[" + registre[i].bitSirka + "]";
				}
				vysl += "[" + registre[i].bitDlzka + "]";
				vysl += ";\r\n";
			}
			foreach (KeyValuePair<String, String> entry in moduly)
			{
				vysl += Objekt.tabulator(2) + entry.Value + " " + entry.Key + ";\r\n";
			}
			vysl += Objekt.tabulator(2) + "}\r\n" + Objekt.tabulator(1) + "}\r\n\r\n";
			
			return vysl;
		}
		
		String generujModuly(Dictionary<String, String> moduly)
		{
			String vysl = "";
			foreach (KeyValuePair<String, String> entry in moduly)
			{
				vysl += Objekt.tabulator(1) + entry.Value + " " + entry.Key + ";\r\n";
			}
			return vysl + "\r\n";
		}
		
		String generujProgram(Struktura korenProgramu)
		{
			String program = Objekt.tabulator(1) + "always {\r\n";
			/*foreach (Objekt o in objekty.Values)
			{
				if (o is VolanieFunkcie)
				{
					program += o.generujInit();
				}
			}*/
			if (vystupy != null)
			{
				for (int i = 0 ; i < vystupy.Count ; i++)
				{
					program += Objekt.tabulator(2) + vystupy[i].meno + " = ";
					if (vystupy[i].bitSirka == 1)
					{
						program += "0";
					}
					else
					{
						program += vystupy[i].bitSirka + "x{{"+ vystupy[i].bitDlzka +"hxx}}";
					}
					program += ";\r\n";
				}
			}
			foreach (Struktura s in korenProgramu.deti)
			{
				program += s.strukturujLucidKod(this, 2);
			}
			program += Objekt.tabulator(1) + "}\r\n}";
			return program;
		}
	}
	
	class Struktura : IComparable 
	{
		public String id;
		public List<Struktura> deti;
		
		public Struktura(String nId, List<Struktura> nDeti = null)
		{
			id = nId;
			deti = nDeti;
			if (deti == null)
			{
				deti = new List<Struktura>();
			}
		}
		
		public bool pridajDieta(String otecId, Struktura dieta)
		{
			if (id == otecId)
			{
				deti.Add(dieta);
				return true;
			}
			for (int i = 0 ; i < deti.Count; i++)
			{
				if (deti[i].pridajDieta(otecId, dieta))
				{
					return true;
				}
			}
			return false;
		}
		
		public String strukturujLucidKod(Modul f, int tab)
		{
			String vysl;
			if (deti.Count > 0)
			{
				deti.Sort();
				vysl = f.objekty[id].generujLucidKod(f, tab);
				foreach (Struktura s in deti)
				{
					vysl += s.strukturujLucidKod(f, tab+1);
				}
				vysl += Objekt.tabulator(tab) + "}\r\n";
			}
			else
			{
				vysl = f.objekty[id].generujLucidKod(f, tab);
			}
			return vysl;
		}
		
		public override string ToString()
		{
			if (deti == null || deti.Count == 0)
			{
				return id;
			}
			String vysl = id + "{";
			for (int i = 0 ; i < deti.Count ; i++)
			{
				vysl += deti[i].ToString();
				if (i + 1 < deti.Count)
				{
					vysl += ", ";
				}
			}
			vysl += "}";
			return vysl;
		}
		
		#region IComparable implementation
		public int CompareTo(object obj)
		{
			if (obj == null) return 1;

	        Struktura s2 = obj as Struktura;
	        if (s2 != null) 
	            return this.deti.Count.CompareTo(s2.deti.Count);
	        throw new ArgumentException("Object is not a Struktura");
		}
		#endregion
			}
}


