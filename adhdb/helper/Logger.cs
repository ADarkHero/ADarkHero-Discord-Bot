using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adhdb.bot
{
	class Logger
	{
		public Logger(String log)
		{
			LogTextToFile(log);
		}

		/// <summary>
		/// Logs the inputted text to a log file.
		/// </summary>
		/// <param name="log">Text to log.</param>
		public void LogTextToFile(String log)
		{
			try
			{
				using (StreamWriter sw = File.AppendText("log.log"))
				{
					log = DateTime.Now.ToString("dd.MM.yyyy hh:mm:tt") + log;
					sw.WriteLine(log);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

		}
	}
}
