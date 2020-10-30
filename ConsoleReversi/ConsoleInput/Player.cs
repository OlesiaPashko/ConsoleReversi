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
        private bool isFirst;

        int[][] CellRate = new int[][]{
            new int[]{251, -55, 10, 13},
            new int[]{-55, -73,  -10, -3},
            new int[]{10, -10,  -14, -2},
            new int[]{13, -3,  -2, 0}
        };

        public Player(GameManager gameManager, bool isFirst)
        {
            this.gameManager = gameManager;
            this.isFirst = isFirst;
        }
        public void MakeMove()
        {
            List<Tuple<int, int>> availableCells = gameManager.GetAvailableCells();

            //Console.WriteLine(availableCells.Count);
            if (availableCells.Count == 0)
            {
                //  Console.WriteLine("------------------Pass in Player.MakeMove()-----------------------------------");
                gameManager.Pass();
                //Console.WriteLine("pass");
                return;
            }
            Tuple<int, int> bestMove = availableCells[0];
            int bestScore = int.MaxValue;
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

            //Console.WriteLine("Move " + move.Item1 + "   " + move.Item2);
            Console.WriteLine(CoordsToString(bestMove));
            gameManager.MakeMove(bestMove);
        }

        public string CoordsToString(Tuple<int, int> coords)
        {
            return firstLetters[coords.Item2].ToString() + secondLetters[coords.Item1];
        }

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

        public int MiniMax(int depth, int alpha, int beta, bool isMinimizing)
        {
            if(depth == 0 || gameManager.IsGameFinished())
            {
                return Eval(gameManager.GetCells());
            }

            if (isMinimizing)
            {
                int bestScore = int.MaxValue;
                var availableMoves = gameManager.GetAvailableCells();
                if (availableMoves.Count == 0)
                {
                    List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                    gameManager.passedMovesCount += 1;
                    gameManager.SwitchPlayer();
                    int score = MiniMax(depth-1, alpha, beta, false);
                    bestScore = GetMin(score, bestScore);
                    gameManager.UndoMove(cellsBeforeMove);
                }
                else
                {
                    //Console.WriteLine(availableMoves.Count);
                    foreach (var move in availableMoves)
                    {
                        List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                        gameManager.MakeMove(move);
                        int score = MiniMax(depth - 1, beta, alpha, false);
                        bestScore = GetMin(score, bestScore);
                        beta = GetMin(beta, bestScore);

                        gameManager.UndoMove(cellsBeforeMove);

                        if (beta <= alpha)
                            break;
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MinValue;
                var availableMoves = gameManager.GetAvailableCells();
                if (availableMoves.Count == 0)
                {
                    List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                    gameManager.passedMovesCount += 1;
                    gameManager.SwitchPlayer();
                    int score = MiniMax(depth - 1, alpha, beta, true);
                    bestScore = GetMax(score, bestScore);
                    gameManager.UndoMove(cellsBeforeMove);
                }
                else
                {
                    //Console.WriteLine(availableMoves.Count);
                    foreach (var move in availableMoves)
                    {
                        List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                        gameManager.MakeMove(move);
                        int score = MiniMax(depth - 1, beta, alpha, true);
                        bestScore = GetMax(score, bestScore);
                        alpha = GetMax(beta, bestScore);

                        gameManager.UndoMove(cellsBeforeMove);

                        if (beta <= alpha)
                            break;
                    }
                }
                return bestScore;
            }
        }

        /*public int Min(int depth, int beta, int alpha)
        {
            //Console.WriteLine("In min");
            //Console.WriteLine(depth);
            if (gameManager.IsGameFinished())
            {
                bool isFirstPlayerWinner = gameManager.IsFirstPlayerWon();
                if ((isFirst && isFirst) || (!isFirst && !isFirstPlayerWinner))
                {
                    return int.MaxValue;
                }
                return int.MinValue;
            }
            else if (depth <= 0)
            {
                return Eval(gameManager.GetCells());
            }
            else
            {
                int bestScore = int.MaxValue;
                var availableMoves = gameManager.GetAvailableCells();
                if (availableMoves.Count == 0)
                {
                    List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                    gameManager.passedMovesCount += 1;
                    gameManager.SwitchPlayer();
                    int score = Max(depth - 1);
                    if (score < bestScore)
                    {
                        bestScore = score;
                    }
                    gameManager.UndoMove(cellsBeforeMove);
                }
                else
                {
                    //Console.WriteLine(availableMoves.Count);
                    foreach (var move in availableMoves)
                    {
                        List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                        gameManager.MakeMove(move);
                        int score = Max(depth - 1, beta, alpha);
                        bestScore = GetMin(score, bestScore);
                        beta = GetMin(beta, bestScore);

                        gameManager.UndoMove(cellsBeforeMove);

                        if (beta <= alpha)
                            return;
                    }
                }
                return bestScore;
            }
        }

        public int Max(int depth, int beta, int alpha)
        {
            //Console.WriteLine("In max");
            //Console.WriteLine(depth);
            if (gameManager.IsGameFinished())
            {
                bool isFirstPlayerWinner = gameManager.IsFirstPlayerWon();
                if ((isFirst && isFirst) || (!isFirst && !isFirstPlayerWinner))
                {
                    return int.MaxValue;
                }
                return int.MinValue;
            }
            else if (depth <= 0)
            {
                return Eval(gameManager.GetCells());
            }
            else
            {
                int bestScore = int.MinValue;
                var availableMoves = gameManager.GetAvailableCells();
                if (availableMoves.Count == 0)
                {
                    List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                    gameManager.passedMovesCount += 1;
                    gameManager.SwitchPlayer();
                    int score = Max(depth - 1);
                    if (score > bestScore)
                    {
                        bestScore = score;
                    }
                    gameManager.UndoMove(cellsBeforeMove);
                }
                else
                {
                    foreach (var move in availableMoves)
                    {
                        List<List<Cell>> cellsBeforeMove = MakeDeepCopy(gameManager.GetCells());
                        gameManager.MakeMove(move);
                        int score = Min(depth - 1, beta, alpha);
                        if (score > bestScore)
                        {
                            bestScore = score;
                        }
                        gameManager.UndoMove(cellsBeforeMove);
                    }
                }
                return bestScore;
            }
        }*/

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
