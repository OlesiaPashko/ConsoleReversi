using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public static class FieldHandler
    {

        private static List<Tuple<int, int>> directions = new List<Tuple<int, int>>()
        {
             new Tuple<int, int>(-1, -1),
             new Tuple<int, int>(-1, 0),
             new Tuple<int, int>(-1, 1),
             new Tuple<int, int>(0, -1),
             new Tuple<int, int>(0, 1),
             new Tuple<int, int>(1, -1),
             new Tuple<int, int>(1, 0),
             new Tuple<int, int>(1, 1)
        };
        public static List<List<Cell>> SetCell(CellState playerColor, Tuple<int, int> coords, List<List<Cell>> cells)
        {
            List<List<Cell>> cellsCopy = new List<List<Cell>>(cells);
            cellsCopy[coords.Item1][coords.Item2].State = playerColor;
            cellsCopy = UpdateField(playerColor, coords, cellsCopy);
            return cellsCopy;
        }

        private static List<List<Cell>> UpdateField(CellState playerColor, Tuple<int, int> coords, List<List<Cell>> cells)
        {
            List<List<Cell>> cellsCopy = new List<List<Cell>>(cells);
            int rowIndex = coords.Item1;
            int columnIndex = coords.Item2;

            foreach (var direction in directions)
            {
                if (IsInLine(playerColor, rowIndex, columnIndex, direction.Item1, direction.Item2, cellsCopy))
                    cellsCopy = PaintInLine(playerColor, rowIndex, columnIndex, direction.Item1, direction.Item2, cellsCopy);
            }
            return cellsCopy;
        }

        private static List<List<Cell>> PaintInLine(CellState playerColor, int rowIndex, int columnIndex, int rowDiff, int columnDiff, List<List<Cell>> cells)
        {
            List<List<Cell>> cellsCopy = new List<List<Cell>>(cells);
            rowIndex += rowDiff;
            columnIndex += columnDiff;
            while (IsCellInsideField(rowIndex, columnIndex, cellsCopy) && cellsCopy[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
            {
                cellsCopy[rowIndex][columnIndex].State = playerColor;
                rowIndex += rowDiff;
                columnIndex += columnDiff;
            }
            return cellsCopy;
        }

        public static CellState GetOppositeColor(CellState color)
        {
            if (color == CellState.Black)
            {
                return CellState.White;
            }
            else if (color == CellState.White)
            {
                return CellState.Black;
            }
            return CellState.Empty;
        }

        //returns collection of coordinates (row and column in cells)
        public static List<Tuple<int, int>> GetAvailableCells(CellState playerColor, List<List<Cell>> cells)
        {
            List<Tuple<int, int>> availableCells = new List<Tuple<int, int>>();
            for (int i = 0; i < cells.Count; i++)
            {
                for (int j = 0; j < cells[i].Count; j++)
                {
                    Cell cell = cells[i][j];
                    if (cell.State == CellState.Empty && IsCellAvailable(playerColor, i, j, cells))
                    {
                        availableCells.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return availableCells;
        }

        public static int CountCells(CellState playerColor, List<List<Cell>> cells)
        {
            int count = 0;
            foreach (var row in cells)
            {
                foreach (var cell in row)
                {
                    if (cell.State == playerColor)
                        count++;
                }
            }
            return count;
        }

        public static bool isFull(List<List<Cell>> cells)
        {
            foreach (var row in cells)
            {
                foreach (var cell in row)
                {
                    if (cell.State == CellState.Empty)
                        return false;
                }
            }
            return true;
        }

        private static bool IsCellAvailable(CellState playerColor, int rowIndex, int columnIndex, List<List<Cell>> cells)
        {
            foreach (var direction in directions)
            {
                if (IsInLine(playerColor, rowIndex, columnIndex, direction.Item1, direction.Item2, cells))
                    return true;
            }
            return false;
        }

        private static bool IsInLine(CellState playerColor, int rowIndex, int columnIndex, int rowDiff, int columnDiff, List<List<Cell>> cells)
        {
            rowIndex += rowDiff;
            columnIndex += columnDiff;
            int cellsInLine = 0;
            while (IsCellInsideField(rowIndex, columnIndex, cells) && cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
            {
                cellsInLine += 1;
                rowIndex += rowDiff;
                columnIndex += columnDiff;
            }

            return (IsCellInsideField(rowIndex, columnIndex, cells) && cells[rowIndex][columnIndex].State == playerColor && cellsInLine > 0);
        }

        private static bool IsCellInsideField(int rowIndex, int columnIndex, List<List<Cell>> cells)
        {
            return (rowIndex >= 0 && columnIndex >= 0 && rowIndex < cells.Count && columnIndex < cells.Count);
        }
    }
}
