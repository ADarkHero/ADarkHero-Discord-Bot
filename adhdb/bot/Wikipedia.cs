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
					String corrSearch = SearchOnWikipedia(search);

					String returnString = GetWikipediaArticle(corrSearch);

					//Check if the search term actually exists.
					if (String.IsNullOrEmpty(returnString))
					{
						//The term was not found.
						return "Zu dem Begriff wurden leider keine Einträge gefunden.";
					}
					else
					{
						//Put link in front of the string
						returnString = "https://de.wikipedia.org/wiki/" + corrSearch.Replace(" ", "%20") + "\r\n" + returnString;

						//We don't want to spam everything... We only want to display the first paragraph.
						if (returnString.Contains("=="))
						{
							returnString = returnString.Substring(0, returnString.IndexOf("=="));
						}
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
				//Unknown error.
				return "Unbekannter Fehler.\r\n\r\n" + ex.ToString();
			}

		}

		/// <summary>
		/// Searches a term on wikipedia and returns the first result.
		/// </summary>
		/// <param name="search">The term you want to search Wikipedia for.</param>
		/// <returns>The name of the first Wikipedia result.</returns>
		private static string SearchOnWikipedia(String search)
		{
			try
			{
				String sUrl = "https://de.wikipedia.org/w/api.php?action=query&format=xml&list=search&srsearch=" + search;

				String corrSearch = "";
				using (WebClient wc = new WebClient())
				{
					wc.Encoding = Encoding.UTF8;
					var xml = wc.DownloadString(sUrl);

					//Create xml document and select node
					XDocument document = XDocument.Parse(xml);
					//Get all search entries (p)
					var extract = document.Descendants("p");

					//Return the first entry.
					foreach (var foo in extract)
					{
						corrSearch = foo.Attribute("title").Value;
						if (!String.IsNullOrEmpty(corrSearch)) { break; }
					}
				}

				return corrSearch;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error.
				return "Unbekannter Fehler.\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// Displays the text of a Wikipedia article
		/// </summary>
		/// <param name="search">The article you want to dispay. Note: The article must exist!</param>
		/// <returns>The text of the article.</returns>
		private static string GetWikipediaArticle(String search)
		{
			try
			{
				String url = "https://de.wikipedia.org/w/api.php?format=xml&action=query&prop=extracts&explaintext=1&titles=" + search;

				using (WebClient wc = new WebClient())
				{
					wc.Encoding = Encoding.UTF8;
					var xml = wc.DownloadString(url);

					//Create xml document and select node
					XDocument document = XDocument.Parse(xml);
					var extract = document.Descendants("extract");

					foreach (var foo in extract)
					{
						return foo.Value;
					}
				}

				return "";
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error.
				return "Unbekannter Fehler.\r\n\r\n" + ex.ToString();
			}
		}


	}
}
