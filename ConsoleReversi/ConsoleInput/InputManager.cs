
using Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleInput
{
    /*
    Black hole is chosen to: G5
Chosen color for player: White
E6
G3
F3
E3
F5
C4
H5
D1
D3
B5
B3
C2
A2
B1
F1
H3
H2
C5
A5
B7
A7
F6
E7
G7
D7
G8
B8
E8
D8
    */

    public class InputManager
    {
        private readonly GameManager gameManager;
        private List<char> firstLetters = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        private List<char> secondLetters = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8' };
        public InputManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void StartGame()
        {
            Player player;
            Tuple<int,int> blackHole = GetBlackHole();
            CellState color = GetColor();
            if (color == CellState.White)
            {
                player = new Player(gameManager, false);
                //Console.WriteLine("i moves white");
                gameManager.StartGame(blackHole, MakeMove());
            }
            else
            {
                player = new Player(gameManager, true);
                //Console.WriteLine("i moves black");
                gameManager.StartGame(blackHole);
            }
            while (true)
            {
                //Console.WriteLine("First player makes move");
                player.MakeMove();
                //Console.WriteLine("Second player makes move");
                //player.MakeRandomMove();
                //MakeMove();
                MakeMove();
            }
        }

        public Tuple<int, int> GetBlackHole()
        {
            //Console.WriteLine("Input Black Hole coords");
            return ParseCoords(Console.ReadLine());
        }

        public CellState GetColor()
        {
            //Console.WriteLine("Input color");
            string color = Console.ReadLine();
            //Console.WriteLine(color);
            if(color == "white") 
            {
                return CellState.White;
            }
            else if(color == "black")
            {
                return CellState.Black;
            }
            return CellState.Black;
        }

        public Tuple<int,int> MakeMove()
        {
            List<Tuple<int,int>> availableCells = gameManager.GetAvailableCells();
            string move;
            Tuple<int, int> moveCoords = new Tuple<int, int>(-1,-1);
            do
            {
                //Console.WriteLine("Please, make move");
               // Console.WriteLine("Passed moves count " + gameManager.passedMovesCount);
                move = Console.ReadLine();
                move = move.Trim().ToLower();
                //Console.WriteLine("!IsCorrectMove(move): " + !IsCorrectMove(move));
                if (!IsCorrectMove(move))
                {
                    if (move == "pass")
                    {
                        gameManager.SwitchPlayer();
                        gameManager.passedMovesCount += 1;
                        break;
                    }
                    //Console.WriteLine("Input is incorrect. Correct input is like A3 or b2");
                    continue;
                }
                moveCoords = ParseCoords(move);
                //Console.WriteLine("availableCells.Contains(moveCoords): " + availableCells.Contains(moveCoords));
                if (availableCells.Contains(moveCoords))
                {
                    gameManager.MakeMove(moveCoords);
                    break;
                }
                //Console.WriteLine("This cell isn`t available");
            }
            while (true);
            
            return moveCoords;
        }


        public bool IsCorrectMove(string move)
        {
            if (move.Length != 2)
                return false;
            return firstLetters.Contains(move[0]) && secondLetters.Contains(move[1]);
        }

        public Tuple<int,int> ParseCoords(string coords)
        {
            coords = coords.ToLower().Trim();
            int column = firstLetters.FindIndex(x => x == coords[0]);
            int row = secondLetters.FindIndex(x => x == coords[1]);
            return new Tuple<int, int>(row, column);
        }
    }
}
