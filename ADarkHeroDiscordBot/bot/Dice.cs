using ADarkHeroDiscordBot.bot;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADarkHeroDiscordBot.bot
{
	public class Dice
	{
		private SettingsReader Sett = new SettingsReader();
		private SocketMessage Msg;
		private String Command = "";

		public Dice(SocketMessage message)
		{
			Msg = message;
			Command = Msg.Content.Substring(Sett.DiscordChar.Length);
		}

		/// <summary>
		/// Rolls a single dice.
		/// </summary>
		/// <param name="roll">What is the highest number on the dice?</param>
		/// <returns>The number, that got rolled.</returns>
		private int Roll(int roll)
		{
			Random random = new Random();
			int diceRoll = random.Next(0, roll + 1);


			return diceRoll;
		}

		/// <summary>
		/// Rolls a single dice and returns the string, that should be displayed by the bot.
		/// </summary>
		/// <returns>The string, that should be displayed by the bot.</returns>
		public String RollStr()
		{
			try
			{
				int diceRoll = Convert.ToInt16(Regex.Replace(Command, "[^0-9]", ""));
				return "<@" + Msg.Author.Id + "> hat eine " + Roll(diceRoll) + " gewürfelt!";
			}
			catch (Exception ex)
			{
				return "Bitte eine Ganzzahl eingeben, um Würfel zu werfen. z.B. w20, w6, d20, d6 etc.\r\n\r\n" + ex.ToString();
			}
		}
	}


}
