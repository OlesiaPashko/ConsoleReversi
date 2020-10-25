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
    public CellState State { get; set; }
    public void ChangeColor()
    {
        if (State == CellState.Black)
            State = CellState.White;
        else if(State == CellState.White)
            State = CellState.Black;
    }

    public Cell(CellState state)
    {
        this.State = state;
    }

}
