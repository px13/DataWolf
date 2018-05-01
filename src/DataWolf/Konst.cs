using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DataWolf
{
	public class Konst : Objekt
	{
		public String[] values;
		
		public Konst(int nX, int nY, String nId, String nOp, int nBitSirka, int nBitDlzka, String[] nValues)
			: base(nX, nY, nId, nOp)
		{
			bg = Brushes.Gray;
			rx = 15;
			ry = 15;
			dx = 30;
			dy = 30;
			bodky = new []{new ObjektBodka(-4, ry, Brushes.Black, this, 0, Projekt.DATABAZA[op].vystupy[0])};
			setBitSirka(nBitSirka);
			setBitDlzka(nBitDlzka);
			values = new String[getBitSirka()];
			for (int i = 0 ; i < getBitSirka() ; i++)
			{
				values[i] = "0";
				if (nValues != null && i < nValues.Length)
				{
					values[i] = nValues[i];
				}
			}
			ciary = new Ciara[0];
		}
		
		public int getBitSirka()
		{
			return bodky[0].info.bitSirka;
		}
		
		public void setBitSirka(int nBitSirka)
		{
			bodky[0].info.bitSirka = nBitSirka;
		}
		
		public int getBitDlzka()
		{
			return bodky[0].info.bitDlzka;
		}
		
		public void setBitDlzka(int nBitDlzka)
		{
			bodky[0].info.bitDlzka = nBitDlzka;
		}
		
		public void zmenaBitSirky()
		{
			List<String> temp = new List<String>();
			for (int i = 0 ; i < values.Length ; i++)
			{
				if (i >= getBitSirka()) break;
				temp.Add(values[i]);
			}
			for (int i = values.Length ; i < getBitSirka() ; i++)
			{
				temp.Add("0");
			}
			values = temp.ToArray();
		}
		
		public override string getText()
		{
			String text = id;
			if (getBitSirka() == 1)
			{
				text = values[0];
			}
			return text;
		}
		
		public override void kresli(Graphics g)
		{
			upravSirkuObjektu(g);
			base.kresli(g);
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
			
			panel.Controls.Add(vytvorLabel("Hodnoty:", 3, 54));
			zvysYRozhrania();
			
			for (int i = 0 ; i < getBitSirka() ; i++)
			{
				panel.Controls.Add(vytvorLabel(Convert.ToString(i) + ":", 3, 30));
				panel.Controls.Add(vytvorTextBox(values[i], 36, 64));
				zvysYRozhrania();
			}
		}
		
		protected override void ulozEditovaneVlastnosti(object sender, EventArgs e)
		{
			base.ulozEditovaneVlastnosti(sender, e);
			int nBitSirka = int.Parse(textBoxy[3].Text);
			if (getBitSirka() != nBitSirka)
			{
				setBitSirka(nBitSirka);
				zmenaBitSirky();
			}
			setBitDlzka(int.Parse(textBoxy[4].Text));
			for (int i = 5 ; i < textBoxy.Count ; i++)
			{
				if (i-5 >= getBitSirka()) break;
				values[i-5] = textBoxy[i].Text;
			}
			Panel panel = (Panel) textBoxy[0].Parent;
			Button b = (Button) panel.Controls.Find("OkTlacitko", false)[0];
			zrusEditovaciePrvky(panel);
			nastavEditovaciePrvky(panel);
			b.Location = new Point(3, panelY+10);
			panel.Controls.Add(b);
		}
		
		protected override string dwKodParametre()
		{
			String param = " (";
			for (int i = 0 ; i < getBitSirka() ; i++)
			{
				param += values[i];
				if (i+1 != getBitSirka()) param += " ";
			}
			param += ")";
			param += " # " + getBitSirka() + " " + getBitDlzka();
			return param;
		}
		
		public override String getLucidCitanieDat(int b)
		{
			String vysl = "";
			if (getBitSirka() > 1)
			{
				vysl += "{";
			}
			for (int i = getBitSirka() - 1 ; i >= 0 ; i--)
			{
				if (cisloMaRozsirenyTvar(values[i]))
				{
					vysl += getBitDlzka() + values[i];
				}
				else
				{
					vysl += getBitDlzka() + "d" + values[i];
				}
				if (i - 1 >= 0)
				{
					vysl += ", ";
				}
			}
			if (getBitSirka() > 1)
			{
				vysl += "}";
			}
			return vysl;
		}
 
		public override bool vlastnyModul()
		{
			return false;
		}
		
		bool cisloMaRozsirenyTvar(String cislo)
		{
			if (cislo.IndexOf('h') >= 0 || cislo.IndexOf('b') >= 0 || cislo.IndexOf('d') >= 0)
			{
				return true;
			}
			return false;
		}
		
	}
}
