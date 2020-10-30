using ConsoleInput;
using ConsoleOutput;
using Models;
using System;

namespace Linker
{
    class Program
    {
        static void Main(string[] args)
        {
            OutputManager outputManager = new OutputManager();
            GameManager gameManager = new GameManager();
            InputManager inputManager = new InputManager(gameManager);
            outputManager.ListenTo(gameManager);
            inputManager.StartGame();
        }
    }
}
