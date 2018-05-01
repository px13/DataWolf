using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DataWolf
{

	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}
		
		public Projekt projekt;
		public Modul modul;
		
		public RegistreOkno registreOkno;
		public VstupyOkno vstupyOkno;
		public VystupyOkno vystupyOkno;
		
		String click;
		String tahaj;
		String clickRozmery;
		ObjektBodka clickBodka;
		TempCiara tempCiara;
		CiaraBodka clickMalaBodka;
		
		int noveRozmeryPaneluX;
		int noveRozmeryPaneluY;
		int clickX, clickY;
		
		ContextMenuStrip contextMenuStrip;
		
		void MainFormLoad(object sender, EventArgs e)
		{
	        contextMenuStrip = new ContextMenuStrip();
	        contextMenuStrip.Opening += vytvorContextMenu;
	        tabControl1.ContextMenuStrip = contextMenuStrip;
	        KeyPreview = true;
	        
	        panel1.AutoScroll = true;
	        
	        ResetProjekt();
	        ResetModul();
		}
		
		void vytvorContextMenu(object sender, System.ComponentModel.CancelEventArgs e)
	    {
	        contextMenuStrip.Items.Clear();
			
	        if (modul == null) return;
	        
	        ToolStripMenuItem level1;
	        ToolStripMenuItem[] level2;
	        ToolStripMenuItem[] level3;
	        
	        //----------------------------------
	        
	        level2 = new ToolStripMenuItem[2];
	        
	        level2[0] = new ToolStripMenuItem(Projekt.DATABAZA["const"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "const"));
	        level2[1] = new ToolStripMenuItem(Projekt.DATABAZA["index"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "index"));
	        
	        level1 = new ToolStripMenuItem("Hodnoty", null, level2);
	        contextMenuStrip.Items.Add(level1);
	        	
	        //----------------------------------
	        
	        level2 = new ToolStripMenuItem[2];
	        
        	level3 = new ToolStripMenuItem[modul.vstupy.Count];
        	for (int j = 0 ; j < modul.vstupy.Count ; j++)
        	{
        		BodkaInfo vstup = modul.vstupy[j];
        		level3[j] = new ToolStripMenuItem(modul.vstupy[j].meno, null, (sender2, e2) => vytvorIoOperaciu(sender2, e2, "in", vstup));
        	}
        	level2[0] = new ToolStripMenuItem(Projekt.DATABAZA["in"].text, null, level3);
        	level3 = new ToolStripMenuItem[modul.vystupy.Count];
        	for (int j = 0 ; j < modul.vystupy.Count ; j++)
        	{
        		BodkaInfo vystup = modul.vystupy[j];
        		level3[j] = new ToolStripMenuItem(modul.vystupy[j].meno, null, (sender2, e2) => vytvorIoOperaciu(sender2, e2, "out", vystup));
        	}
        	level2[1] = new ToolStripMenuItem(Projekt.DATABAZA["out"].text, null, level3);
	        
	        
	        level1 = new ToolStripMenuItem("IO operácie", null, level2);
	        contextMenuStrip.Items.Add(level1);
	        
	        //----------------------------------
	        
	        level2 = new ToolStripMenuItem[7];
	        
	        level2[0] = new ToolStripMenuItem(Projekt.DATABAZA["add"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "add"));
	        level2[1] = new ToolStripMenuItem(Projekt.DATABAZA["sub"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "sub"));
	        level2[2] = new ToolStripMenuItem(Projekt.DATABAZA["mul"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "mul"));
	        level2[3] = new ToolStripMenuItem(Projekt.DATABAZA["div"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "div"));
	        level2[4] = new ToolStripMenuItem(Projekt.DATABAZA["mod"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "mod"));
	        level2[5] = new ToolStripMenuItem(Projekt.DATABAZA["inc"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "inc"));
	        level2[6] = new ToolStripMenuItem(Projekt.DATABAZA["dec"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "dec"));
	        
	        level1 = new ToolStripMenuItem("Aritmetické operácie", null, level2);
	        contextMenuStrip.Items.Add(level1);
	        
	        //----------------------------------
	        
	        level2 = new ToolStripMenuItem[6];
	        
	        level2[0] = new ToolStripMenuItem(Projekt.DATABAZA["bit_and"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "bit_and"));
	        level2[1] = new ToolStripMenuItem(Projekt.DATABAZA["bit_or"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "bit_or"));
	        level2[2] = new ToolStripMenuItem(Projekt.DATABAZA["bit_xor"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "bit_xor"));
	        level2[3] = new ToolStripMenuItem(Projekt.DATABAZA["bit_not"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "bit_not"));
	        level2[4] = new ToolStripMenuItem(Projekt.DATABAZA["shift_left"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "shift_left"));
	        level2[5] = new ToolStripMenuItem(Projekt.DATABAZA["shift_right"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "shift_right"));
	        
	        level1 = new ToolStripMenuItem("Bitové operácie", null, level2);
	        contextMenuStrip.Items.Add(level1);
	        
	        //----------------------------------
	        
	        level2 = new ToolStripMenuItem[3];
	        
	        level2[0] = new ToolStripMenuItem(Projekt.DATABAZA["log_and"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "log_and"));
	        level2[1] = new ToolStripMenuItem(Projekt.DATABAZA["log_or"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "log_or"));
	        level2[2] = new ToolStripMenuItem(Projekt.DATABAZA["log_not"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "log_not"));
	        
	        level1 = new ToolStripMenuItem("Logické operácie", null, level2);
	        contextMenuStrip.Items.Add(level1);
	        
	        //----------------------------------
	        
	        level2 = new ToolStripMenuItem[6];
	        
	        level2[0] = new ToolStripMenuItem(Projekt.DATABAZA["gt"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "gt"));
	        level2[1] = new ToolStripMenuItem(Projekt.DATABAZA["gteq"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "gteq"));
	        level2[2] = new ToolStripMenuItem(Projekt.DATABAZA["lt"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "lt"));
	        level2[3] = new ToolStripMenuItem(Projekt.DATABAZA["lteq"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "lteq"));
	        level2[4] = new ToolStripMenuItem(Projekt.DATABAZA["eq"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "eq"));
	        level2[5] = new ToolStripMenuItem(Projekt.DATABAZA["noteq"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "noteq"));
	        
	        level1 = new ToolStripMenuItem("Porovnávacie operácie", null, level2);
	        contextMenuStrip.Items.Add(level1);
	        
	        //----------------------------------
	        
	        level2 = new ToolStripMenuItem[4];
	        
	        level2[0] = new ToolStripMenuItem(Projekt.DATABAZA["concat"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "concat"));
	        level2[1] = new ToolStripMenuItem(Projekt.DATABAZA["reverse"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "reverse"));
	        level2[2] = new ToolStripMenuItem(Projekt.DATABAZA["change"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "change"));
	        level2[3] = new ToolStripMenuItem(Projekt.DATABAZA["if"].text, null, (sender2, e2) => vytvorOperaciu(sender2, e2, "if"));
	        
	        level1 = new ToolStripMenuItem("Ďalšie operácie", null, level2);
	        contextMenuStrip.Items.Add(level1);
	        
	        //----------------------------------
	        
	        List<String> pole;
	        
	        if (modul.registre.Count > 0)
	        {
		        level2 = new ToolStripMenuItem[3];
		        pole = new List<String>{"regC", "regZ", "regZ_index"};
		        for (int i = 0 ; i < pole.Count ; i++)
		        {
		        	String op = pole[i];
		        	level3 = new ToolStripMenuItem[modul.registre.Count];
		        	for (int j = 0 ; j < modul.registre.Count ; j++)
		        	{
		        		RegisterInfo reg = modul.registre[j];
		        		level3[j] = new ToolStripMenuItem(modul.registre[j].meno, null, (sender2, e2) => vytvorRegisterOperaciu(sender2, e2, op, reg));
		        	}
		        	level2[i] = new ToolStripMenuItem(Projekt.DATABAZA[pole[i]].text, null, level3);
		        }
		        
		        level1 = new ToolStripMenuItem("Register", null, level2);
		        contextMenuStrip.Items.Add(level1);
	        }
	        
	        //----------------------------------
	        
	        pole = new List<String>();
	        foreach (Modul m in projekt.moduly)
			{
				if (m.meno != "main" && modul != m)
				{
					pole.Add(m.meno);
				}
			}
	        
	        if (pole.Count > 0)
	        {
		        level2 = new ToolStripMenuItem[pole.Count];
		        for (int i = 0 ; i < pole.Count ; i++)
		        {
		        	String f = pole[i];
		        	level2[i] = new ToolStripMenuItem(Projekt.DATABAZA[pole[i]].text, null, (sender2, e2) => vytvorVolanieOperaciu(sender2, e2, f));
		        }
		        
		        level1 = new ToolStripMenuItem("Vlastné moduly", null, level2);
		        contextMenuStrip.Items.Add(level1);
	        }
	        
	    }
		
		void vytvorOperaciu(object sender, EventArgs e, String op)
		{
			Objekt temp;
			String id = modul.getOpId(op);
			if (op == "if")
			{
				IfBlok blok = new IfBlok(clickX, clickY, clickX+100, clickY+100, id);
				modul.ifBloky.Add(blok);
				temp = blok;
			}
			else if (op == "in" || op == "out")
			{
				temp = new IoOperacia(clickX, clickY, id, op);
			}
			else if (op == "const")
			{
				temp = new Konst(clickX, clickY, id, op, 1, 32, null);
			}
			else if (op == "change")
			{
				temp = new ZmenaBitov(clickX, clickY, id, 1, 32);
			}
			else if (op == "concat")
			{
				temp = new Zretazenie(clickX, clickY, id);
			}
			else if (op == "index")
			{
				temp = new Index(clickX, clickY, id);
			}
			else if (op == "reverse")
			{
				temp = new OtocenieBitov(clickX, clickY, id);
			}
			else if (op == "mod")
			{
				temp = new Modulo(clickX, clickY, id);
			}
			else if (op == "inc" || op == "dec")
			{
				temp = new IncDecOperacia(clickX, clickY, id, op);
			}
			else
			{
				temp = new Operacia(clickX, clickY, id, op);
			}
			temp.setM(modul);
			modul.objekty.Add(id, temp);
			aktualizujPlochu();
		}
		
		void vytvorVolanieOperaciu(object sender, EventArgs e, String op)
		{
			String id = modul.getOpId(op);
			modul.objekty.Add(id, new PouzitieModulu(clickX, clickY, id, op));
			modul.objekty[id].setM(modul);
			aktualizujPlochu();
		}
		
		void vytvorIoOperaciu(object sender, EventArgs e, String op, BodkaInfo info)
		{
			String id = modul.getOpId(op);
			modul.objekty.Add(id, new IoOperacia(clickX, clickY, id, op, info));
			modul.objekty[id].setM(modul);
			aktualizujPlochu();
		}
		
		void vytvorRegisterOperaciu(object sender, EventArgs e, String op, RegisterInfo r)
		{
			Register temp = null;
			String id = modul.getOpId(op);
			if (op == "regC")
			{
				temp = new RegisterC(clickX, clickY, id, r);
			}
			else if (op == "regZ")
			{
				temp = new RegisterZ(clickX, clickY, id, r);
			}
			else if (op == "regZ_index")
			{
				temp = new RegisterZIndex(clickX, clickY, id, r);
			}
			temp.setM(modul);
			modul.objekty.Add(id, temp);
			aktualizujPlochu();
		}
		
		void resetOpVlastnosti()
		{
			if (click == "") return;
			modul.objekty[click].zrusEditovaciePrvky(panel1);
			aktualizujPlochu();
		}
		
		public void registerComboBox()
		{
			ComboBox cb = (ComboBox) panel1.Controls.Find("comboBox", false)[0];
			foreach (RegisterInfo r in modul.registre)
			{
				cb.Items.Add(r.meno);
			}
			cb.SelectedIndexChanged += registerComboBoxSelectedIndexChanged;
		}
		
		void registerComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox cb = (ComboBox) sender;
			RegisterInfo r = modul.registre[cb.SelectedIndex];
			Register register = (Register) modul.objekty[click];
			register.r = r;
			register.bodky[0].info.bitDlzka = r.bitDlzka;
			if (!(register is RegisterZIndex))
			{
				register.bodky[0].info.bitSirka = r.bitSirka;
			}
			register.zrusEditovaciePrvky(panel1);
			register.nastavEditovaciePrvky(panel1);
			registerComboBox();
			register.pridajOkTlacitko(this);
			aktualizujPlochu();
		}
		
		public void vstupComboBox()
		{
			ComboBox cb = (ComboBox) panel1.Controls.Find("comboBox", false)[0];
			foreach (BodkaInfo b in modul.vstupy)
			{
				cb.Items.Add(b.meno);
			}
			cb.SelectedIndexChanged += vstupComboBoxSelectedIndexChanged;
		}
		
		void vstupComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox cb = (ComboBox) sender;
			BodkaInfo b = modul.vstupy[cb.SelectedIndex];
			IoOperacia io = (IoOperacia) modul.objekty[click];
			io.bodky[0].info = b;
			io.zrusEditovaciePrvky(panel1);
			io.nastavEditovaciePrvky(panel1);
			vstupComboBox();
			io.pridajOkTlacitko(this);
			aktualizujPlochu();
		}
		
		public void vystupComboBox()
		{
			ComboBox cb = (ComboBox) panel1.Controls.Find("comboBox", false)[0];
			foreach (BodkaInfo b in modul.vystupy)
			{
				cb.Items.Add(b.meno);
			}
			cb.SelectedIndexChanged += vystupComboBoxSelectedIndexChanged;
		}
		
		void vystupComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox cb = (ComboBox) sender;
			BodkaInfo b = modul.vystupy[cb.SelectedIndex];
			IoOperacia io = (IoOperacia) modul.objekty[click];
			io.bodky[0].info = b;
			io.zrusEditovaciePrvky(panel1);
			io.nastavEditovaciePrvky(panel1);
			vstupComboBox();
			io.pridajOkTlacitko(this);
			aktualizujPlochu();
		}
		
		public Panel getPanelVlastnosti()
		{
			return panel1;
		}
		
		public void aktualizujPlochu(object sender, EventArgs e)
		{
			tabControl1.SelectedTab.Controls[0].Invalidate();
		}
		
		public void aktualizujPlochu()
		{
			tabControl1.SelectedTab.Controls[0].Invalidate();
		}
		
		public void aktualizujVstupyModulu(String menoModulu)
		{
			foreach (Modul m in projekt.moduly)
			{
				foreach (Objekt o in m.objekty.Values)
				{
					if (o.op == menoModulu)
					{
						((PouzitieModulu) o).aktualizujVstupy();
					}
				}
			}
		}
		
		public void aktualizujVystupyModulu(String menoModulu)
		{
			foreach (Modul m in projekt.moduly)
			{
				foreach (Objekt o in m.objekty.Values)
				{
					if (o.op == menoModulu)
					{
						((PouzitieModulu) o).aktualizujVystupy();
					}
					for (int j = 0 ; j < o.ciary.Length ; j++)
					{
						if (o.ciary == null || o.ciary[j] == null) continue;
						if (o.ciary[j].bodkaVystup.obj.op == menoModulu)
						{
							o.ciary[j] = null;
							o.bodky[j].stav = Brushes.Red;
						}
					}
				}
			}
		}
		
		void nastavOpVlastnosti(Objekt o)
		{
			o.nastavEditovaciePrvky(panel1);
			if (o is Register)
			{
				registerComboBox();
			}
			else if (o is IoOperacia)
			{
				if (o.op == "in")
				{
					vstupComboBox();
				}
				else if (o.op == "out")
				{
					vystupComboBox();
				}
			}
			o.pridajOkTlacitko(this);
		}
		
		void MainFormMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				clickX = e.X;
				clickY = e.Y;
				clickBodka = null;
				tempCiara = null;
				aktualizujPlochu();
				return;
			}
			
			if (clickBodka != null)
			{
				foreach (var o in modul.objekty.Values)
				{
					foreach (var b in o.bodky)
					{
						if (clickBodka == b) continue;
						
						if (b.isClick(e.X, e.Y))
						{
							clickBodka.obj.ciary[clickBodka.id] = new Ciara(tempCiara.bodkaVstup, b, tempCiara.cesta);
							clickBodka.obj.bodky[clickBodka.id].stav = Brushes.LawnGreen;
							clickBodka = null;
							tempCiara = null;
							aktualizujPlochu();
							return;
						}
					}
					
					if (o.ciary == null) continue;
					
					foreach (var c in o.ciary)
					{
						if (c == null || c.cesta == null) continue;
						
						for (int i = 0 ; i < c.cesta.Count ; i++)
						{
							if (c.cesta[i].isClick(e.X, e.Y))
							{
								for (int j = i ; j < c.cesta.Count ; j++)
								{
									tempCiara.cesta.Add(c.cesta[j]);
									c.cesta[j].ciary.Add(tempCiara);
								}
								tempCiara.bodkaVystup = c.bodkaVystup;
								clickBodka.obj.ciary[clickBodka.id] = tempCiara;
								clickBodka.obj.bodky[clickBodka.id].stav = Brushes.LawnGreen;
								clickBodka = null;
								tempCiara = null;
								aktualizujPlochu();
								return;
							}
						}
					}
				}
				List<Ciara> temp = new List<Ciara> {tempCiara};
				CiaraBodka malaBodka = new CiaraBodka(e.X, e.Y, temp);
				tempCiara.cesta.Add(malaBodka);
				aktualizujPlochu();
				return;
			}
			
			if (clickMalaBodka != null)
			{
				clickMalaBodka.prepniAktivnost();
				aktualizujPlochu();
				modul.priradCiarovuBodkuIfBloku(clickMalaBodka);
				clickMalaBodka = null;
				return;
			}
						
			foreach (Objekt o in modul.objekty.Values)
			{
				if (o.isClick(e.X, e.Y))
				{
					resetOpVlastnosti();
					click = o.id;
					tahaj = o.id;
					nastavOpVlastnosti(o);
					return;
				}
				if (o is IfBlok)
				{
					IfBlok temp = (IfBlok) o;
					if (temp.isClick2(e.X, e.Y))
					{
						clickRozmery = o.id;
						return;	
					}
				}
				
				for (int j = 0 ; j < o.bodky.Length ; j++)
				{
					if (o.bodky[j].stav == Brushes.Black) continue;
					if (o.bodky[j].isClick(e.X, e.Y))
					{
						clickBodka = o.bodky[j];
						tempCiara = new TempCiara(clickBodka, new Bodka(e.X, e.Y, 4));
						return;
					}
				}
				
				if (o.ciary == null) continue;
				
				for (int j = 0 ; j < o.ciary.Length ; j++)
				{
					if (o.ciary[j] == null) continue;
					if (o.ciary[j].cesta == null) continue;
					
					for (int k = 0 ; k < o.ciary[j].cesta.Count ; k++)
					{
						if (o.ciary[j].cesta[k].isClick(e.X, e.Y))
						{
							clickMalaBodka = o.ciary[j].cesta[k];
							clickMalaBodka.prepniAktivnost();
							return;
						}
					}
				}
				
			}
			
			resetOpVlastnosti();
			
		}
		
		void MainFormMouseMove(object sender, MouseEventArgs e)
		{
			if (tahaj != "")
			{
				modul.objekty[click].setX(e.X);
				modul.objekty[click].setY(e.Y);
			}
			else if (clickRozmery != "")
			{
				IfBlok temp = (IfBlok) modul.objekty[clickRozmery];
				if (e.X > temp.getX() && e.Y > temp.getY())
				{
					temp.x2 = e.X;
					temp.y2 = e.Y;
				}
			}
			else if (clickBodka != null)
			{
				tempCiara.tempBodka.setX(e.X);
				tempCiara.tempBodka.setY(e.Y);
			}
			else if (clickMalaBodka != null)
			{
				clickMalaBodka.setX(e.X);
				clickMalaBodka.setY(e.Y);
			}
			else
			{
				return;
			}
			aktualizujPlochu();
		}
		
		void MainFormMouseUp(object sender, MouseEventArgs e)
		{
			nastavRozmery();
			if (tahaj != "")
			{
				modul.priradObjektIfBloku(modul.objekty[tahaj]);
			}
			else if (clickRozmery != "")
			{
				IfBlok blok = (IfBlok) modul.objekty[clickRozmery];
				blok.aktualizuj();
				clickRozmery = "";
			}
			tahaj = "";
		}
		
		void nastavRozmery()
		{
			if (noveRozmeryPaneluX > 0)
			{
				tabControl1.SelectedTab.Controls[0].Width = noveRozmeryPaneluX;
				noveRozmeryPaneluX = -1;
			}
			if (noveRozmeryPaneluY > 0)
			{
				tabControl1.SelectedTab.Controls[0].Height = noveRozmeryPaneluY;
				noveRozmeryPaneluY = -1;
			}
		}
		
		void NovyProjektDialog(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();  
			saveFileDialog1.Filter = "dw files (*.dw)|*.dw|All files (*.*)|*.*";  
			saveFileDialog1.Title = "Ulož projekt";  
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				String cesta = Path.GetDirectoryName(saveFileDialog1.FileName);
				String meno = Path.GetFileNameWithoutExtension(saveFileDialog1.FileName);
				File.WriteAllText(@cesta+"\\"+meno+".dw", Projekt.vytvorMain());
				OtvorProjekt(cesta, meno);
			}
		}
		
		void OtvorProjektDialog(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();  
			openFileDialog1.Filter = "dw files (*.dw)|*.dw|All files (*.*)|*.*";  
			openFileDialog1.Title = "Otvor projekt";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				String cesta = Path.GetDirectoryName(openFileDialog1.FileName);
				String meno = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
				OtvorProjekt(cesta, meno);
			}
		}
		
		void UlozProjekt(object sender, EventArgs e)
		{
			projekt.uloz();
		}
		
		void OtvorProjekt(String cesta, String meno)
		{
			ResetProjekt();
			Text = "DataWolf - " + meno;
			projekt = new Projekt(meno, cesta);
			foreach (Modul m in projekt.moduly)
			{
				pridajTabPage(m.meno);
			}
			modul = projekt.moduly[0];
			ResetModul();
		}
		
		void ResetProjekt()
		{
			odstranEventy();
			tabControl1.TabPages.Clear();
			projekt = null;
			modul = null;
		}
		
		void ResetModul()
		{
			click = "";
			tahaj = "";
			clickRozmery = "";
			clickBodka = null;
			tempCiara = null;
			clickMalaBodka = null;
			nastavMenu();
			aktualizujOkna();
		}
		
		void aktualizujOkna()
		{
			if (registreOkno != null)
			{
				registreOkno.init();
			}
			if (vstupyOkno != null)
			{
				vstupyOkno.init();
			}
			if (vystupyOkno != null)
			{
				vystupyOkno.init();
			}
		}
		
		void nastavMenu()
		{
			if (projekt != null)
			{
				menuStrip1.Items[1].Enabled = true;
				menuStrip1.Items[2].Enabled = true;
				if (modul.meno == "main")
				{
					menuStrip1.Items.Find("premenujModulToolStripMenuItem", true)[0].Enabled = false;
					menuStrip1.Items.Find("zmazModulToolStripMenuItem", true)[0].Enabled = false;
				}
				else
				{
					menuStrip1.Items.Find("premenujModulToolStripMenuItem", true)[0].Enabled = true;
					menuStrip1.Items.Find("zmazModulToolStripMenuItem", true)[0].Enabled = true;
				}
			}
			else
			{
				menuStrip1.Items[1].Enabled = false;
				menuStrip1.Items[2].Enabled = false;
			}
		}
		
		void kresli(object sender, PaintEventArgs e)
		{
			if (modul == null) return;
			
			foreach (Objekt o in modul.objekty.Values)
			{
				o.aktualizujStav();
			}
			
			Graphics g = e.Graphics;
			
			if (tempCiara != null)
			{
				tempCiara.kresli(g);
			}
			int maxX = 1000, maxY = 1000;
			foreach (Objekt o in modul.objekty.Values)
			{
				if (o.ciary == null) continue;
				foreach (Ciara c in o.ciary)
				{
					if (c == null) continue;
					c.kresli(g);
				}
				maxX = Math.Max(maxX, o.getMaxX() + 10);
				maxY = Math.Max(maxY, o.getMaxY() + 10);
			}
			
			foreach (Objekt o in modul.objekty.Values)
			{
				o.kresli(g);
			}
			
			DoubleBufferedPanel panel = (DoubleBufferedPanel) sender;
			if (panel.Width > maxX)
			{
				noveRozmeryPaneluX = maxX;
			}
			else
			{
				panel.Width = maxX;
			}
			if (panel.Height > maxY)
			{
				noveRozmeryPaneluY = maxY;
			}
			else
			{
				panel.Height = maxY;
			}
		}
		
		void VyberModulu(object sender, EventArgs e)
		{
			if (tabControl1.SelectedIndex < 0) return;
			modul = projekt.moduly[tabControl1.SelectedIndex];
			ResetModul();
		}
		
		void NovyModul(object sender, EventArgs e)
		{
			
			String meno = Microsoft.VisualBasic.Interaction.InputBox("", "Zadaj názov modulu", "novy_modul", 200, 200);
			if (meno == null) return;
			meno = meno.Trim().Replace(' ', '_');
			if (meno.Length == 0)
			{
				MessageBox.Show("Nie je zadaný názov modulu.",
								"Chyba",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error,
								MessageBoxDefaultButton.Button1);
				return;
			}
			if (projekt.existujeModul(meno))
			{
				MessageBox.Show("Modul s týmto názvom už existuje.",
								"Chyba",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error,
								MessageBoxDefaultButton.Button1);
				return;
			}
			Modul m = new Modul(meno, new Dictionary<String, Objekt>(),
		                        new List<BodkaInfo>(), new List<BodkaInfo>(),
		                        new List<RegisterInfo>());
			projekt.moduly.Add(m);
			Projekt.DATABAZA[m.meno] = new ObjektInfo(m.meno, m.meno, null, null, true);
			pridajTabPage(meno);
		}
		
		public void pridajTabPage(String meno)
		{
			DoubleBufferedTabPage tabPage = new DoubleBufferedTabPage();
			tabPage.Name = meno;
			tabPage.Text = meno;
			tabPage.BackColor = Color.White;
			tabPage.AutoScroll = true;
			
			DoubleBufferedPanel panel = new DoubleBufferedPanel();
			panel.Location = new Point(0, 0);
			panel.Size = new Size(1000, 1000);
			panel.Paint += kresli;
			panel.MouseDown += MainFormMouseDown;
			panel.MouseMove += MainFormMouseMove;
			panel.MouseUp += MainFormMouseUp;
			
			tabPage.Controls.Add(panel);
			tabControl1.TabPages.Add(tabPage);
		}
				
		void odstranEventy()
		{
			foreach (DoubleBufferedTabPage t in tabControl1.TabPages)
			{
				t.Controls[0].Paint -= kresli;
				t.Controls[0].MouseDown -= MainFormMouseDown;
				t.Controls[0].MouseMove -= MainFormMouseMove;
				t.Controls[0].MouseUp -= MainFormMouseUp;
			}
		}

		void MainFormKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (clickMalaBodka != null)
				{
					clickMalaBodka.zmaz();
					clickMalaBodka = null;
				}
				else if (click != "")
				{
					modul.objekty[click].zrusEditovaciePrvky(panel1);
					modul.zmazObjekt(click);
					click = "";
				}
				else
				{
					return;
				}
				aktualizujPlochu();
			}
		}

		void RegistreToolStripMenuItemClick(object sender, EventArgs e)
		{
			registreOkno = new RegistreOkno(this);
			registreOkno.Show();
		}
		
		void VstupyToolStripMenuItemClick(object sender, EventArgs e)
		{
			vstupyOkno = new VstupyOkno(this);
			vstupyOkno.Show();
		}
		void VystupyToolStripMenuItemClick(object sender, EventArgs e)
		{
			vystupyOkno = new VystupyOkno(this);
			vystupyOkno.Show();
		}
		
		void KompilujToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (kontrolaSpravnostiProgramu())
			{
				projekt.generujLucidKod();
			}
			else
			{
				MessageBox.Show("Program obsahuje chyby a nie je možné ho skompilovať.",
								"Chyba",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error,
								MessageBoxDefaultButton.Button1);
			}
		}
		
		bool kontrolaSpravnostiProgramu()
		{
			foreach (Modul m in projekt.moduly)
			{
				foreach (Objekt o in m.objekty.Values)
				{
					foreach (ObjektBodka b in o.bodky)
					{
						if (b.stav == Brushes.Red)
						{
							return false;
						}
					}
					if (o.ciary == null) continue;
					foreach (Ciara c in o.ciary)
					{
						if (c.stav == Pens.Red)
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		
		void PremenujModulToolStripMenuItemClick(object sender, EventArgs e)
		{
			String meno = Microsoft.VisualBasic.Interaction.InputBox("", "Zadaj názov modulu", tabControl1.SelectedTab.Text, 200, 200);
			if (meno == null) return;
			meno = meno.Trim().Replace(' ', '_');
			if (meno.Length == 0)
			{
				return;
			}
			if (projekt.existujeModul(meno) && meno != tabControl1.SelectedTab.Text)
			{
				MessageBox.Show("Modul s týmto názvom už existuje.",
								"Chyba",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error,
								MessageBoxDefaultButton.Button1);
				return;
			}
			projekt.moduly[tabControl1.SelectedIndex].meno = meno;
			Projekt.DATABAZA[meno] = new ObjektInfo(meno, meno, null, null, true);
			foreach (Modul m in projekt.moduly)
			{
				foreach (Objekt o in m.objekty.Values)
				{
					if (o is PouzitieModulu && o.op == tabControl1.SelectedTab.Text)
					{
						o.op = meno;
					}
				}
			}
			tabControl1.SelectedTab.Text = meno;
		}
		
		void ZmazModulToolStripMenuItemClick(object sender, EventArgs e)
		{
			foreach (Modul m in projekt.moduly)
			{
				List<String> trebaZmazat = new List<String>();
				foreach (Objekt o in m.objekty.Values)
				{
					if (o is PouzitieModulu && o.op == tabControl1.SelectedTab.Text)
					{
						trebaZmazat.Add(o.id);
					}
				}
				foreach (String id in trebaZmazat)
				{
					m.zmazObjekt(id);
				}
			}
			tabControl1.TabPages.Remove(tabControl1.SelectedTab);
			//tabControl1.SelectedIndex = 0;
			
		}
		
	}
}

class DoubleBufferedTabPage : TabPage
{
	public DoubleBufferedTabPage()
	{
		this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
	}
}

class DoubleBufferedPanel : Panel
{
	public DoubleBufferedPanel()
	{
		this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
	}
}
