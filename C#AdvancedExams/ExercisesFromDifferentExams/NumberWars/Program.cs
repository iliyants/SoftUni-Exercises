using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    class Card
    {
        public Card(int cardNumber, char cardLetter)
        {
            this.CardNumber = cardNumber;
            this.CardLetter = cardLetter;
        }
        public int CardNumber { get; set; }
        public char CardLetter { get; set; }
    }


    static void Main()
    {

        string[] pOne = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string[] pTwo = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var playerOneCards = new Queue<Card>();
        var playerTwoCards = new Queue<Card>();

        foreach (var item in pOne)
        {
            Card card = new Card(int.Parse(item.Substring(0, item.Length - 1)), item[item.Length - 1]);
            playerOneCards.Enqueue(card);
        }
        foreach (var item in pTwo)
        {
            Card card = new Card(int.Parse(item.Substring(0, item.Length - 1)), item[item.Length - 1]);
            playerTwoCards.Enqueue(card);
        }

        var cardsOnTheTable = new List<Card>();

        int turns = 0;
        while (playerOneCards.Any() && playerTwoCards.Any() && turns < 1000000)
        {
            turns++;

            Card firstPlayerCard = playerOneCards.Dequeue();
            Card secondPlayerCard = playerTwoCards.Dequeue();

            if (firstPlayerCard.CardNumber > secondPlayerCard.CardNumber)
            {
                playerOneCards.Enqueue(firstPlayerCard);
                playerOneCards.Enqueue(secondPlayerCard);
            }
            else if (firstPlayerCard.CardNumber < secondPlayerCard.CardNumber)
            {
                playerTwoCards.Enqueue(secondPlayerCard);
                playerTwoCards.Enqueue(firstPlayerCard);
            }
            else
            {
                cardsOnTheTable.Add(firstPlayerCard);
                cardsOnTheTable.Add(secondPlayerCard);

                while (playerOneCards.Count >= 3 && playerTwoCards.Count >= 3)
                {
                    int firstPlayerSum = 0;
                    int secondPlayerSum = 0;
                    for (int j = 1; j <= 3; j++)
                    {
                        Card firstCard = playerOneCards.Dequeue();
                        char firstLetter = firstCard.CardLetter;
                        firstPlayerSum += char.ToUpper(firstLetter) - 64;
                        Card secondCard = playerTwoCards.Dequeue();
                        char seconLetter = secondCard.CardLetter;
                        secondPlayerSum += char.ToUpper(seconLetter) - 64;

                        cardsOnTheTable.Add(firstCard);
                        cardsOnTheTable.Add(secondCard);
                    }
                    if (firstPlayerSum > secondPlayerSum)
                    {
                        foreach (var card in cardsOnTheTable.OrderByDescending(x => x.CardNumber))
                        {
                            playerOneCards.Enqueue(card);
                        }
                        cardsOnTheTable.Clear();
                        break;
                    }
                    else if (firstPlayerSum < secondPlayerSum)
                    {
                        foreach (var card in cardsOnTheTable.OrderByDescending(x => x.CardNumber))
                        {
                            playerTwoCards.Enqueue(card);
                        }
                        cardsOnTheTable.Clear();
                        break;
                    }
                    Check(playerOneCards, playerTwoCards, turns);
                }

            }
        }

        WinnerCheck(playerOneCards, playerTwoCards, turns);

    }

    private static void WinnerCheck(Queue<Card> playerOneCards, Queue<Card> playerTwoCards, int turns)
    {
        if (playerOneCards.Any() && !playerTwoCards.Any())
        {
            Console.WriteLine($"First player wins after {turns} turns");
        }
        else if (!playerOneCards.Any() && playerTwoCards.Any())
        {
            Console.WriteLine($"Second player wins after {turns} turns");
        }
        else if (turns == 1000000)
        {
            if (playerOneCards.Count > playerTwoCards.Count)
            {
                Console.WriteLine($"First player wins after {turns} turns");
            }
            else
            {
                Console.WriteLine($"Second player wins after {turns} turns");
            }
        }
    }

    private static void Check(Queue<Card> playerOneCards, Queue<Card> playerTwoCards, int turns)
    {
        if (playerOneCards.Count < 3 || playerTwoCards.Count < 3)
        {
            Console.WriteLine($"Draw after {turns} turns");
            return;
        }
    }
}