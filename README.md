# ADarkHero Discord B(r)ot
![ADarkHero Discord Bot](https://i.imgur.com/BnYfz1s.jpg)

A Discord Bot made for self hosting and customization.

## Functions
* **Simple functions/commands**
  * If the user inputs a command, the bot sends a fixed response
    * !ping => Pong!
    * !fun => https://www.youtube.com/watch?v=aZFwMwQdvQM
    * !about => @ADarkHero ist schon 1 nicer dude.
  * Server admins are able to add simple commands via Discord (!addcommand)
    * !addcommand commandNameInOneWord What should the command display?
* **Pen&Paper / RPG / DSA**
  * Flip a coin (!cf)
  * Roll a single dice (!d20, !d6, !d12 etc.)
  * Roll multiple dice (!2d20, !4d6, !2d12 etc.)
  * Roll multiple dice and manipulate them (!2d20+8, !4d8-5, !2d12\*2 etc.)
  * Roll a dice for a specific talent/spell in "Das Schwarze Auge / The Black Eye" [currently only in Germany] (!dsa Kochen, !dsa Sinnenschärfe etc.)
    * There is also the possibility to list all dsa talents.
  * Calculate a dsa crit, that multiplies the number by 1,5 (!crit 20, !crit 15, !crit 6 etc.)
  * Random loot (!randomloot) with different rarities (!lootrarity)
* **League of Legends**
  * Show meta builds for league champions (!lol Ashe, !lol Lux, !lol Gnar etc.)
* **Youtube**
  * Display a Youtube video, based on user inputs (!yt All Star, !yt asdf etc.)
* **Wikipedia**
  * Searches Wikipedia (!wiki League of Legends, !wiki Penis etc.)
* **GitHub**
  * Display the current changelog, based on the most recent GitHub release
* **Reactions**
  * Reacting to specific keywords in messages
    * 69 => 😏
    * rip => ☠
    * uwu => 🐙
  * Reacting to other reactions, for example it also reacts with a ❤, when another person reacts with a ❤
* **Other**
  * Do simple math with two numbers (!2+4, !3\*8, !4/2 etc.)
  
## Uses the following C# packages
* Discord.Net
* HtmlAgilityPack
* Newtonsoft.Json

## Quick setup guide
* Download the most recent [Release](https://github.com/ADarkHero/ADarkHero-Discord-Bot/releases)
* Register at the [Discord Developer Portal](https://discordapp.com/developers)
* Create a new application
* Go to your applications settings, click on "Bot" and generate a new token
* Create a token.txt file, that contains your generated token, in the applications base directory (the same directory, as the exe file)
* Click on oauth in the Discord Developer Portal, look for the menu point "Scopes" and click on "Bot"
* Set bot permissions (Send Messages, Read Messages, Add Reactions)
* Open the generated oauth link and add the bot to your server (you'll need to be a server admin)
* Start the adhdb.exe - It will run completely in the background. You'll only see it in the task manager.
