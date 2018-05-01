using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DataWolf
{
	public class Register : Objekt
	{
		public RegisterInfo r;
		
		public Register(int nX, int nY, String nId, String nOp, RegisterInfo nR)
			: base(nX, nY, nId, nOp)
		{
			r = nR;
			rx = 25;
			dx = 50;
			ry = 25;
			dy = 50;
		}
		
		public override string getText()
		{
			return r.meno;
		}
		
		public override void kresli(Graphics g)
		{
			upravSirkuObjektu(g);
			base.kresli(g);
			Rectangle rect = new Rectangle(x - rx + 3, y - ry + 3, dx - 6, dy - 6);
			g.DrawRectangle(stav, rect);
		}
		
		public override void nastavEditovaciePrvky(Panel panel)
		{
			base.nastavEditovaciePrvky(panel);
			
			panel.Controls.Add(vytvorLabel("Meno:", 3, 40));
			
			ComboBox comboBox = new ComboBox();
			comboBox.Name = "comboBox";
			nastavUdajeObjektuRozhrania(comboBox, r.meno, 46, 94);
			panel.Controls.Add(comboBox);
			zvysYRozhrania();
		}
		
		protected override String dwKodParametre()
		{
			return " " + r.meno;
		}
		
		public override String generujInit()
		{
			return "";
		}
		
		public override String getLucidZapisovanieDat(int b)
		{
			return r.meno + ".d";
		}
		
		public override String getLucidCitanieDat(int b)
		{
			return r.meno + ".q";
		}
		
		public override bool vlastnyModul()
		{
			return false;
		}
	}
	
	public class RegisterC : Register
	{
		public RegisterC(int nX, int nY, String nId, RegisterInfo nR)
			: base(nX, nY, nId, "regC", nR)
		{
			bodky = new []{new ObjektBodka(-4, ry, Brushes.Black, this, 0, new BodkaInfo("out", r.bitSirka, false, r.bitDlzka, false))};
		}
	}
	
	public class RegisterZ : Register
	{
		public RegisterZ(int nX, int nY, String nId, RegisterInfo nR)
			: base(nX, nY, nId, "regZ", nR)
		{
			bodky = new []{new ObjektBodka(-4, -ry - 8, Brushes.Red, this, 0, new BodkaInfo("data", r.bitSirka, false, r.bitDlzka, false))};
			ciary = new Ciara[1];
		}
		
		public override String generujLucidKod(Modul f, int tab)
		{
			String vysl = "";
			vysl += tabulator(tab) + getLucidZapisovanieDat(0) + " = ";
			vysl += ciary[0].bodkaVystup.obj.getLucidCitanieDat(ciary[0].bodkaVystup.id);
			vysl += ";\r\n";
			return vysl;
		}
	}
	
	public class RegisterZIndex : Register
	{
		public RegisterZIndex(int nX, int nY, String nId, RegisterInfo nR)
			: base(nX, nY, nId, "regZ_index", nR)
		{
			bodky = new []{	new ObjektBodka(-14, -ry - 8, Brushes.Red, this, 0, new BodkaInfo("data", 1, false, r.bitDlzka, false)),
							new ObjektBodka(6, -ry - 8, Brushes.Red, this, 1, Projekt.DATABAZA[op].vstupy[1])};
			ciary = new Ciara[2];
		}
		
		public override String generujLucidKod(Modul f, int tab)
		{
			String vysl = "";
			vysl += tabulator(tab) + getLucidZapisovanieDat(0) + "[" + ciary[1].bodkaVystup.obj.getLucidCitanieDat(ciary[1].bodkaVystup.id) + "]" + " = ";
			vysl += ciary[0].bodkaVystup.obj.getLucidCitanieDat(ciary[0].bodkaVystup.id);
			vysl += ";\r\n";
			return vysl;
		}
	}
}
