using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day18 : IDay
    {
        public void PlayPartOne(string text)
        {
            long answer = 0;
            string[] expressions = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var exprs = expressions.Select(ParseExpression);
            var rests = exprs.Select(Evaluate);
            answer = rests.Sum();

            Console.WriteLine($"Day18:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            long answer = 0;
            string[] expressions = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var exprs = expressions.Select(ParseExpression);
            var rests = exprs.Select(EvaluateAdvanced);
            answer = rests.Sum();

            Console.WriteLine($"Day18:PartTwo: {answer}");
        }

        long Evaluate(List<Component> expression)
        {
            long result = 0;
            int startIndex = 0;
            while (expression.Count > 0)
            {
                // Print(expression);
                var operatorIndex = expression.FindIndex(startIndex, c => c.type == ComponentType.MathOperation);
                if (operatorIndex == -1)
                {
                    result = expression.Find(c => c.type == ComponentType.Operand).value.Value;
                    break;
                }
                if (expression[operatorIndex - 1].type == ComponentType.CloseParenthesis && expression[operatorIndex - 2].type == ComponentType.Operand)
                {
                    expression.RemoveAt(operatorIndex - 1);
                    expression.RemoveAt(operatorIndex - 3);
                    startIndex = 0;
                    continue;
                }
                if (expression[operatorIndex - 1].type == ComponentType.Operand && expression[operatorIndex + 1].type == ComponentType.Operand)
                {
                    var operation = expression[operatorIndex].operation;
                    long tempResult = operation(expression[operatorIndex - 1].value.Value, expression[operatorIndex + 1].value.Value);
                    expression.RemoveRange(operatorIndex - 1, 3);
                    expression.Insert(operatorIndex - 1, new Component { type = ComponentType.Operand, value = tempResult });
                    startIndex = 0;
                    continue;
                }
                if (expression[operatorIndex + 1].type == ComponentType.OpenParenthesis && expression[operatorIndex + 3].type == ComponentType.CloseParenthesis)
                {
                    expression.RemoveAt(operatorIndex + 3);
                    expression.RemoveAt(operatorIndex + 1);
                    startIndex = 0;
                    continue;
                }
                if (expression[operatorIndex - 1].type == ComponentType.Operand && expression[operatorIndex + 1].type == ComponentType.OpenParenthesis)
                {
                    startIndex = operatorIndex + 1;
                    continue;
                }
            }
            return result;
        }

        long EvaluateAdvanced(List<Component> expression)
        {
            long result = 0;
            int startIndex = 0;
            while (expression.Count > 1)
            {
                // Print(expression);
                var openParenthesisIinex = expression.FindIndex(startIndex, c => c.type == ComponentType.OpenParenthesis);
                if (openParenthesisIinex == -1)
                {
                    result = EvaluateSimple(expression);
                    break;
                }
                startIndex = expression.FindIndex(openParenthesisIinex+1, c => c.type == ComponentType.OpenParenthesis || c.type == ComponentType.CloseParenthesis);
                if (expression[startIndex].type == ComponentType.OpenParenthesis)
                {
                    continue;
                }
                if (expression[startIndex].type == ComponentType.CloseParenthesis)
                {
                    long res = EvaluateSimple(expression.GetRange(openParenthesisIinex +1, startIndex - openParenthesisIinex -1));
                    expression.RemoveRange(openParenthesisIinex, startIndex - openParenthesisIinex + 1);
                    expression.Insert(openParenthesisIinex, new Component { type = ComponentType.Operand, value = res });
                    startIndex = 0;
                }
            }
            return result;
        }

        long EvaluateSimple(List<Component> expression)
        {
            int startIndex = 0;
            string checking = "+";
            while (expression.Count > 2)
            {
                // Print(expression);
                var operatorIndex = expression.FindIndex(startIndex, c => c.ToString() == checking);
                if (operatorIndex == -1)
                {
                    checking = "*";
                    continue;
                }
                var operation = expression[operatorIndex].operation;
                long tempResult = operation(expression[operatorIndex - 1].value.Value, expression[operatorIndex + 1].value.Value);
                expression.RemoveRange(operatorIndex - 1, 3);
                expression.Insert(operatorIndex - 1, new Component { type = ComponentType.Operand, value = tempResult });
            }
            return expression.First().value.Value;
        }

        List<Component> ParseExpression(string expression)
        {
            expression = expression.Replace(" ", "");

            var list = new List<Component>();
            foreach (char item in expression)
            {
                var component = item switch
                {
                    '(' => new Component { type = ComponentType.OpenParenthesis },
                    ')' => new Component { type = ComponentType.CloseParenthesis },
                    '+' => new Component { type = ComponentType.MathOperation, operation = add },
                    '*' => new Component { type = ComponentType.MathOperation, operation = multi },
                    _ => new Component() { type = ComponentType.Operand, value = Int64.Parse(item.ToString()) },
                };
                list.Add(component);
            }

            return list;
        }

        public delegate long Operation(long operandA, long operandB);

        Operation add = delegate (long a, long b) { return a + b; };
        Operation multi = delegate (long a, long b) { return a * b; };

        enum ComponentType
        {
            MathOperation,
            Operand,
            OpenParenthesis,
            CloseParenthesis
        }

        class Component
        {
            public ComponentType type;
            public long? value;
            public Operation operation;

            public override string ToString()
            {
                return type switch {
                    ComponentType.OpenParenthesis => "(",
                    ComponentType.CloseParenthesis => ")",
                    ComponentType.Operand => value.Value.ToString(),
                    ComponentType.MathOperation => operation(0,1) == 1 ? "+" : "*",
                    _ => "",
                };
            }
        }

        static void Print(List<Component> list)
        {
            Console.WriteLine(String.Join(" ", list.Select(e => e.ToString())));
        }
    }
}
