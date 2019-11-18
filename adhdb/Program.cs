using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Text.RegularExpressions;
using adhdb.bot;
using System.Data;
using System.Reflection;
using Discord.Commands;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace adhdb
{
	public class Program
	{
		static Form1 MyForm;

		private DiscordSocketClient _client;
		private SQLHelper Sqlh = new SQLHelper();
		private List<Type> ExtensionList = new List<Type>();
		ResourceManager rm = null;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			try
			{
				//Code to show form
				/*
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					MyForm = new Form1();
					Application.Run(MyForm);
				*/

				//Read all settings
				Sqlh.ReadSettings();

				try
				{
					rm = new ResourceManager("adhdb.language." + Properties.Settings.Default.Language + ".Program", Assembly.GetExecutingAssembly());
				}
				catch (Exception ex)
				{
					Logger logger = new Logger(ex.ToString());
				}

				//Loads extensions
				LoadExtensions();

				//Starts the discord bot
				_client = new DiscordSocketClient();
				_client.MessageReceived += MessageReceivedAsync;
				_client.ReactionAdded += ReactionReceivedAsync;
				_client.Log += LogAsync;
				await _client.LoginAsync(TokenType.Bot, Properties.Settings.Default.DiscordToken);
				await _client.StartAsync();

				// Block this task until the program is closed.
				await Task.Delay(-1);
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// Loads all custom extensions
		/// </summary>
		private void LoadExtensions()
		{
			try
			{
				string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\ext\";
				foreach (string dll in Directory.GetFiles(path, "*.dll"))
				{
					var dl = Assembly.LoadFile(dll);

					foreach (Type type in dl.GetExportedTypes())
					{
						ExtensionList.Add(type);
					}
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// Does something, when a message is received.
		/// </summary>
		/// <param name="message">Message, that was typed into Discord.</param>
		/// <returns>Completed Task.</returns>
		private async Task MessageReceivedAsync(SocketMessage msg)
		{
			try
			{
				//Checks for commands
				String command = "";

				//Debugging/Logging
				//await message.Channel.SendMessageAsync((msg.Content.ToString());

				if (msg.Content.StartsWith(Properties.Settings.Default.DiscordChar))
				{

					command = msg.Content.Substring(Properties.Settings.Default.DiscordChar.Length).ToLower(); //Cuts discord char and makes the string lowercase
					if (command.Contains(" "))
					{
						command = command.Substring(0, command.IndexOf(" ")); //Cuts everything after first space
					}

					//Read functions from database
					String sql = "SELECT * FROM commands WHERE CommandName LIKE '%" + command + "%' OR CommandRegex is not null COLLATE NOCASE";
					DataTable sqlResult = Sqlh.SelectSQL(sql);

					Boolean commandFound = false;
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
							if (checkRights(msg, row["CommandRights"].ToString()))
							{
								await msg.Channel.SendMessageAsync(">>> " + row["CommandComment"].ToString());
							}
							else
							{
								await msg.Channel.SendMessageAsync(">>> ");
							}
							commandFound = true;
							break;
						}
						//Regex and more complex functions
						else if (commandType >= 100 && (Regex.Match(command, row["CommandRegex"].ToString()).Success || row["CommandName"].ToString() == command))
						{
							if (checkRights(msg, row["CommandRights"].ToString()))
							{
								String sendMessage = ExecuteFunctionByString(msg, row["CommandObject"].ToString(), row["CommandFunction"].ToString());
								//Discord doesn't like too long messages. Split it, if its too long.
								if (sendMessage.Length > 2000)
								{
									while (sendMessage.Length > 2000)
									{
										await msg.Channel.SendMessageAsync(">>> " + sendMessage.Substring(0, 1995)); //Post 2000 chars
										sendMessage = sendMessage.Substring(1995); //Cut first 2000 chars
									}
								}
								await msg.Channel.SendMessageAsync(">>> " + sendMessage);
							}
							else
							{
								await msg.Channel.SendMessageAsync(">>> ");
							}
							commandFound = true;
							break;
						}
					}

					if (!commandFound)
					{
						await msg.Channel.SendMessageAsync(">>> " + rm.GetString("CommandNotFound"));
					}
				}
				//Adds reactions to stuff
				Reactor re = new Reactor();
				await re.ReactorAsync(msg);
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// Checks, if the user has enought rights to use an command
		/// </summary>
		/// <param name="msg">Message that was sent by the user</param>
		/// <param name="rights">Rights that are needed to use a command</param>
		/// <returns></returns>
		private bool checkRights(SocketMessage msg, String rights)
		{
			if (String.IsNullOrEmpty(rights))
			{
				return true;
			}
			else
			{
				var user = msg.Author as SocketGuildUser;
				var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Role");

				//User needs server admin rights
				if (rights.Equals("1"))
				{
					if (user.GuildPermissions.Administrator)
					{
						return true;
					}
					return false;
				}
				//Add more right checks here
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cachedMessage"></param>
		/// <param name="originChannel"></param>
		/// <param name="reaction"></param>
		/// <returns></returns>
		private async Task ReactionReceivedAsync(Cacheable<IUserMessage, ulong> cachedMessage,
			ISocketMessageChannel originChannel, SocketReaction reaction)
		{
			Reactor re = new Reactor();
			await re.ReactToReactions(cachedMessage, reaction);
		}




		/// <summary>
		/// We give the function an object name and a method name and the function exectues it.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="command"></param>
		/// <param name="row"></param>
		/// <returns></returns>
		private String ExecuteFunctionByString(SocketMessage msg, String commandObject, String commandFunction)
		{
			try
			{
				Type t = Type.GetType("adhdb.bot." + commandObject);
				Object[] args = { msg };
				object o = null;
				try
				{
					o = Activator.CreateInstance(t, args);
				}
				//If we can't create an object the "normal" way, the object must be an extension.
				catch (Exception ex)
				{
					foreach (Type extType in ExtensionList)
					{
						if (extType.Name.Contains(commandObject))
						{
							o = Activator.CreateInstance(extType, args);
						}
					}
				}

				Type thisType = o.GetType();
				MethodInfo theMethod = thisType.GetMethod(commandFunction);

				return (String)theMethod.Invoke(o, null);
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return ex.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		private Task LogAsync(LogMessage message)
		{
			try
			{
				if (message.Exception is CommandException cmdException)
				{
					Logger logger = new Logger($"[Command/{message.Severity}] {cmdException.Command.Aliases.First()}"
						+ $" failed to execute in {cmdException.Context.Channel}.");
					logger.LogTextToFile(cmdException.ToString());
				}
				else
					Console.WriteLine($"[General/{message.Severity}] {message}");

				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return null;
			}
		}
	}
}
