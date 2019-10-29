using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adhdb.bot
{
	class SQLHelper
	{
		private SQLiteConnection sqlite = new SQLiteConnection("Data Source=database.db");

		private SocketMessage Msg;
		private String Com = "";
		private DataRow Row = null;

		public string DiscordChar { get; set; } //Character, that indicates the start of a discord command
		public string DiscordToken { get; } //Token of the Discord Bot

		public SQLHelper()
		{
			DiscordToken = File.ReadAllText("token.txt");
			readSettings();
		}
		public SQLHelper(SocketMessage message, string command, DataRow drow)
		{
			Msg = message;
			Com = command;
			Row = drow;
		}


		/// <summary>
		/// 
		/// </summary>
		private void readSettings()
		{
			DataTable botSettings = this.selectSQL("SELECT * FROM settings");
			foreach (DataRow row in botSettings.Rows)
			{
				if (row["SettingsName"].ToString() == "DiscordChar")
				{
					DiscordChar = row["SettingsValue"].ToString();
				}
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public String ListAllFunctions()
		{
			String returnString = "";

			DataTable commands = selectSQL("SELECT * FROM commands ORDER BY CommandName");
			foreach (DataRow row in commands.Rows)
			{
				returnString += "**" + DiscordChar + row["CommandName"].ToString() + "**: " + row["CommandComment"].ToString() + "\r\n";
			}

			return returnString;
		}

		/// <summary>
		/// Reads stuff from sql database.
		/// </summary>
		/// <param name="query">Select query that should be executed.</param>
		/// <returns>Datatable with the queries content.</returns>
		public DataTable selectSQL(string query)
		{
			SQLiteDataAdapter ad;
			DataTable dt = new DataTable();

			try
			{
				SQLiteCommand cmd;
				sqlite.Open();  //Initiate connection to the db
				cmd = sqlite.CreateCommand();
				cmd.CommandText = query;  //set the passed query
				ad = new SQLiteDataAdapter(cmd);
				ad.Fill(dt); //fill the datasource
			}
			catch (SQLiteException ex)
			{
				//Add your exception code here.
			}
			sqlite.Close();
			return dt;
		}
	}
}
