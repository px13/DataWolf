using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DataWolf
{
	public class Operacia : Objekt
	{
		public Operacia(int nX, int nY, String nId, String nOp)
			: base(nX, nY, nId, nOp)
		{
			int pocetVstupov = Projekt.DATABAZA[op].pocetVstupov();
			int pocetVystupov = Projekt.DATABAZA[op].pocetVystupov();
			int rxVstupy = Math.Max(18, (10 + 8*pocetVstupov));
			int rxVystupy = Math.Max(18, (10 + 8*pocetVystupov));
			rx = Math.Max(rxVstupy, rxVystupy);
			dx = 2*rx;
			ry = 15;
			dy = 2*ry;
			ciary = new Ciara[pocetVstupov];
			bodky = new ObjektBodka[pocetVstupov+pocetVystupov];
			for (int i = 0, j = -rxVstupy + 14 ; i < pocetVstupov ; i++, j += dx / (pocetVstupov+1))
			{
				bodky[i] = new ObjektBodka(j, -ry - 8, Brushes.Red, this, i, Projekt.DATABAZA[op].vstupy[i]);
			}
			for (int i = pocetVstupov, j = -rxVystupy + 14 ; i < bodky.Length ; i++, j += dx / (pocetVystupov+1))
			{
				bodky[i] = new ObjektBodka(j, ry, Brushes.Black, this, i, Projekt.DATABAZA[op].vystupy[i-pocetVstupov]);
			}
		}
		
		public override bool aktualizujStav()
		{
			bool zmena = false;
			if (ciary.Length == 0) return zmena;
			int maxBitSirka = 1;
			int maxBitDlzka = 1;
			BodkaInfo b1Info;
			for (int i = 0 ; i < ciary.Length ; i++)
			{
				if (ciary[i] == null) continue;
				b1Info = bodky[i].info;
				ciary[i].stav = Pens.Black;
				if (b1Info.bitSirka != ciary[i].bodkaVystup.info.bitSirka && !b1Info.bitSirkaZmena)
				{
					ciary[i].stav = Pens.Red;
				}
				else if (b1Info.bitSirka != ciary[i].bodkaVystup.info.bitSirka)
				{
					zmena = true;
					b1Info.bitSirka = ciary[i].bodkaVystup.info.bitSirka;
				}
				maxBitSirka = Math.Max(maxBitSirka, b1Info.bitSirka);
				if (b1Info.bitDlzka != ciary[i].bodkaVystup.info.bitDlzka && !b1Info.bitDlzkaZmena)
				{
					if (ciary[i].bodkaVystup.info.bitDlzka > b1Info.bitDlzka)
					{
						ciary[i].stav = Pens.Red;
					}
				}
				else if (b1Info.bitDlzka != ciary[i].bodkaVystup.info.bitDlzka)
				{
					zmena = true;
					b1Info.bitDlzka = ciary[i].bodkaVystup.info.bitDlzka;
				}
				maxBitDlzka = Math.Max(maxBitDlzka, b1Info.bitDlzka);
			}
			if (zmena && Projekt.DATABAZA[op].pocetVystupov() != 0)
			{
				b1Info = bodky[ciary.Length].info;
				if (b1Info.bitSirkaZmena)
				{
					b1Info.bitSirka = maxBitSirka;
				}
				if (b1Info.bitDlzkaZmena)
				{
					b1Info.bitDlzka = maxBitDlzka;
				}
			}
			return zmena;
		}
	}
	
	public class Modulo : Operacia
	{
		public Modulo(int nX, int nY, String nId)
			:base(nX, nY, nId, "mod")
		{
		}
		
		public override void vytvorModul(string cesta, string meno)
		{
			String vysl = "";
			vysl += "module " + meno + " (\r\n";
			for (int i = 0 ; i < ciary.Length ; i++)
			{
				vysl += tabulator(2) + "input [" + (bodky[i].info.bitDlzka-1) + ":0] " + bodky[i].info.meno + ",\r\n";
			}
			vysl += tabulator(2) + "output reg [" + (bodky[ciary.Length].info.bitDlzka-1) + ":0] " + bodky[ciary.Length].info.meno + "\r\n";
			vysl += tabulator(1) + ");\r\n\r\n";
			vysl += tabulator(1) + "always @* begin\r\n";
			vysl += programModulu();
			vysl += tabulator(1) + "end\r\nendmodule";
			System.IO.File.WriteAllText(@cesta+"\\"+meno+typModulu(), vysl);
		}
		
		public override string typModulu()
		{
			return ".v";
		}
	}
	
	public class Index : Operacia
	{
		public Index(int nX, int nY, String nId)
			:base(nX, nY, nId, "index")
		{
		}
		
		public override bool aktualizujStav()
		{
			bool zmena = false;
			if (ciary[0] != null)
			{
				if (bodky[0].info.bitSirka != ciary[0].bodkaVystup.info.bitSirka)
				{
					zmena = true;
					bodky[0].info.bitSirka = ciary[0].bodkaVystup.info.bitSirka;
				}
				if (bodky[0].info.bitDlzka != ciary[0].bodkaVystup.info.bitDlzka)
				{
					zmena = true;
					bodky[0].info.bitDlzka = ciary[0].bodkaVystup.info.bitDlzka;
				}
				if (bodky[0].info.bitSirka > 1)
				{
					bodky[2].info.bitDlzka = ciary[0].bodkaVystup.info.bitDlzka;
				}
				else
				{
					bodky[2].info.bitDlzka = 1;
				}
			}
			if (ciary[1] != null)
			{
				if (ciary[1].bodkaVystup.info.bitSirka != 1)
				{
					ciary[1].stav = Pens.Red;
				}
				else
				{
					ciary[1].stav = Pens.Black;
					bodky[1].info.bitDlzka = ciary[1].bodkaVystup.info.bitDlzka;
				}
			}
			return zmena;
		}
		
		public override bool vlastnyModul()
		{
			return true;
		}
		
		public override String generujInit()
		{
			if (vlastnyModul())
			{
				return base.generujInit();
			}
			return "";
		}
		
		public override String getLucidCitanieDat(int b)
		{
			if (vlastnyModul())
			{
				return base.getLucidCitanieDat(b);
			}
			return ciary[0].bodkaVystup.obj.getLucidCitanieDat(ciary[0].bodkaVystup.obj.ciary.Length) + "[" + ciary[1].bodkaVystup.obj.getLucidCitanieDat(ciary[1].bodkaVystup.obj.ciary.Length) + "]";
		}
		
		public override String generujLucidKod(Modul f, int tab)
		{
			if (vlastnyModul())
			{
				return base.generujLucidKod(f, tab);
			}
			return "";
		}
		
		protected override string programModulu()
		{
			return tabulator(2) + bodky[ciary.Length].info.meno + " = " + bodky[0].info.meno + "[" + bodky[1].info.meno + "]" + ";\r\n";
		}
	}
	
	public class Zretazenie : Operacia
	{
		public Zretazenie(int nX, int nY, String nId)
			:base(nX, nY, nId, "concat")
		{
		}
		
		public override bool aktualizujStav()
		{
			bool zmena = false;
			int sumaSirok = 0;
			for (int i = 0; i < ciary.Length; i++)
			{
				if (ciary[i] != null)
				{
					if (bodky[i].info.bitSirka != ciary[i].bodkaVystup.info.bitSirka)
					{
						zmena = true;
						bodky[i].info.bitSirka = ciary[i].bodkaVystup.info.bitSirka;
					}
					if (bodky[i].info.bitDlzka != ciary[i].bodkaVystup.info.bitDlzka)
					{
						zmena = true;
						bodky[i].info.bitDlzka = ciary[i].bodkaVystup.info.bitDlzka;
					}
					sumaSirok += ciary[i].bodkaVystup.info.bitSirka;
				}
			}
			bodky[ciary.Length].info.bitSirka = sumaSirok;
			bodky[ciary.Length].info.bitDlzka = bodky[0].info.bitDlzka;
			for (int i = 0; i < ciary.Length; i++)
			{
				if (ciary[i] != null)
				{
					if (bodky[i].info.bitDlzka != bodky[ciary.Length].info.bitDlzka)
					{
						ciary[i].stav = Pens.Red;
					}
					else
					{
						ciary[i].stav = Pens.Black;
					}
				}
			}
			return zmena;
		}
		
		protected override string programModulu()
		{
			String vysl = "";
			if (bodky[0].info.bitSirka - 1 == 0)
			{
				vysl += tabulator(2) + bodky[ciary.Length].info.meno + "[0] = " + bodky[0].info.meno + ";\r\n";
			}
			else
			{
				vysl += tabulator(2) + bodky[ciary.Length].info.meno + "[" + (bodky[0].info.bitSirka - 1) + ":0] = " + bodky[0].info.meno + ";\r\n";
			}
			if (bodky[ciary.Length].info.bitSirka - 1 == bodky[0].info.bitSirka)
			{
				vysl += tabulator(2) + bodky[ciary.Length].info.meno + "[" + bodky[0].info.bitSirka + "] = " + bodky[1].info.meno + ";\r\n";	
			}
			else
			{
				vysl += tabulator(2) + bodky[ciary.Length].info.meno + "[" + (bodky[ciary.Length].info.bitSirka - 1) + ":" + bodky[0].info.bitSirka + "] = " + bodky[1].info.meno + ";\r\n";
			}
			return vysl;
		}
	}
	
	public class ZmenaBitov : Operacia
	{
		public ZmenaBitov(int nX, int nY, String nId, int nBitSirka, int nBitDlzka)
			:base(nX, nY, nId, "change")
		{
			setBitSirka(nBitSirka);
			setBitDlzka(nBitDlzka);
		}
		
		public override void nastavEditovaciePrvky(Panel panel)
		{
			base.nastavEditovaciePrvky(panel);
			
			panel.Controls.Add(vytvorLabel("Bit. šírka:", 3, 54));
			panel.Controls.Add(vytvorTextBox(Convert.ToString(getBitSirka()), 60, 30));
			zvysYRozhrania();
			
			panel.Controls.Add(vytvorLabel("Bit. dĺžka:", 3, 54));
			panel.Controls.Add(vytvorTextBox(Convert.ToString(getBitDlzka()), 60, 30));
			zvysYRozhrania();
		}
		
		public int getBitSirka()
		{
			return bodky[1].info.bitSirka;
		}
		
		public void setBitSirka(int nBitSirka)
		{
			bodky[1].info.bitSirka = nBitSirka;
		}
		
		public int getBitDlzka()
		{
			return bodky[1].info.bitDlzka;
		}
		
		public void setBitDlzka(int nBitDlzka)
		{
			bodky[1].info.bitDlzka = nBitDlzka;
		}
		
		protected override void ulozEditovaneVlastnosti(object sender, EventArgs e)
		{
			base.ulozEditovaneVlastnosti(sender, e);
			setBitSirka(int.Parse(textBoxy[3].Text));
			setBitDlzka(int.Parse(textBoxy[4].Text));
			
			Panel panel = (Panel) textBoxy[0].Parent;
			Button b = (Button) panel.Controls.Find("OkTlacitko", false)[0];
			zrusEditovaciePrvky(panel);
			nastavEditovaciePrvky(panel);
			b.Location = new Point(3, panelY+10);
			panel.Controls.Add(b);
		}
		
		public override bool aktualizujStav()
		{
			bool zmena = false;
			if (ciary[0] == null) return zmena;
			if (bodky[0].info.bitSirka != ciary[0].bodkaVystup.info.bitSirka)
			{
				zmena = true;
				bodky[0].info.bitSirka = ciary[0].bodkaVystup.info.bitSirka;
			}
			if (bodky[0].info.bitDlzka != ciary[0].bodkaVystup.info.bitDlzka)
			{
				zmena = true;
				bodky[0].info.bitDlzka = ciary[0].bodkaVystup.info.bitDlzka;
			}
			if (ciary[0].bodkaVystup.info.bitSirka * ciary[0].bodkaVystup.info.bitDlzka != bodky[1].info.bitSirka * bodky[1].info.bitDlzka)
			{
				ciary[0].stav = Pens.Red;
			}
			else
			{
				ciary[0].stav = Pens.Black;	
			}
			return zmena;
		}
		
		protected override String dwKodParametre()
		{
			return " # " + getBitSirka() + " " + getBitDlzka();
		}
		
		public override string menoModulu()
		{
			String meno = base.menoModulu();
			meno += "_" + bodky[ciary.Length].info.bitSirka;
			meno += "_" + bodky[ciary.Length].info.bitDlzka;
			return meno;
		}
		
		protected override string programModulu()
		{
			String vysl = "";
			vysl += tabulator(2) + bodky[1].info.meno + " = ";
			if (bodky[0].info.bitSirka > bodky[1].info.bitSirka)
			{
				if (bodky[1].info.bitSirka != 1)
				{
					vysl += "{";
				}
				int pomer = bodky[0].info.bitSirka / bodky[1].info.bitSirka;
				for (int i = bodky[1].info.bitSirka - 1; i >= 0 ; i--)
				{
					vysl += "c{";
					for (int index = pomer*i + pomer - 1 ; index >= pomer*i ; index--)
					{
						vysl += bodky[0].info.meno + "[" + index + "]";
						if (index - 1 >= pomer*i)
						{
							vysl += ", ";
						}
					}
					vysl += "}";
					if (i - 1 >= 0)
					{
						vysl += ", ";
					}
				}
				if (bodky[1].info.bitSirka != 1)
				{
					vysl += "}";
				}
			}
			else if (bodky[0].info.bitSirka < bodky[1].info.bitSirka)
			{
				vysl += "{";
				int index = bodky[0].info.bitSirka;
				int zostavajucaBitDlzka = bodky[0].info.bitDlzka;
				
				for (int i = bodky[1].info.bitSirka - 1 ; i >= 0 ; i--)
				{
					vysl += bodky[0].getMenoIndex(index-1) + "[" + (zostavajucaBitDlzka - 1) + ":" + (zostavajucaBitDlzka - + bodky[1].info.bitDlzka) + "]";
					if (i - 1 >= 0)
					{
						vysl += ", ";
					}
					zostavajucaBitDlzka -= bodky[1].info.bitDlzka;
					if (zostavajucaBitDlzka == 0)
					{
						zostavajucaBitDlzka = bodky[0].info.bitDlzka;
						index--;
					}
				}
				vysl += "}";
			}
			vysl += ";\r\n";
			return vysl;
		}
	}
	
	public class OtocenieBitov : Operacia
	{
		
		public OtocenieBitov(int nX, int nY, String nId)
			:base(nX, nY, nId, "reverse")
		{}
		
		protected override string programModulu()
		{
			String vysl = "";
			if (bodky[0].info.bitSirka == 1)
			{
				for (int i = 0 ; i < bodky[0].info.bitDlzka ; i++)
				{
					vysl += tabulator(2) + bodky[1].info.meno + "[" + i + "]" + " = " + bodky[0].info.meno + "[" + (bodky[0].info.bitDlzka - i - 1) + "];\r\n";
				}
				return vysl;
			}
			for (int i = 0 ; i < bodky[0].info.bitSirka ; i++)
			{
				vysl += tabulator(2) + bodky[1].getMenoIndex(i) + " = " + bodky[0].getMenoIndex(bodky[0].info.bitSirka - i - 1) + ";\r\n";
			}
			return vysl;
		}
	}
	
	public class PouzitieModulu : Operacia
	{
		public PouzitieModulu(int nX, int nY, String nId, String nOp)
			:base(nX, nY, nId, nOp)
		{
		}
		
		public void aktualizujVstupy()
		{
			int pocetVstupov = Projekt.DATABAZA[op].pocetVstupov();
			int pocetVystupov = Projekt.DATABAZA[op].pocetVystupov();
			rx = Math.Max(Math.Max(18, (10 + 8*pocetVstupov)), 10 + 8*pocetVystupov);
			dx = 2*rx;
			ciary = new Ciara[pocetVstupov];
			ObjektBodka[] noveBodky  = new ObjektBodka[pocetVstupov+pocetVystupov];
			for (int i = 0, j = -rx + 14 ; i < pocetVstupov && i < bodky.Length ; i++, j += dx / (pocetVstupov+1))
			{
				noveBodky[i] = new ObjektBodka(j, -ry - 8, Brushes.Red, this, i, Projekt.DATABAZA[op].vstupy[i]);
			}
			for (int i = bodky.Length-pocetVystupov, j = pocetVstupov ; i < bodky.Length ; i++, j++)
			{
				noveBodky[j] = bodky[i];
				noveBodky[j].id = j;
			}
			bodky = noveBodky;
		}
		
		public void aktualizujVystupy()
		{
			int pocetVstupov = Projekt.DATABAZA[op].pocetVstupov();
			int pocetVystupov = Projekt.DATABAZA[op].pocetVystupov();
			rx = Math.Max(Math.Max(18, (10 + 8*pocetVstupov)), 10 + 8*pocetVystupov);
			dx = 2*rx;
			ObjektBodka[] noveBodky = new ObjektBodka[pocetVstupov+pocetVystupov];
			for (int i = 0 ; i < pocetVstupov && i < bodky.Length ; i++)
			{
				noveBodky[i] = bodky[i];
			}
			for (int i = pocetVstupov, j = -4 ; i < bodky.Length ; i++, j += dx / (pocetVystupov+1))
			{
				noveBodky[i] = new ObjektBodka(j, ry, Brushes.Black, this, i, Projekt.DATABAZA[op].vystupy[i-pocetVstupov]);
			}
			bodky = noveBodky;
		}
		
		public override void kresli(Graphics g)
		{
			upravSirkuObjektu(g);
			base.kresli(g);
		}
		
		protected override String dwKodPrikaz()
		{
			return " use " + op;
		}
	}
	
	public class IncDecOperacia : Operacia
	{
		public IncDecOperacia(int nX, int nY, String nId, String nOp)
			:base(nX, nY, nId, nOp)
		{
		}
		
		protected override String programModulu()
		{
			return tabulator(2) + bodky[ciary.Length].info.meno + " = " + bodky[0].info.meno + " " + Projekt.DATABAZA[op].znamienko[0] + " 1;\r\n";
		}
	}
	
	public class IoOperacia : Operacia
	{
		public IoOperacia(int nX, int nY, String nId, String nOp, BodkaInfo b = null)
			:base(nX, nY, nId, nOp)
		{
			bg = Brushes.LightBlue;
			if (b != null)
			{
				bodky[0].info = b;	
			}
		}
		
		public override void nastavEditovaciePrvky(Panel panel)
		{
			base.nastavEditovaciePrvky(panel);
			
			panel.Controls.Add(vytvorLabel("Meno:", 3, 40));
			
			ComboBox comboBox = new ComboBox();
			comboBox.Name = "comboBox";
			nastavUdajeObjektuRozhrania(comboBox, bodky[0].info.meno, 46, 94);
			panel.Controls.Add(comboBox);
			zvysYRozhrania();
			
			panel.Controls.Add(vytvorLabel("Bit. šírka: " + bodky[0].info.bitSirka, 3, 117));
			zvysYRozhrania();
			
			panel.Controls.Add(vytvorLabel("Bit. dĺžka: " + bodky[0].info.bitDlzka, 3, 117));
			zvysYRozhrania();
		}
		
		public override string getText()
		{
			return bodky[0].info.meno;
		}
		
		public override void kresli(Graphics g)
		{
			upravSirkuObjektu(g);
			base.kresli(g);
		}
		
		protected override String dwKodParametre()
		{
			return " " + bodky[0].info.meno;
		}
		
		public override String generujInit()
		{
			return "";
		}
		
		public override String getLucidZapisovanieDat(int b)
		{
			return bodky[b].info.meno;
		}
		
		public override String getLucidCitanieDat(int b)
		{
			return bodky[b].info.meno;
		}
		
		public override bool vlastnyModul()
		{
			return false;
		}
	}
		
}
