using System;
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
        if(IsAvailableGoingUp(playerColor, rowIndex, columnIndex))
            PaintGoingUp(playerColor, rowIndex, columnIndex);
        if (IsAvailableGoingDown(playerColor, rowIndex, columnIndex))
            PaintGoingDown(playerColor, rowIndex, columnIndex);
        if(IsAvailableGoingLeft(playerColor, rowIndex, columnIndex))
            PaintGoingLeft(playerColor, rowIndex, columnIndex);
        if (IsAvailableGoingRight(playerColor, rowIndex, columnIndex))
            PaintGoingRight(playerColor, rowIndex, columnIndex);
        if (IsAvailableGoingUpAndLeft(playerColor, rowIndex, columnIndex))
            PaintGoingUpAndLeft(playerColor, rowIndex, columnIndex);
        if (IsAvailableGoingUpAndRight(playerColor, rowIndex, columnIndex))
            PaintGoingUpAndRight(playerColor, rowIndex, columnIndex);
        if (IsAvailableGoingDownAndLeft(playerColor, rowIndex, columnIndex))
            PaintGoingDownAndLeft(playerColor, rowIndex, columnIndex);
        if (IsAvailableGoingDownAndRight(playerColor, rowIndex, columnIndex))
            PaintGoingDownAndRight(playerColor, rowIndex, columnIndex);
    }

    private void PaintGoingUp(CellState playerColor, int rowIndex, int columnIndex)
    {
        rowIndex--;
        while (rowIndex > 0 && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            Cells[rowIndex][columnIndex].ChangeColor();
            rowIndex--;
        }
    }

    private void PaintGoingDown(CellState playerColor, int rowIndex, int columnIndex)
    {
        rowIndex++;
        while (rowIndex < Cells.Count - 1 && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            Cells[rowIndex][columnIndex].ChangeColor();
            rowIndex++;
        }
    }

    private void PaintGoingLeft(CellState playerColor, int rowIndex, int columnIndex)
    {
        columnIndex--;
        while (columnIndex > 0 && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            Cells[rowIndex][columnIndex].ChangeColor();
            columnIndex--;
        }
    }

    private void PaintGoingRight(CellState playerColor, int rowIndex, int columnIndex)
    {
        columnIndex++;
        while (columnIndex < Cells.Count - 1 && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            Cells[rowIndex][columnIndex].ChangeColor();
            columnIndex++;
        }
    }

    private void PaintGoingUpAndLeft(CellState playerColor, int rowIndex, int columnIndex)
    {
        columnIndex--;
        rowIndex--;
        while (columnIndex > 0 && rowIndex > 0
            && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            Cells[rowIndex][columnIndex].ChangeColor();
            columnIndex--;
            rowIndex--;
        }
    }

    private void PaintGoingUpAndRight(CellState playerColor, int rowIndex, int columnIndex)
    {
        columnIndex++;
        rowIndex--;
        while (columnIndex < Cells.Count - 1 && rowIndex > 0
            && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            Cells[rowIndex][columnIndex].ChangeColor();
            columnIndex++;
            rowIndex--;
        }
    }

    private void PaintGoingDownAndLeft(CellState playerColor, int rowIndex, int columnIndex)
    {
        columnIndex--;
        rowIndex++;
        while (columnIndex > 0 && rowIndex < Cells.Count - 1
            && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            Cells[rowIndex][columnIndex].ChangeColor();
            columnIndex--;
            rowIndex++;
        }
    }

    private void PaintGoingDownAndRight(CellState playerColor, int rowIndex, int columnIndex)
    {
        columnIndex++;
        rowIndex++;
        while (columnIndex < Cells.Count - 1 && rowIndex < Cells.Count - 1
            && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            Cells[rowIndex][columnIndex].ChangeColor();
            columnIndex++;
            rowIndex++;
        }
    }

    public void ChangeColorOfAllCells()
    {
        foreach(var cellsRow in Cells)
        {
            foreach(var cell in cellsRow)
            {
                cell.ChangeColor();
            }
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
        return IsAvailableGoingUp(playerColor, rowIndex, columnIndex) 
            ||IsAvailableGoingDown(playerColor, rowIndex, columnIndex)
            ||IsAvailableGoingLeft(playerColor, rowIndex, columnIndex)
            || IsAvailableGoingRight(playerColor, rowIndex, columnIndex)
            || IsAvailableGoingUpAndLeft(playerColor, rowIndex, columnIndex)
            || IsAvailableGoingUpAndRight(playerColor, rowIndex, columnIndex)
            || IsAvailableGoingDownAndLeft(playerColor, rowIndex, columnIndex)
            || IsAvailableGoingDownAndRight(playerColor, rowIndex, columnIndex);
    }

    private bool IsAvailableGoingUp(CellState playerColor, int rowIndex, int columnIndex)
    {
        if (rowIndex < 2)
            return false;
        rowIndex--;

        bool isAtLeastOneEnemyCell = false;

        while (rowIndex > 0 && Cells[rowIndex][columnIndex].State != CellState.Empty
            &&Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            rowIndex--;
            isAtLeastOneEnemyCell = true;
        }

        if (Cells[rowIndex][columnIndex].State == playerColor&&isAtLeastOneEnemyCell)
            return true;

        return false;
    }

    private bool IsAvailableGoingDown(CellState playerColor, int rowIndex, int columnIndex)
    {
        if (rowIndex > Cells.Count - 2)
            return false;

        rowIndex++;

        bool isAtLeastOneEnemyCell = false;
        while (rowIndex < Cells.Count - 1 && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            rowIndex++;
            isAtLeastOneEnemyCell = true;
        }

        if (Cells[rowIndex][columnIndex].State == playerColor && isAtLeastOneEnemyCell)
            return true;

        return false;
    }

    private bool IsAvailableGoingLeft(CellState playerColor, int rowIndex, int columnIndex)
    {
        if (columnIndex < 2)
            return false;

        columnIndex--;

        bool isAtLeastOneEnemyCell = false;

        while (columnIndex>0 && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            columnIndex--;
            isAtLeastOneEnemyCell = true;
        }

        if(Cells[rowIndex][columnIndex].State == playerColor&&isAtLeastOneEnemyCell)
            return true;

        return false;
    }

    private bool IsAvailableGoingRight(CellState playerColor, int rowIndex, int columnIndex)
    {
        if (columnIndex > Cells.Count - 2)
            return false;

        bool isAtLeastOneEnemyCell = false;

        columnIndex++;
        while (columnIndex < Cells.Count - 1 && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            columnIndex++;
            isAtLeastOneEnemyCell = true;
        }

        if (Cells[rowIndex][columnIndex].State == playerColor && isAtLeastOneEnemyCell)
            return true;

        return false;
    }

    private bool IsAvailableGoingUpAndLeft(CellState playerColor, int rowIndex, int columnIndex)
    {
        if (rowIndex < 2 || columnIndex < 2)
            return false;
        columnIndex--;
        rowIndex--;
        bool isAtLeastOneEnemyCell = false;

        while (columnIndex > 0 && rowIndex > 0
            && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            columnIndex--;
            rowIndex--;
            isAtLeastOneEnemyCell = true;
        }

        if (Cells[rowIndex][columnIndex].State == playerColor&&isAtLeastOneEnemyCell)
            return true;

        return false;
    }

    private bool IsAvailableGoingUpAndRight(CellState playerColor, int rowIndex, int columnIndex)
    {
        if (rowIndex < 2 || columnIndex>Cells.Count-2)
            return false;
        columnIndex++;
        rowIndex--;
        bool isAtLeastOneEnemyCell = false;
        while (columnIndex < Cells.Count - 1 && rowIndex > 0
            && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            columnIndex++;
            rowIndex--;
            isAtLeastOneEnemyCell = true;
        }

        if (Cells[rowIndex][columnIndex].State == playerColor&&isAtLeastOneEnemyCell)
            return true;

        return false;
    }

    private bool IsAvailableGoingDownAndRight(CellState playerColor, int rowIndex, int columnIndex)
    {
        if (rowIndex > Cells.Count-2||columnIndex>Cells.Count-2)
            return false;
        columnIndex++;
        rowIndex++;
        bool isAtLeastOneEnemyCell = false;
        while (columnIndex < Cells.Count - 1 && rowIndex < Cells.Count - 1
            && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            columnIndex++;
            rowIndex++;
            isAtLeastOneEnemyCell = true;
        }

        if (Cells[rowIndex][columnIndex].State == playerColor&&isAtLeastOneEnemyCell)
            return true;

        return false;
    }

    private bool IsAvailableGoingDownAndLeft(CellState playerColor, int rowIndex, int columnIndex)
    {
        if (rowIndex > Cells.Count-2||columnIndex < 2)
            return false;
        columnIndex--;
        rowIndex++;
        bool isAtLeastOneEnemyCell = false;
        while (columnIndex > 0 && rowIndex < Cells.Count - 1
            && Cells[rowIndex][columnIndex].State != CellState.Empty
            && Cells[rowIndex][columnIndex].State == GetOppositeColor(playerColor))
        {
            columnIndex--;
            rowIndex++;
            isAtLeastOneEnemyCell = true;
        }

        if (Cells[rowIndex][columnIndex].State == playerColor&&isAtLeastOneEnemyCell)
            return true;

        return false;
    }
}
