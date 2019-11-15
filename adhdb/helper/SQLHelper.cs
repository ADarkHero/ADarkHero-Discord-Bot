﻿using Discord.WebSocket;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace adhdb.bot
{
	class SQLHelper
	{
		private SQLiteConnection sqlite = new SQLiteConnection("Data Source=database.db");

		private SocketMessage Msg;

		public SQLHelper()
		{
		}
		public SQLHelper(SocketMessage message)
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
		/// 
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
				return "Das folgende command wurde erfolgreich angelegt:\r\n" + commandName + " | " + commandText;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return "Fehler beim Anlegen des commands.\r\n\r\n" + ex.Message;
			}


		}



		/// <summary>
		/// 
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
					returnString += row["CommandName"].ToString() + "**: " + row["CommandComment"].ToString() + "\r\n";
				}

				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return "Unbekannter Fehler!\r\n\r\n" + ex.ToString();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// 
		/// TODO: Probably move to DSA() => Don't forget to also change the function in the database
		public String ListAllDSAFunctions()
		{
			try
			{
				String returnString = "";

				DataTable commands = SelectSQL("SELECT * FROM dsa WHERE DSAAlias is null ORDER BY DSATalentName");
				foreach (DataRow row in commands.Rows)
				{
					returnString += "**" + row["DSATalentName"].ToString() + "** " +
						"(" + row["DSATrait1"].ToString() + ", " + row["DSATrait2"].ToString() + ", " + row["DSATrait3"].ToString() + "), ";
				}

				return returnString;
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
				return "Unbekannter Fehler!\r\n\r\n" + ex.ToString();
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
	}
}