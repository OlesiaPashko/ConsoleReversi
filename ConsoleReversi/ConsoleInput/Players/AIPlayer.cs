using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInput
{
    public class AIPlayer
    {

        private List<char> firstLetters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        private List<char> secondLetters = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8' };
        private bool isFirst;
        public GameManager gameManager;

        // estimates for 1/4 of field
        //was got by many many(100 000) runs of code
        int[][] CellRate = new int[][]{
            new int[]{251, -55, 10, 13},
            new int[]{-55, -73,  -10, -3},
            new int[]{10, -10,  -14, -2},
            new int[]{13, -3,  -2, 0}
        };

        public AIPlayer(GameManager gameManager, bool isFirst)
        {
            this.gameManager = gameManager;
            this.isFirst = isFirst;
        }

        /// <summary>
        /// Make move using MiniMax
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

            //set dafault values
            Tuple<int, int> bestMove = availableCells[0];
            int bestScore = int.MaxValue;

            //get the best move from all
            foreach (var move in availableCells)
            {
                List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                gameManager.MakeMove(move);
                int score = MiniMax(3, int.MinValue, int.MaxValue, true);
                if (score < bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }

                gameManager.UndoMove(cellsBeforeMove);
            }

            //make move
            Console.WriteLine(CoordsToString(bestMove));
            gameManager.MakeMove(bestMove);
        }

        /// <summary>
        /// Transform (0, 0) to A1
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public string CoordsToString(Tuple<int, int> coords)
        {
            return firstLetters[coords.Item2].ToString() + secondLetters[coords.Item1];
        }

        /// <summary>
        /// Makes deep copy of array of arrays
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<List<Cell>> MakeDeepCopy(List<List<Cell>> list)
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

        /// <summary>
        /// MiniMax algorithm for seaching best move
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="isMinimizing"></param>
        /// <returns></returns>
        public int MiniMax(int depth, int alpha, int beta, bool isMinimizing)
        {
            if(depth == 0 || gameManager.IsGameFinished())
            {
                return Eval(gameManager.GetCells());
            }

            if (isMinimizing)
            {
                return Min(depth, alpha, beta);
            }
            else
            {
                return Max(depth, alpha, beta);
            }
        }

        /// <summary>
        /// Get Minimum
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        private int Min(int depth, int alpha, int beta)
        {
            int bestScore = int.MaxValue;
            var availableMoves = gameManager.GetAvailableCells();
            //pass if there is no available moves
            if (availableMoves.Count == 0)
            {
                List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                gameManager.PassWithoutMassage();
                int score = MiniMax(depth - 1, alpha, beta, false);
                bestScore = GetMin(score, bestScore);
                gameManager.UndoMove(cellsBeforeMove);
            }
            else
            {
                //go deeper in tree for every posible move
                foreach (var move in availableMoves)
                {
                    List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                    gameManager.MakeMove(move);
                    int score = MiniMax(depth - 1, beta, alpha, false);
                    bestScore = GetMin(score, bestScore);
                    beta = GetMin(beta, bestScore);

                    gameManager.UndoMove(cellsBeforeMove);

                    //alpha-beta puning
                    if (beta <= alpha)
                        break;
                }
            }
            return bestScore;
        }

        /// <summary>
        /// Get Maximum
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        private int Max(int depth, int alpha, int beta)
        {
            int bestScore = int.MinValue;
            var availableMoves = gameManager.GetAvailableCells();

            //pass if there is no available moves
            if (availableMoves.Count == 0)
            {
                List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                gameManager.PassWithoutMassage();
                int score = MiniMax(depth - 1, alpha, beta, true);
                bestScore = GetMax(score, bestScore);
                gameManager.UndoMove(cellsBeforeMove);
            }
            else
            {
                //go deeper in tree for every posible move
                foreach (var move in availableMoves)
                {
                    List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                    gameManager.MakeMove(move);
                    int score = MiniMax(depth - 1, beta, alpha, true);
                    bestScore = GetMax(score, bestScore);
                    alpha = GetMax(beta, bestScore);

                    gameManager.UndoMove(cellsBeforeMove);

                    //alpha-beta puning
                    if (beta <= alpha)
                        break;
                }
            }
            return bestScore;
        }

        /// <summary>
        /// Evaluation function to estimate cell position
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public int Eval(List<List<Cell>> cells) {
            int score = 0;
            CellState playerColor;
            if (isFirst)
            {
                playerColor = CellState.Black;
            }
            else
            {
                playerColor = CellState.White;
            }
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    if (cells[i][j].State == playerColor) {
                        score += CellRate[MapIndex(i)][MapIndex(j)];
                    }
                }
            }
            return score;
        }

        /// <summary>
        /// Map index in CellRate 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int MapIndex(int index)
        {
            if (index > 3)
            {
                return 7 - index;
            }
            return index;
        }

        private int GetMin(int firstValue, int secondValue)
        {
            if (firstValue < secondValue)
                return firstValue;
            return secondValue;
        }

        private int GetMax(int firstValue, int secondValue)
        {
            if (firstValue > secondValue)
                return firstValue;
            return secondValue;
        }
    }
}
