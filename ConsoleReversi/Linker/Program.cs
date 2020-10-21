using Assets.Models;
using ConsoleInput;
using ConsoleOutput;
using System;

namespace Linker
{
    class Program
    {
        static void Main(string[] args)
        {
            /*CellsDrawer view = GetComponent<CellsDrawer>();
            CellsController cellsController = GetComponent<CellsController>();
            GameManager gameManager = new GameManager();
            cellsController.GameManager = gameManager;
            view.ListenTo(gameManager);
            cellsController.StartGame();*/

            OutputManager outputManager = new OutputManager();
            GameManager gameManager = new GameManager();
            InputManager inputManager = new InputManager(gameManager);
            outputManager.ListenTo(gameManager);
            inputManager.StartGame();
        }
    }
}
