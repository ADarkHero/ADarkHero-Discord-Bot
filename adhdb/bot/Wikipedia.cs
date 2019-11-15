using Discord.WebSocket;
using System;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Xml.Linq;

namespace adhdb.bot
{
	class Wikipedia
	{
		private SocketMessage Msg;
		private SQLHelper sqlh = new SQLHelper();
		private ResourceManager rm;

		public Wikipedia(SocketMessage message)
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
		/// Displays the first paragraph of a Wikipedia page in Dicord.
		/// </summary>
		/// <returns>Message that should be displayed by the bot.</returns>
		public String ReadWikipediaEntry()
		{
			try
			{
				String[] stringPairs = Msg.Content.Split(' ');
				if (stringPairs.Length > 1)
				{
					String search = Msg.Content.Remove(0, Msg.Content.IndexOf(' ') + 1).Replace(" ", "_");
					String corrSearch = SearchOnWikipedia(search);

					String returnString = GetWikipediaArticle(corrSearch);

					//Check if the search term actually exists.
					if (String.IsNullOrEmpty(returnString))
					{
						//The term was not found.
						return rm.GetString("ReadWikipediaEntryNoEntriesFound");
					}
					else
					{
						//Put link in front of the string
						returnString = "https://" + Properties.Settings.Default.Language + ".wikipedia.org/wiki/" + corrSearch.Replace(" ", "_") + "\r\n" + returnString;

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
					return rm.GetString("ReadWikipediaEntryMissingSearchterm");
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error.
				return rm.GetString("ReadWikipediaEntryError") + "\r\n\r\n" + ex.ToString();
			}

		}

		/// <summary>
		/// Searches a term on wikipedia and returns the first result.
		/// </summary>
		/// <param name="search">The term you want to search Wikipedia for.</param>
		/// <returns>The name of the first Wikipedia result.</returns>
		private string SearchOnWikipedia(String search)
		{
			try
			{
				String sUrl = "https://" + Properties.Settings.Default.Language + ".wikipedia.org/w/api.php?action=query&format=xml&list=search&srsearch=" + search;

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
				return rm.GetString("SearchOnWikipediaError") + "\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// Displays the text of a Wikipedia article
		/// </summary>
		/// <param name="search">The article you want to dispay.</param>
		/// <returns>The text of the article or an empty String, if the article does not exist.</returns>
		private string GetWikipediaArticle(String search)
		{
			try
			{
				String url = "https://" + Properties.Settings.Default.Language + ".wikipedia.org/w/api.php?format=xml&action=query&prop=extracts&explaintext=1&titles=" + search;

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
				return rm.GetString("SearchOnWikipediaError") + "\r\n\r\n" + ex.ToString();
			}
		}


	}
}
