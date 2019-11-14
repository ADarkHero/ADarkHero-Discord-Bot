using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace adhdb.bot
{
	class Youtube
	{
		private SocketMessage Msg;
		private String Com = "";
		private DataRow Row = null;

		public Youtube()
		{

		}
		public Youtube(SocketMessage message, string command, DataRow drow)
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
		/// Searches a video on youtube and returns the result as String
		/// </summary>
		/// <returns>String that the bot should display.</returns>
		public String SearchVideo()
		{
			try
			{
				String[] stringPairs = Msg.Content.Split(' ');
				if (stringPairs.Length > 1)
				{
					SQLHelper sqlh = new SQLHelper();

					string jsonStr = string.Empty;
					string url = @"https://www.googleapis.com/youtube/v3/search" +
								"?part=snippet" +
								"&q=" + Msg.Content.Remove(0, Msg.Content.IndexOf(' ') + 1) +
								"&type=video" +
								"&maxResults=1" +
								"&key=" + sqlh.YoutubeApi;

					//Create a get-webrequst to get the apis data
					String returnString = "";
					using (WebClient wc = new WebClient())
					{
						wc.Encoding = Encoding.UTF8;

						var jsonText = wc.DownloadString(url);
						dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonText);

						//Create returnstring
						returnString = "**" + data.items[0].snippet.title + "**" +
										" von *" + data.items[0].snippet.channelTitle + "*\r\n\r\n" +
										data.items[0].snippet.description + "\r\n\r\n\r\n" +
										"https://www.youtube.com/watch?v=" + data.items[0].id.videoId;
					}

					//If there was some kind of error, return an error message
					if (String.IsNullOrEmpty(returnString))
					{
						Logger logger = new Logger("Unbekannter Fehler. Bitte erneut versuchen?");
						//Unknown error. Please try again?
						return "Unbekannter Fehler. Bitte erneut versuchen?";
					}

					return returnString;
				}
				else
				{
					//Please input a search result!
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
