using Discord;
using Discord.WebSocket;
using System;
using System.Linq;

namespace adhdb.bot
{
	class CommandManipulator
	{
		private SocketMessage Msg;

		public CommandManipulator()
		{

		}
		public CommandManipulator(SocketMessage message)
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
		/// Adds a new command.
		/// </summary>
		/// <returns>String that shows the user, that the command was added.</returns>
		public String AddNewCommand()
		{
			try
			{
				var user = Msg.Author as SocketGuildUser;
				var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Role");

				if (user.GuildPermissions.Administrator)
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
					return "Bitte gib alle notwendigen Argumente ein. z.B.: !addcommand ping Pong Pong Pong.";
				}

				//Only administrators can use this command.
				return "Dieser Befehl steht nur Administratoren zur Verfügung.";
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				//Unknown error!
				return "Unbekannter Fehler!\r\n\r\n" + ex.ToString();
			}

		}
	}
}
