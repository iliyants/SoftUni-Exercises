using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp87
{

    class Program
    {

        static void Main(string[] args)
        {
            var field = new char[8][];

            FillField(field);


            while (true)
            {
                string command = Console.ReadLine();
                if (command == "END")
                {
                    break;
                }
                var moves = command.ToCharArray();
                char currentFigure = moves[0];

                int currentFigureRow = (int)Char.GetNumericValue(moves[1]);
                int currentFigureCol = (int)Char.GetNumericValue(moves[2]);

                int rowToBe = (int)Char.GetNumericValue(moves[4]);
                int colToBe = (int)Char.GetNumericValue(moves[5]);

                if (field[currentFigureRow][currentFigureCol] == currentFigure)
                {
                    bool IsInside = ChecksIfMoveCoordinatesExist(field, rowToBe, colToBe);
                    if (IsInside == false)
                    {
                        Console.WriteLine("Move go out of board!");
                        continue;
                    }
                    bool moveUpdate = ChecksIfTheMoveIsValid(field, currentFigureRow, currentFigureCol, rowToBe, colToBe, currentFigure);
                    if (moveUpdate)
                    {
                        continue;
                    }
                    Console.WriteLine("Invalid move!");
                }
                else
                {
                    Console.WriteLine("There is no such a piece!");
                    continue;
                }


            }



        }

        private static bool ChecksIfTheMoveIsValid(char[][] field, int currentFigureRow, int currentFigureCol, int rowToBe, int colToBe, char currentFigure)
        {
            if (currentFigure == 'K')
            {
                bool kingIsLegal = ChecksKing(field, currentFigureRow, currentFigureCol, rowToBe, colToBe, currentFigure);
                if (kingIsLegal)
                {
                    return true;
                }
            }
            else if (currentFigure == 'R')
            {
                bool rookIsLegal = ChecksStraigthLine(field, currentFigureRow, currentFigureCol, rowToBe, colToBe, currentFigure);
                if (rookIsLegal)
                {
                    return true;
                }
            }
            else if (currentFigure == 'B')
            {
                bool bishopIsLegal = ChecksDiagonals(field, currentFigureRow, currentFigureCol, rowToBe, colToBe, currentFigure);
                if (bishopIsLegal)
                {
                    return true;
                }
            }
            else if (currentFigure == 'Q')
            {
                bool queenLegalDiagonals = ChecksDiagonals(field, currentFigureRow, currentFigureCol, rowToBe, colToBe, currentFigure);
                bool queenLegalStraightLines = ChecksStraigthLine(field, currentFigureRow, currentFigureCol, rowToBe, colToBe, currentFigure);
                if (queenLegalDiagonals || queenLegalStraightLines)
                {
                    return true;
                }

            }
            else if (currentFigure == 'P')
            {
                bool pawnMove = ChecksPawnMove(field, currentFigureRow, currentFigureCol, rowToBe, colToBe, currentFigure);
                if (pawnMove)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ChecksPawnMove(char[][] field, int currentFigureRow, int currentFigureCol, int rowToBe, int colToBe, char currentFigure)
        {
            if (currentFigureRow - 1 == rowToBe || currentFigureRow == rowToBe)
            {
                field[currentFigureRow][currentFigureCol] = 'x';
                field[rowToBe][colToBe] = 'P';
                return true;
            }
            return false;
        }

        private static bool ChecksDiagonals(char[][] field, int currentFigureRow, int currentFigureCol, int rowToBe, int colToBe, char currentFigure)
        {
            int staticRow = currentFigureRow;
            int staticCol = currentFigureCol;
            if (currentFigureRow > rowToBe)
            {
                if (currentFigureCol > colToBe)
                {
                    for (int i = currentFigureRow; i >= 0; i--)
                    {
                        if (i == rowToBe && currentFigureCol == colToBe)
                        {
                            field[staticRow][staticCol] = 'x';
                            if (currentFigure == 'Q')
                            {
                                field[rowToBe][colToBe] = 'Q';
                            }
                            else
                            {
                                field[rowToBe][colToBe] = 'B';
                            }
                            return true;
                        }
                        currentFigureCol--;
                    }
                }
                else if (currentFigureCol < colToBe)
                {
                    for (int i = currentFigureRow; i >= 0; i--)
                    {
                        if (i == rowToBe && currentFigureCol == colToBe)
                        {
                            field[staticRow][staticCol] = 'x';
                            if (currentFigure == 'Q')
                            {
                                field[rowToBe][colToBe] = 'Q';
                            }
                            else
                            {
                                field[rowToBe][colToBe] = 'B';
                            }
                            return true;
                        }
                        currentFigureCol++;
                    }
                }
            }
            else if (currentFigureRow < rowToBe)
            {
                if (currentFigureCol > colToBe)
                {
                    for (int i = currentFigureRow; i <= field.Length - 1; i++)
                    {
                        if (i == rowToBe && currentFigureCol == colToBe)
                        {
                            field[staticRow][staticCol] = 'x';
                            if (currentFigure == 'Q')
                            {
                                field[rowToBe][colToBe] = 'Q';
                            }
                            else
                            {
                                field[rowToBe][colToBe] = 'B';
                            }
                            return true;
                        }
                        currentFigureCol--;
                    }
                }
                else if (currentFigureCol < colToBe)
                {
                    for (int i = currentFigureRow; i <= field.Length - 1; i++)
                    {
                        if (i == rowToBe && currentFigureCol == colToBe)
                        {
                            field[staticRow][staticCol] = 'x';
                            if (currentFigure == 'Q')
                            {
                                field[rowToBe][colToBe] = 'Q';
                            }
                            else
                            {
                                field[rowToBe][colToBe] = 'B';
                            }
                            return true;
                        }
                        currentFigureCol++;
                    }
                }
            }
            return false;
        }

        private static bool ChecksStraigthLine(char[][] field, int currentFigureRow, int currentFigureCol, int rowToBe, int colToBe, char currentFigure)
        {
            if (currentFigureRow == rowToBe || currentFigureCol == colToBe)
            {
                field[currentFigureRow][currentFigureCol] = 'x';
                if (currentFigure == 'R')
                {
                    field[rowToBe][colToBe] = 'R';
                }
                else
                {
                    field[rowToBe][colToBe] = 'Q';
                }

                return true;
            }
            return false;
        }

        private static bool ChecksKing(char[][] field, int currentFigureRow, int currentFigureCol, int rowToBe, int colToBe, char currentFigure)
        {

            bool topLeft = (currentFigureRow - 1 == rowToBe && currentFigureCol - 1 == colToBe);
            bool topRight = (currentFigureRow - 1 == rowToBe && currentFigureCol + 1 == colToBe);
            bool botLeft = (currentFigureRow + 1 == rowToBe && currentFigureCol - 1 == colToBe);
            bool botRight = (currentFigureRow + 1 == rowToBe && currentFigureCol + 1 == colToBe);
            bool top = (currentFigureRow - 1 == rowToBe && currentFigureCol == colToBe);
            bool bot = (currentFigureRow + 1 == rowToBe && currentFigureCol == colToBe);
            bool left = (currentFigureRow == rowToBe && currentFigureCol - 1 == colToBe);
            bool right = (currentFigureRow == rowToBe && currentFigureCol + 1 == colToBe);




            if (topLeft || topRight || botLeft || botRight || top || bot || left || right)

            {
                field[currentFigureRow][currentFigureCol] = 'x';
                field[rowToBe][colToBe] = 'K';
                return true;
            }

            if (currentFigureRow == rowToBe && currentFigureCol == colToBe)
            {
                return true;
            }

            return false;
        }

        private static bool ChecksIfMoveCoordinatesExist(char[][] field, int rowToBe, int colToBe)
        {
            bool checkUp = rowToBe >= 0 && rowToBe <= field.Length - 1 && colToBe >= 0 && colToBe <= field[0].Length - 1;
            return checkUp;
        }

        private static void FillField(char[][] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                char[] input = Console.ReadLine().Split(',').Select(char.Parse).ToArray();
                field[i] = new char[input.Length];
                for (int j = 0; j < field[i].Length; j++)
                {
                    field[i][j] = input[j];
                }
            }
        }

        private static void Print(char[][] field)
        {
            foreach (var item in field)
            {
                Console.WriteLine(string.Join("", item));
            }
        }
    }
}