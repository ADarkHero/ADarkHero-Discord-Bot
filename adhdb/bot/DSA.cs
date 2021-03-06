﻿using adhdb.bot.DSAObj;
using Discord.WebSocket;
using HtmlAgilityPack;
using System;
using System.Data;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

namespace adhdb.bot
{
	class DSA
	{
		private SQLHelper sqlh = new SQLHelper();
		private SocketMessage Msg;
		private ResourceManager rm;
		private Random rand = new Random(); //The object has to be created here to work. If we create it in Roll(), it will always return the same number.

		public DSA(SocketMessage message)
		{
			try
			{
				rm = new ResourceManager("adhdb.language." + Properties.Settings.Default.Language + ".DSA", Assembly.GetExecutingAssembly());

				Msg = message;
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
						//@Auther rolls for the talent XX:
						returnString = String.Format(rm.GetString("RollDSABase"), Msg.Author.Id, row["DSATalentName"].ToString()) + "\r\n" +
										"**" + row["DSATrait1"].ToString() + ": " + d.Roll(20).ToString() + "** | " +
										"**" + row["DSATrait2"].ToString() + ": " + d.Roll(20).ToString() + "** | " +
										"**" + row["DSATrait3"].ToString() + ": " + d.Roll(20).ToString() + "**";
					}
					if (String.IsNullOrEmpty(returnString))
					{
						//The talent does not exist. Did you mistype?
						return rm.GetString("RollDSATalentNonExistent");
					}
					return returnString;
				}
				else
				{
					//@Auther rolled the following:
					returnString = String.Format(rm.GetString("RollDSABaseWithoutTalent"), Msg.Author.Id) + "**" +
						d.Roll(20).ToString() + " " + d.Roll(20).ToString() + " " + d.Roll(20).ToString() + "**!";
				}

				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error
				return rm.GetString("RollDSAError") + "\r\n\r\n" + ex.ToString();
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
					//Critical hit! XX damage!
					return String.Format(rm.GetString("CritDamage"), crit.ToString());
				}
				else
				{
					//Critical failure! You forgot to input a number, that should be calculated!
					return rm.GetString("CritMissingNumber");
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error.
				return rm.GetString("CritError") + "\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public String RandomLootStr()
		{
			try
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
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error.
				return rm.GetString("RandomLootStrError") + "\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public String MultipleRandomLootStr()
		{
			try
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
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error.
				return rm.GetString("MultipleLootStrError") + "\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataRow RandomLoot()
		{
			try
			{
				int rarityCheck = rand.Next(1, 101);

				String sql = "SELECT * from loot " +
					"INNER JOIN rarity ON loot.LootRarity = rarity.RarityID " +
					"WHERE RarityPercentageA <= " + rarityCheck + " AND RarityPercentageB >= " + rarityCheck + " ";

				DataTable dt = sqlh.SelectSQL(sql);

				int lootSelect = rand.Next(1, dt.Rows.Count + 1);
				foreach (DataRow row in dt.Rows)
				{
					lootSelect--;
					if (lootSelect == 0)
					{
						return row;
					}
				}
				return null;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return null;
			}


		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public String LootChances()
		{
			try
			{
				String sql = "SELECT *, " +
					"(SELECT count(LootRarity) FROM loot WHERE LootRarity = 0) AS Row0, " +
					"(SELECT count(LootRarity) FROM loot WHERE LootRarity = 1) AS Row1, " +
					"(SELECT count(LootRarity) FROM loot WHERE LootRarity = 2) AS Row2, " +
					"(SELECT count(LootRarity) FROM loot WHERE LootRarity = 3) AS Row3 " +
					"from rarity";

				DataTable dt = sqlh.SelectSQL(sql);

				String returnString = "";
				int i = 0;
				foreach (DataRow row in dt.Rows)
				{
					int rarity = Convert.ToInt16(row["RarityPercentageB"]) - Convert.ToInt16(row["RarityPercentageA"]) + 1;
					returnString += "**" + row["RarityName"] + "**: " + rarity.ToString() + "% *(" + row["Row" + i.ToString()] + " Items)*\r\n";
					i++;
				}

				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error.
				return rm.GetString("LootChancesError") + "\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// Lists all DSA talents
		/// </summary>
		/// <returns>String that gets displayed by the bot.</returns>
		public String ListAllDSAFunctions()
		{
			try
			{
				String returnString = "";

				DataTable commands = sqlh.SelectSQL("SELECT * FROM dsa WHERE DSAAlias is null ORDER BY DSATalentName");
				foreach (DataRow row in commands.Rows)
				{
					returnString += "**" + row["DSATalentName"].ToString() + "** " +
						"(" + row["DSATrait1"].ToString() + ", " + row["DSATrait2"].ToString() + ", " + row["DSATrait3"].ToString() + "), ";
				}

				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return rm.GetString("ListAllDSAFunctionsError") + "\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// Takes 10 different tropes for characters and shows it to the user.
		/// </summary>
		/// <returns>String that should be displayed by the bot.</returns>
		public String DisplayCharacterGenerator()
		{
			try
			{
				String returnString = rm.GetString("DisplayCharacterGeneratorInfo") + "\r\n\r\n\r\n";
				for (int i = 0; i < 10; i++)
				{
					DSACharacterTrope dsa = GenerateCharacterTrope();
					returnString += "**" + dsa.Name + "** (" + dsa.Link + ")" + "\r\n\r\n";
				}
				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return rm.GetString("DisplayCharacterGeneratorError") + "\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// Reads trope details from tvtropes.
		/// </summary>
		/// <returns>A DSACharacterTrope object with all the tropes informations.</returns>
		private DSACharacterTrope GenerateCharacterTrope()
		{
			try
			{
				String url = "https://tvtropes.org/pmwiki/randomitem.php?p=1";
				String htmlCode = "";
				try
				{
					WebClient client = new WebClient();
					client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.87 Safari/537.36");

					htmlCode = client.DownloadString(url);
				}
				catch (Exception ex)
				{
					return new DSACharacterTrope("", ex.ToString(), rm.GetString("GenerateCharacterTropeError"));
				}

				var doc = new HtmlDocument();
				doc.LoadHtml(htmlCode);

				DSACharacterTrope c = new DSACharacterTrope();
				var list = doc.DocumentNode.SelectNodes("//meta");
				foreach (var node in list)
				{
					string metaname = node.GetAttributeValue("name", "");
					string content = node.GetAttributeValue("content", "");

					if (metaname.Contains("twitter:title"))
					{
						//Remove " - TV Tropes" from string
						content = content.Substring(0, content.IndexOf(" - TV Tropes"));
						c.Name = content;
						//Generate Link for the trope
						c.Link = "https://tvtropes.org/pmwiki/pmwiki.php/Main/" + Regex.Replace(content, "/[^A-Za-z0-9]/", "").Replace(" ", "");
					}
					if (metaname.Contains("twitter:description"))
					{
						c.Description = content + "...";
					}
				}

				return c;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return new DSACharacterTrope("", ex.ToString(), rm.GetString("GenerateCharacterTropeError"));
			}
		}

	}
}
