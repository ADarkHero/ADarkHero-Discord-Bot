using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord;
using Discord.WebSocket;
using System.Text.RegularExpressions;
using adhdb.bot;
using System.Data;
using System.Reflection;
using System.IO;

namespace adhdb
{
	public class Program
	{
		static Form1 MyForm;

		private DiscordSocketClient _client;
		private SQLHelper sqlh = new SQLHelper();


		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			//Code to show form
			/*
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				MyForm = new Form1();
				Application.Run(MyForm);
			*/

			//Starts the discord bot
			_client = new DiscordSocketClient();
			_client.MessageReceived += MessageReceivedAsync;
			_client.ReactionAdded += ReactionReceivedAsync;
			await _client.LoginAsync(TokenType.Bot, sqlh.DiscordToken);
			await _client.StartAsync();

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}

		/// <summary>
		/// Does something, when a message is received.
		/// </summary>
		/// <param name="message">Message, that was typed into Discord.</param>
		/// <returns>Completed Task.</returns>
		private async Task MessageReceivedAsync(SocketMessage msg)
		{
			//Checks for commands
			String command = "";

			if (msg.Content.StartsWith(sqlh.DiscordChar))
			{

				command = msg.Content.Substring(sqlh.DiscordChar.Length).ToLower(); //Cuts discord char and makes the string lowercase
				if (command.Contains(" "))
				{
					command = command.Substring(0, command.IndexOf(" ")); //Cuts everything after first space
				}

				//Read functions from database
				String sql = "SELECT * FROM commands WHERE CommandName LIKE '%" + command + "%' OR CommandRegex is not null COLLATE NOCASE";
				DataTable sqlResult = sqlh.SelectSQL(sql);

				foreach (DataRow row in sqlResult.Rows)
				{
					/*
					 * Command types
					 * 0: Simple
					 * 99: User generated
					 * 100: Regex
					 * 101: Complex
					 */
					//Simple functions
					int commandType = Convert.ToInt16(row["CommandType"].ToString());
					if (row["CommandName"].ToString() == command && commandType < 100)
					{
						await msg.Channel.SendMessageAsync(row["CommandComment"].ToString());
					}
					//Regex and more complex functions
					else if (commandType >= 100 && (Regex.Match(command, row["CommandRegex"].ToString()).Success || row["CommandName"].ToString() == command))
					{
						String sendMessage = ExecuteFunctionByString(msg, command, row);
						//Discord doesn't like too long messages. Split it, if its too long.
						if (sendMessage.Length > 2000)
						{
							while (sendMessage.Length > 2000)
							{
								await msg.Channel.SendMessageAsync(sendMessage.Substring(0, 1999)); //Post 2000 chars
								sendMessage = sendMessage.Substring(1999); //Cut first 2000 chars
							}
						}
						await msg.Channel.SendMessageAsync(sendMessage);
					}
				}
			}
			//Adds reactions to stuff
			Reactor re = new Reactor(msg);

			//Log message
			Console.WriteLine(msg.ToString());
		}

		private async Task ReactionReceivedAsync(Cacheable<IUserMessage, ulong> cachedMessage,
			ISocketMessageChannel originChannel, SocketReaction reaction)
		{
			var message = await cachedMessage.GetOrDownloadAsync();

			//Hash Code for ❤ because other methods don't seem to work?
			//If someone reacts with a heart, the bot reacts with a heart too. Love for everyone! <3
			if (reaction.Emote.GetHashCode() == -842361668)
			{
				Reactor re = new Reactor();
				await re.AddNewReaction(message, new Emoji("❤"));
			}
		}




		/// <summary>
		/// We give the function an object name and a method name and the function exectues it.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="command"></param>
		/// <param name="row"></param>
		/// <returns></returns>
		private String ExecuteFunctionByString(SocketMessage msg, string command, DataRow row)
		{
			try
			{
				Type t = Type.GetType("adhdb.bot." + row["CommandObject"].ToString());
				Object[] args = { msg, command, row };
				object o = Activator.CreateInstance(t, args);

				Type thisType = o.GetType();
				MethodInfo theMethod = thisType.GetMethod(row["CommandFunction"].ToString());

				return (String)theMethod.Invoke(o, null);
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}
	}
}
