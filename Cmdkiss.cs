using System;
using System.IO;

namespace MCDzienny
{
    public class CmdKiss : Command
    {
        public override string name { get { return "kiss"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            if (p.muted)
            {
                Player.SendMessage(p, "%cError: %fYou may not use this as you are %bmuted%f.");
                return;
            }
            if (message == "") { Help(p); return; }
            string two = message.Split(' ')[0];
            Player who = Player.Find(two);

            if (who == null)
            {
                Player.SendMessage(p, "Player is not online.");
                return;
            }
            Random rnd = new Random();
               int msg = rnd.Next(1, 5);
               switch (msg)
               {
                   case 1:
                       Player.GlobalMessage(p.color + p.PublicName + "%e kissed " + who.color + who.PublicName);
                       break;
                   case 2:
                       Player.GlobalMessage(p.color + p.PublicName + "%e kissed " + who.color + who.PublicName + "%e softly on the lips %c<3");
                       break;
                   case 3:
                       Player.GlobalMessage(p.color + p.PublicName + "%e tried to kiss " + who.color + who.PublicName + "%e and poked their eye with his/her nose.");
                       break;
                   case 4:
                       Player.GlobalMessage(p.color + p.PublicName + "%e gave " + who.color + who.PublicName + " %ea kiss on the cheek.");
                       break;
               }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/kiss (player) - Kiss (player).");
        }
    }
}
