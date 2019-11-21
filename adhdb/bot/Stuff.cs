using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace adhdb.bot
{
	class Stuff
	{
		private SocketMessage Msg;
		private ResourceManager rm;

		public Stuff(SocketMessage message)
		{
			try
			{
				rm = new ResourceManager("adhdb.language." + Properties.Settings.Default.Language + ".Stuff", Assembly.GetExecutingAssembly());

				Msg = message;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// Pings an adress or the Google dns 10 times and returns the result to the bot.
		/// </summary>
		/// <returns>A string with the message, that the bot should display.</returns>
		public String PingAdressStr()
		{
			try
			{
				String returnString = "";
				String adress = "8.8.8.8";
				String[] stringPairs = Msg.Content.Split(' ');

				//Did the user input a custom adress?
				//If not, just ping google dns
				if (stringPairs.Length > 1)
				{
					adress = stringPairs[1];
				}

				for (int i = 0; i < 10; i++)
				{
					returnString += PingAddress(adress) + "\r\n";
					Thread.Sleep(50); //Wait 50 ms until next ping
				}
				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return rm.GetString("PingAdressStrError") + "\r\n\r\n" + ex.ToString();
			}
		}

		/// <summary>
		/// Pings an adress and returns a string with its result.
		/// </summary>
		/// <param name="adress">The adress that should be pinged.</param>
		/// <returns>A string with the ping result.</returns>
		private String PingAddress(String adress)
		{
			try
			{
				Ping p = new Ping();
				PingReply r;
				r = p.Send(adress);

				if (r.Status == IPStatus.Success)
				{
					return String.Format(rm.GetString("PingAddressString"), adress, r.Address.ToString(), r.RoundtripTime.ToString());
				}
				else
				{
					return rm.GetString("PingAddressHostNotFound");
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return rm.GetString("PingAddressError") + "\r\n\r\n" + ex.ToString();
			}
		}
	}
}
