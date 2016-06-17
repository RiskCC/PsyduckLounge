using Discord;
using Discord.Commands;
using Discord.Modules;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using DiscordBot.Modules.Timer;

namespace DiscordBot.Modules.Border
{
    internal class BorderModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        private TimerModule tm = new TimerModule();

        private string ssCsv = "http://deresuteborder.web.fc2.com/csv/event_latest.csv";
        private string sifenCsv = "https://docs.google.com/spreadsheets/d/1a2ihrwVgyZnjy3OjqKYsFyJLxECXBO5WrPkEK1WEivw/export?format=csv&id=1a2ihrwVgyZnjy3OjqKYsFyJLxECXBO5WrPkEK1WEivw&gid=2089803644";
        private string sifjpCsv = "http://llborder.web.fc2.com/summary.csv";
        private string result, csv, current, previous;
        private string[] splitCsv, a, b;
        private int[] d;
        private object[] args;
        private TimeSpan elapsed;

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = _manager.Client;

            manager.CreateCommands("border", group =>
            {
                group.CreateCommand("")
                       .Description("Returns the usage for Border module.")
                       .Do(e =>
                       {
                           return _client.Reply(e,
                               $"Usage: border <game>\n" +
                               "ss - Starlight Stage\n" +
                               "sifen - School Idol Festival English\n" +
                               "sifjp - School Idol Festival Japanese");
                       });
                group.CreateCommand("help")
                       .Description("Returns the usage for Border module.")
                       .Do(e =>
                       {
                           return _client.Reply(e,
                               $"Usage: border <game>\n" +
                               "ss - Starlight Stage\n" +
                               "sifen - School Idol Festival English\n" +
                               "sifjp - School Idol Festival Japanese");
                       });
                group.CreateCommand("ss")
                    .Description("Returns the current Starlight Stage tier borders.")
                    .Do(e =>
                    {
                        return GetBorderSS(e, "");
                    });
                group.CreateCommand("sifen")
                    .Description("Returns the current School Idol Festival EN tier borders.")
                    .Do(e =>
                    {
                        return GetBorderSIFEN(e, "");
                    });
                group.CreateCommand("sifjp")
                    .Description("Returns the current School Idol Festival JP tier borders.")
                    .Do(e =>
                    {
                        return GetBorderSIFJP(e, "");
                    });
            });
            manager.CreateCommands("", group =>
            {
                group.CreateCommand("sifen")
                    .Description("Returns the current School Idol Festival EN tier borders.")
                    .Do(e =>
                    {
                        return GetBorderSIFEN(e, "");
                    });
                group.CreateCommand("ss")
                    .Description("Returns the current Starlight Stage tier borders.")
                    .Do(e =>
                    {
                        return GetBorderSS(e, "");
                    });
                group.CreateCommand("sifjp")
                    .Description("Returns the current School Idol Festival JP tier borders.")
                    .Do(e =>
                    {
                        return GetBorderSIFJP(e, "");
                    });
            });
        }

        public async Task GetBorderSS(CommandEventArgs e, string target)
        {
            await e.Channel.SendIsTyping();
            csv = GetCSV(ssCsv);
            splitCsv = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            current = splitCsv.GetValue(splitCsv.Count() - 2).ToString();
            previous = splitCsv.GetValue(splitCsv.Count() - 3).ToString();
            a = current.Split(',');
            b = previous.Split(',');

            d = new int[] { 0, Int32.Parse(a[1]) - Int32.Parse(b[1]), Int32.Parse(a[2]) - Int32.Parse(b[2]), Int32.Parse(a[3]) - Int32.Parse(b[3]), Int32.Parse(a[4]) - Int32.Parse(b[4]), Int32.Parse(a[5]) - Int32.Parse(b[5]) };
            elapsed = DateTime.Parse(a[0]) - DateTime.Parse(b[0]);

            args = new object[] { a[0], a[1], a[2], a[3], a[4], a[5], d[1], d[2], d[3], d[4], d[5], elapsed.TotalMinutes, tm.GetTimer("ss", "event") };

            result = String.Format("Remaining: {12}\nLast Updated: {0} JST (+{11} min)\nT1: {1} (+{6})\nT2: {2} (+{7})\nT3: {3} (+{8})\nT4: {4} (+{9})\nT5: {5} (+{10})", args);
            await e.Channel.SendMessage($"{result}");
        }

        private async Task GetBorderSIFEN(CommandEventArgs e, string target)
        {
            await e.Channel.SendIsTyping();
            csv = GetCSV(sifenCsv);
            splitCsv = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int i = 0;
            foreach (string entry in splitCsv)
            {
                string[] splitEntry = entry.Split(',');
                if (String.IsNullOrEmpty(splitEntry[3]))
                {
                    break;
                }
                i++;
            }
            b = splitCsv[i - 2].Split(',');
            a = splitCsv[i - 1].Split(',');
            try
            {
                elapsed = DateTime.Parse(a[1]) - DateTime.Parse(b[1]);
                args = new object[] { a[1], a[4], a[5], a[6], a[7], a[9], a[10], a[11], a[12], elapsed.TotalMinutes, tm.GetTimer("sifen", "event") };
            }
            catch (Exception ex)
            {
                args = new object[] { a[1], a[4], a[5], a[6], a[7], a[9], a[10], a[11], a[12], "null", tm.GetTimer("sifen", "event") };
            }

            result = String.Format("Remaining: {10}\nLast Updated: {0} UTC (+{9} min)\nT1: {1} (+{5})\nT2: {2} (+{6})\nT3: {3} (+{7})\nT4: {4} (+{8})", args);
            await e.Channel.SendMessage($"{result}");
        }

        private async Task GetBorderSIFJP(CommandEventArgs e, string target)
        {
            await e.Channel.SendIsTyping();
            csv = GetCSV(sifjpCsv);
            splitCsv = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            current = splitCsv.GetValue(splitCsv.Count() - 2).ToString();
            previous = splitCsv.GetValue(splitCsv.Count() - 3).ToString();
            a = current.Split(',');
            b = previous.Split(',');

            d = new int[] { 0, Int32.Parse(a[1]) - Int32.Parse(b[1]), Int32.Parse(a[2]) - Int32.Parse(b[2]), Int32.Parse(a[3]) - Int32.Parse(b[3]), Int32.Parse(a[4]) - Int32.Parse(b[4]), Int32.Parse(a[5]) - Int32.Parse(b[5]) };
            elapsed = DateTime.Parse(a[0]) - DateTime.Parse(b[0]);

            args = new object[] { a[0], a[1], a[2], a[3], a[4], a[5], d[1], d[2], d[3], d[4], d[5], elapsed.TotalMinutes, tm.GetTimer("sifjp", "event") };

            result = String.Format("Remaining: {12}\nLast Updated: {0} JST (+{11} min)\nT1: {1} (+{6})\nT2: {2} (+{7})\nT3: {3} (+{8})\nT4: {4} (+{9})", args);
            await e.Channel.SendMessage($"{result}");
        }

        private string GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }
    }
}
