using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DataWolf
{
	public class Objekt
	{
		public String id;
		public String op;
		
		public ObjektBodka[] bodky;
		public Ciara[] ciary;
		
		public IfBlok ifBlok;
		
		protected Brush bg;
		protected int x, y;
		protected int rx, ry;
		protected int dx, dy;
		protected int panelY;
		protected List<Label> popisky;
		protected List<TextBox> textBoxy;
		protected Pen stav;
		protected Modul m;
		
		static Font font = new Font(FontFamily.GenericSansSerif, 13, FontStyle.Regular);
		static TextFormatFlags flags = 	TextFormatFlags.HorizontalCenter |
        								TextFormatFlags.VerticalCenter |
										TextFormatFlags.WordBreak;
		
		public Objekt(int nX, int nY, String nId, String nOp)
		{
			x = nX;
			y = nY;
			id = nId;
			op = nOp;
			bg = Brushes.White;
			stav = Pens.Black;
			ciary = new Ciara[0];
		}
		
		public bool editovanie()
		{
			if (stav == Pens.Black)
			{
				return false;
			}
			return true;
		}
		
		public void setM(Modul nM)
		{
			m = nM;
		}
		
		public int getX()
		{
			return x;
		}
		
		public int getY()
		{
			return y;
		}
		
		public virtual void setX(int nX)
		{
			x = nX;
			if (editovanie())
			{
				textBoxy[1].Text = Convert.ToString(x);
			}
		}
		
		public virtual void setY(int nY)
		{
			y = nY;
			if (editovanie())
			{
				textBoxy[2].Text = Convert.ToString(y);
			}
		}
		
		public virtual int getMaxX()
		{
			return x + dx;
		}
		
		public virtual int getMaxY()
		{
			return y + dy;
		}
		
		public virtual String getText()
		{
			return Projekt.DATABAZA[op].znamienko;
		}
		
		public virtual void kresli(Graphics g)
		{
			Rectangle rect = new Rectangle(x - rx, y - ry, dx, dy);
			g.FillRectangle(bg, rect);
			g.DrawRectangle(stav, rect);
			for (int i = 0 ; i < bodky.Length ; i++)
			{
				bodky[i].kresli(g);
			}
			TextRenderer.DrawText(g, getText(), font, rect, Color.Black, flags);
		}
		
		public bool isClick(int x, int y)
		{
			return x <= this.x + rx && x >= this.x - rx && y <= this.y + ry && y >= this.y - ry;
		}
		
		protected void upravSirkuObjektu(Graphics g)
		{
			var size = g.MeasureString(getText(), font);
			dx = Convert.ToInt32(size.Width) + 10;
			rx = dx/2;
		}

		public virtual void zrusEditovaciePrvky(Panel panel)
		{
			if (!editovanie())
			{
				return;
			}
			panel.Controls.Clear();
			stav = Pens.Black;
			foreach (Label l in popisky)
			{
				l.Dispose();
			}
			foreach (TextBox t in textBoxy)
			{
				t.Dispose();
			}
		}
		
		protected Label vytvorLabel(String popis, int x, int sirka)
		{
			Label l = new Label();
			nastavUdajeObjektuRozhrania(l, popis, x, sirka);
			popisky.Add(l);
			return l;
		}
		
		protected TextBox vytvorTextBox(String text, int x, int sirka)
		{
			TextBox t = new TextBox();
			nastavUdajeObjektuRozhrania(t, text, x, sirka);
			textBoxy.Add(t);
			return t;
		}
		
		protected void nastavUdajeObjektuRozhrania(Control c, String text, int x, int sirka)
		{
			c.Location = new Point(x, panelY);
			c.Text = text;
			c.Size = new Size(sirka, 20);
		}
		
		protected void zvysYRozhrania()
		{
			panelY += 23;
		}
		
		public virtual void nastavEditovaciePrvky(Panel panel)
		{
			popisky = new List<Label>();
			textBoxy = new List<TextBox>();
			panelY = 3;
			stav = Pens.Gold;
						
			panel.Controls.Add(vytvorLabel(Projekt.DATABAZA[op].text, 3, 90));
			zvysYRozhrania();
			
			panel.Controls.Add(vytvorLabel("ID", 3, 30));
			panel.Controls.Add(vytvorTextBox(id, 36, 84));
			zvysYRozhrania();
			
			panel.Controls.Add(vytvorLabel("Umiestnenie", 3, 90));
			zvysYRozhrania();
			
			panel.Controls.Add(vytvorLabel("x:", 50, 25));
			panel.Controls.Add(vytvorTextBox(Convert.ToString(x), 78, 42));
			zvysYRozhrania();
			
			panel.Controls.Add(vytvorLabel("y:", 50, 25));
			panel.Controls.Add(vytvorTextBox(Convert.ToString(y), 78, 42));
			zvysYRozhrania();
			
			for (int i = 0; i < bodky.Length ; i++)
			{
				if (i == 0 && ciary.Length != 0)
				{
					panel.Controls.Add(vytvorLabel("Vstupy:", 3, 54));
					zvysYRozhrania();
				}
				if (i == 0 || i == ciary.Length)
				{
					panel.Controls.Add(vytvorLabel("Výstupy:", 3, 54));
					zvysYRozhrania();
				}
				
				panel.Controls.Add(vytvorLabel(bodky[i].info.meno + ":", 10, 100));
				zvysYRozhrania();
				
				panel.Controls.Add(vytvorLabel("Bit. šírka: " + bodky[i].info.bitSirka, 17, 80));
				zvysYRozhrania();
								
				panel.Controls.Add(vytvorLabel("Bit. dĺžka: " + bodky[i].info.bitDlzka, 17, 80));
				zvysYRozhrania();
			}
		}
		
		public String getId()
		{
			return id;
		}
		
		public void setId(String nId)
		{
			m.objektNoveId(id, nId);
			id = nId;
		}
		
		protected virtual void ulozEditovaneVlastnosti(object sender, EventArgs e)
		{
			setId(textBoxy[0].Text);
			setX(int.Parse(textBoxy[1].Text));
			setY(int.Parse(textBoxy[2].Text));
		}
		
		public void pridajOkTlacitko(MainForm main)
		{
			Button b = new Button();
			b.Name = "OkTlacitko";
			b.Location = new Point(3, panelY+10);
			b.Size = new Size(50, 20);
			b.Text = "OK";
			b.Click += ulozEditovaneVlastnosti;
			b.Click += main.aktualizujPlochu;
			main.getPanelVlastnosti().Controls.Add(b);
		}
		
		public virtual bool aktualizujStav()
		{
			bool zmena = false;
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
			}
			return zmena;
		}
		
		protected virtual String dwKodId()
		{
			return "{" + id + "}";
		}
		
		protected virtual String dwKodPrikaz()
		{
			return " " + op;
		}
		
		protected virtual String dwKodParametre()
		{
			return "";
		}
		
		protected virtual String dwKodSuradnice()
		{
			return " # " + x + " " + y + "\r\n";
		}
		
		public String generujDWKod()
		{
			return dwKodId() + dwKodPrikaz() + dwKodParametre() + dwKodSuradnice();
		}
		
		public virtual String generujInit()
		{
			String vysl = "";
			for (int i = 0 ; i < ciary.Length ; i++)
			{
				vysl += tabulator(2) + id + "." + bodky[i].info.meno + " = ";
				if (bodky[i].info.bitSirka == 1)
				{
					vysl += "0";
				}
				else
				{
					vysl += bodky[i].info.bitSirka + "x{{"+ bodky[i].info.bitDlzka +"hxx}}";
				}
				vysl += ";\r\n";
			}
			return vysl;
		}
		
		public virtual String getLucidZapisovanieDat(int b)
		{
			return id + "." + bodky[b].info.meno;
		}
		
		public virtual String getLucidCitanieDat(int b)
		{
			return id + "." + bodky[b].info.meno;
		}
		
		public virtual String generujLucidKod(Modul f, int tab)
		{
			String vysl = "";
			for (int i = 0 ; i < ciary.Length ; i++)
			{
				vysl += tabulator(tab);
				vysl += getLucidZapisovanieDat(i);
				vysl += " = ";
				vysl += ciary[i].bodkaVystup.obj.getLucidCitanieDat(ciary[i].bodkaVystup.id);
				vysl += ";\r\n";
			}
			return vysl;
		}
		
		public static String tabulator(int tab)
		{
			String vysl = "";
			for (int i = 0 ; i < tab ; i++)
			{
				vysl += "\t";
			}
			return vysl;
		}
		
		public virtual bool vlastnyModul()
		{
			return true;
		}
		
		public virtual String typModulu()
		{
			return ".luc";
		}
		
		public virtual void vytvorModul(String cesta, String meno)
		{
			String vysl = "";
			vysl += "module " + meno + " (\r\n";
			for (int i = 0 ; i < ciary.Length ; i++)
			{
				vysl += tabulator(2) + "input " + bodky[i].info.meno + bitovaSirkaAkViacAko1(bodky[i].info.bitSirka) + "[" + bodky[i].info.bitDlzka + "],\r\n";
			}
			vysl += tabulator(2) + "output " + bodky[ciary.Length].info.meno + bitovaSirkaAkViacAko1(bodky[ciary.Length].info.bitSirka) + "[" + bodky[ciary.Length].info.bitDlzka + "]\r\n";
			vysl += tabulator(1) + ") {\r\n\r\n";
			vysl += tabulator(1) + "always {\r\n";
			vysl += programModulu();
			vysl += tabulator(1) + "}\r\n}";
			System.IO.File.WriteAllText(@cesta+"\\"+meno+typModulu(), vysl);
		}
		
		public virtual String menoModulu()
		{
			String meno = op;
			for (int i = 0 ; i < Projekt.DATABAZA[op].pocetVstupov() ; i++)
			{
				if (bodky[i].info.bitSirkaZmena)
				{
					meno += "_" + bodky[i].info.bitSirka;
				}
				if (bodky[i].info.bitDlzkaZmena)
				{
					meno += "_" + bodky[i].info.bitDlzka;
				}
			}
			return meno;
		}
		
		protected virtual String programModulu()
		{
			if (ciary.Length == 1)
			{
				return tabulator(2) + bodky[ciary.Length].info.meno + " = " + Projekt.DATABAZA[op].znamienko + " " + bodky[0].info.meno + ";\r\n";
			}
			if (ciary.Length == 2)
			{
				String znamienko = Projekt.DATABAZA[op].znamienko;
				if (znamienko == "&&")
				{
					znamienko = "&";
				}
				else if (znamienko == "&&&&")
				{
					znamienko = "&&";
				}
				return tabulator(2) + bodky[ciary.Length].info.meno + " = " + bodky[0].info.meno + " " + znamienko + " " + bodky[1].info.meno + ";\r\n";
			}
			return "";
		}
		
		public static String bitovaSirkaAkViacAko1(int bitSirka)
		{
			if (bitSirka == 1)
			{
				return "";
			}
			return "[" + bitSirka + "]";
		}
	}
}
