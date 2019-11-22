using Discord.WebSocket;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace adhdb.bot
{
	class Spotify
	{

		private SocketMessage Msg;
		private ResourceManager rm;

		public Spotify(SocketMessage message)
		{
			try
			{
				rm = new ResourceManager("adhdb.language." + Properties.Settings.Default.Language + ".Spotify", Assembly.GetExecutingAssembly());
				Msg = message;
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
		public String SpotifyTrackStr()
		{
			String result = SearchSpotifyTrackAsync().Result;
			//If we have a space in our results, it's probably an error and not a spotify track.
			if (result.Contains(" "))
			{
				return result;
			}
			return "https://open.spotify.com/track/" + result;
		}

		/// <summary>
		/// Searches the spotify API for an track
		/// </summary>
		/// <returns>A task string with the id of the first found track.</returns>
		public async Task<String> SearchSpotifyTrackAsync()
		{
			try
			{
				String[] stringPairs = Msg.Content.Split(' ');
				if (stringPairs.Length > 1)
				{
					CredentialsAuth auth = new CredentialsAuth(Properties.Settings.Default.SpotifyClientID, Properties.Settings.Default.SpotifyClientSecret);
					Token token = await auth.GetToken();
					SpotifyWebAPI _spotify = new SpotifyWebAPI()
					{
						AccessToken = token.AccessToken,
						TokenType = token.TokenType
					};

					//Search for the first track on Spotify
					String message = Msg.Content;
					String search = message.Substring(message.IndexOf(" "));
					SearchItem item = _spotify.SearchItemsEscaped(search, SearchType.Track, 1);

					return item.Tracks.Items[0].Id;
				}

				return rm.GetString("SearchSpotifyTrackAsyncMissingSearchTerm");
			}
			catch (ArgumentOutOfRangeException ex)
			{
				Logger logger = new Logger(ex.ToString());
				return rm.GetString("SearchSpotifyTrackAsyncTrackNotFound") + "\r\n\r\n" + ex.ToString();
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return rm.GetString("SearchSpotifyTrackAsyncError") + "\r\n\r\n" + ex.ToString();
			}
		}

	}
}
