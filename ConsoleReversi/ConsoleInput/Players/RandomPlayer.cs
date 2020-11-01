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
        public void MakeMoveWithoutMessage()
        {
            List<Tuple<int, int>> availableCells = gameManager.GetAvailableCells();

            //pass if there are no available moves
            if (availableCells.Count == 0)
            {

                //Console.WriteLine("Pass in RandomPlayer");
                gameManager.PassWithoutMassage();
                return;
            }

            Random random = new Random();
            var randomMove = availableCells[random.Next(availableCells.Count)];
            //Console.WriteLine("------------In random player before move");
            gameManager.MakeMove(randomMove);
        }
    }
}
