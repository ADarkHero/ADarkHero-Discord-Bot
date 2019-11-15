using Discord.WebSocket;
using System;
using System.Net;

namespace adhdb.bot
{
	class GitHub
	{
		private SocketMessage Msg;

		public GitHub()
		{

		}
		public GitHub(SocketMessage message)
		{
			try
			{
				Msg = message;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// Reads the current changelog from GitHub and displays it.
		/// </summary>
		/// <returns>String that should be displayed by the bot.</returns>
		public String DisplayChangelog()
		{
			try
			{
				string url = "https://api.github.com/repos/adarkhero/ADarkHero-Discord-Bot/releases";

				String returnString = "";
				using (WebClient wc = new WebClient())
				{
					//GitHub wants a user agent header
					wc.Headers.Add("User-Agent", "C# Program");
					var jsonText = wc.DownloadString(url);
					dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonText);

					//Create returnstring
					returnString = "__**" + data[0].tag_name + " - " + data[0].name + "**__ \r\n https://github.com/ADarkHero/ADarkHero-Discord-Bot/releases \r\n\r\n" + data[0].body;
				}

				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return ex.ToString();
			}

		}
	}
}
