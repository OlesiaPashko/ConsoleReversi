
using ConsoleInput.Players;
using Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleInput
{
    public class InputManager
    {
        private readonly GameManager gameManager;
        
        public InputManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void StartGame()
        {
            AIMonteCarloPlayer firstPlayer;
            ConsolePlayer secondPlayer = new ConsolePlayer(gameManager);
            //set black hole
            Tuple<int,int> blackHole = GetBlackHole();

            //set players turn
            CellState color = GetColor();
            if (color == CellState.White)
            {
                firstPlayer = new AIMonteCarloPlayer(gameManager, false);
                gameManager.StartGame(blackHole, secondPlayer.MakeMove());
            }
            else
            {
                firstPlayer = new AIMonteCarloPlayer(gameManager, true);
                gameManager.StartGame(blackHole);
            }

            //game loop
            while (true)
            {
                firstPlayer.MakeMove();
                secondPlayer.MakeMove();
            }
        }

        public Tuple<int, int> GetBlackHole()
        {
            return ParseCoords(Console.ReadLine());
        }

        public CellState GetColor()
        {
            string color = Console.ReadLine();
            if(color == "white") 
            {
                return CellState.White;
            }
            else if(color == "black")
            {
                return CellState.Black;
            }
            return CellState.Black;
        }

        public static Tuple<int, int> ParseCoords(string coords)
        {
            List<char> firstLetters = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            List<char> secondLetters = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8' };

            coords = coords.ToLower().Trim();
            int column = firstLetters.FindIndex(x => x == coords[0]);
            int row = secondLetters.FindIndex(x => x == coords[1]);
            return new Tuple<int, int>(row, column);
        }
    }
}
