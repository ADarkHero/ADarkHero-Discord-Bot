using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adhdb.bot.DSAObj
{
	class DSACharacterTrope
	{
		public string Description { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }

		public DSACharacterTrope()
		{

		}
		public DSACharacterTrope(String description, String name, String link)
		{
			Description = description;
			Name = name;
			Link = link;
		}
	}
}
