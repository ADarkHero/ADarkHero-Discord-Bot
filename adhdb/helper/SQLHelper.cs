﻿using Discord.WebSocket;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Resources;

namespace adhdb.bot
{
	class SQLHelper
	{
		private SQLiteConnection sqlite = new SQLiteConnection("Data Source=database.db");

		private SocketMessage Msg;

		private ResourceManager rm;

		public SQLHelper()
		{
			rm = new ResourceManager("adhdb.language." + Properties.Settings.Default.Language + ".SQLHelper", Assembly.GetExecutingAssembly());
		}
		public SQLHelper(SocketMessage message)
		{
			try
			{
				Msg = message;

				rm = new ResourceManager("adhdb.language." + Properties.Settings.Default.Language + ".SQLHelper", Assembly.GetExecutingAssembly());
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}


		/// <summary>
		/// Reads settings from the database and saves them to setting variables.
		/// </summary>
		public void ReadSettings()
		{
			try
			{
				Properties.Settings.Default.DiscordToken = File.ReadAllText("token.txt");
				Properties.Settings.Default.Save();

				DataTable botSettings = this.SelectSQL("SELECT * FROM settings");
				foreach (DataRow row in botSettings.Rows)
				{
					if (row["SettingsName"].ToString() == "DiscordChar")
					{
						Properties.Settings.Default.DiscordChar = row["SettingsValue"].ToString();
						Properties.Settings.Default.Save();
					}

					if (row["SettingsName"].ToString() == "YoutubeApi")
					{
						Properties.Settings.Default.YoutubeApi = row["SettingsValue"].ToString();
						Properties.Settings.Default.Save();
					}

					if (row["SettingsName"].ToString() == "Language")
					{
						Properties.Settings.Default.Language = row["SettingsValue"].ToString();
						Properties.Settings.Default.Save();
					}

					if (row["SettingsName"].ToString() == "SpotifyClientID")
					{
						Properties.Settings.Default.SpotifyClientID = row["SettingsValue"].ToString();
						Properties.Settings.Default.Save();
					}

					if (row["SettingsName"].ToString() == "SpotifyClientSecret")
					{
						Properties.Settings.Default.SpotifyClientSecret = row["SettingsValue"].ToString();
						Properties.Settings.Default.Save();
					}
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <param name="commandText"></param>
		public String InsertNewCommand(string commandName, string commandText)
		{
			try
			{
				SQLiteCommand insertSQL = new SQLiteCommand(
				"INSERT INTO commands (CommandName, CommandComment, CommandType, CommandRegex, CommandObject, CommandFunction, CommandAlias, CommandRights) " +
				"VALUES (@param1,@param2,99,null,null,null,null,null)", sqlite);
				insertSQL.Parameters.Add(new SQLiteParameter("@param1", commandName));
				insertSQL.Parameters.Add(new SQLiteParameter("@param2", commandText));

				sqlite.Open();  //Initiate connection to the db

				insertSQL.ExecuteNonQuery();
				return rm.GetString("ListAllFunctionsSuccess") + "\r\n" + commandName + " | " + commandText;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return rm.GetString("ListAllFunctionsError") + "\r\n\r\n" + ex.Message;
			}


		}

		/// <summary>
		/// Lists all functions
		/// </summary>
		/// <returns></returns>
		public String ListAllFunctions()
		{
			try
			{
				String returnString = "";

				DataTable commands = SelectSQL("SELECT * FROM commands WHERE CommandAlias is null ORDER BY CommandName");
				foreach (DataRow row in commands.Rows)
				{
					returnString += "**";
					if (row["CommandRegex"].ToString() == "")
					{
						returnString += Properties.Settings.Default.DiscordChar;
					}


					returnString += row["CommandName"].ToString() + "**: ";

					try
					{
						if (!String.IsNullOrEmpty(row["CommandComment_" + Properties.Settings.Default.Language].ToString()))
						{
							returnString += row["CommandComment_" + Properties.Settings.Default.Language].ToString();
						}
						else
						{
							throw new Exception();
						}

					}
					catch (Exception ex)
					{
						returnString += row["CommandComment"].ToString();
					}

					returnString += "\r\n";
				}

				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return rm.GetString("ListAllFunctionsError") + "\r\n\r\n" + ex.ToString();
			}
		}


		/// <summary>
		/// Reads stuff from sql database.
		/// </summary>
		/// <param name="query">Select query that should be executed.</param>
		/// <returns>Datatable with the queries content.</returns>
		public DataTable SelectSQL(string query)
		{
			try
			{
				SQLiteDataAdapter ad;
				DataTable dt = new DataTable();

				SQLiteCommand cmd;
				sqlite.Open();  //Initiate connection to the db
				cmd = sqlite.CreateCommand();
				cmd.CommandText = query;  //set the passed query
				ad = new SQLiteDataAdapter(cmd);
				ad.Fill(dt); //fill the datasource

				sqlite.Close();
				return dt;
			}
			catch (SQLiteException ex)
			{
				Logger logger = new Logger(ex.ToString());
				return null;
			}

		}

		/// <summary>
		/// Writes the language to the database
		/// </summary>
		/// <param name="lang">Language code.</param>
		public String SetLanguage(String lang)
		{
			try
			{
				Properties.Settings.Default.Language = lang;
				rm = new ResourceManager("adhdb.language." + lang + ".SQLHelper", Assembly.GetExecutingAssembly());
				//If the language does not exist, this method throws an error
				String returnString = rm.GetString("SetLanguageLanguageSet");

				SQLiteCommand insertSQL = new SQLiteCommand(
								"UPDATE settings " +
								"SET SettingsValue = @param1 " +
								"WHERE SettingsName = 'Language'", sqlite);
				insertSQL.Parameters.Add(new SQLiteParameter("@param1", lang));

				sqlite.Open();  //Initiate connection to the db

				insertSQL.ExecuteNonQuery();

				return returnString;
			}
			catch (SQLiteException ex)
			{
				Logger logger = new Logger(ex.ToString());
				return rm.GetString("SetLanguageLanguageNotExistent") + "\r\n\r\n" + ex.ToString();
			}
		}

	}
}
