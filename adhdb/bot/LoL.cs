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
			Msg = message;
			Com = command;
			Row = drow;
		}

		public String ShowBestBuilds()
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

				String build = "";
				String[,] buildHelper = new String[4, 2] {
				{ "Starting Items", "3" },
				{ "Boots", "1" },
				{ "Core Items", "40" },
				{ "End items", "5" }
			};
				for (int i = 0; i < 4; i++)
				{
					build += "**" + buildHelper[i, 0] + "**\r\n";
					//Time to get core items
					String htmlCodeTemp = htmlCode.Substring(htmlCode.IndexOf(buildHelper[i, 0]) + 1);

					HtmlDocument doc = new HtmlDocument();
					doc.LoadHtml(@htmlCodeTemp);

					var img = doc.DocumentNode.Descendants("img");

					int j = 1;
					foreach (var node in img)
					{
						//Gets alt attributes.
						build += node.GetAttributeValue("alt", string.Empty);

						int rest = j % 4;

						//40 Items are shown
						j++;
						if (j > Convert.ToUInt16(buildHelper[i, 1]))
						{
							build += "\r\n\r\n"; break;
						}
						else if (rest == 0)
						{
							build += "\r\n";
						}
						else
						{
							build += " => ";
						}
					}
				}

				return build;
			}
			else
			{
				return "Bitte Championnamen angeben! z.B. !lol Ashe";
			}

		}
	}
}
