using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace ADarkHeroDiscordBot.bot
{
	class SettingsReader
	{
		private SQLiteConnection sqlite = new SQLiteConnection("Data Source=database.db");

		//TODO: Put in config-file
		public string DiscordChar { get; set; } //Character, that indicates the start of a discord command
		public string DiscordToken { get; } //Token of the Discord Bot


		public SettingsReader()
		{
			DiscordToken = File.ReadAllText("token.txt");
			readSettings();
		}

		private void readSettings()
		{
			DataTable botSettings = selectSQL("SELECT * FROM settings");
			foreach (DataRow row in botSettings.Rows)
			{
				if (row["SettingsName"].ToString() == "DiscordChar")
				{
					DiscordChar = row["SettingsValue"].ToString();
				}
			}

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
