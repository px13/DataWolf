using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DataWolf
{
	public partial class InfoOkno: Form
	{
		protected MainForm main;
		protected Modul f;
		
		public InfoOkno(MainForm nMain)
		{
			InitializeComponent();
			main = nMain;
		}
		
		protected List<ElementPanel> elementy;
		protected int poslednyIndex;
		
		public virtual void init()
		{
			if (f != null)
			{
				uloz();
				foreach (ElementPanel e in elementy)
				{
					e.Dispose();
				}
			}
			f = main.modul;
		}
		
		void InfoOknoLoad(object sender, EventArgs e)
		{
			vytvorElementPanelInfo();
			init();
		}
		
		protected Label info1;
		Label info2;
		Label info3;
		
		void vytvorElementPanelInfo()
		{
			info1 = new Label();
			info1.Location = new Point(15, 15);
			info1.Name = "info1";
			info1.Size = new Size(124, 29);
			info1.Text = "";
			info1.TextAlign = ContentAlignment.MiddleCenter;
			Controls.Add(info1);
			
			info2 = new Label();
			info2.Location = new Point(145, 15);
			info2.Name = "info2";
			info2.Size = new Size(77, 29);
			info2.Text = "Bitová šírka";
			info2.TextAlign = ContentAlignment.MiddleCenter;
			Controls.Add(info2);
			
			info3 = new Label();
			info3.Location = new Point(228, 15);
			info3.Name = "info3";
			info3.Size = new Size(77, 29);
			info3.Text = "Bitová dĺžka";
			info3.TextAlign = ContentAlignment.MiddleCenter;
			Controls.Add(info3);
		}
		
		protected ElementPanel vytvorElementPanel(Info info, int i)
		{
			int y = vypocetY(i);
			TextBox meno = new TextBox();
			meno.Location = new Point(15, y);
			meno.Name = "id"+i;
			meno.Size = new Size(124, 20);
			meno.Text = info.meno;
			if (!editovanie())
			{
				meno.Enabled = false;
			}
			Controls.Add(meno);
			
			TextBox bitSirka = new TextBox();
			bitSirka.Location = new Point(166, y);
			bitSirka.Name = "bitSirka"+i;
			bitSirka.Size = new Size(37, 20);
			bitSirka.Text = Convert.ToString(info.bitSirka);
			if (!editovanie())
			{
				bitSirka.Enabled = false;
			}
			Controls.Add(bitSirka);
			
			TextBox bitDlzka = new TextBox();
			bitDlzka.Location = new Point(251, y);
			bitDlzka.Name = "bitDlzka"+i;
			bitDlzka.Size = new Size(37, 20);
			bitDlzka.Text = Convert.ToString(info.bitDlzka);
			if (!editovanie())
			{
				bitDlzka.Enabled = false;
			}
			Controls.Add(bitDlzka);
			
			Button zmaz = new Button();
			if (editovanie())
			{
				zmaz.Location = new Point(312, y);
				zmaz.Name = Convert.ToString(i);
				zmaz.Size = new Size(43, 20);
				zmaz.Text = "Zmaž";
				zmaz.UseVisualStyleBackColor = true;
				zmaz.Click += ZmazClick;
				Controls.Add(zmaz);
			}
			
			return new ElementPanel(info, meno, bitSirka, bitDlzka, zmaz);
		}
		
		void ZmazClick(object sender, EventArgs e)
		{
			Button b = (Button) sender;
			int i = int.Parse(b.Name);
			elementy[i].Dispose();
			elementy.RemoveAt(i);
			for (; i < elementy.Count ; i++)
			{
				elementy[i].meno.Location = new Point(elementy[i].meno.Location.X, elementy[i].meno.Location.Y - 23);
				elementy[i].bitSirka.Location = new Point(elementy[i].bitSirka.Location.X, elementy[i].bitSirka.Location.Y - 23);
				elementy[i].bitDlzka.Location = new Point(elementy[i].bitDlzka.Location.X, elementy[i].bitDlzka.Location.Y - 23);
				elementy[i].zmaz.Location = new Point(elementy[i].zmaz.Location.X, elementy[i].zmaz.Location.Y - 23);
			}
			aktualizujPolohuTlacitiek();
		}
		
		protected virtual Info vytvorNovyElement()
		{
			return null;
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Info elem = vytvorNovyElement();
			elementy.Add(vytvorElementPanel(elem, poslednyIndex));
			poslednyIndex++;
			aktualizujPolohuTlacitiek();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			uloz();
		}
		
		protected virtual void InfoOknoFormClosing(object sender, FormClosingEventArgs e)
		{
		}
		
		protected virtual void uloz()
		{
		}
		
		protected void aktualizujPolohuTlacitiek()
		{
			int y = vypocetY(elementy.Count);
			button2.Location = new Point(15, y);
			button1.Location = new Point(125, y);
		}
		
		protected virtual bool editovanie()
		{
			return true;
		}
		
		int vypocetY(int i)
		{
			return 47 + 23*i;
		}
		
	}
	
	public class RegistreOkno : InfoOkno
	{
		public RegistreOkno(MainForm nMain)
			:base(nMain)
		{
		}
		
		public override void init()
		{
			base.init();
			Text = "Registre - " + f.meno;
			info1.Text = "Register";
			
			elementy = new List<ElementPanel>();
			poslednyIndex = 0;
			for (int i = 0 ; i < f.registre.Count ; i++, poslednyIndex++)
			{
				elementy.Add(vytvorElementPanel(f.registre[i], i));
			}
			aktualizujPolohuTlacitiek();
		}
		
		protected override Info vytvorNovyElement()
		{
			return new RegisterInfo("novy_register"+poslednyIndex, 1, 32);
		}
		
		protected override void uloz()
		{
			f.registre = new List<RegisterInfo>();
			foreach (ElementPanel elem in elementy)
			{
				elem.uloz();
				f.registre.Add((RegisterInfo) elem.info);
			}
			main.aktualizujPlochu();
		}
		
		protected override void InfoOknoFormClosing(object sender, FormClosingEventArgs e)
		{
			base.InfoOknoFormClosing(sender, e);
			main.registreOkno = null;
		}
	}
	
	public class VstupyOkno : InfoOkno
	{
		public VstupyOkno(MainForm nMain)
			:base(nMain)
		{
		}
		
		public override void init()
		{
			base.init();
			Text = "Vstupy - " + f.meno;
			info1.Text = "Vstup";
			if (editovanie())
			{
				button1.Show();
				button2.Show();
			}
			else
			{
				button1.Hide();
				button2.Hide();
			}
			
			elementy = new List<ElementPanel>();
			poslednyIndex = 0;
			for (int i = 0 ; i < f.vstupy.Count ; i++, poslednyIndex++)
			{
				elementy.Add(vytvorElementPanel(f.vstupy[i], i));
			}
			aktualizujPolohuTlacitiek();
		}
		
		protected override Info vytvorNovyElement()
		{
			return new BodkaInfo("novy_vstup"+poslednyIndex, 1, false, 32, false);
		}
		
		protected override void uloz()
		{
			List<BodkaInfo> temp = new List<BodkaInfo>();
			foreach (ElementPanel elem in elementy)
			{
				elem.uloz();
				temp.Add((BodkaInfo) elem.info);
			}
			if (f.aktualizujVstupy(temp))
			{
				main.aktualizujVstupyModulu(f.meno);
			}
			main.aktualizujPlochu();
		}
		
		protected override void InfoOknoFormClosing(object sender, FormClosingEventArgs e)
		{
			base.InfoOknoFormClosing(sender, e);
			main.vstupyOkno = null;
		}
		
		protected override bool editovanie()
		{
			if (f.meno == "main")
			{
				return false;
			}
			return true;
		}
	}
	
	public class VystupyOkno : InfoOkno
	{
		public VystupyOkno(MainForm nMain)
			:base(nMain)
		{
		}
		
		public override void init()
		{
			base.init();
			Text = "Výstupy - " + f.meno;
			info1.Text = "Výstup";
			
			if (editovanie())
			{
				button1.Show();
				button2.Show();
			}
			else
			{
				button1.Hide();
				button2.Hide();
			}
			
			elementy = new List<ElementPanel>();
			poslednyIndex = 0;
			for (int i = 0 ; i < f.vystupy.Count ; i++, poslednyIndex++)
			{
				elementy.Add(vytvorElementPanel(f.vystupy[i], i));
			}
			aktualizujPolohuTlacitiek();
		}
		
		protected override Info vytvorNovyElement()
		{
			return new BodkaInfo("novy_vystup"+poslednyIndex, 1, false, 32, false);
		}
		
		protected override void uloz()
		{
			List<BodkaInfo> temp = new List<BodkaInfo>();
			foreach (ElementPanel elem in elementy)
			{
				elem.uloz();
				temp.Add((BodkaInfo) elem.info);
			}
			if (f.aktualizujVystupy(temp))
			{
				main.aktualizujVystupyModulu(f.meno);
			}
			main.aktualizujPlochu();
		}
		
		protected override void InfoOknoFormClosing(object sender, FormClosingEventArgs e)
		{
			base.InfoOknoFormClosing(sender, e);
			main.vystupyOkno = null;
		}
		
		protected override bool editovanie()
		{
			if (f.meno == "main")
			{
				return false;
			}
			return true;
		}
	}
	
}

public class ElementPanel
{
	public DataWolf.Info info;
	public TextBox meno;
	public TextBox bitSirka;
	public TextBox bitDlzka;
	public Button zmaz;
	
	public ElementPanel(DataWolf.Info nInfo, TextBox nMeno, TextBox nBitSirka, TextBox nBitDlzka, Button nZmaz)
	{
		info = nInfo;
		meno = nMeno;
		bitSirka = nBitSirka;
		bitDlzka = nBitDlzka;
		zmaz = nZmaz;
	}
	
	public void uloz()
	{
		info.meno = meno.Text;
		info.bitSirka = int.Parse(bitSirka.Text);
		info.bitDlzka = int.Parse(bitDlzka.Text);
	}
	
	public void Dispose()
	{
		meno.Dispose();
		bitSirka.Dispose();
		bitDlzka.Dispose();
		zmaz.Dispose();
	}
	
}
