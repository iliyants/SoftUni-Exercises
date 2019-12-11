using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TicketTrouble
{
    class Program
    {
        static void Main(string[] args)
        {
            string location = Console.ReadLine();
            string input = Console.ReadLine();

            var squareBrackets = new Stack<int>();
            var curlBrackets = new Stack<int>();
            var ticketValidation = new List<string>();
            int start = 0;
            string subString = string.Empty;
            string wholeTicketString = string.Empty;


            var allTickets = new Dictionary<string, List<string>>();
            {
                allTickets[location] = new List<string>();
            }

            for (int i = 0; i < input.Length; i++)
            {

                if (input[i] == '[')
                {
                    if (squareBrackets.Count != 1)
                    {
                        squareBrackets.Push(i);
                    }
                    else
                    {
                        ticketValidation.Clear();
                        squareBrackets.Clear();
                        curlBrackets.Clear();
                        i--;
                    }
                }
                else if (input[i] == '{')
                {
                    if (curlBrackets.Count != 1)
                    {
                        curlBrackets.Push(i);
                    }
                    else
                    {
                        ticketValidation.Clear();
                        squareBrackets.Clear();
                        curlBrackets.Clear();
                        i--;
                    }
                }
                else if (input[i] == ']')
                {
                    if (squareBrackets.Any())
                    {
                        start = squareBrackets.Pop();
                        subString = input.Substring(start, i - start + 1);
                        ticketValidation.Add(subString);
                    }
                    else
                    {
                        ticketValidation.Clear();
                        squareBrackets.Clear();
                        curlBrackets.Clear();
                    }
                }
                else if (input[i] == '}')
                {
                    if (curlBrackets.Any())
                    {
                        start = curlBrackets.Pop();
                        subString = input.Substring(start, i - start + 1);
                        ticketValidation.Add(subString);
                    }
                    else
                    {
                        ticketValidation.Clear();
                        squareBrackets.Clear();
                        curlBrackets.Clear();
                    }

                }


                if (ticketValidation.Count == 3)
                {
                    bool bracketCheck = BracketCheck(ticketValidation);
                    if (bracketCheck)
                    {
                        string wholeticketLocation = ticketValidation[0];
                        string ticketLocation = wholeticketLocation.Substring(1, wholeticketLocation.Length - 2);
                        if (ticketLocation == location)
                        {
                            string wholeSeat = ticketValidation[1];
                            string seat = wholeSeat.Substring(1, wholeSeat.Length - 2);
                            allTickets[location].Add(seat);
                        }
                    }
                    ticketValidation.Clear();
                    squareBrackets.Clear();
                    curlBrackets.Clear();
                }

            }

            var allSeats = allTickets[location].ToList();
            if (allTickets[location].Count > 2)
            {
                for (int i = 0; i < allSeats.Count; i++)
                {
                    string firstSeat = allSeats[0];
                    string secondSeat = string.Empty;
                    string number = firstSeat.Substring(1, firstSeat.Length - 1);
                    if (allSeats.Any(x => x.EndsWith(number)))
                    {
                        allSeats.Remove(firstSeat);
                        foreach (var item in allSeats.Where(x => x.EndsWith(number)))
                        {
                            secondSeat = item;
                            Console.WriteLine($"You are traveling to {location} on seats {firstSeat} and {secondSeat}.");
                            return;
                        }
                    }
                }
            }
            else
            {
                if (allSeats.Count == 2)
                {
                    Console.WriteLine($"You are traveling to {location} on seats {allSeats[0]} and {allSeats[1]}.");
                }

            }



        }

        public static bool BracketCheck(List<string> ticketValidation)
        {
            string printableRegex = @"[ -~]*";
            string firstBracketPattern = @"^[A-Z]{3}\s{1,}([A-Z]{2})*$";
            Regex firstBracketRegex = new Regex(firstBracketPattern);
            Regex regex = new Regex(printableRegex);
            string wholeTicket = ticketValidation[2];
            string firstBracket = ticketValidation[0];
            string secondBracket = ticketValidation[1];

            if (wholeTicket[0] != firstBracket[0] && wholeTicket[0] != secondBracket[0] &&
                wholeTicket[wholeTicket.Length - 1] != firstBracket[firstBracket.Length - 1] &&
                wholeTicket[wholeTicket.Length - 1] != secondBracket[secondBracket.Length - 1])
            {
                if (regex.IsMatch(wholeTicket))
                {

                    if (firstBracket.Length >= 7)
                    {
                        string trimWholeTicket = firstBracket.Substring(1, firstBracket.Length - 2);
                        if (regex.IsMatch(trimWholeTicket))
                        {
                            if (secondBracket.Length == 4 || secondBracket.Length == 5)
                            {
                                for (int i = 1; i < secondBracket.Length - 2; i++)
                                {
                                    if (char.IsUpper(secondBracket[i]))
                                    {
                                        continue;
                                    }
                                    else if (char.IsNumber(secondBracket[i]))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                                return true;
                            }
                        }

                    }
                }
            }

            return false;

        }

    }
}