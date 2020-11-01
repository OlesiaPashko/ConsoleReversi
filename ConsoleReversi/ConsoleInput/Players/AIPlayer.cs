using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInput.Players
{
    public class AIPlayer
    {

        private List<char> firstLetters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        private List<char> secondLetters = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8' };

        protected bool isFirst;
        protected GameManager gameManager;
        public AIPlayer(GameManager gameManager, bool isFirst)
        {
            this.gameManager = gameManager;
            this.isFirst = isFirst;
        }


        /// <summary>
        /// Transform (0, 0) to A1
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        protected string CoordsToString(Tuple<int, int> coords)
        {
            return firstLetters[coords.Item2].ToString() + secondLetters[coords.Item1];
        }


        /// <summary>
        /// Makes deep copy of array of arrays
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected List<List<Cell>> MakeDeepCopy(List<List<Cell>> list)
        {
            List<List<Cell>> copy = new List<List<Cell>>();
            foreach (var row in list)
            {
                List<Cell> rowCopy = new List<Cell>();
                foreach (var cell in row)
                {
                    Cell cellCopy = new Cell(cell.State);
                    rowCopy.Add(cellCopy);
                }
                copy.Add(rowCopy);
            }
            return copy;
        }
    }
}
