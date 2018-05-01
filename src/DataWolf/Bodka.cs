using System;
using System.Collections.Generic;
using System.Drawing;

namespace DataWolf
{
	public class Bodka
	{
		public Brush stav;
		
		protected int x, y;
		protected int r, d;
				
		public Bodka(int nX, int nY, int nR, Brush nStav = null)
		{
			x = nX;
			y = nY;
			r = nR;
			d = 2*r;
			stav = nStav;
			if (stav == null)
			{
				stav = Brushes.Black;
			}
		}
		
		public void kresli(Graphics g)
		{
			g.FillEllipse(stav, getX() - r, getY() - r, d, d);
			g.DrawEllipse(Pens.Black, getX() - r, getY() - r, d, d);
		}
		
		public bool isClick(int x, int y)
		{
			return (getX() - x)*(getX() - x) + (getY() - y)*(getY() - y) < r*r;
		}

		public virtual int getX()
		{
			return x;
		}
		
		public virtual int getY()
		{
			return y;
		}
		
		public void setX(int nX)
		{
			x = nX;
		}
		
		public void setY(int nY)
		{
			y = nY;
		}
	}
	
	public class ObjektBodka : Bodka
	{
		public int id;
		public Objekt obj;
		public BodkaInfo info;
		
		public ObjektBodka(int nX, int nY, Brush nStav, Objekt nObj, int nId, BodkaInfo nInfo)
			: base(nX, nY, 4, nStav)
		{
			obj = nObj;
			id = nId;
			if (nInfo != null)
			{
				info = new BodkaInfo(nInfo.meno, nInfo.bitSirka, nInfo.bitSirkaZmena, nInfo.bitDlzka, nInfo.bitDlzkaZmena);
			}
		}
		
		public override int getX()
		{
			return obj.getX() + x + r;
		}
		
		public override int getY()
		{
			return obj.getY() + y + r;
		}
		
		public String getMenoIndex(int index)
		{
			if (info.bitSirka == 1)
			{
				return info.meno;
			}
			return info.meno + "[" + index + "]";
		}
		
	}
	
	public class CiaraBodka : Bodka
	{
		public List<Ciara> ciary;
		public IfBlok ifBlok;
		
		public CiaraBodka(int nX, int nY, List<Ciara> nCiary = null)
			: base(nX, nY, 4)
		{
			ciary = nCiary;
			if (ciary == null)
			{
				ciary = new List<Ciara>();
			}
		}
		
		public void zmaz()
		{
			foreach (var c in ciary)
			{
				c.cesta.Remove(this);
			}
			if (ifBlok != null)
			{
				ifBlok.ciaroveBodky.Remove(this);
			}
		}
		
		public void prepniAktivnost()
		{
			if (stav == Brushes.Black)
			{
				stav = Brushes.Yellow;
			}
			else
			{
				stav = Brushes.Black;
			}
		}
	}
}
