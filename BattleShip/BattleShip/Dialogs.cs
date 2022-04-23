using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
