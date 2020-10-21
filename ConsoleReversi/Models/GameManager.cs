using System;
using System.Collections.Generic;

namespace Assets.Models
{
    public class GameManager
    {
        private CellState firstPlayerColor;
        private CellState secondPlayerColor;
        private CellState currentPlayerColor;
        Field field;
        public event Action<List<List<Cell>>> MoveMade;

        public event Action<List<List<Cell>>> GameStarted;

        public event Action<List<Tuple<int, int>>> AvailableCellsCalculated;

        public event Action<int, int> GameFinished;

        public event Action<int, int> ScoresCalculated;

        public event Action GameRestarted;
        public GameManager()
        {
            field = new Field();
        }

        public List<Tuple<int, int>> GetAvailableCells()
        {
            var availableCells = field.GetAvailableCells(currentPlayerColor);
            //AvailableCellsCalculated?.Invoke(availableCells);
            return availableCells;
        }

        public void MakeMove(Tuple<int, int> coolds)
        {
            field.SetCell(currentPlayerColor, coolds);
            MoveMade?.Invoke(field.Cells);
            SwitchPlayer();
            AvailableCellsCalculated?.Invoke(GetAvailableCells());
            if (field.isFull())
            {
                FinishGame();
                return;
            }

            if (GetAvailableCells().Count == 0)
                SwitchPlayer();
        }
        public void RestartGame(Tuple<int, int> blackHoleCoords)
        {
            field = new Field();
            field.SetBlackHole(blackHoleCoords);
            GameRestarted?.Invoke();
        }

        public void StartGame(CellState firstPlayerColor, Tuple<int, int> blackHoleCoords)
        {
            this.firstPlayerColor = firstPlayerColor;
            currentPlayerColor = firstPlayerColor;
            secondPlayerColor = Field.GetOppositeColor(firstPlayerColor);

            field.SetBlackHole(blackHoleCoords);

            GameStarted?.Invoke(field.Cells);

            var availableCells = GetAvailableCells();
            AvailableCellsCalculated?.Invoke(availableCells);
        }

        public void CalculatePlayersScore()
        {
            ScoresCalculated?.Invoke(field.CountCells(firstPlayerColor), field.CountCells(secondPlayerColor));
        }

        public void FinishGame()
        {
            int firstPlayerCellsCount = field.CountCells(firstPlayerColor);
            int secondPlayerCellsCount = field.CountCells(secondPlayerColor);
            GameFinished?.Invoke(firstPlayerCellsCount, secondPlayerCellsCount);
        }

        private void SwitchPlayer()
        {
            currentPlayerColor = currentPlayerColor == firstPlayerColor ? secondPlayerColor : firstPlayerColor;
        }
    }
}
