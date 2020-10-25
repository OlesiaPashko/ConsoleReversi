using System;
using System.Collections;
using System.Collections.Generic;

public class Field 
{
    public List<List<Cell>> Cells { get; private set; }

    public Field()
    {
        int size = 8;
        Cells = new List<List<Cell>>(size);
        for (int i = 0; i < size; i++)
        {
            Cells.Add(new List<Cell>(size));
            for (int j = 0; j < size; j++)
            {
                Cells[i].Add(new Cell(CellState.Empty));
            }
        }
        SetInitialFieldState();
    }

    public void SetBlackHole(Tuple<int, int> coords)
    {
        Cells[coords.Item1][coords.Item2].State = CellState.BlackHole;
    }

    private void SetInitialFieldState()
    {
        Cells[3][3].State = CellState.White;
        Cells[4][4].State = CellState.White;
        Cells[3][4].State = CellState.Black;
        Cells[4][3].State = CellState.Black;
    }

    public void SetCell(CellState playerColor, Tuple<int, int> coords)
    {
        Cells[coords.Item1][coords.Item2].State = playerColor;
        UpdateField(playerColor, coords);
    }

    private void UpdateField(CellState playerColor, Tuple<int, int> coords)
    {
        int rowIndex = coords.Item1;
        int columnIndex = coords.Item2;
        List<Tuple<int, int>> directions = new List<Tuple<int, int>>()
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
        foreach (var direction in directions)
        {
            if (IsInLine(playerColor, rowIndex, columnIndex, direction.Item1, direction.Item2))
                PaintInLine(playerColor, rowIndex, columnIndex, direction.Item1, direction.Item2);
        }
    }

    private void PaintInLine(CellState playerColor, int rowIndex, int columnIndex, int rowDiff, int columnDiff)
    {
        rowIndex += rowDiff;
        columnIndex += columnDiff;
        while (IsCellInsideField(rowIndex, columnIndex) && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            Cells[rowIndex][columnIndex].State = playerColor;
            rowIndex += rowDiff;
            columnIndex += columnDiff;
        }
    }

    public static CellState GetOppositeColor(CellState color)
    {
        if(color == CellState.Black)
        {
            return CellState.White;
        }
        else if(color == CellState.White)
        {
            return CellState.Black;
        }
        return CellState.Empty;
    }

    //returns collection of coordinates (row and column in cells)
    public List<Tuple<int, int>> GetAvailableCells(CellState playerColor)
    {
        List<Tuple<int, int>> availableCells = new List<Tuple<int, int>>();
        for(int i = 0; i < Cells.Count; i++)
        {
            for(int j = 0; j < Cells[i].Count; j++)
            {
                Cell cell = Cells[i][j];
                if (cell.State == CellState.Empty && IsCellAvailable(playerColor, i, j))
                {
                    availableCells.Add(new Tuple<int, int>(i, j));
                }
            }
        }
        return availableCells;
    }

    public int CountCells(CellState playerColor)
    {
        int count = 0;
        foreach(var row in Cells)
        {
            foreach(var cell in row)
            {
                if (cell.State == playerColor)
                    count++;
            }
        }
        return count;
    }

    public bool isFull()
    {
        foreach (var row in Cells)
        {
            foreach (var cell in row)
            {
                if (cell.State == CellState.Empty)
                    return false;
            }
        }
        return true;
    }

    private bool IsCellAvailable(CellState playerColor, int rowIndex, int columnIndex)
    {
        List<Tuple<int, int>> directions = new List<Tuple<int, int>>()
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
        foreach (var direction in directions)
        {
            if (IsInLine(playerColor, rowIndex, columnIndex, direction.Item1, direction.Item2))
                return true;
        }
        return false;
    }

    private bool IsInLine(CellState playerColor, int rowIndex, int columnIndex, int rowDiff, int columnDiff)
    {
        rowIndex += rowDiff;
        columnIndex += columnDiff;
        int cellsInLine = 0;
        while(IsCellInsideField(rowIndex, columnIndex) && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            cellsInLine += 1;
            rowIndex += rowDiff;
            columnIndex += columnDiff;
        }

        return (IsCellInsideField(rowIndex, columnIndex) && Cells[rowIndex][columnIndex].State == playerColor && cellsInLine > 0);
    }

    private bool IsCellInsideField(int rowIndex, int columnIndex)
    {
        return (rowIndex >= 0 && columnIndex >= 0 && rowIndex < Cells.Count && columnIndex < Cells.Count);
    }
}
