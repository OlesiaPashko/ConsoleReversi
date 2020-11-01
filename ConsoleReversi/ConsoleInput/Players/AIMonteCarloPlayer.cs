using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConsoleInput.Players
{
    public class AIMonteCarloPlayer : AIPlayer
    {
        public AIMonteCarloPlayer(GameManager gameManager, bool isFirst) : base(gameManager, isFirst) { }
        
        /// <summary>
        /// Make move using MonteCarlo
        /// </summary>
        public void MakeMove()
        {
            List<Tuple<int, int>> availableCells = gameManager.GetAvailableCells();

            //pass if there is no available moves
            if (availableCells.Count == 0)
            {
                gameManager.Pass();
                return;
            }

            //get the best move from all and do it
            Tuple<int, int> bestMove = MonteCarlo();
            gameManager.MakeMove(bestMove);
            Console.WriteLine(CoordsToString(bestMove));
        }

        /// <summary>
        /// Gets the best move using Monte Carlo alhorithm
        /// </summary>
        /// <returns></returns>
        private Tuple<int, int> MonteCarlo()
        {
            //get available moves
            List<Tuple<int, int>> availableCells = gameManager.GetAvailableCells();

            //set initial values
            int[] winRates = new int[availableCells.Count];
            DateTime startTime = DateTime.Now;
            int i = 0;

            //try to calculate the best move
            while (DateTime.Now.Subtract(startTime).TotalMilliseconds < 400)
            {
                //cope state before move
                var cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                var currentPlayer = gameManager.currentPlayerColor;
                var passedCount = gameManager.passedMovesCount;

                //make move
                gameManager.MakeMove(availableCells[i]);

                //play random to the end
                bool isWinner = PlayRandom();

                //if player won - increment winRate
                if (isWinner)
                {
                    winRates[i]++;
                }

                //restore state before move
                gameManager.UndoMove(cellsBeforeMove);
                gameManager.currentPlayerColor = currentPlayer;
                gameManager.passedMovesCount = passedCount;

                //go to next available move
                i++;
                if (i == availableCells.Count)
                    i = 0;
             }


            var bestMove = availableCells[GetMaxIndex(winRates)];
            return bestMove;
        }

        /// <summary>
        /// Get index of maximum value in array
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private int GetMaxIndex(int[] list)
        {
            int index = 0;
            int max = int.MinValue;
            for(int i = 0; i<list.Length; i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                    index = i;
                }
            }
            return index;
        }

        /// <summary>
        /// Game loop of random players
        /// </summary>
        /// <returns></returns>
        private bool PlayRandom()
        {
            RandomPlayer player1 = new RandomPlayer(gameManager);
            RandomPlayer player2 = new RandomPlayer(gameManager);
            while (!gameManager.IsGameFinished())
            {
                player1.MakeMoveWithoutMessage();
                player2.MakeMoveWithoutMessage();
            }
            return (isFirst && gameManager.IsFirstPlayerWon()) || (!isFirst && !gameManager.IsFirstPlayerWon());
        }       
    
    }
}
