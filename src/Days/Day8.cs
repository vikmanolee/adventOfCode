using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day8 : IDay
    {
        List<Instruction> Program;

        public void PlayPartOne(string text)
        {
            int answer = 0;
            string[] instructions = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            Program = instructions.Select(i => new Instruction(i)).ToList();
            var finalState = Run(Program);
            answer = finalState.Accumulator;
            Console.WriteLine($"Day8:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;
            string[] instructions = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            Program = instructions.Select(i => new Instruction(i)).ToList();

            var finalState = Run(Program);
            var callStack = new Stack<int>(finalState.Index.ToArray().Reverse());
            callStack.Pop();

            do
            {
                var fixingInex = callStack.Pop();
                var program = TryFix(Program, fixingInex);
                finalState = Run(program);
            }
            while (finalState.Index.Peek() < Program.Count);

            answer = finalState.Accumulator;
            Console.WriteLine($"Day8:PartTwo: {answer}");
        }

        private List<Instruction> TryFix(List<Instruction> program, int index)
        {
            var newProgram = Program.Select(i => i.Clone()).ToList();
            newProgram[index] = TryFix(newProgram[index]);
            return newProgram;
        }

        private Instruction TryFix(Instruction instruction)
        {
            string newOp = instruction.Operation switch
            {
                "acc" => "acc",
                "jmp" => "nop",
                "nop" => "jmp",
                _ => instruction.Operation
            };

            return new Instruction {
                Argument = instruction.Argument,
                Operation = newOp
            };
        }

        private State Run(List<Instruction> program)
        {
            var state = new State { Accumulator = 0, Index = new Stack<int>(new int[] {0}) };

            while (state.Index.Peek() < program.Count() && program[state.Index.Peek()].IsNotExecuted())
            {
                program[state.Index.Peek()].Executed();
                state = Execute(program[state.Index.Peek()], state);
            }
            return state;
        }

        private State Execute(Instruction instruction, State currentState)
        {
            switch (instruction.Operation)
            {
                case "acc":
                    currentState.Index.Push(currentState.Index.Peek() + 1);
                    return new State
                    {
                        Accumulator = currentState.Accumulator + instruction.Argument,
                        Index = currentState.Index
                    };
                case "jmp":
                    currentState.Index.Push(currentState.Index.Peek() + instruction.Argument);
                    return new State
                    {
                        Accumulator = currentState.Accumulator,
                        Index = currentState.Index
                    };
                case "nop":
                    currentState.Index.Push(currentState.Index.Peek() + 1);
                    return new State {
                        Accumulator = currentState.Accumulator,
                        Index = currentState.Index
                    };
                default:
                    return currentState;
            }
        }

        record State
        {
            public int Accumulator { get; init; }
            public Stack<int> Index { get; init; }
        }

        class Instruction
        {
            private bool _executed;
            public string Operation { get; set; }
            public int Argument { get; set; }
            public void Executed() => _executed = true;
            public bool IsNotExecuted() => !_executed;

            public Instruction(string instruction)
            {
                _executed = false;

                var matches = Regex.Match(instruction, @"^(\w{3})\s([\+|\-]\d+)");

                Operation = matches.Groups[1].Value;
                Argument = Int32.Parse(matches.Groups[2].Value);
            }

            public Instruction() : base() {}

            public Instruction Clone()
            {
                return new Instruction {
                    Operation = this.Operation,
                    Argument = this.Argument
                };
            }
        }
    }
}
