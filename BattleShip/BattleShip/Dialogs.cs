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
            board += "|  ||1||2||3||4||5||6||7||8||9||10\n";
            for(int i = 0; i < playerBoard.Count; i++)
            {
                board += i != 9 ? $"| {i + 1}|" : $"|{i + 1}|";
                foreach (var cell in playerBoard[i])
                {
                    board += $"|{cell}|";
                }

                board += "\n";
            }

            Console.WriteLine(board);
        }

        internal static void DisplayAllPlayerBoards(Player player)
        {
            Console.Clear();
            Dialogs.DisplayBoard(player.PlayerBoard);
            Dialogs.DisplayBoard(player.OpponentBoard);
        }

        public static bool BattleDialog(ref Coordinate attackCoordinates)
        {
            Console.WriteLine("Attack opponent, enter coordinates");
            Console.WriteLine("Example: 1, 1");

            string userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Enter attack coordinates");
                return false;
            }

            var attackCoordinatesArray = userInput.Split(",");
            if (attackCoordinatesArray.Length != 2)
            {
                Console.WriteLine("Add comma to separate coordinates");
                return false;
            }

            if (int.TryParse(attackCoordinatesArray[0], out int rowCoordinate) &&
                int.TryParse(attackCoordinatesArray[1], out int columnCoordinate))
            {
                if ((rowCoordinate > 10 || rowCoordinate < 1) &&
                    (columnCoordinate > 10 || columnCoordinate < 1))
                {
                    Console.WriteLine("Coordinate value should be between 1 and 10");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid coordinates");
                return false;
            }

            attackCoordinates.Row = rowCoordinate - 1; // -1 because user imput starts from 1 not from 0
            attackCoordinates.Column = columnCoordinate - 1;

            return true;
        }

        public static bool PlaceShip(ref string userInput)
        {
            Console.WriteLine("Enter ship placement (vertical - V, horizontal - H) and coordinates");
            Console.WriteLine("Example: V, 1, 1");

            userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Enter ship placement information");
                return false;
            }

            var shipPlacementInfoArray = userInput.Split(",");
            if (shipPlacementInfoArray.Length != 3)
            {
                Console.WriteLine("Add comma to separate information");
                return false;
            }

            if (shipPlacementInfoArray[0].ToUpper() != "V" && shipPlacementInfoArray[0].ToUpper() != "H")
            {
                Console.WriteLine("Ship possition can only be V and H");
                return false;
            }

            if (int.TryParse(shipPlacementInfoArray[1], out int rowCoordinate) &&
                int.TryParse(shipPlacementInfoArray[2], out int columnCoordinate))
            {
                if ((rowCoordinate > 10 || rowCoordinate < 1) &&
                    (columnCoordinate > 10 || columnCoordinate < 1))
                {
                    Console.WriteLine("Coordinate value should be between 1 and 10");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid coordinates");
                return false;
            }

            return true;
        }
    }
}
