using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.IO;

namespace time_to_waste_on_movies
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (Directory.Exists(args[0]))
                {
                    var files = GetFiles(args[0]);
                    var timeSpan = new TimeSpan();
                    foreach (var file in files)
                    {
                        if (file.EndsWith(".mp4") || file.EndsWith(".mkv") || file.EndsWith(".webm"))
                        {
                            timeSpan = timeSpan.Add(GetVideoDuration(file));
                        }
                    }

                    Console.WriteLine("Time to watch all: " + timeSpan);
                    Console.ReadKey();
                }
            }
        }

        private static string[] GetFiles(string directory)
        {
            var result = new List<string>();

            var dirs = Directory.GetDirectories(directory);
            foreach (var dir in dirs)
            {
                result.AddRange(GetFiles(dir));
            }

            var files = Directory.GetFiles(directory);
            result.AddRange(files);

            return result.ToArray();
        }

        private static TimeSpan GetVideoDuration(string filePath)
        {
            using (var shell = ShellObject.FromParsingName(filePath))
            {
                IShellProperty prop = shell.Properties.System.Media.Duration;
                try
                {
                    var t = (ulong)prop.ValueAsObject;
                    var span = TimeSpan.FromTicks((long)t);
                    return span;
                }
                catch (NullReferenceException)
                {
                    throw;
                }
            }
        }
    }
}