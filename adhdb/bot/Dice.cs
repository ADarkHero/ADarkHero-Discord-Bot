using adhdb.bot;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adhdb.bot
{
	public class Dice
	{
		private SQLHelper sqlh = new SQLHelper();
		private SocketMessage Msg;
		private String Com = "";
		private DataRow Row = null;
		Random rand = new Random(); //The object has to be created here to work. If we create it in Roll(), it will always return the same number.

		public Dice(SocketMessage message, string command, DataRow drow)
		{
			Msg = message;
			Com = command;
			Row = drow;
		}

		/// <summary>
		/// Rolls a single dice.
		/// </summary>
		/// <param name="roll">What is the highest number on the dice?</param>
		/// <returns>The number, that got rolled.</returns>
		private int Roll(int roll)
		{
			int diceRoll = rand.Next(1, roll + 1);

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
				int diceRoll = Convert.ToInt16(Regex.Replace(Com, "[^0-9]", ""));
				return "<@" + Msg.Author.Id + "> hat eine **" + Roll(diceRoll) + "** gewürfelt!";
			}
			catch (Exception ex)
			{
				return "Bitte eine Ganzzahl eingeben, um Würfel zu werfen. z.B. w20, w6, d20, d6 etc.\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// Rolls multiple dice and returns the string, that should be displayed by the bot.
		/// </summary>
		/// <returns>The string, that should be displayed by the bot.</returns>
		public String RollMultipleStr()
		{
			try
			{
				/* The regex is split into multiple groups
				 *
				 * Group 1: Number of dices
				 * Group 2: Dice indicator (d or w)
				 * Group 3: Maximum dice roll
				 * Group 4: Manipulator (+, -, *, /)
				 * Group 5: Manipulator value
				 */
				var match = Regex.Match(Com, @Row["CommandRegex"].ToString());

				int numberOfDice = Convert.ToInt16(match.Groups[1].Value);
				int diceRoll = Convert.ToInt16(match.Groups[3].Value);

				int roll = 0;
				double sum = 0;
				//Write the rolls to a string and calculate a sum of all rolls.
				String returnStr = "<@" + Msg.Author.Id + "> hat folgendes gewürfelt: **";
				for (int i = 0; i < numberOfDice; i++)
				{
					roll = Roll(diceRoll);
					returnStr += roll + ", ";
					sum += roll;
				}

				//Remove last , and add a ! instead!
				returnStr = returnStr.Remove(returnStr.Length - 2);
				returnStr += "**! ";

				try
				{
					//If there are manipulators, apply them at the end.
					String manipulator = match.Groups[4].Value;
					double manipulatorValue = Convert.ToDouble(match.Groups[5].Value.Replace(".", ","));

					sum = MathOperation(manipulator, sum, manipulatorValue);

					returnStr += "Das Ergebnis wurde um **" + manipulator + manipulatorValue.ToString() + "** abgeändert. ";
				}
				catch (Exception ex) { Console.WriteLine(ex.ToString()); }

				returnStr += "\r\nDas Gesamtergebnis beträgt **" + sum.ToString() + "**!";

				return returnStr;
			}
			catch (Exception ex)
			{
				return "Bitte eine Ganzzahl eingeben, um Würfel zu werfen. z.B. 1w20, 2w6, 3d20, 4d6 etc.\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public String RollDSA()
		{
			String[] stringPairs = Msg.Content.Split(' ');
			String returnString = "";
			if (stringPairs.Length > 1)
			{
				String talent = stringPairs[1];
				String sql = "SELECT * FROM dsa WHERE DSATalentName = '" + talent + "'";
				DataTable dt = sqlh.SelectSQL(sql);

				//Search for the right talent
				foreach (DataRow row in dt.Rows)
				{
					if (row["DSATalentName"].ToString() == talent)
					{
						returnString = "<@" + Msg.Author.Id + "> würfelt auf " + row["DSATalentName"].ToString() + ":\r\n" +
					"**" + row["DSATrait1"].ToString() + ": " + Roll(20).ToString() + "** | " +
					"**" + row["DSATrait2"].ToString() + ": " + Roll(20).ToString() + "** | " +
					"**" + row["DSATrait3"].ToString() + ": " + Roll(20).ToString() + "**!";
					}
				}
				return returnString;
			}
			else
			{
				returnString = "<@" + Msg.Author.Id + "> hat folgendes gewürfelt: **" +
					Roll(20).ToString() + " " + Roll(20).ToString() + " " + Roll(20).ToString() + "**!";
			}

			return returnString;
		}



		/// <summary>
		/// Flips a coin and returns the string, to be sent by the bot.
		/// </summary>
		/// <returns>String, that should be displayed.</returns>
		public String CoinFlipStr()
		{
			Boolean cf = CoinFlip();
			if (cf)
			{
				return "KOPF!";
			}
			return "ZAHL!";
		}

		/// <summary>
		/// Flips a coin.
		/// </summary>
		/// <returns>True/False</returns>
		private Boolean CoinFlip()
		{
			if (rand.Next(0, 2) == 1)
			{
				return true;
			}
			return false;
		}


		/// <summary>
		/// Does simple math.
		/// </summary>
		/// <returns>A string that contains the result of the math.</returns>
		public String DoSimpleMathStr()
		{
			var match = Regex.Match(Com, @Row["CommandRegex"].ToString());

			try
			{
				String mathOperator = match.Groups[2].Value;
				double x = Convert.ToDouble(match.Groups[1].Value);
				double y = Convert.ToDouble(match.Groups[3].Value);
				double result = MathOperation(mathOperator, x, y);

				return "Das Ergebnis von **" + x.ToString() + mathOperator + y.ToString() + "** ist **" + result.ToString() + "**!";
			}
			catch (Exception ex)
			{
				return "Ungültige Eingabe.\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// Makes mathematical operators out of "String operators". "Converts" (String)"+" in +
		/// </summary>
		/// <param name="op">Operator</param>
		/// <param name="x">First value</param>
		/// <param name="y">Second value</param>
		/// <returns>Result of the mathematical operation.</returns>
		private double MathOperation(string op, double x, double y)
		{
			switch (op)
			{
				case "+":
					return x + y;
				case "-":
					return x - y;
				case "*":
					return x * y;
				case "/":
					return x / y;
				default:
					return x;
			}
		}
	}


}
