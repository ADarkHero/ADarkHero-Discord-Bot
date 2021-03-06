﻿using Discord.WebSocket;
using System;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;

namespace adhdb.bot
{
	class Youtube
	{
		private SocketMessage Msg;
		private ResourceManager rm;

		public Youtube(SocketMessage message)
		{
			try
			{
				rm = new ResourceManager("adhdb.language." + Properties.Settings.Default.Language + ".LoL", Assembly.GetExecutingAssembly());

				Msg = message;
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
								"&key=" + Properties.Settings.Default.YoutubeApi;

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
						Logger logger = new Logger(rm.GetString("SearchVideoError"));
						//Unknown error. Please try again?
						return rm.GetString("SearchVideoError");
					}

					return returnString;
				}
				else
				{
					//Please input a search result!
					return rm.GetString("SearchVideoMissingSearchTerm");
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
