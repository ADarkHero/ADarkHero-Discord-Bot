using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace adhdb.bot
{
	class Wikipedia
	{
		private SocketMessage Msg;
		private String Com = "";
		private DataRow Row = null;

		public Wikipedia()
		{

		}
		public Wikipedia(SocketMessage message, string command, DataRow drow)
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
		/// 
		/// </summary>
		/// <returns></returns>
		public String ReadWikipediaEntry()
		{
			try
			{
				String[] stringPairs = Msg.Content.Split(' ');
				if (stringPairs.Length > 1)
				{
					String search = Msg.Content.Remove(0, Msg.Content.IndexOf(' ') + 1).Replace(" ", "%20");
					String url = "https://de.wikipedia.org/w/api.php?format=xml&action=query&prop=extracts&explaintext=1&titles=" + search;
					String returnString = "";
					using (WebClient wc = new WebClient())
					{
						wc.Encoding = Encoding.UTF8;
						var xml = wc.DownloadString(url);

						//Create xml document and select node
						XDocument document = XDocument.Parse(xml);
						var extract = document.Descendants("extract");

						foreach (var foo in extract)
						{
							//Put link in front of the string
							returnString = "https://de.wikipedia.org/wiki/" + search + "\r\n" + foo.Value;

							//We don't want to spam everything... We only want the first paragraph.
							if (returnString.Contains("=="))
							{
								returnString = returnString.Substring(0, returnString.IndexOf("=="));
							}
						}
					}

					//Check if the search term actually exists.
					if (String.IsNullOrEmpty(returnString))
					{
						//The term was not found. Wikipedia considers casing of the searches.
						return "Der Begriff wurde leider nicht gefunden. Wikipedia achtet meistens auf die Groß-/Kleinschreibung des Suchbegriffes!";
					}


					return returnString;
				}
				else
				{
					//Please input a search term
					return "Bitte einen Suchbegriff eingeben!";
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return ex.ToString();
			}

		}
	}
}
