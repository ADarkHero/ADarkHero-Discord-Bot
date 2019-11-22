using Discord;
using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adhdb.bot
{
	class Reactor
	{
		public Reactor()
		{

		}

		/// <summary>
		/// Checks if the message contains specific strings.
		/// </summary>
		/// <param name="msg">Message that should be checked.</param>
		/// <returns>Task.</returns>
		public async Task ReactorAsync(SocketMessage msg)
		{
			try
			{
				var usermsg = msg as IUserMessage;
				await ReactorAsync(usermsg);
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}
		public async Task ReactorAsync(IUserMessage usermsg)
		{
			try
			{
				String content = usermsg.Content.ToLower();

				/*
				 * Reacting to usernames
				 */

				if (content.Contains("162591575250042881"))
				{
					await usermsg.AddReactionAsync(new Emoji("🍜"));
				}

				if (content.Contains("189035680310099968"))
				{
					await usermsg.AddReactionAsync(new Emoji("🦑"));
				}

				if (content.Contains("189031931701231616"))
				{
					await usermsg.AddReactionAsync(new Emoji("🐧"));
					await usermsg.AddReactionAsync(new Emoji("⚔"));
				}

				if (content.Contains("565867907423404035"))
				{
					await usermsg.AddReactionAsync(new Emoji("🦴"));
				}

				if (content.Contains("638730142687952896"))
				{
					await usermsg.AddReactionAsync(new Emoji("🍞"));
				}

				if (content.Contains("347809880540971028"))
				{
					await usermsg.AddReactionAsync(new Emoji("👻"));
				}

				if (content.Contains("84978507234410496"))
				{
					await usermsg.AddReactionAsync(new Emoji("🐧"));
				}

				if (content.Contains("345633776199794689"))
				{
					await usermsg.AddReactionAsync(new Emoji("\U0001F1F1"));
					await usermsg.AddReactionAsync(new Emoji("\U0001F1FA"));
					await usermsg.AddReactionAsync(new Emoji("\U0001F1E6"));
				}

				if (content.Contains("184216641314357248"))
				{
					await usermsg.AddReactionAsync(new Emoji("👄"));
				}

				if (content.Contains("219229420420988929"))
				{
					await usermsg.AddReactionAsync(new Emoji("🐦"));
				}

				if (content.Contains("245589394759745536"))
				{
					await usermsg.AddReactionAsync(new Emoji("🧅"));
				}



				//Removes userids from the string, to not trigger multiple reactions
				//Also removes every number, that is longer than 3 characters, so the bot will not trigger with links.
				content = Regex.Replace(content, @"\d{4,}", "");

				/*
				 * Reacting to keywords
				 */

				if (content.Contains("69") ||
				content.Contains("lewd") ||
				content.Contains("nude") ||
				content.Contains("penis") ||
				content.Contains("sex") ||
				content.Contains("friends"))
				{
					await usermsg.AddReactionAsync(new Emoji("😏"));
				}

				if (content.Contains("20") && (!content.Contains("w20") && !content.Contains("d20")))
				{
					//Facepalm emoji
					await usermsg.AddReactionAsync(new Emoji("\U0001F926"));
				}

				if (content.Equals("1") || content.Contains("**1**") || content.Contains(" 1**") || content.Contains(" 1 "))
				{
					await usermsg.AddReactionAsync(new Emoji("👍"));
				}

				if (content.Contains("13"))
				{
					await usermsg.AddReactionAsync(new Emoji("🖤"));
				}

				if (content.Contains("42"))
				{
					await usermsg.AddReactionAsync(new Emoji("🔥"));
				}

				if (content.Contains("420"))
				{
					await usermsg.AddReactionAsync(new Emoji("🍁"));
				}

				if (content.Contains("666"))
				{
					await usermsg.AddReactionAsync(new Emoji("😈"));
				}

				if (content.Contains("kill me"))
				{
					await usermsg.AddReactionAsync(new Emoji("🛑"));
				}

				if (content.Contains("fuck me"))
				{
					await usermsg.AddReactionAsync(new Emoji("😏"));
					await usermsg.AddReactionAsync(new Emoji("👍"));
				}

				if (content.Contains("uwu") || content.Contains("owo"))
				{
					await usermsg.AddReactionAsync(new Emoji("🐙"));
				}

				if (content.Contains("love") || content.Contains("liebe"))
				{
					await usermsg.AddReactionAsync(new Emoji("❤"));
				}

				if (content.Contains("lul"))
				{
					await usermsg.AddReactionAsync(new Emoji("🦀"));
				}

				if (content.Contains("nicole") || content.Contains("347809880540971028"))
				{
					await usermsg.AddReactionAsync(new Emoji("💕"));
				}

				if (content.Contains("marina"))
				{
					await usermsg.AddReactionAsync(new Emoji("💖"));
				}

				if (content.Contains("jaden") || content.Contains("140236351315509248"))
				{
					await usermsg.AddReactionAsync(new Emoji("🐈"));
				}

				if (content.Contains("coffee") || content.Contains("kaffee"))
				{
					await usermsg.AddReactionAsync(new Emoji("☕"));
				}

				if (content.Contains("kappa"))
				{
					await usermsg.AddReactionAsync(new Emoji("🐧"));
				}

				if (content.Contains("rip"))
				{
					//F
					await usermsg.AddReactionAsync(new Emoji("\U0001F1EB"));
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// Checks for specific reactions and reacts to them.
		/// </summary>
		/// <param name="cachedMessage">Message that should be reacted to.</param>
		/// <param name="reaction">Reaction that was added to the message.</param>
		/// <returns></returns>
		public async Task ReactToReactions(Cacheable<IUserMessage, ulong> cachedMessage, SocketReaction reaction)
		{
			try
			{
				var message = await cachedMessage.GetOrDownloadAsync();

				//Debugging/Logging
				//await message.Channel.SendMessageAsync(reaction.Emote.Name);

				//If someone reacts with a heart, the bot reacts with a heart too. Love for everyone! <3
				if (reaction.Emote.Name.Contains("❤"))
				{
					await AddNewReaction(message, new Emoji("❤"));
				}

				//😏😏😏
				if (reaction.Emote.Name.Contains("😏"))
				{
					await AddNewReaction(message, new Emoji("😏"));
				}
			}
			catch (Exception ex)
			{
				Logger logger = new Logger(ex.ToString());
			}
		}

		/// <summary>
		/// Adds a new reaction.
		/// </summary>
		/// <param name="message">Message that should be reacted to.</param>
		/// <param name="emo">Emoji that should be added.</param>
		/// <returns></returns>
		public async Task AddNewReaction(IUserMessage message, Emoji emo)
		{
			await message.AddReactionAsync(emo);
		}
	}
}
