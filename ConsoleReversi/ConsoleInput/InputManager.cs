using Assets.Models;
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
            gameManager.StartGame(CellState.Black, new Tuple<int, int>(1, 1));
            while (true)
            {
                MakeMove();
            }
        }

        public void MakeMove()
        {
            List<Tuple<int,int>> availableCells = gameManager.GetAvailableCells();
            string move;
            Tuple<int, int> moveCoords;
            do
            {
                Console.WriteLine("Please, make move");
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
            gameManager.MakeMove(moveCoords);
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
        //public void SetBlackHole()
        //{
        //    _gameManager.SetBlackHole(new Tuple<int, int>(1, 1));
        //}
    }
}
