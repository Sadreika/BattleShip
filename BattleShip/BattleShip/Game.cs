using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip
{
    public class Game
    {
        public void StartGame()
        {
            var startGame = Dialogs.StartGameDialog(out string name);
            if(!startGame)
            {
                Dialogs.ExitGame();
                return;
            }
        }
    }
}
