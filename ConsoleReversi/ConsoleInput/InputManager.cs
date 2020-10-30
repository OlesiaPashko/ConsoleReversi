
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
            Player player;
            Tuple<int,int> blackHole = GetBlackHole();
            CellState color = GetColor();
            if (color == CellState.White)
            {
                player = new Player(gameManager, false);
                gameManager.StartGame(blackHole, MakeMove());
            }
            else
            {
                player = new Player(gameManager, true);
                gameManager.StartGame(blackHole);
            }
            while (true)
            {
                player.MakeMove();
                MakeMove();
            }
        }

        public Tuple<int, int> GetBlackHole()
        {
            return ParseCoords(Console.ReadLine());
        }

        public CellState GetColor()
        {
            string color = Console.ReadLine();
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
                move = Console.ReadLine();
                move = move.Trim().ToLower();
                if (!IsCorrectMove(move))
                {
                    if (move == "pass")
                    {
                        gameManager.SwitchPlayer();
                        gameManager.passedMovesCount += 1;
                        break;
                    }
                    continue;
                }
                moveCoords = ParseCoords(move);
                if (availableCells.Contains(moveCoords))
                {
                    gameManager.MakeMove(moveCoords);
                    break;
                }
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
