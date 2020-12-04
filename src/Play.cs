using System;
using System.IO;

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
            catch (System.Exception)
            {
                return null;
            }
        }

        public static string GetInputText(int dayNumber)
        {
            var fullFileName =  Path.Combine(Directory.GetCurrentDirectory(), inputsFolder, $"{dayNumber}.txt");
            try
            {
                return System.IO.File.ReadAllText(fullFileName);
            }
            catch (System.IO.FileNotFoundException)
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