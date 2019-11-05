﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adhdb.bot
{
	class DSA
	{
		private SQLHelper sqlh = new SQLHelper();
		private SocketMessage Msg;
		private String Com = "";
		private DataRow Row = null;
		Random rand = new Random(); //The object has to be created here to work. If we create it in Roll(), it will always return the same number.

		public DSA()
		{

		}
		public DSA(SocketMessage message, string command, DataRow drow)
		{
			try
			{
				Msg = message;
				Com = command;
				Row = drow;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// Rolls three d20 for dsa talents.
		/// </summary>
		/// <returns>String for the bot.</returns>
		public String RollDSA()
		{
			try
			{
				String[] stringPairs = Msg.Content.Split(' ');
				String returnString = "";
				Dice d = new Dice();

				if (stringPairs.Length > 1)
				{
					String talent = stringPairs[1];
					String sql = "SELECT * FROM dsa WHERE DSATalentName LIKE '" + talent + "' COLLATE NOCASE";
					DataTable dt = sqlh.SelectSQL(sql);

					//Search for the right talent
					foreach (DataRow row in dt.Rows)
					{
						returnString = "<@" + Msg.Author.Id + "> würfelt auf " + row["DSATalentName"].ToString() + ":\r\n" +
					"**" + row["DSATrait1"].ToString() + ": " + d.Roll(20).ToString() + "** | " +
					"**" + row["DSATrait2"].ToString() + ": " + d.Roll(20).ToString() + "** | " +
					"**" + row["DSATrait3"].ToString() + ": " + d.Roll(20).ToString() + "**";
					}
					if (String.IsNullOrEmpty(returnString))
					{
						return "Das Talent existiert nicht. Wurde sich eventuell vertippt?";
					}
					return returnString;
				}
				else
				{
					returnString = "<@" + Msg.Author.Id + "> hat folgendes gewürfelt: **" +
						d.Roll(20).ToString() + " " + d.Roll(20).ToString() + " " + d.Roll(20).ToString() + "**!";
				}

				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return "Unbekannter Fehler.\r\n\r\n" + ex.ToString();
			}
		}


		/// <summary>
		/// Multiplies a number by 1.5
		/// </summary>
		/// <returns>String with the bot message.</returns>
		public String Crit()
		{
			try
			{
				String[] stringPairs = Msg.Content.Split(' ');
				if (stringPairs.Length > 1)
				{
					double crit = Convert.ToDouble(stringPairs[1]) * 1.5;
					return "Kritischer Treffer! **" + crit.ToString() + "** Schaden!";
				}
				else
				{
					return "Kritischer Fehlschlag! Es wurde vergessen, eine Zahl einzugeben, die berechnet werden soll!";
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return "Unbekannter Fehler.\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public String RandomLootStr()
		{
			DataRow dt = RandomLoot();

			String returnString = "";

			returnString += "**" + dt["LootName"].ToString() + "**";
			if (!String.IsNullOrEmpty(dt["LootFlavorText"].ToString()))
			{
				returnString += "\r\n\r\n" + "*\"" + dt["LootFlavorText"].ToString() + "\"*";
			}
			returnString += "\r\n\r\n" + "*(" + dt["RarityName"].ToString();
			if (!String.IsNullOrEmpty(dt["LootType"].ToString()))
			{
				returnString += " " + dt["LootType"].ToString() + ")* ";
			}
			else
			{
				returnString += ")* ";
			}
			if (!String.IsNullOrEmpty(dt["LootDescription"].ToString()))
			{
				returnString += dt["LootDescription"].ToString();
			}
			if (!String.IsNullOrEmpty(dt["LootPicture"].ToString()))
			{
				returnString += "\r\n\r\n" + dt["LootPicture"].ToString();
			}

			return returnString;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public String MultipleRandomLootStr()
		{
			String returnString = "";
			String[] stringPairs = Msg.Content.Split(' ');
			int lootNumber = 1;
			if (stringPairs.Length > 1)
			{
				lootNumber = Convert.ToInt16(stringPairs[1].ToString());
			}

			for (int i = 0; i < lootNumber; i++)
			{
				returnString += RandomLootStr();
				if (lootNumber > 1)
				{
					returnString += "\r\n\r\n\r\n";
				}
			}

			return returnString;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataRow RandomLoot()
		{
			String sql = "SELECT *, " +
				"(select sum(RarityPercentage) from loot INNER JOIN rarity ON loot.LootRarity = rarity.RarityID)  as TotalRarity " +
				"FROM loot INNER JOIN rarity ON loot.LootRarity = rarity.RarityID";
			DataTable dt = sqlh.SelectSQL(sql);

			Boolean first = true;
			int totalRarity = 0; int currentRarity = 0;
			foreach (DataRow row in dt.Rows)
			{
				//Implementing a rarity function. Common items should be returned more often, than rare ones.
				if (first)
				{
					totalRarity = Convert.ToInt16(row["TotalRarity"].ToString());
					currentRarity = rand.Next(1, totalRarity);
					first = false;
				}

				currentRarity -= Convert.ToInt16(row["RarityPercentage"].ToString());
				if (currentRarity <= 0)
				{
					return row;
				}
			}
			return null;
		}

	}
}