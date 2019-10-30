using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adhdb.bot
{
	class CommandManipulator
	{
		private SocketMessage Msg;
		private String Com = "";
		private DataRow Row = null;

		public CommandManipulator()
		{

		}
		public CommandManipulator(SocketMessage message, string command, DataRow drow)
		{
			Msg = message;
			Com = command;
			Row = drow;
		}

		public String AddNewCommand()
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

					return sqlh.InsertNewCommand(stringPairs[1], commandText); ;
				}

				return "Bitte gib alle notwendigen Argumente ein. z.B.: !addcommand ping Pong Pong Pong.";
			}

			return "Dieser Befehl steht nur Administratoren zur Verfügung.";
		}
	}
}
