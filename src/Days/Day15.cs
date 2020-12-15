using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day15 : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer = 0;
            var startingNumbers = text.Split(",").Select(Int32.Parse).ToList();

            var game = new Game(startingNumbers, verbose: true);
            answer = game.Until(2020).Play();

            Console.WriteLine($"Day15:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;
            var startingNumbers = text.Split(",").Select(Int32.Parse).ToList();

            var game = new Game(startingNumbers);
            answer = game.Until(30000000).Play();

            Console.WriteLine($"Day15:PartTwo: {answer}");
        }

        class Game
        {
            private bool _verbose;
            private List<int> _startingNumbers;
            private Dictionary<int, int> _spokenNumbers;
            private int _lastTurn;

            public Game(bool verbose = false)
            {
                _spokenNumbers = new Dictionary<int, int>();
                _verbose = verbose;
            }

            public Game(List<int> startingNumbers, bool verbose = false)
            {
                _verbose = verbose;
                _startingNumbers = startingNumbers;
                _spokenNumbers = new Dictionary<int, int>();
            }

            public Game Until(int turns)
            {
                _lastTurn = turns;
                return this;
            }

            public int Play()
            {
                for (int i = 0; i < _startingNumbers.Count - 1; i++)
                    AddNumberToSpoken(_startingNumbers[i], i + 1);

                int currentNumber = _startingNumbers.Last(); // just to return in case _lastTurn is not set
                int previousNumber = _startingNumbers.Last();
                for (int turn = _startingNumbers.Count + 1; turn <= _lastTurn; turn++)
                {
                    var lastOccurence = GetPreviousOccurence(previousNumber);
                    currentNumber = (lastOccurence == 0) ? 0 : turn - 1 - lastOccurence;
                    AddNumberToSpoken(previousNumber, turn - 1);
                    previousNumber = currentNumber;
                }
                if (_verbose) Tell(currentNumber, _lastTurn);

                return currentNumber;
            }

            void AddNumberToSpoken(int number, int turn)
            {
                if (_spokenNumbers.ContainsKey(number))
                {
                    _spokenNumbers[number] = turn;
                }
                else
                {
                    _spokenNumbers.Add(number, turn);
                }
                if (_verbose) Tell(number, turn);
            }

            int GetPreviousOccurence(int number)
            {
                return _spokenNumbers.GetValueOrDefault(number);
            }

            public void Tell(int number, int turn)
            {
                Console.WriteLine($"Turn {turn,4}: {number,4}.");
            }
        }
    }
}
