using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace time_to_waste_on_movies
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (Directory.Exists(args[0]))
                {
                    var files = GetFiles(args[0]);
                    var timeSpan = new TimeSpan();
                    foreach (var file in files)
                    {
                        timeSpan.Add(GetVideoDuration(file));
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
                var t = (ulong)prop.ValueAsObject;
                return TimeSpan.FromTicks((long)t);
            }
        }
    }
}
