using Discord.WebSocket;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adhdb.bot
{
	class LoL
	{
		private SocketMessage Msg;
		private String Com = "";
		private DataRow Row = null;

		public LoL()
		{

		}
		public LoL(SocketMessage message, string command, DataRow drow)
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

		public String ShowBestBuilds()
		{
			try
			{
				String[] stringPairs = Msg.Content.Split(' ');
				if (stringPairs.Length > 1)
				{
					//Page where the information lies
					String url = "https://www.leagueofgraphs.com/champions/items/" + stringPairs[1].ToLower() + "/master";

					//Download information to string
					String htmlCode = "";
					try
					{
						WebClient client = new WebClient();
						client.Headers.Add("User-Agent", "C# console program");

						htmlCode = client.DownloadString(url);
					}
					catch (Exception ex)
					{
						return "Der Champion wurde nicht gefunden! Bitte Championnamen ohne Leer- und Sonderzeichen eingeben! Groß-/Kleinschreibung ist egal. z.B. !lol kogmaw";
					}

					String build = "**League of Legends Build for " + stringPairs[1] + "**\r\n" + url + "\r\n\r\n";
					String[,] buildHelper = new String[4, 2] {
						{ "Starting Items", "6" },
						{ "Boots", "1" },
						{ "Core Items", "40" },
						{ "End items", "10" }
					};
					for (int i = 0; i < 4; i++)
					{
						build = CreateBuild(htmlCode, build, buildHelper, i);
					}

					return build;
				}
				else
				{
					return "Bitte Championnamen angeben! z.B. !lol Ashe";
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return "Unbekannter Fehler!\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// Creates the lol build.
		/// </summary>
		/// <param name="htmlCode">HTML code with the build informations</param>
		/// <param name="build">String that should be written by the bot in the end</param>
		/// <param name="buildHelper">Informations about the build types (core, end, boots etc.) and how many items should be displayed.</param>
		/// <param name="i">Which build type are we currently on?</param>
		/// <returns>The string that should be written by the bot.</returns>
		private static string CreateBuild(string htmlCode, string build, string[,] buildHelper, int i)
		{
			//Creates build-headline (Starting Items, Boots, Core items...)
			build += "**" + buildHelper[i, 0] + "**\r\n🔹 ";

			//Time to get some items
			String htmlCodeTemp = htmlCode.Substring(htmlCode.IndexOf(buildHelper[i, 0]) + 1);

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(@htmlCodeTemp);

			//Search for img tags - The items names are in the imgs alt tags
			var img = doc.DocumentNode.Descendants("img");

			int j = 1;
			foreach (var node in img)
			{
				//Gets alt attributes.
				if (buildHelper[i, 0] == "Core Items" && j % 4 == 1)
				{
					build += "*";
				}
				build += node.GetAttributeValue("alt", string.Empty);

				//After how many items should a linebreak be inserted?
				int rest = 0;
				switch (buildHelper[i, 0])
				{
					case "Starting Items":
						rest = j % 3;
						break;
					case "Boots":
						rest = j % 1;
						break;
					case "Core Items":
						rest = j % 4;
						break;
					case "End items":
						rest = j % 10;
						break;
					default:
						rest = j % 4;
						break;
				}

				if (buildHelper[i, 0] == "Core Items" && j % 4 == 1)
				{
					build += "*";
				}


				j++;
				//If j is greater than the second number in the array, break to the next itembuilds
				if (j > Convert.ToUInt16(buildHelper[i, 1]))
				{
					build += "\r\n\r\n"; break;
				}
				//No rest: Start a new line
				else if (rest == 0)
				{
					build += "\r\n🔹 ";
				}
				//Add an arrow; no additional formatting needed.
				else
				{
					if (buildHelper[i, 0] == "End items")
					{
						build += " | ";
					}
					else
					{
						build += " => ";
					}
				}
			}

			return build;
		}
	}
}
