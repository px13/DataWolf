using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace DataWolf
{
	public class Projekt
	{
		public String meno;
		public String cesta;
		public List<Modul> moduly;
		
		public Projekt(String nMeno, String nCesta)
		{
			vytvorDatabazu();
			meno = nMeno;
			cesta = nCesta;
			a = new Analyzator(System.IO.File.ReadAllText(@cesta+"\\"+meno+".dw"));
			a.scan();
			moduly = new List<Modul>();
			volania = new List<TempPouzitieModulu>();
			nacitajDWKod();
			spracujPouzitiaModulov();
			foreach (Modul m in moduly)
			{
				m.nastavGraf();
				bool aktualizujStavy;
				do
				{
					aktualizujStavy = false;
					foreach (Objekt o in m.objekty.Values)
					{
						aktualizujStavy = aktualizujStavy || o.aktualizujStav();
					}
				}
				while (aktualizujStavy);
			}
		}
		
		public void vytvorDatabazu()
		{
			List<BodkaInfo> VSTUP_INT = new List<BodkaInfo>{BodkaInfo.INT("op")};
			List<BodkaInfo> BIN_VSTUPY_INT = new List<BodkaInfo>{BodkaInfo.INT("op1"), BodkaInfo.INT("op2")};
			List<BodkaInfo> VYSTUP_INT = new List<BodkaInfo>{BodkaInfo.INT("res")};
			List<BodkaInfo> VSTUP_BOOL = new List<BodkaInfo>{BodkaInfo.BOOL("op")};
			List<BodkaInfo> BIN_VSTUPY_BOOL = new List<BodkaInfo>{BodkaInfo.BOOL("op1"), BodkaInfo.BOOL("op2")};
			List<BodkaInfo> VYSTUP_BOOL = new List<BodkaInfo>{BodkaInfo.BOOL("res")};
			DATABAZA = new Dictionary<String, ObjektInfo>();
			//Hodnoty
			DATABAZA["const"] = new ObjektInfo("const", "Konštanta", null, VYSTUP_INT);
			DATABAZA["index"] = new ObjektInfo("index", "Prvok z poľa",
			                                   new List<BodkaInfo>{new BodkaInfo("pole", 1, true, 32, true),
								                                   	BodkaInfo.INT("index")},
			                                   new List<BodkaInfo>{new BodkaInfo("out", 1, true, 32, true)});
		    //IO operácie
		    DATABAZA["in"] = new ObjektInfo("in", "Vstup", null,
		                                      new List<BodkaInfo>{BodkaInfo.INT("in")});
		    DATABAZA["out"] = new ObjektInfo("out", "Výstup",
		                                      new List<BodkaInfo>{BodkaInfo.INT("out")}, null);
		    //Aritmetické operácie
		    DATABAZA["add"] = new ObjektInfo("+", "Sčítanie", BIN_VSTUPY_INT, VYSTUP_INT);
		    DATABAZA["sub"] = new ObjektInfo("-", "Odčítanie", BIN_VSTUPY_INT, VYSTUP_INT);
		    DATABAZA["mul"] = new ObjektInfo("*", "Násobenie", BIN_VSTUPY_INT, VYSTUP_INT);
		    DATABAZA["div"] = new ObjektInfo("/", "Celočíselné delenie", BIN_VSTUPY_INT, VYSTUP_INT);
		    DATABAZA["mod"] = new ObjektInfo("%", "Zvyšok po delení", BIN_VSTUPY_INT, VYSTUP_INT);
		    DATABAZA["inc"] = new ObjektInfo("++", "Inkrementácia", VSTUP_INT, VYSTUP_INT);
		    DATABAZA["dec"] = new ObjektInfo("--", "Dekrementácia", VSTUP_INT, VYSTUP_INT);
		    //Bitové operácie
		    DATABAZA["bit_and"] = new ObjektInfo("&&", "AND", BIN_VSTUPY_INT, VYSTUP_INT);
		    DATABAZA["bit_or"] = new ObjektInfo("|", "OR", BIN_VSTUPY_INT, VYSTUP_INT);
		    DATABAZA["bit_xor"] = new ObjektInfo("^", "XOR", BIN_VSTUPY_INT, VYSTUP_INT);
		    DATABAZA["bit_not"] = new ObjektInfo("~", "NOT", VSTUP_INT, VYSTUP_INT);
		    DATABAZA["shift_left"] = new ObjektInfo("<<", "Bitový posun do ľava", BIN_VSTUPY_INT, VYSTUP_INT);
		    DATABAZA["shift_right"] = new ObjektInfo(">>", "Bitový posun do prava", BIN_VSTUPY_INT, VYSTUP_INT);
		    //Logické operácie
		    DATABAZA["log_and"] = new ObjektInfo("&&&&", "AND", BIN_VSTUPY_BOOL, VYSTUP_BOOL);
		    DATABAZA["log_or"] = new ObjektInfo("||", "OR", BIN_VSTUPY_BOOL, VYSTUP_BOOL);
		    DATABAZA["log_not"] = new ObjektInfo("!", "NOT", VSTUP_BOOL, VYSTUP_BOOL);
		    //Porovnávacie operácie
		    DATABAZA["gt"] = new ObjektInfo(">", "Väčší", BIN_VSTUPY_INT, VYSTUP_BOOL);
		    DATABAZA["gteq"] = new ObjektInfo(">=", "Väčší alebo rovný", BIN_VSTUPY_INT, VYSTUP_BOOL);
		    DATABAZA["lt"] = new ObjektInfo("<", "Menší", BIN_VSTUPY_INT, VYSTUP_BOOL);
		    DATABAZA["lteq"] = new ObjektInfo("<=", "Menší alebo rovný", BIN_VSTUPY_INT, VYSTUP_BOOL);
		    DATABAZA["eq"] = new ObjektInfo("==", "Rovná sa", BIN_VSTUPY_INT, VYSTUP_BOOL);
		    DATABAZA["noteq"] = new ObjektInfo("!=", "Nerovná sa", BIN_VSTUPY_INT, VYSTUP_BOOL);
		    DATABAZA["concat"] = new ObjektInfo("c", "Zreťazenie",  new List<BodkaInfo>{
		                                        	new BodkaInfo("op1", 1, true, 32, true),
		                                        	new BodkaInfo("op2", 1, true, 32, true)},
		                                        	new List<BodkaInfo>{
		                                        	new BodkaInfo("res", 2, true, 32, true)});
		    DATABAZA["reverse"] = new ObjektInfo("r", "Otočenie bitov", new List<BodkaInfo>{new BodkaInfo("op", 1, true, 32, true)},
		                                        new List<BodkaInfo>{new BodkaInfo("res", 1, true, 32, true)});
		    DATABAZA["change"] = new ObjektInfo("-|=", "Zmena bitovej šírky", new List<BodkaInfo>{
		                                        	new BodkaInfo("op", 1, true, 32, true)},
		                                        	new List<BodkaInfo>{
		                                        	new BodkaInfo("res", 1, true, 32, true)});//
		    DATABAZA["if"] = new ObjektInfo("if", "Podmienkový blok", new List<BodkaInfo>{BodkaInfo.BOOL("podmienka")}, null);
		    //Register
		    DATABAZA["regC"] = new ObjektInfo("regC", "Čítaj z registra", null, new List<BodkaInfo>{new BodkaInfo("out", 1, true, 32, true)});//
		    DATABAZA["regZ"] = new ObjektInfo("regZ", "Zapíš do registra", new List<BodkaInfo>{new BodkaInfo("data", 1, true, 32, true)}, null);
		    DATABAZA["regZ_index"] = new ObjektInfo("regZ_index", "Zapíš do registra na index",
		                                            new List<BodkaInfo>{new BodkaInfo("data", 1, false, 32, true),
		                                            	new BodkaInfo("index", 1, false, 32, true)}, null);
		    //Vlastné moduly
		    DATABAZA["main"] = new ObjektInfo("main", "main", null, null, true);
		}
		
		public static String vytvorMain()
		{
			String vysl = "def main \r\n[\r\n";
			vysl += "input read_new # 1 1\r\n";
			vysl += "input read_data # 1 8\r\n";
			vysl += "input write_busy # 1 1\r\n";
			vysl += "output write_new # 1 1\r\n";
			vysl += "output write_data # 1 8\r\n";
			vysl += "]";
			return vysl;
		}
				
		public static Dictionary<String, ObjektInfo> DATABAZA;
		
		public bool existujeModul(String meno)
		{
			foreach (Modul i in moduly)
			{
				if (i.meno.Equals(meno)) return true;
			}
			return false;
		}
		
		Analyzator a;
		
		bool isToken(String token)
		{
			if (a.token == token)
			{
				a.scan();
				return true;
			}
			return false;
		}
		
		void getToken(String token)
		{
			if (a.token != token)
			{
				Debug.WriteLine("Očakával som "+ token +", ale dostal som: " + a.token);
			}
			a.scan();
		}
		
		String popToken()
		{
			String token = a.token;
			a.scan();
			return token;
		}
		
		public void uloz()
		{
			String vysl = "";
			foreach (Modul m in moduly)
			{
				vysl += m.generujDWKod();
			}
			System.IO.File.WriteAllText(@cesta+"\\"+meno+".dw", vysl);
		}
		
		List<CiaraBodka> poleBodiek;
		List<List<String>> poleIndexov;
		
		public void nacitajGraf()
		{
			poleBodiek = new List<CiaraBodka>();
			poleIndexov = new List<List<String>>();
			while (a.poslednyStav == Analyzator.STATE_NUMBER)
			{
				poleBodiek.Add(new CiaraBodka(int.Parse(popToken()), int.Parse(popToken()), new List<Ciara>()));
			}
			while (a.token != "]")
			{
				getToken("#");
				poleIndexov.Add(new List<String>());
				while (a.token != "#" && a.token != "]")
				{
					poleIndexov[poleIndexov.Count-1].Add(popToken());
				}
			}
			
		}
		
		public Dictionary<String, Objekt> objekty;
		public List<BodkaInfo> vstupy;
		public List<BodkaInfo> vystupy;
		public List<RegisterInfo> registre;
		
		List<TempPouzitieModulu> volania;
		
		public void spracujPouzitiaModulov()
		{
			foreach (TempPouzitieModulu temp in volania)
			{
				moduly[temp.idModul].objekty[temp.id] = new PouzitieModulu(temp.sur.a, temp.sur.b, temp.id, temp.menoModulu);
			}			
			volania = null;
		}
		
		public void nacitajDWKod()
		{
			while (a.token != "")
			{
				if (isToken("]"))
				{
					return;
				}
				if (isToken("def"))
				{
					String fMeno = popToken();
				    getToken("[");
				    objekty = new Dictionary<String, Objekt>();
				    registre = new List<RegisterInfo>();
				    vstupy = new List<BodkaInfo>();
				    vystupy = new List<BodkaInfo>();
				    nacitajDWKod();
				    Modul f = new Modul(fMeno, objekty, vstupy, vystupy, registre, poleBodiek, poleIndexov);
				    poleBodiek = null;
				    poleIndexov = null;
				    moduly.Add(f);
				    if (f.meno != "main")
				    {
				    	Projekt.DATABAZA[f.meno] = new ObjektInfo(f.meno, f.meno, vstupy, vystupy);
				    }
				}
				else if (isToken("input"))
				{
					String vstup = popToken();
					IntTuple bit = getTagIntTuple();
					vstupy.Add(new BodkaInfo(vstup, bit.a, false, bit.b, false));
				}
				else if (isToken("output"))
				{
					String vystup = popToken();
					IntTuple bit = getTagIntTuple();
					vystupy.Add(new BodkaInfo(vystup, bit.a, false, bit.b, false));
				}
				else if (isToken("register"))
				{
					String register = popToken();
					IntTuple bit = getTagIntTuple();
					registre.Add(new RegisterInfo(register, bit.a, bit.b));
				}
				else if (isToken("#"))
				{
					nacitajGraf();
				}
				else
				{
					getToken("{");
					String id = popToken();
					getToken("}");
					
					if (isToken("call") || isToken("use"))
					{
						String menoModulu = popToken();
						IntTuple sur = getTagIntTuple();
						volania.Add(new TempPouzitieModulu(moduly.Count, sur, id, menoModulu));
					}
					else if (isToken("const"))
					{
						List<String> values = new List<String>();
						getToken("(");
						while (!isToken(")"))
						{
							values.Add(popToken());
						}
						IntTuple bit = getTagIntTuple();
						IntTuple sur = getTagIntTuple();
						objekty[id] = new Konst(sur.a, sur.b, id, "const", bit.a, bit.b, values.ToArray());
					}
					
					else if (isToken("in"))
					{
						String io = popToken();
						IntTuple sur = getTagIntTuple();
						BodkaInfo b = getVstup(io);
						objekty[id] = new IoOperacia(sur.a, sur.b, id, "in", b);
					}
					else if (isToken("out"))
					{
						String io = popToken();
						IntTuple sur = getTagIntTuple();
						BodkaInfo b = getVystup(io);
						objekty[id] = new IoOperacia(sur.a, sur.b, id, "out", b);
					}
					else if (isToken("regC"))
					{
						String register = popToken();
						IntTuple sur = getTagIntTuple();
						RegisterInfo r = getRegister(register);
						objekty[id] = new RegisterC(sur.a, sur.b, id, r);
					}
					else if (isToken("regZ"))
					{
						String register = popToken();
						IntTuple sur = getTagIntTuple();
						RegisterInfo r = getRegister(register);
						objekty[id] = new RegisterZ(sur.a, sur.b, id, r);
					}
					else if (isToken("regZ_index"))
					{
						String register = popToken();
						IntTuple sur = getTagIntTuple();
						RegisterInfo r = getRegister(register);
						objekty[id] = new RegisterZIndex(sur.a, sur.b, id, r);
					}
					
					else if (isToken("change"))
					{
						IntTuple bit = getTagIntTuple();
						IntTuple sur = getTagIntTuple();
						objekty[id] = new ZmenaBitov(sur.a, sur.b, id, bit.a, bit.b);
					}
					else
					{
						objekty[id] = nacitajOp(id);
					}
				}
				
			}
		}
		
		RegisterInfo getRegister(String register)
		{
			foreach (RegisterInfo r in registre)
			{
				if (r.meno == register)
					return r;
			}
			return null;
		}
		
		BodkaInfo getVstup(String vstup)
		{
			foreach (BodkaInfo b in vstupy)
			{
				if (b.meno == vstup)
					return b;
			}
			return null;
		}
		
		BodkaInfo getVystup(String vystup)
		{
			foreach (BodkaInfo b in vystupy)
			{
				if (b.meno == vystup)
					return b;
			}
			return null;
		}
		
		IntTuple getTagIntTuple()
		{
			getToken("#");
			return new IntTuple(int.Parse(popToken()), int.Parse(popToken()));
		}
		
		IntTuple getIntTuple()
		{
			return new IntTuple(int.Parse(popToken()), int.Parse(popToken()));
		}
		
		Objekt nacitajOp(String id)
		{
			String op = popToken();
			IntTuple sur = getTagIntTuple();
			if (op == "index")
			{
				return new Index(sur.a, sur.b, id);
			}
			if (op == "concat")
			{
				return new Zretazenie(sur.a, sur.b, id);
			}
			if (op == "reverse")
			{
				return new OtocenieBitov(sur.a, sur.b, id);
			}
			if (op == "inc" || op == "dec")
			{
				return new IncDecOperacia(sur.a, sur.b, id, op);
			}
			if (op == "mod")
			{
				return new Modulo(sur.a, sur.b, id);
			}
			if (op == "if")
			{
				IntTuple sur2 = getIntTuple();
				return new IfBlok(sur.a, sur.b, sur2.a, sur2.b, id);
			}
			return new Operacia(sur.a, sur.b, id, op);
		}
		
		public void generujLucidKod()
		{
			String cestaKompilacie = cesta + "//DataWolfProjekt_" + meno;
			if (Directory.Exists(cestaKompilacie))
			{
				Directory.Delete(cestaKompilacie , true);
			}
			String cestaSrc = cestaKompilacie  + "//source";
			Directory.CreateDirectory(cestaKompilacie);
			Directory.CreateDirectory(cestaSrc);
			Dictionary<String, Objekt> generovaneModuly = new Dictionary<String, Objekt>();
			
			foreach (Modul m in moduly)
			{
				Projekt.DATABAZA[m.meno].clkRstSignal = true;
			}
			foreach (Modul m in moduly)
			{
				foreach (var modul in m.generujLucidKod(cestaSrc))
				{
					try
					{
						generovaneModuly.Add(modul.Value, m.objekty[modul.Key]);
					}
					catch (ArgumentException) {}
				}
			}
			foreach (var modul in generovaneModuly)
			{
				modul.Value.vytvorModul(cestaSrc, modul.Key);
			}
			System.IO.File.WriteAllText(@cestaKompilacie+"\\"+meno+".mojo", generujLucidProjektInfo(generovaneModuly));
			System.IO.File.WriteAllText(@cestaSrc+"\\mojo_top.luc", vytvorMojoTopModul());
		}
		
		String generujLucidProjektInfo(Dictionary<String, Objekt> generovaneModuly)
		{
			String vysl = "";
			vysl += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n";
			vysl += "<project name=\"" + meno + "\" board=\"Mojo V3\" language=\"Lucid\">\r\n";
			vysl += Objekt.tabulator(1) + "<files>\r\n";
			vysl += Objekt.tabulator(2) + "<src top=\"true\">mojo_top.luc</src>\r\n";
			foreach (String modul in generovaneModuly.Keys)
			{
				vysl += Objekt.tabulator(2) + "<src>" + modul + generovaneModuly[modul].typModulu() + "</src>\r\n";
			}
			foreach (Modul m in moduly)
			{
				vysl += Objekt.tabulator(2) + "<src>" + m.meno + ".luc</src>\r\n";
			}
			vysl += Objekt.tabulator(2) + "<ucf lib=\"true\">mojo.ucf</ucf>\r\n";
			vysl += Objekt.tabulator(2) + "<component>spi_slave.luc</component>\r\n";
			vysl += Objekt.tabulator(2) + "<component>uart_rx.luc</component>\r\n";
			vysl += Objekt.tabulator(2) + "<component>cclk_detector.luc</component>\r\n";
			vysl += Objekt.tabulator(2) + "<component>reset_conditioner.luc</component>\r\n";
			vysl += Objekt.tabulator(2) + "<component>uart_tx.luc</component>\r\n";
			vysl += Objekt.tabulator(2) + "<component>avr_interface.luc</component>\r\n";
			vysl += Objekt.tabulator(1) + "</files>\r\n";
			vysl += "</project>\r\n";
			return vysl;
		}
		
		public static String vytvorMojoTopModul()
		{
			return @"module mojo_top(
	input clk,
	input rst_n,
	output led [8],
	input cclk,
	output spi_miso,
	input spi_ss,
	input spi_mosi,
	input spi_sck,
	output spi_channel [4],
	input avr_tx,
	output avr_rx,
	input avr_rx_busy
) {

.clk(clk), .rst(~rst_n){
	avr_interface avr;
	main main;
}

always {
    avr.cclk = cclk;
    avr.spi_ss = spi_ss;
    avr.spi_mosi = spi_mosi;
    avr.spi_sck = spi_sck;
    avr.rx = avr_tx;

	spi_miso = avr.spi_miso;
	spi_channel = avr.spi_channel;
	avr_rx = avr.tx;
	
	avr.channel = hf;
	avr.tx_block = avr_rx_busy;
	
	main.read_new = avr.new_rx_data;
	main.read_data = avr.rx_data;
	avr.new_tx_data = main.write_new;
	avr.tx_data = main.write_data;
	main.write_busy = avr.tx_busy;
	
	led = 8h00;
	}
}";
			
		}
	}
}

class IntTuple
{
	public int a, b;
	public IntTuple(int nA, int nB)
	{
		a = nA;
		b = nB;
	}
}

class TempPouzitieModulu
{
	public int idModul;
	public IntTuple sur;
	public String id;
	public String menoModulu;
	
	public TempPouzitieModulu(int nIdModul, IntTuple nSur, String nId, String nMenoModulu)
	{
		idModul = nIdModul;
		sur = nSur;
		id = nId;
		menoModulu = nMenoModulu;
	}
}
