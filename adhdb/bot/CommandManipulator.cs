using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace adhdb.bot
{
	class CommandManipulator
	{
		private SocketMessage Msg;
		private ResourceManager rm;

		public CommandManipulator(SocketMessage message)
		{
			try
			{
				rm = new ResourceManager("adhdb.language." + Properties.Settings.Default.Language + ".CommandManipulator", Assembly.GetExecutingAssembly());

				Msg = message;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// Adds a new command.
		/// </summary>
		/// <returns>String that shows the user, that the command was added.</returns>
		public String AddNewCommand()
		{
			try
			{
				String[] stringPairs = Msg.Content.Split(' ');

				//Only trigger if the user inputs 3 stringpairs (!addCommand ping Pong)
				if (stringPairs.Length > 2)
				{
					//Reconnects the string that we split earlier. Leaves out the first two pairs of course.
					String commandText = "";
					for (int i = 2; i < stringPairs.Length; i++)
					{
						commandText += stringPairs[i] + " ";
					}
					//Insert command into db
					SQLHelper sqlh = new SQLHelper();

					return sqlh.InsertNewCommand(stringPairs[1], commandText);
				}

				//Please input all neccessary arguments. For example !addcommand ping Pong Pong Pong.
				return rm.GetString("AddNewCommandMissingArguments");
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error!
				return rm.GetString("AddNewCommandError") + "\r\n\r\n" + ex.ToString();
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public String ChangeLanguage()
		{
			try
			{
				String[] stringPairs = Msg.Content.Split(' ');

				if (stringPairs.Length > 1)
				{
					SQLHelper sqlh = new SQLHelper();
					return sqlh.SetLanguage(stringPairs[1]);
				}
				return rm.GetString("ChangeLanguageMissingInput");
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error!
				return rm.GetString("ChangeLanguageError") + "\r\n\r\n" + ex.ToString();
			}
		}
	}
}
