using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInput
{
    public class Player
    {
        private readonly GameManager gameManager;

        private List<char> firstLetters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        private List<char> secondLetters = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8' };
        public Player(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }
        public void MakeRandomMove()
        {
            List<Tuple<int, int>> availableCells = gameManager.GetAvailableCells();
            Random r = new Random();
            Tuple<int, int> move = availableCells[r.Next(0, availableCells.Count)];
            Console.WriteLine(CoordsToString(move));
            gameManager.MakeMove(move);
        }

        public string CoordsToString(Tuple<int, int> coords)
        {
            return firstLetters[coords.Item1].ToString() + secondLetters[coords.Item2];
        }
    }
}
