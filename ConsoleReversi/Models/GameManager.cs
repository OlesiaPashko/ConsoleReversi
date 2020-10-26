using System;
using System.Collections.Generic;

namespace Models
{
    public class GameManager
    {
        private CellState firstPlayerColor = CellState.Black;
        private CellState secondPlayerColor = CellState.White;
        private CellState currentPlayerColor;
        Field field;
        public event Action<List<List<Cell>>> MoveMade;

        public event Action<List<List<Cell>>> GameStarted;

        public event Action<List<Tuple<int, int>>> AvailableCellsCalculated;

        public event Action<int, int> GameFinished;

        public event Action<int, int> ScoresCalculated;

        public event Action GameRestarted;

        public event Action MovePassed;
        public GameManager()
        {
            field = new Field();
        }

        public List<Tuple<int, int>> GetAvailableCells()
        {
            var availableCells = FieldHandler.GetAvailableCells(currentPlayerColor, field.Cells);
            //AvailableCellsCalculated?.Invoke(availableCells);
            return availableCells;
        }

        public void MakeMove(Tuple<int, int> coolds)
        {
            List<List<Cell>> cells = FieldHandler.SetCell(currentPlayerColor, coolds, field.Cells);
            MoveMade?.Invoke(cells);
            SwitchPlayer();
            AvailableCellsCalculated?.Invoke(GetAvailableCells());
            CalculatePlayersScore(cells);
            if (FieldHandler.isFull(cells))
            {
                FinishGame(cells);
                return;
            }

            if (GetAvailableCells().Count == 0)
            {
                SwitchPlayer();
                MovePassed?.Invoke();
            }
                
        }
        public void RestartGame(Tuple<int, int> blackHoleCoords)
        {
            field = new Field();
            field.SetBlackHole(blackHoleCoords);
            GameRestarted?.Invoke();
        }

        public void StartGame(Tuple<int, int> blackHoleCoords)
        {
            currentPlayerColor = firstPlayerColor;

            field.SetBlackHole(blackHoleCoords);

            GameStarted?.Invoke(field.Cells);

            var availableCells = GetAvailableCells();
            AvailableCellsCalculated?.Invoke(availableCells);
        }

        public void StartGame(Tuple<int, int> blackHoleCoords, Tuple<int, int> firstMove)
        {
            currentPlayerColor = firstPlayerColor;

            field.SetBlackHole(blackHoleCoords);

            GameStarted?.Invoke(field.Cells);

            var availableCells = GetAvailableCells();
            AvailableCellsCalculated?.Invoke(availableCells);
            MakeMove(firstMove);
        }

        public void CalculatePlayersScore(List<List<Cell>> cells)
        {
            ScoresCalculated?.Invoke(FieldHandler.CountCells(firstPlayerColor, cells), FieldHandler.CountCells(secondPlayerColor, cells));
        }

        public void FinishGame(List<List<Cell>> cells)
        {
            int firstPlayerCellsCount = FieldHandler.CountCells(firstPlayerColor, cells);
            int secondPlayerCellsCount = FieldHandler.CountCells(secondPlayerColor, cells);
            GameFinished?.Invoke(firstPlayerCellsCount, secondPlayerCellsCount);
        }

        private void SwitchPlayer()
        {
            currentPlayerColor = currentPlayerColor == firstPlayerColor ? secondPlayerColor : firstPlayerColor;
        }
    }
}
