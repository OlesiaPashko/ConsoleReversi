using System;

public enum CellState
{
    Black,
    White,
    Empty,
    BlackHole
}
public class Cell
{
    public CellState State;
    public void ChangeColor()
    {
        if (State == CellState.Black)
            State = CellState.White;
        else
            State = CellState.Black;
    }

    public Cell(CellState state)
    {
        this.State = state;
    }

}
