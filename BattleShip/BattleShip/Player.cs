using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip
{
    public class Player
    {
        public string Name { get; set; }
        public List<List<string>> PlayerBoard { get; set; }
        public List<List<string>> OpponentBoard { get; set;}
        public List<Ship> PlayerShips { get; set; }
    }
}
