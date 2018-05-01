using System;
using System.Diagnostics;

namespace DataWolf
{
	public class Analyzator
	{
		public const int STATE_END = 0;
		public const int STATE_START = 1;
		public const int STATE_NUMBER = 2;
		public const int STATE_WORD = 3;
		public const int STATE_SYMBOL = 4;
		
		int stav;
		int[][] automat;
		readonly Vstup vstup;
		public String token;
		public int poslednyStav;
		
		public Analyzator(String nVstup)
		{
			vstup = new Vstup(nVstup);
			nastavAutomat();
			vstup.next();
		}
		
		void nastavAutomat()
		{
			automat = new int[5][];
			for (int i = 0 ; i < 5 ; i++)
			{
				automat[i] = new int[256];
			}
			automat[STATE_START][32] = STATE_START;
			automat[STATE_START][10] = STATE_START;
			automat[STATE_START][13] = STATE_START;
			for (int i = 48 ; i <= 57 ; i++)//0..9
			{
				automat[STATE_START][i] = STATE_NUMBER;
				automat[STATE_NUMBER][i] = STATE_NUMBER;
				automat[STATE_WORD][i] = STATE_WORD;
			}
			for (int i = 65 ; i <= 90 ; i++)//A..Z
			{
				automat[STATE_START][i] = STATE_WORD;
				automat[STATE_WORD][i] = STATE_WORD;
			}
			for (int i = 97 ; i <= 122 ; i++)//a..z
			{
				automat[STATE_START][i] = STATE_WORD;
				automat[STATE_WORD][i] = STATE_WORD;
			}
			automat[STATE_WORD]['_'] = STATE_WORD;
			char[] symboly = {'[', ']', '(', ')', '{', '}', '#'};
			foreach (char symbol in symboly)
			{
				automat[STATE_START][symbol] = STATE_SYMBOL;
			}
		}
		
		public void scan()
		{
			token = "";
			stav = automat[STATE_START][vstup.look];
			poslednyStav = automat[STATE_START][vstup.look];
			while (stav != STATE_END)
			{
				if (stav != STATE_START)
				{
					token += vstup.look;
				}
				vstup.next();
				poslednyStav = stav;
				stav = automat[stav][vstup.look];
			}
		}
	}
}

class Vstup
	{
		readonly String vstup;
		int index;
		public char look;
		
		public Vstup(String nVstup)
		{
			vstup = nVstup;
			index = 0;
		}
		
		public void next()
		{
			if (index >= vstup.Length)
			{
				look = '\0';
			}
			else
			{
				look = vstup[index];
				index += 1;
			}
		}
	}
