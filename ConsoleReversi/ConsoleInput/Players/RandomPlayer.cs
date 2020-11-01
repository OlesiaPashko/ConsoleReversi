using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInput.Players
{
    public class RandomPlayer
    {
        public GameManager gameManager;
        public RandomPlayer(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }
        /// <summary>
        /// Makes random move or passes without any messages in console
        /// </summary>
        public void MakeMoveWithoutMessage()
        {
            List<Tuple<int, int>> availableCells = gameManager.GetAvailableCells();

            //pass if there are no available moves
            if (availableCells.Count == 0)
            {

                gameManager.PassWithoutMassage();
                return;
            }

            Random random = new Random();
            var randomMove = availableCells[random.Next(availableCells.Count)];
            gameManager.MakeMove(randomMove);
        }
    }
}
