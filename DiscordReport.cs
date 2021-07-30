/*
DiscordReport - Send an embedded report to the MCCH staff on Discord.
@author Panda
*/
using System;
using System.IO;

using MCGalaxy;
using MCGalaxy.Commands;
using MCGalaxy.Modules.Relay.Discord;

namespace MCGalaxy {

    public class DiscordReportPlugin : Plugin {
        public override string creator { get { return "Panda"; } }
        public override string MCGalaxy_Version { get { return "1.9.1.2"; } }
        public override string name { get { return "DiscordReport"; } }

        public override void Load(bool startup) {
            Command.Register(new CmdDiscordReport());
        }

        public override void Unload(bool shutdown) {
            Command.Unregister(Command.Find("DiscordReport"));
        }
    }

    public sealed class CmdDiscordReport : Command {
        public override string name { get { return "DiscordReport"; } }
        public override string type { get { return "moderation"; } }
        public override string shortcut { get { return "dr"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public const string channelID = "YOUR CHANNEL ID HERE"; // ACTION REQUIRED

        public override void Use(Player p, string message) {
            DiscordBot discBot = DiscordPlugin.Bot;
            try
            {
                EmbedReport(discBot, p, channelID, message);
            }
            catch (Exception e)
            {
                p.Message("Failed to submit report. Error message:");
                p.Message(e.StackTrace);
                return;
            }

            p.Message("Submitted Report: %c\"{0}\" %Sto the staff team.", message);
            p.Message("Please be patient. Staff are generally AFK, and will privately contact you ASAP!");
        }

        public void EmbedReport(DiscordBot disc, Player p, string channelID, string message)
        {
            ChannelSendEmbed embed = new ChannelSendEmbed(channelID);
            DiscordConfig config = DiscordPlugin.Config;
            embed.Color = config.EmbedColor;
            embed.Title = string.Format("Player Report by {0} at {1}", p.name, DateTime.UtcNow);
            embed.Fields.Add("Report", message);
            disc.Send(embed);
        }

        public override void Help(Player p)
		{
			p.Message("/DiscordReport (message) - Send a detailed message report to staff on Discord.");
            p.Message("Shortcut: /dr (message).");
		}
    }
}
