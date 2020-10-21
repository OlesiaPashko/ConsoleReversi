using Assets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleOutput
{
    public class OutputManager
    {
        private List<List<Cell>> cells;
        public void ListenTo(GameManager gameManager)
        {
            gameManager.MoveMade += DrawField;
            gameManager.GameStarted += SetCells;
            gameManager.AvailableCellsCalculated += DrawAvailable;
           //gameManager.GameFinished;
           //gameManager.ScoresCalculated;
           //gameManager.GameRestarted;
        }

        public void SetCells(List<List<Cell>> cells)
        {
            this.cells = cells;
        }

        public void DrawField(List<List<Cell>> cells)
        {
            this.cells = cells;
        }

        public void DrawAvailable(List<Tuple<int, int>> availableCells)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("  A B C D E F G H ");
            for( int i = 0; i<cells.Count;i++)
            {
                Console.Write((i + 1) + " ");
                for(int j = 0;j<cells[i].Count;j++)
                {
                    Cell cell = cells[i][j];
                    if (cell.State == CellState.Black)
                        Console.ForegroundColor = ConsoleColor.Black;
                    else if (cell.State == CellState.White)
                        Console.ForegroundColor = ConsoleColor.White;
                    else if (cell.State == CellState.BlackHole)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    if (availableCells.Contains(new Tuple<int, int>(i, j)))
                        Console.Write("O ");
                    else
                        Console.Write("o ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                Console.WriteLine();
            }
        }
    }
}
