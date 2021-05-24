/*
BanAll - Bans all players actively playing on the server.
@author Panda
*/
using System;

namespace MCGalaxy 
{
	public class CmdBanall : Command
	{
		public override string name { get { return "Banall"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }
		public override void Use(Player p, string message)
		{
			Player[] players = PlayerInfo.Online.Items;
            foreach (Player pl in players)
			{
				 
            	Command.Find("tempban").Use(null, pl.name + " 24h Banned by " + p.name + " using /BanAll!");
				Command.Find("kick").Use(null, pl.name + " ALL HAVE BEEN BANNED!");
            }
			Chat.MessageGlobal(p.color + p.name + Server.DefaultColor + " ISSUED /BANALL!");
		}

		public override void Help(Player p)
		{
			p.Message("/Banall - Ban everyone... Literally.");
		}
	}
}
