using System;
using System.Collections.Generic;
using System.Drawing;

namespace DataWolf
{

	public class Ciara
	{
		public ObjektBodka bodkaVstup;
		public List<CiaraBodka> cesta;
		public ObjektBodka bodkaVystup;
		public Pen stav;
		
		public Ciara(ObjektBodka nBodkaVstup, ObjektBodka nBodkaVystup, List<CiaraBodka> nCesta = null)
		{
			bodkaVstup = nBodkaVstup;
			bodkaVystup = nBodkaVystup;
			cesta = nCesta;
			if (cesta == null)
			{
				cesta = new List<CiaraBodka>();
			}
			stav = Pens.Black;
		}
		
		public virtual void kresli(Graphics g)
		{
			Point[] body = new Point[2+cesta.Count];
			body[0] = new Point(bodkaVstup.getX(), bodkaVstup.getY());
			for (int i = 0 ; i < cesta.Count ; i++)
			{
				body[i+1] = new Point(cesta[i].getX(), cesta[i].getY());
				cesta[i].kresli(g);
			}
			body[1+cesta.Count] = new Point(bodkaVystup.getX(), bodkaVystup.getY());
			g.DrawLines(stav, body);
		}
	}
	
	public class TempCiara : Ciara
	{
		public Bodka tempBodka;
		
		public TempCiara(ObjektBodka nB1, Bodka nTempBodka)
			:base(nB1, null)
		{
			tempBodka = nTempBodka;
		}
		
		public override void kresli(Graphics g)
		{
			Point[] body = new Point[2+cesta.Count];
			body[0] = new Point(bodkaVstup.getX(), bodkaVstup.getY());
			for (int i = 0 ; i < cesta.Count ; i++)
			{
				body[i+1] = new Point(cesta[i].getX(), cesta[i].getY());
				cesta[i].kresli(g);
			}
			body[1+cesta.Count] = new Point(tempBodka.getX(), tempBodka.getY());
			g.DrawLines(stav, body);
		}
	}
}
