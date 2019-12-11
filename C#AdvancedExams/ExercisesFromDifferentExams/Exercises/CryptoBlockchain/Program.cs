using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MovieTime
{

    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            var text = string.Empty;
            var validBlocks = new Queue<string>();
            var allNumbers = new List<string>();

            for (int i = 0; i < n; i++)
            {
                var input = Console.ReadLine();
                text += input;
            }

            var stack = new Stack<int>();
            int startIndex = 0;
            string currentBlock = string.Empty;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '[')
                {
                    stack.Push(i);
                }
                else if (text[i] == '{')
                {
                    stack.Push(i);
                }
                if (stack.Any() && text[stack.Peek()] == '[' && text[i] == ']')
                {
                    startIndex = stack.Pop();
                    currentBlock = text.Substring(startIndex, i - startIndex + 1);

                }
                else if (stack.Any() && text[stack.Peek()] == '{' && text[i] == '}')
                {
                    startIndex = stack.Pop();
                    currentBlock = text.Substring(startIndex, i - startIndex + 1);
                }

                Match printable = Regex.Match(currentBlock, @"[ -~]+");
                Match atleast3Digits = Regex.Match(currentBlock, @"([\d]{3,})");
                bool totalDigitsCout = CountsTheDigits(currentBlock);

                if (printable.Success && atleast3Digits.Success && totalDigitsCout)
                {
                    validBlocks.Enqueue(currentBlock);
                    currentBlock = string.Empty;
                }
            }

            foreach (var item in validBlocks)
            {
                Match numbers = Regex.Match(item, @"(?<number>[\d]+)");
                if (numbers.Success)
                {
                    allNumbers.Add(numbers.Groups["number"].Value);
                }
            }
            var result = string.Empty;
            foreach (var item in allNumbers)
            {
                for (int i = 0; i < item.Length; i += 3)
                {
                    int digitNumber = int.Parse(item.Substring(i, 3));
                    char currentChar = (char)(digitNumber - validBlocks.Peek().Length);
                    result += currentChar;
                }
                validBlocks.Dequeue();
            }
            Console.WriteLine(result);


        }

        private static bool CountsTheDigits(string currentBlock)
        {
            int counter = 0;
            for (int i = 0; i < currentBlock.Length; i++)
            {
                if (char.IsNumber(currentBlock[i]))
                {
                    counter++;
                }
            }
            if (counter % 3 != 0)
            {
                return false;
            }
            return true;
        }
    }
}