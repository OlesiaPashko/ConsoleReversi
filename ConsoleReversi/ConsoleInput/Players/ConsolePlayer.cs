using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInput
{
    public class ConsolePlayer
    {
        private List<char> firstLetters = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        private List<char> secondLetters = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8' };
        private GameManager gameManager;

        public ConsolePlayer(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        /// <summary>
        /// Make move from console
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> MakeMove()
        {
            List<Tuple<int, int>> availableCells = gameManager.GetAvailableCells();
            string move;
            Tuple<int, int> moveCoords = new Tuple<int, int>(-1, -1);

            //read from console white input isn`t correct
            do
            {
                move = Console.ReadLine();
                move = move.Trim().ToLower();
                
                
                if (!IsCorrectMove(move))
                {
                    //if this is word "pass" - make move pass
                    if (move == "pass")
                    {
                        gameManager.PassWithoutMassage();
                        break;
                    }
                    continue;
                }

                //make correct and available move
                moveCoords = InputManager.ParseCoords(move);
                if (availableCells.Contains(moveCoords))
                {
                    gameManager.MakeMove(moveCoords);
                    break;
                }
            }
            while (true);

            return moveCoords;
        }

        /// <summary>
        /// Checks is move correct
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool IsCorrectMove(string move)
        {
            if (move.Length != 2)
                return false;
            return firstLetters.Contains(move[0]) && secondLetters.Contains(move[1]);
        }
        
    }
}
