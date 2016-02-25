using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;

namespace DiscordBot.Modules.Border
{
    public class BorderModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        private string ssCsv, sifCsv;
        private SpreadsheetsService myService;

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = _manager.Client;

           // Authenticate();
            manager.CreateCommands("border", group =>
            {
                group.CreateCommand("ss")
                    .Description("Returns the current Starlight Stage tier borders.")
                    .Do(e =>
                    {
                        return GetBorderSS(e, "");
                    });
                group.CreateCommand("sif")
                    .Description("Returns the current School Idol Festival tier borders.")
                    .Do(e =>
                    {
                        return GetBorderSIF(e, "");
                    });
            });
        }

        private async Task GetBorderSS(CommandEventArgs e, string target)
        {
            ssCsv = "http://deresuteborder.web.fc2.com/csv/event_latest.csv";

            string csv, entry, prevEntry, formattedResult;
            string[] splitCsv, a, b;
            int[] delta = { };
            object[] args;
            TimeSpan elapsed;
            csv = GetCSV(ssCsv);
            splitCsv = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            entry = splitCsv.GetValue(splitCsv.Count() - 2).ToString();
            prevEntry = splitCsv.GetValue(splitCsv.Count() - 3).ToString();
            a = entry.Split(',');
            b = prevEntry.Split(',');

            delta = new int[] { 0, Int32.Parse(a[1]) - Int32.Parse(b[1]), Int32.Parse(a[2]) - Int32.Parse(b[2]), Int32.Parse(a[3]) - Int32.Parse(b[3]), Int32.Parse(a[4]) - Int32.Parse(b[4]), Int32.Parse(a[5]) - Int32.Parse(b[5]) };
            elapsed = DateTime.Parse(a[0]) - DateTime.Parse(b[0]);
            args = new object[] { a[0], a[1], a[2], a[3], a[4], a[5], delta[1], delta[2], delta[3], delta[4], delta[5], elapsed.TotalMinutes };

            formattedResult = String.Format("\nLast Updated: {0} JST (+{11} min)\nT1: {1} (+{6})\nT2: {2} (+{7})\nT3: {3} (+{8})\nT4: {4} (+{9})\nT5: {5} (+{10})", args);

            await _client.Reply(e, $"{formattedResult}");

        }
        private async Task GetBorderSIF(CommandEventArgs e, string target)
        {
            sifCsv = "https://docs.google.com/spreadsheets/d/1a2ihrwVgyZnjy3OjqKYsFyJLxECXBO5WrPkEK1WEivw/export?format=csv&id=1a2ihrwVgyZnjy3OjqKYsFyJLxECXBO5WrPkEK1WEivw&gid=2089803644";
            string csv = GetCSV(sifCsv);
            string[] splitCsv = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string[] latest, previous = { };
            int i = 0;
            foreach(string entry in splitCsv)
            {
                string[] splitEntry = entry.Split(',');
                if (String.IsNullOrEmpty(splitEntry[3]))
                {
                    break;
                }
                i++;
            }

            //previous = splitCsv[i - 2].Split(',');
            latest = splitCsv[i - 1].Split(',');

            object[] args = new object[] { latest[1], latest[4], latest[5], latest[6], latest[7], latest[9], latest[10], latest[11], latest[12] };

            string formattedResult = String.Format("\nLast Updated: {0} UTC\nT1: {1} (+{5})\nT2: {2} (+{6})\nT3: {3} (+{7})\nT4: {4} (+{8})", args);
            
            await _client.Reply(e, $"{formattedResult}");

        }
        public string GetCSV(string url)
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
