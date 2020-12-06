using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day4 : IDay
    {
        public static string[] requiredFields = { "byr:", "iyr:", "eyr:", "hgt:", "hcl:", "ecl:", "pid:" };

        public void PlayPartOne(string text)
        {
            int answer = 0;

            var passports = text.Split("\n\n");

            foreach (var passport in passports)
            {
                if (IsValidPassport(passport)) answer++;
            }

            Console.WriteLine($"Day4:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;

            var passports = text.Split("\n\n");

            foreach (var passport in passports)
            {
                if (IsValidPassport(passport) && HasValidFields(passport))
                    answer++;
            }

            Console.WriteLine($"Day4:PartTwo: {answer}");
        }

        private bool IsValidPassport(string passport)
        {
            return requiredFields.Select(x => Regex.IsMatch(passport, x)).Count(r => r) == requiredFields.Length;
        }

        private bool HasValidFields(string passport)
        {
            var fields = passport.Split(new string[] { "\n", " " }, StringSplitOptions.RemoveEmptyEntries);
            var validFields = 0;
            foreach (var field in fields)
            {
                var parts = field.Split(":");
                switch (parts[0])
                {
                    case "byr":
                        if (ValidateBirthYear(parts[1]))
                            validFields++;
                        continue;
                    case "iyr":
                        if (ValidateIssueYear(parts[1]))
                            validFields++;
                        continue;
                    case "eyr":
                        if (ValidateExpirationYear(parts[1]))
                            validFields++;
                        continue;
                    case "hgt":
                        if (ValidateHeight(parts[1]))
                            validFields++;
                        continue;
                    case "hcl":
                        if (ValidateHairColor(parts[1]))
                            validFields++;
                        continue;
                    case "ecl":
                        if (ValidateEyeColor(parts[1]))
                            validFields++;
                        continue;
                    case "pid":
                        if (ValidatePassportID(parts[1]))
                            validFields++;
                        continue;
                    default:
                        continue;
                }
            }
            return (validFields == requiredFields.Length);
        }

        private bool ValidateBirthYear(string val)
        {
            return Int32.TryParse(val, out int year) && year >= 1920 && year <= 2002;
        }

        private bool ValidateIssueYear(string val)
        {
            return Int32.TryParse(val, out int year) && year >= 2010 && year <= 2020;
        }

        private bool ValidateExpirationYear(string val)
        {
            return Int32.TryParse(val, out int year) && year >= 2020 && year <= 2030;
        }

        private bool ValidateHeight(string val)
        {
            if (val.Length < 3) return false;
            var unit = val.Substring(val.Length - 2);
            switch (unit)
            {
                case "cm":
                    return Int32.TryParse(val.Substring(0, val.Length - 2), out int height1) && height1 >= 150 && height1 <= 193;
                case "in":
                    return Int32.TryParse(val.Substring(0, val.Length - 2), out int height2) && height2 >= 59 && height2 <= 76;
                default:
                    return false;
            }
        }

        private bool ValidateHairColor(string val)
        {
            return Regex.IsMatch(val, "^#([0-9a-f]{6})$");
        }

        private bool ValidateEyeColor(string val)
        {
            return Regex.IsMatch(val, "^(amb|blu|brn|gry|grn|hzl|oth)$");
        }

        private bool ValidatePassportID(string val)
        {
            return Regex.IsMatch(val, "^[0-9]{9}$");
        }
    }
}