using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ArraySlider
{
    class Program
    {
        static void Main(string[] args)
        {

            var input = Console.ReadLine()
                 .Split(new char[] { ' ', '\t' },
                 StringSplitOptions.RemoveEmptyEntries).Select(BigInteger.Parse)
                 .ToArray();

            int elementPossition = 0;

            while (true)
            {
                var command = Console.ReadLine()
                    .Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                if (command[0] == "stop")
                {
                    break;
                }

                var offSet = int.Parse(command[0]) % input.Length;
                string operation = command[1];
                var operand = int.Parse(command[2]);

                if (offSet < 0)
                {
                    offSet += input.Length;
                }

                elementPossition = (elementPossition + offSet) % input.Length;

                switch (operation)
                {
                    case "&":
                        input[elementPossition] &= operand;
                        break;
                    case "|":
                        input[elementPossition] |= operand;
                        break;
                    case "^":
                        input[elementPossition] ^= operand;
                        break;
                    case "+":
                        input[elementPossition] += operand;
                        break;
                    case "-":
                        input[elementPossition] -= operand;
                        break;
                    case "*":
                        input[elementPossition] *= operand;
                        break;
                    case "/":
                        input[elementPossition] /= operand;
                        break;
                    default:
                        break;
                }
                if (input[elementPossition] < 0)
                {
                    input[elementPossition] = 0;
                }

            }

            Console.WriteLine("[" + string.Join(", ", input) + "]");
        }
    }
}