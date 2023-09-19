using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace _2Y_2324_OOP_MP1_TicTacToe
{
    internal class Program
    {
        static Random _rnd = new Random();

        static void Main(string[] args)
        {
            while(true)
            {
                GameStart(true, InitializeBoard());
            }
        }

        static bool PlayerFirstMove()
        {
            int rNum = _rnd.Next(0, 100) + 1;
            if (rNum % 2 == 0)
                return true;
            
            return false;
        }

        static char[,] InitializeBoard()
        {
            char[,] board = new char[3, 3];

            for (int x = 0; x < board.GetLength(0); x++)
                for (int y = 0; y < board.GetLength(1); y++)
                    board[x, y] = ' ';

            return board;
        }

        static void GameStart(bool playerFirst, char[,] board)
        {
            bool playerTurn = playerFirst;
            int[] moveCoord = { };
            List<int> boardState = new List<int>();

            for(int x = 1; x <= 9; x++)
                boardState.Add(x);

            while(true)
            {
                DisplayBoard(board);
                if(playerTurn) 
                    moveCoord = PlayerMove();
                else
                    moveCoord = ComputerMove(boardState);

                if (ValidateMove(board, moveCoord))
                {
                    board = UpdateBoard(board, moveCoord, playerTurn);
                    boardState.Remove(CalculateForBoardState(moveCoord));
                    if (CheckVictory(board, playerTurn))
                    {
                        Console.WriteLine("Congratulations!!!!");
                        Console.ReadKey();
                        break;
                    }
                    playerTurn = !playerTurn;
                }
                else if (playerTurn)
                {
                    Console.WriteLine("\nYou performed an invalid move! Press any key to try again.");
                    Console.ReadKey();
                }
                DisplayBoard(board);

                if (boardState.Count == 0)
                {
                    Console.WriteLine("No more possible moves.");
                    Console.ReadKey();
                    break;
                }
            }


        }

        static int[] PlayerMove()
        {
            int[] moveCoords = new int[] { 0, 0 };
            string uInput = "";
            string[] uInputs = { };

            while (true)
            {
                
                Console.WriteLine("Please enter valid coordinates using the following format x-y." +
                    "\nx is the column number (0-2)" +
                    "\ny is the row number (0-2)" +
                    "\n");
                uInput = Console.ReadLine();
                uInputs = uInput.Split('-');
                if(uInputs.Length >= 2)
                {
                    if (int.TryParse(uInputs[0], out moveCoords[0]) 
                        && int.TryParse(uInputs[1], out moveCoords[1]))
                    {
                        break;
                    }
                }

                Console.WriteLine("Invalid Coordinate input. Press any key to try again...");
                Console.ReadKey();
            }


            return moveCoords;
        }

        static int[] ComputerMove(List<int> boardState)
        {
            int[] moveCoords = new int[] { 0, 0 };
            int boardMove = boardState[_rnd.Next(boardState.Count)];
            moveCoords[1] = boardMove % 3;
            moveCoords[0] = (boardMove - moveCoords[1]) / 3;            

            return moveCoords;
        }

        static void DisplayBoard(char[,] board)
        {
            Console.Clear();
            Console.WriteLine("-------------");
            for (int x = 0; x < board.GetLength(0); x++)
            {
                Console.Write("|");
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    Console.Write($" {board[x,y]} |");
                }
                Console.WriteLine("\n-------------");
            }
        }

        static bool ValidateMove(char[,] board, int[] moveCoord)
        {
            if (board[moveCoord[0], moveCoord[1]] == ' ')
                return true;

            return false;
        }

        static char[,] UpdateBoard(char[,] board, int[] moveCoord, bool playerTurn)
        {
            char moveChar = 'X';

            if (playerTurn)
                moveChar = 'O';

            board[moveCoord[0], moveCoord[1]] = moveChar;

            return board;
        }

        static int CalculateForBoardState(int[] moveCoord)
        {
            return (3 * moveCoord[0]) + moveCoord[1];
        }

        static bool CheckVictory(char[,] board, bool playerTurn)
        {
            char checkFor = 'X';
            bool victoryDetected = true;

            if (playerTurn)
                checkFor = 'O';

            // check rows
            for(int x = 0; x <  board.GetLength(0); x++) 
            {
                victoryDetected = true;
                for(int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] != checkFor)
                    {
                        victoryDetected = false;
                        break;
                    }
                }
            }

            // check columns
            if(!victoryDetected)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    victoryDetected = true;
                    for (int x = 0; x < board.GetLength(0); x++)
                    {
                        if (board[x, y] != checkFor)
                        {
                            victoryDetected = false;
                            break;
                        }
                    }
                }
            }

            // check diagonals
            if(!victoryDetected)
            {
                if ((board[0, 0] == checkFor && board[1, 1] == checkFor && board[2, 2] == checkFor)
                    || (board[0, 2] == checkFor && board[1, 1] == checkFor && board[2, 0] == checkFor))
                    victoryDetected = true;
            }

            return victoryDetected;
        }
    }
}
