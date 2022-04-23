using System;
using System.Collections.Generic;

namespace BattleShip
{
    public static class Dialogs
    {
        public static bool StartGameDialog(out string name)
        {
            Console.WriteLine("BATTLESHIP GAME");
            Console.WriteLine("Enter your name and press Enter to start game");
            Console.WriteLine("Press Enter to exit");

            name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            return true;
        }

        public static void ExitGame()
        {
            Console.WriteLine("Game over");
        }

        public static void DisplayBoard(List<List<string>> playerBoard)
        {
            string board = string.Empty;
            foreach(var row in playerBoard)
            {
                foreach(var cell in row)
                {
                    board += $"|{cell}|";
                }

                board += "\n";
            }

            Console.WriteLine(board);
        }
    }
}
