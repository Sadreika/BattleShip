using System.Collections.Generic;

namespace BattleShip
{
    public class Player
    {
        public string Name { get; set; }
        public List<List<string>> PlayerBoard { get; set; }
        public List<List<string>> OpponentBoard { get; set;}
        public List<Ship> PlayerShips { get; set; }

        public Player()
        {
            PlayerBoard = CreateBoard();
            OpponentBoard = CreateBoard();
            PlayerShips = new List<Ship>()
            {
                new Ship() { Size = 4},
                new Ship() { Size = 3},
                new Ship() { Size = 3},
                new Ship() { Size = 2},
                new Ship() { Size = 2},
                new Ship() { Size = 2}
            };
        }

        private List<List<string>> CreateBoard()
        {
            return new List<List<string>>()
            {
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                new List<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
            };
        }
    }
}
