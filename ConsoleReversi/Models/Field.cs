using System;
using System.Collections;
using System.Collections.Generic;

namespace Models
{
    public class Field
    {
        public List<List<Cell>> Cells { get; private set; }

        private List<Tuple<int, int>> directions = new List<Tuple<int, int>>()
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

        public Field(List<List<Cell>> cells)
        {
            Cells = cells;
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
    }
}
