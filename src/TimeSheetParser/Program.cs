using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TimeSheetParser
{
    internal static class Program
    {
        private static readonly Regex TimeLineRegex = new Regex("([0-9]{4,})\\s+(.*)");
        private static readonly Regex TicketRegex = new Regex("([A-Z][A-Z0-9]+-[1-9][0-9]*)\\s+(.*)");
        
        internal static void Main()
        {
            // using var file = File.OpenRead(@"C:\Users\Matthew\Dropbox\TripStack\Timesheets\timesheet-mar.txt");
            // using var textReader = new StreamReader(file);
            // Go(textReader, Console.Out);

            Go(Console.In, Console.Out);
        }

        private static void Go(TextReader stdin, TextWriter stdout)
        {
            var timeLineItems = new List<TimeLineItem>();
            
            string line;

            while ((line = stdin.ReadLine()) != null)
            {
                var timeLineMatch = TimeLineRegex.Match(line);

                if (timeLineMatch.Success)
                {
                    var timeOfDay = TimeSpan.ParseExact(timeLineMatch.Groups[1].Value, "hhmm", null);
                    var summary = timeLineMatch.Groups[2].Value;
                    var item = CreateTimeLineItem(timeOfDay, summary);
                    timeLineItems.Add(item);
                }
                else
                {
                    WriteSummary(stdout, timeLineItems);
                    
                    stdout.WriteLine(line);
                }
            }
            
            WriteSummary(stdout, timeLineItems);
        }

        private static void WriteSummary(TextWriter stdout, IReadOnlyList<TimeLineItem> timeLineItems)
        {
            var items = GetCanonicalWorkItems(timeLineItems);

            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    stdout.WriteLine($"{item.TimeSpent.TotalHours:F2} {item.Description}");
                }

                var total = items.Select(x => x.TimeSpent).Aggregate(TimeSpan.Zero, (x, y) => x + y);

                stdout.WriteLine($"Total Time: {total.Hours}h {total.Minutes}m");
            }
        }

        private static IReadOnlyCollection<CanonicalWorkItem> GetCanonicalWorkItems(IReadOnlyList<TimeLineItem> timeLineItems)
        {
            var workItems = new Dictionary<string, CanonicalWorkItem>(StringComparer.OrdinalIgnoreCase);
            
            for (var i = 0; i < timeLineItems.Count - 1; i++)
            {
                var currentItem = timeLineItems[i];
                var nextItem = timeLineItems[i + 1];

                var timeSpent = nextItem.TimeOfDay - currentItem.TimeOfDay;
                
                if (timeSpent < TimeSpan.Zero)
                {
                    timeSpent += TimeSpan.FromDays(1);
                }

                if (workItems.TryGetValue(currentItem.Key, out var workItem))
                {
                    workItem.TimeSpent += timeSpent;
                }
                else
                {
                    workItem = new CanonicalWorkItem
                    {
                        TimeSpent = timeSpent,
                        Description = currentItem.Description
                    };

                    workItems[currentItem.Key] = workItem;
                }
            }

            return workItems.Values;
        }

        private static TimeLineItem CreateTimeLineItem(TimeSpan timeOfDay, string summary)
        {
            var ticketMatch = TicketRegex.Match(summary);

            string key, description;

            if (ticketMatch.Success)
            {
                key = ticketMatch.Groups[1].Value;
                description = summary;
            }
            else
            {
                key = summary;
                description = summary;
            }

            return new TimeLineItem(key, description, timeOfDay);
        }

        private sealed class TimeLineItem
        {
            public TimeLineItem(string key, string description, in TimeSpan timeOfDay)
            {
                Key = key;
                Description = description;
                TimeOfDay = timeOfDay;
            }

            public string Key { get; }
            public string Description { get; }
            public TimeSpan TimeOfDay { get; }
        }

        private sealed class CanonicalWorkItem
        {
            public TimeSpan TimeSpent { get; set; }
            public string Description { get; set; }
        }
    }
}