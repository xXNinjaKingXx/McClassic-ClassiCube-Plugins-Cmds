using System;
using System.IO;
using System.Collections.Generic;

namespace MCDzienny
{
    public class CmdReport : Command
    {
        // Where the report file is found.
        public static string filePath = "extra/reports.txt";

        // The rank you are allowing to view/delete reports.
        // Players cannot report this rank and higher.
        private static LevelPermission staffPermission = LevelPermission.Operator;

        public override string name { get { return "report"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }

        public override void Init()
        {
            if (!File.Exists(filePath))
            {
                File.CreateText(filePath);
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/report [player] [reason] - report a player for staff to view.");

            if (p.group.Permission >= staffPermission)
            {
                Player.SendMessage(p, "/report [list] - get the list of unresolved reports.");
                Player.SendMessage(p, "/report [view] [#] - get a detailed view of a specified report.");
                Player.SendMessage(p, "/report [clear] [#] - clear the specified report from the list.");
            }
        }

        public override void Use(Player p, string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                Help(p);
                return;
            }
            if (message.ToLower() == "list" && p.group.Permission >= staffPermission)
            {
                int counter = 0;
                foreach (string line in File.ReadAllLines(filePath))
                {
                    counter++;
                    Player.SendMessage(p, "[" + counter + "] " + Group.findPlayerGroup(line.Split(':')[0]).color + line.Split(':')[0] + Server.DefaultColor + " reported " + Group.findPlayerGroup(line.Split(':')[1]).color + line.Split(':')[1]);
                }
                if (counter == 0)
                {
                    Player.SendMessage(p, "No reports found.");
                }
                return;
            }
            if (message.IndexOf(' ') == -1)
            {
                Help(p);
                return;
            }
            switch (message.Split(' ')[0].ToLower())
            {
                case "view":
                    int reportNum;
                    int counter = 0;
                    bool foundAny = false;

                    if (!int.TryParse(message.Split(' ')[1], out reportNum))
                    {
                        Player.SendMessage(p, "Invalid report number.");
                        return;
                    }
                    foreach (string line in File.ReadAllLines(filePath))
                    {
                        counter++;
                        if (counter == reportNum)
                        {
                            foundAny = true;
                            string[] s = line.Split(':');
                            Player.SendMessage(p, "Report number - " + reportNum);
                            Player.SendMessage(p, Group.findPlayerGroup(s[0]).color + s[0] + Server.DefaultColor + " reported " + Group.findPlayerGroup(s[1]).color + s[1] + Server.DefaultColor + " on the " + s[2] + "/" + s[3] + "/" + s[4] + " at " + s[5] + ":" + s[6]);
                            Player.SendMessage(p, "Reason: " + c.lime + s[7]);
                        }
                    }
                    if (!foundAny)
                    {
                        Player.SendMessage(p, "No report found at this specified number.");
                    }
                    break;
                case "clear":
                    int reportNumber;
                    if (!int.TryParse(message.Split(' ')[1], out reportNumber))
                    {
                        Player.SendMessage(p, "Invalid report number.");
                        return;
                    }
                    Report.RemoveReport(reportNumber);
                    Player.SendMessage(p, "Report resolved and removed.");
                    break;
                default:
                    Player who = Player.Find(message.Split(' ')[0].ToLower());
                    if (ComparePlayer(who, p))
                    {
                        if (who == p)
                        {
                            Player.SendMessage(p, "Very funny.");
                            return;
                        }
                        if (!Report.AlreadyReported(p, who))
                        {
                            Player.GlobalMessageOps(p.color + p.PublicName + c.red + " has reported " + who.color + who.PublicName + c.red + ". Use /report to resolve the issue.");
                            Player.GlobalMessageOps("Reason: " + c.red + message.Substring(message.IndexOf(' ') + 1));
                            Report.AddReport(p, who, message.Substring(message.IndexOf(' ') + 1), DateTime.Now);
                        }
                        else
                        {
                            Player.SendMessage(p, "You have already reported this player previously.");
                            Player.SendMessage(p, "You must wait for staff to resolve this report.");
                        }
                    }
                    break;
            }
        }
        private bool ComparePlayer(Player who, Player p)
        {
            if (who == null || who.hidden)
            {
                Player.SendMessage(p, "Player could not be found.");
                return false;
            }
            if (who.group.Permission >= staffPermission)
            {
                Player.SendMessage(p, "You cannot report staff.");
                return false;
            }
            return true;
        }
    }

    public class Report
    {
        public static void AddReport(Player reporter, Player reported, string reason, DateTime time)
        {
            File.AppendAllText(CmdReport.filePath, reporter.PublicName + ":" + reported.PublicName + ":" + time.Day + ":" + time.Month + ":" + time.Year + ":" + time.Hour + ":" + time.Minute + ":" + reason + Environment.NewLine);
        }

        public static void RemoveReport(int reportNum)
        {
            int counter = 0;
            List<string> toPaste = new List<string>();

            foreach (string line in File.ReadAllLines(CmdReport.filePath))
            {
                counter++;
                if (counter != reportNum)
                {
                    toPaste.Add(line);
                }
            }
            File.WriteAllLines(CmdReport.filePath, toPaste.ToArray());
        }

        public static bool AlreadyReported(Player reporter, Player reported)
        {
            foreach (string line in File.ReadAllLines(CmdReport.filePath))
            {
                if (line.Split(':')[0].ToLower() == reporter.PublicName.ToLower() && line.Split(':')[1].ToLower() == reported.PublicName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
