
using Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleInput
{
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
            Player player = new Player(gameManager);
            Tuple<int,int> blackHole = GetBlackHole();
            CellState color = GetColor();
            if (color == CellState.White)
            {
                //Console.WriteLine("i moves white");
                gameManager.StartGame(blackHole, MakeMove());
            }
            else
            {
                //Console.WriteLine("i moves black");
                gameManager.StartGame(blackHole);
            }
            while (true)
            {
                Console.WriteLine("tralala");
                player.MakeRandomMove();
                Console.WriteLine("tralala 2");
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
            Tuple<int, int> moveCoords;
            do
            {
                //Console.WriteLine("Please, make move");
                move = Console.ReadLine();
                move = move.Trim().ToLower();
                if (!IsCorrectMove(move))
                {
                    Console.WriteLine("Input is incorrect. Correct input is like A3 or b2");
                    continue;
                }
                moveCoords = ParseCoords(move);
                if (availableCells.Contains(moveCoords))
                {
                    break;
                }
                Console.WriteLine("This cell isn`t available");
            }
            while (true);
            Console.WriteLine("There");
            gameManager.MakeMove(moveCoords);
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
            int column = firstLetters.FindIndex(x => x == coords[0]);
            int row = secondLetters.FindIndex(x => x == coords[1]);
            return new Tuple<int, int>(row, column);
        }
    }
}
