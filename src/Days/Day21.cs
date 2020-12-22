using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day21 : IDay
    {
        List<(HashSet<string>, HashSet<string>)> Foods;
        Dictionary<string, List<int>> AllergensInFoods;

        public void PlayPartOne(string text)
        {
            int answer = 0;
            Foods = text.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                                .Select(Ingredients).ToList();

            var anyAllergenicIngredients = new HashSet<string>();

            AllergensInFoods = new Dictionary<string, List<int>>();
            for (int i = 0; i < Foods.Count; i++)
            {
                var food = Foods[i];
                foreach (var allergen in food.Item2)
                {
                    var foodsWithAllergen = AllergensInFoods.GetValueOrDefault(allergen, new List<int>());
                    foodsWithAllergen.Add(i);
                    AllergensInFoods[allergen] = foodsWithAllergen;
                }
            }

            for (var i = 0; i < Foods.Count; i++)
            {
                (var ingredients, var allergens) = Foods[i];

                foreach (var ingredient in ingredients)
                {
                    var couldContainAlrgs = false;
                    foreach (var allergen in allergens)
                    {
                        if (IngredientExistsInAllRecipiesOfTheMentionedAllergen(ingredient, allergen, i))
                        {
                            couldContainAlrgs = true;
                            break;
                        }
                    }
                    if (couldContainAlrgs)
                        anyAllergenicIngredients.Add(ingredient);
                }
            }

            var nonAllergenicIngredients = new HashSet<string>();
            foreach ((HashSet<string> ingred, HashSet<string> allerg) in Foods)
            {
                foreach (var i in ingred)
                {
                    if (!anyAllergenicIngredients.Contains(i))
                        nonAllergenicIngredients.Add(i);
                }
            }

            foreach (var item in nonAllergenicIngredients)
            {
                answer += Regex.Matches(text, item).Count();
            }
            Console.WriteLine($"Day21:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            string answer = "0";
            Foods = text.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                                .Select(Ingredients).ToList();

            AllergensInFoods = new Dictionary<string, List<int>>();
            for (int i = 0; i < Foods.Count; i++)
            {
                var food = Foods[i];
                foreach (var allergen in food.Item2)
                {
                    var foodsWithAllergen = AllergensInFoods.GetValueOrDefault(allergen, new List<int>());
                    foodsWithAllergen.Add(i);
                    AllergensInFoods[allergen] = foodsWithAllergen;
                }
            }

            var dangerousIngredientList = new Dictionary<string, HashSet<string>>();
            var possibleAllergenicIngedients = new HashSet<string>();
            for (var i = 0; i < Foods.Count; i++)
            {
                (var ingredients, var allergens) = Foods[i];

                foreach (var ingredient in ingredients)
                {
                    var couldContainAlrgs = false;
                    foreach (var allergen in allergens)
                    {
                        if (IngredientExistsInAllRecipiesOfTheMentionedAllergen(ingredient, allergen, i))
                        {
                            couldContainAlrgs = true;
                            var set = dangerousIngredientList.GetValueOrDefault(allergen, new HashSet<string>());
                            set.Add(ingredient);
                            dangerousIngredientList[allergen] = set;
                        }
                    }
                    if (couldContainAlrgs)
                    {
                        possibleAllergenicIngedients.Add(ingredient);
                    }
                }
            }

            var nonAllergenicIngredients = new HashSet<string>();
            foreach ((HashSet<string> ingred, HashSet<string> allerg) in Foods)
            {
                foreach (var i in ingred)
                {
                    if (!possibleAllergenicIngedients.Contains(i))
                        nonAllergenicIngredients.Add(i);
                }
            }

            var yetAnotherList = new Dictionary<string, string>();
            while (dangerousIngredientList.Count > 0)
            {
                foreach (var item in dangerousIngredientList)
                {
                    item.Value.ExceptWith(yetAnotherList.Values);
                    if (item.Value.Count == 1)
                    {
                        yetAnotherList.Add(item.Key, item.Value.First());
                        dangerousIngredientList.Remove(item.Key);
                    }
                }
            }

            answer = String.Join(",", yetAnotherList.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));
            Console.WriteLine($"Day21:PartTwo: {answer}");
        }

        (HashSet<string>, HashSet<string>) Ingredients(string food)
        {
            var parts = food.Split(" (contains ");
            var ingredients = parts[0].Split(' ').ToHashSet();
            var allergens = parts[1].TrimEnd(')').Split(", ").ToHashSet();
            return (ingredients, allergens);
        }

        bool IngredientExistsInAllRecipiesOfTheMentionedAllergen(string ingredient, string allergen, int foodNum)
        {
            var foodList  = AllergensInFoods[allergen];
            foreach (var item in foodList)
            {
                (HashSet<string> ingredients, HashSet<string> allergens) = Foods[item];
                if (!ingredients.Contains(ingredient))
                    return false;
            }

            return true;
        }
    }
}
