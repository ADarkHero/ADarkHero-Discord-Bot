﻿using Discord;
using Discord.WebSocket;
using System;
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

				if (content.Contains("nicole"))
				{
					await usermsg.AddReactionAsync(new Emoji("💕"));
				}

				if (content.Contains("marina"))
				{
					await usermsg.AddReactionAsync(new Emoji("💖"));
				}

				if (content.Contains("jaden"))
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
					await usermsg.AddReactionAsync(new Emoji("☠"));
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
