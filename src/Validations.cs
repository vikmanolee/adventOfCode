using System;

namespace AdventOfCode
{
    public static class Validations
    {
        public static void ValidateOrExit(int dayNumber)
        {
            if (dayNumber < 1 || dayNumber > 25)
            {
                Console.WriteLine("Cannot play days other than 1-25.");
                Console.WriteLine($"Tried to play Day: {dayNumber}");
                Environment.Exit(1001);
            }
        }

        public static void ValidateOrExit(IDay daytoPlay)
        {
            if (daytoPlay == null)
            {
                Console.WriteLine("The class for the Day you are trying to play does not exist.");
                Environment.Exit(1002);
            }
        }

        public static void ValidateOrExit(string input)
        {
            if (input == null)
            {
                Console.WriteLine("The input file for the Day you are trying to play does not exist.");
                Console.WriteLine("Do you have a .txt file in the `./input` folder with the corresponding number?");
                Environment.Exit(1003);
            }
        }
    }
}