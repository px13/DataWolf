using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DataWolf
{
	public class IfBlok : Objekt
	{
		public int x2, y2;
		public List<Objekt> objekty;
		public List<CiaraBodka> ciaroveBodky;
		
		public IfBlok(int nX, int nY, int nX2, int nY2, String nId)
			: base(nX, nY, nId, "if")
		{
			x2 = nX2;
			y2 = nY2;
			rx = 15;
			dx = 2*rx;
			ry = 15;
			dy = 2*ry;
			ciary = new Ciara[1];
			bodky = new []{new ObjektBodka(-4, -ry - 8, Brushes.Red, this, 0, Projekt.DATABAZA[op].vstupy[0])};
			objekty = new List<Objekt>();
			ciaroveBodky = new List<CiaraBodka>();
		}
		
		public override int getMaxX()
		{
			return x2 + dx;
		}
		
		public override int getMaxY()
		{
			return y2 + dy;
		}
		
		public override void setX(int nX)
		{
			int posun = nX - x;
			x2 += posun;
			x += posun;
			posunObjekty(posun, 0);
			posunMaleBodky(posun, 0);
		}
		
		public override void setY(int nY)
		{
			int posun = nY - y;
			y2 += posun;
			y += posun;
			posunObjekty(0, posun);
			posunMaleBodky(0, posun);
		}
		
		void posunObjekty(int posunX, int posunY)
		{
			if (objekty == null) return;
			foreach (Objekt o in objekty)
			{
				o.setX(o.getX()+posunX);
				o.setY(o.getY()+posunY);
			}
		}
		
		void posunMaleBodky(int posunX, int posunY)
		{
			if (ciaroveBodky == null) return;
			foreach (CiaraBodka mb in ciaroveBodky)
			{
				mb.setX(mb.getX()+posunX);
				mb.setY(mb.getY()+posunY);
			}
		}
		
		public bool jeVBloku(int x, int y)
		{
			return x > this.x && y > this.y && x < x2 && y < y2;
		}
		
		public void aktualizuj()
		{
			aktualizujObjekty();
			aktualizujBodky();
		}
		
		public void aktualizujObjekty()
		{
			List<Objekt> noveObjekty = new List<Objekt>();
			foreach (Objekt o in objekty)
			{
				if (jeVBloku(o.getX(), o.getY()))
				{
					noveObjekty.Add(o);
				}
			}
			objekty = noveObjekty;
		}
		
		public void aktualizujBodky()
		{
			List<CiaraBodka> noveBodky = new List<CiaraBodka>();
			foreach (CiaraBodka mb in ciaroveBodky)
			{
				if (jeVBloku(mb.getX(), mb.getY()))
				{
					noveBodky.Add(mb);
				}
			}
			ciaroveBodky = noveBodky;
		}
		
		public bool isClick2(int x, int y)
		{
			return x <= x2 + rx && x >= x2 - rx && y <= y2 + ry && y >= y2 - ry;
		}
		
		public override void kresli(Graphics g)
		{
			base.kresli(g);
			
			Rectangle rect = new Rectangle(x2 - rx, y2 - ry, dx, dy);
			g.FillRectangle(Brushes.Blue, rect);
			g.DrawRectangle(Pens.Blue, rect);
			
			g.DrawLines(Pens.Blue, new[]{new Point(x - rx, y - ry), new Point(x2 + rx, y - ry), new Point(x2 + rx, y2 + ry)});
			g.DrawLines(Pens.Blue, new[]{new Point(x - rx, y - ry), new Point(x - rx, y2 + ry), new Point(x2 + rx, y2 + ry)});
		}
		
		protected override String dwKodSuradnice()
		{
			return " # " + x + " " + y + " " + x2 + " " + y2 + "\r\n";
		}
		
		public override string generujInit()
		{
			return "";
		}
		
		public override string generujLucidKod(Modul f, int tab)
		{
			return tabulator(tab) + "if (" + ciary[0].bodkaVystup.obj.getLucidCitanieDat(ciary[0].bodkaVystup.id) + ") {\r\n";
		}
		
		public override bool vlastnyModul()
		{
			return false;
		}
	}
}
