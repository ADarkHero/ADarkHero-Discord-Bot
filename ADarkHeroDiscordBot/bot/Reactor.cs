using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADarkHeroDiscordBot.bot
{
	class Reactor
	{
		public Reactor()
		{

		}

		public Reactor(SocketMessage msg)
		{
			var usermsg = msg as IUserMessage;
			ReactorAsync(usermsg);
		}

		public Reactor(IUserMessage msg)
		{
			ReactorAsync(msg);
		}

		/// <summary>
		/// Checks if the message contains specific strings.
		/// </summary>
		/// <param name="msg">Message that should be checked.</param>
		/// <returns>Task.</returns>
		public async Task ReactorAsync(IUserMessage usermsg)
		{

			String content = usermsg.Content.ToLower();

			Console.WriteLine(content);

			if (content.Contains("69") ||
				content.Contains("lewd") ||
				content.Contains("nude") ||
				content.Contains("sex"))
			{
				await usermsg.AddReactionAsync(new Emoji("😏"));
			}

			if (content.Equals("1") || content.Contains("**1**"))
			{
				await usermsg.AddReactionAsync(new Emoji("👍"));
			}

			if (content.Equals("20") || content.Contains("**20**"))
			{
				await usermsg.AddReactionAsync(new Emoji("\U0001F926"));
			}
		}


		public async Task AddNewReaction(IUserMessage message, int v)
		{
			await message.AddReactionAsync(new Emoji("❤"));
		}
	}
}
