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

            for(int x = 0; x < 9; x++)
                boardState.Add(x);

            while(true)
            {
                DisplayBoard(board);
                if(playerTurn) 
                    moveCoord = PlayerMove();
                else
                    moveCoord = ComputerMove(boardState, board, playerFirst);

                if (ValidateMove(board, moveCoord))
                {
                    board = UpdateBoard(board, moveCoord, playerTurn);
                    boardState.Remove(CalculateForBoardState(moveCoord));
                    if (CheckVictory(board, playerTurn))
                    {
                        DisplayBoard(board);
                        Console.WriteLine($"Congratulations!!!! {playerTurn}");
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
                else
                {
                    Console.WriteLine($"\nComputer tried to move to {moveCoord[0]} - {moveCoord[1]}");
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
                    "\nx is the row number (0-2)" +
                    "\ny is the column number (0-2)" +
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

        static int[] ComputerMove(List<int> boardState, char[,] board, bool playerFirst)
        {
            int[] moveCoords = new int[] { 0, 0 };
            int boardMove = -1;

            if (!playerFirst && boardState.Count == 9) // computer first and its the first move
                boardMove = 4; // take center
            else if (boardState.Count == 8) // computer moves second and its the first move
            {
                // second move
                if (!boardState.Contains(4)) // if center is taken
                    boardMove = (_rnd.Next(0, 4) * 2) + 1;
                else
                    boardMove = 4; // control center
            }
            else
            {

                Console.WriteLine("Checking Defense...");
                // row check
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    if (board[x, 0] == ' ' || board[x, 1] == ' ' || board[x, 2] == ' ')// if all 3 spaces are taken, its not worth checking
                    {
                        if (board[x, 0] == 'O' && board[x, 1] == 'O' && board[x, 2] == ' ')
                            boardMove = 2 + (2 * x);
                        else if (board[x, 0] == 'O' && board[x, 1] == ' ' && board[x, 2] == 'O')
                            boardMove = 1 + (2 * x);
                        else if (board[x, 0] == ' ' && board[x, 1] == 'O' && board[x, 2] == 'O')
                            boardMove = 0 + (2 * x);
                    }

                    if (boardMove > 0) // if a move has been selected, break the loop
                        break;
                }
                // column check
                if (boardMove == -1) // no move selected
                {
                    for (int x = 0; x < board.GetLength(1); x++)
                    {
                        if (board[0, x] == ' ' || board[1, x] == ' ' || board[2, x] == ' ')// if all 3 spaces are taken, its not worth checking
                        {
                            if (board[0, x] == 'O' && board[1, x] == 'O' && board[2, x] == ' ')
                                boardMove = 0 + x;
                            else if (board[0, x] == 'O' && board[1, x] == ' ' && board[2, x] == 'O')
                                boardMove = 3 + x;
                            else if (board[0, x] == ' ' && board[1, x] == 'O' && board[2, x] == 'O')
                                boardMove = 6 + x;
                        }

                        if (boardMove > 0) // if a move has been selected, break the loop
                            break;
                    }
                }

                // row check
                if (boardMove == -1)
                {
                    if (board[0, 0] == ' ' || board[1, 1] == ' ' || board[2, 2] == ' ')
                    {
                        if (board[0, 0] == 'O' && board[1, 1] == 'O' && board[2, 2] == ' ')
                            boardMove = 8;
                        else if (board[0, 0] == 'O' && board[1, 1] == ' ' && board[2, 2] == 'O')
                            boardMove = 4;
                        else if (board[0, 0] == ' ' && board[1, 1] == 'O' && board[2, 2] == 'O')
                            boardMove = 0;
                    }

                    if (board[0, 2] == ' ' || board[1, 1] == ' ' || board[2, 0] == ' ')
                    {
                        if (board[0, 2] == 'O' && board[1, 1] == 'O' && board[2, 0] == ' ')
                            boardMove = 6;
                        else if (board[0, 2] == 'O' && board[1, 1] == ' ' && board[2, 0] == 'O')
                            boardMove = 4;
                        else if (board[0, 2] == ' ' && board[1, 1] == 'O' && board[2, 0] == 'O')
                            boardMove = 2;
                    }
                }

                Console.WriteLine("Done Checking Defense...");


                Console.WriteLine("Checking Offense...");
                // row check
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    if (board[x, 0] == ' ' || board[x, 1] == ' ' || board[x, 2] == ' ')// if all 3 spaces are taken, its not worth checking
                    {
                        if (board[x, 0] == 'X' && board[x, 1] == 'X' && board[x, 2] == ' ')
                            boardMove = 2 + (2 * x);
                        else if (board[x, 0] == 'X' && board[x, 1] == ' ' && board[x, 2] == 'X')
                            boardMove = 1 + (2 * x);
                        else if (board[x, 0] == ' ' && board[x, 1] == 'X' && board[x, 2] == 'X')
                            boardMove = 0 + (2 * x);
                    }

                    if (boardMove > 0) // if a move has been selected, break the loop
                        break;
                }
                // column check

                for (int x = 0; x < board.GetLength(1); x++)
                {
                    if (board[0, x] == ' ' || board[1, x] == ' ' || board[2, x] == ' ')// if all 3 spaces are taken, its not worth checking
                    {
                        if (board[0, x] == 'X' && board[1, x] == 'X' && board[2, x] == ' ')
                            boardMove = 0 + x;
                        else if (board[0, x] == 'X' && board[1, x] == ' ' && board[2, x] == 'X')
                            boardMove = 3 + x;
                        else if (board[0, x] == ' ' && board[1, x] == 'X' && board[2, x] == 'X')
                            boardMove = 6 + x;
                    }

                    if (boardMove > 0) // if a move has been selected, break the loop
                        break;
                }


                // row check
                if (board[0, 0] == ' ' || board[1, 1] == ' ' || board[2, 2] == ' ')
                {
                    if (board[0, 0] == 'X' && board[1, 1] == 'X' && board[2, 2] == ' ')
                        boardMove = 8;
                    else if (board[0, 0] == 'X' && board[1, 1] == ' ' && board[2, 2] == 'X')
                        boardMove = 4;
                    else if (board[0, 0] == ' ' && board[1, 1] == 'X' && board[2, 2] == 'X')
                        boardMove = 0;
                }

                if (board[0, 2] == ' ' || board[1, 1] == ' ' || board[2, 0] == ' ')
                {
                    if (board[0, 2] == 'X' && board[1, 1] == 'X' && board[2, 0] == ' ')
                        boardMove = 6;
                    else if (board[0, 2] == 'X' && board[1, 1] == ' ' && board[2, 0] == 'X')
                        boardMove = 4;
                    else if (board[0, 2] == ' ' && board[1, 1] == 'X' && board[2, 0] == 'X')
                        boardMove = 2;
                }
                

                Console.WriteLine("Done Checking offense...");


                if (boardMove == -1)
                {
                    boardMove = boardState[_rnd.Next(boardState.Count)];
                }
            }

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
