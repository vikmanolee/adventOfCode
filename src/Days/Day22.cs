using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day22 : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer = 0;
            var decks = text.Split("\n\n", StringSplitOptions.TrimEntries).Select(p => p.Substring(10)).Select(CreateDeck).ToArray();

            int winner = 0;
            while (decks.All(p => p.Count > 0))
            {
                var roundCards = decks.Select(deck => deck.Dequeue()).ToArray();
                winner = (roundCards[0] > roundCards[1]) ? 0 : 1;
                decks[winner].Enqueue(roundCards[winner]);
                decks[winner].Enqueue(roundCards[1 - winner]);
            }

            answer = Score(decks[winner]);
            Console.WriteLine($"Day22:PartOne: {answer}"); // 306
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;
            var decks = text.Split("\n\n", StringSplitOptions.TrimEntries).Select(p => p.Substring(10)).Select(CreateDeck).ToArray();

            int winner = 0;
            var rule = new InfiniteGamePreventionRule();
            while (decks.All(p => p.Count > 0))
            {
                if (rule.Applies(decks))
                {
                    winner = 0;
                    break;
                }
                var roundCards = decks.Select(deck => deck.Dequeue()).ToArray();
                if (decks[0].Count >= roundCards[0] && decks[1].Count >= roundCards[1])
                {
                    var x = decks.Select((deck, i) => new Queue<int>(deck.ToArray().Take(roundCards[i]))).ToArray();
                    winner = SubGame(x);
                }
                else
                    winner = (roundCards[0] > roundCards[1]) ? 0 : 1;

                decks[winner].Enqueue(roundCards[winner]);
                decks[winner].Enqueue(roundCards[1 - winner]);
            }

            answer = Score(decks[winner]);
            Console.WriteLine($"Day22:PartTwo: {answer}"); // 291
        }

        Queue<int> CreateDeck(string playersCards)
        {
            return new Queue<int>(playersCards.Split('\n').Select(Int32.Parse));
        }

        int Score(Queue<int> deck)
        {
            int score = 0;
            int multiplier = deck.Count;
            foreach (var card in deck)
                score += card * multiplier--;
            return score;
        }

        int SubGame(Queue<int>[] decks)
        {
            int winner = 0;
            var rule = new InfiniteGamePreventionRule();
            while (decks.All(p => p.Count > 0))
            {
                if (rule.Applies(decks))
                {
                    winner = 0;
                    break;
                }
                var roundCards = decks.Select(deck => deck.Dequeue()).ToArray();
                if (decks[0].Count >= roundCards[0] && decks[1].Count >= roundCards[1])
                {
                    var x = decks.Select((deck, i) => new Queue<int>(deck.ToArray().Take(roundCards[i]))).ToArray();
                    winner = SubGame(x);
                }
                else
                    winner = (roundCards[0] > roundCards[1]) ? 0 : 1;

                decks[winner].Enqueue(roundCards[winner]);
                decks[winner].Enqueue(roundCards[1 - winner]);
            }
            return winner;
        }

        class InfiniteGamePreventionRule
        {
            private List<string> previousDecks = new List<string>();

            public bool Applies(Queue<int>[] playerDecks)
            {
                var round = RoundToString(playerDecks);
                if (previousDecks.Contains(round))
                {
                    return true;
                }
                else
                {
                    previousDecks.Add(round);
                    return false;
                }
            }

            private string RoundToString(Queue<int>[] playerDecks)
                            => String.Concat(playerDecks[0].ToArray().Select(i => i.ToString())) + "_" + String.Concat(playerDecks[1].ToArray().Select(i => i.ToString()));
        }
    }
}
