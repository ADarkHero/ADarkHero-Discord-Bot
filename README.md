# ADarkHero-Discord-Bot
A Discord Bot made for self hosting and customization.

## Functions
* Simple functions/commands
  * If the user inputs a command, the bot sends a fixed response
    * !ping => Pong!
    * !fun => https://www.youtube.com/watch?v=aZFwMwQdvQM
    * !about => @ADarkHero ist schon 1 nicer dude.
  * Server admins are able to add simple commands via Discord (!addcommand)
    * !addcommand commandNameInOneWord What should the command display?
* Pen&Paper / RPG / DSA
  * Flip a coin (!cf)
  * Roll a single dice (!d20, !d6, !d12 etc.)
  * Roll multiple dice (!2d20, !4d6, !2d12 etc.)
  * Roll multiple dice and manipulate them (!2d20+8, !4d8-5, !2d12\*2 etc.)
  * Roll a dice for a specific talent/spell in "Das Schwarze Auge / The Black Eye" [currently only in Germany] (!dsa Kochen, !dsa Sinnenschärfe etc.)
    * There is also the possibility to list all dsa talents.
  * Calculate a dsa crit, that multiplies the number by 1,5 (!crit 20, !crit 15, !crit 6 etc.)
  * Random loot to give a user random items (!randomloot) with different rarities (!lootrarity)
* League of Legends
  * Show meta builds for league champions (!lol Ashe, !lol Lux, !lol Gnar etc.)
* Youtube
  * Display a Youtube video, based on user inputs (!yt All Star, !yt asdf etc.)
* GitHub
  * Display the current changelog, based on the most recent GitHub release
* Other
  * Do simple math with two numbers (!2+4, !3\*8, !4/2 etc.)
  
## Uses the following C# packages:
* Discord.Net
* HtmlAgilityPack
* Newtonsoft.Json
