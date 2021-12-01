using System;
using System.IO;
using System.Reflection;

namespace AdventOfCode
{
    public static class Play
    {
        const string inputsFolder = "input";

        public static int GetDayOfCode(string[] args)
        {
            var bp = BypassCurrentDay(args);
            if (bp > 0) return bp;

            var now = DateTime.Now;
            if (now.Hour < 7)
                return now.Day - 1;
            else
                return now.Day;
        }

        public static IDay GetDayToPlay(int dayNumber)
        {
            string objectToInstantiate = $"AdventOfCode.Day{dayNumber}, AdventOfCode";
            var objectType = Type.GetType(objectToInstantiate);
            try
            {
                return Activator.CreateInstance(objectType) as IDay;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetInputText(int dayNumber)
        {
            try
            {
                var root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var fullFileName =  Path.Combine(root, inputsFolder, $"{dayNumber}.txt");
                return File.ReadAllText(fullFileName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static int BypassCurrentDay(string[] args)
        {
            return (args.Length > 0 && Int32.TryParse(args[0], out int currentDay)) ? currentDay : 0;
        }
    }
}