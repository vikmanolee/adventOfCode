namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            int dayOfCode = Play.GetDayOfCode(args);
            Validations.ValidateOrExit(dayOfCode);

            IDay day = Play.GetDayToPlay(dayOfCode);
            Validations.ValidateOrExit(day);

            string plainText = Play.GetInputText(dayOfCode);
            Validations.ValidateOrExit(plainText);

            // Run!
            day.PlayPartOne(plainText);
            day.PlayPartTwo(plainText);
        }
    }
}
