using System;
using System.Collections.Generic;

namespace BattleShip
{
    public class Game
    {
        private Player _player { get; set; }
        private Player _opponent { get; set; }
        public void StartGame()
        {
            var startGame = Dialogs.StartGameDialog(out string name);
            if (!startGame)
            {
                Dialogs.ExitGame();
                return;
            }

            Console.Clear();

            _player = new Player() { Name = name };
            _opponent = new Player();

            PrepareGame();
            StartBattle();
        }

        private void StartBattle()
        {
            bool shipLocated = false;

            Coordinate successfulAttackCoordinates = new Coordinate();
            Coordinate plannedCoordinates = new Coordinate();
            bool? verticalAttackedShip = null;

            while (true)
            {
                bool successfulAttack;
                do
                {
                    Console.Clear();
                    Dialogs.DisplayBoard(_player.PlayerBoard);
                    Dialogs.DisplayBoard(_player.OpponentBoard);
                    successfulAttack = PlayerAttack();
                }
                while (successfulAttack);

                do
                {
                    if (shipLocated)
                    {
                        plannedCoordinates = PlanCoordinates(successfulAttackCoordinates, verticalAttackedShip);
                    }

                    successfulAttack = OpponentAttack(plannedCoordinates, out Coordinate attackCoordinates);

                    if (successfulAttack)
                    {
                        successfulAttackCoordinates = attackCoordinates;
                        shipLocated = CheckShipState(attackCoordinates);
                        verticalAttackedShip = CheckShipPossition(attackCoordinates);

                        if (!shipLocated)
                        {
                            plannedCoordinates = null;
                        }
                    }
                }
                while (successfulAttack);
            }
        }

        private bool? CheckShipPossition(Coordinate attackCoordinates)
        {
            var rowCheckStartIndex = -1;
            var columnCheckStartIndex = -1;

            var rowCheckEndIndex = 1;
            var columnCheckEndIndex = 1;

            if (attackCoordinates.Row == 0)
            {
                rowCheckStartIndex = 0;
            }
            else if (attackCoordinates.Row == 9)
            {
                rowCheckEndIndex = 0;
            }

            if (attackCoordinates.Column == 0)
            {
                columnCheckStartIndex = 0;
            }
            else if (attackCoordinates.Column == 9)
            {
                columnCheckEndIndex = 0;
            }

            for (int i = rowCheckStartIndex; i <= rowCheckEndIndex; i++)
            {
                for (int j = columnCheckStartIndex; j <= columnCheckEndIndex; j++)
                {
                    if (i == -1 && j == 0)
                    {
                        if (_player.PlayerBoard[attackCoordinates.Row + i][attackCoordinates.Column + j] == "X")
                        {
                            return true;
                        }
                    }
                    if (i == 0 && j == -1)
                    {
                        if (_player.PlayerBoard[attackCoordinates.Row + i][attackCoordinates.Column + j] == "X")
                        {
                            return false;
                        }

                    }
                    if (i == 0 && j == 1)
                    {
                        if (_player.PlayerBoard[attackCoordinates.Row + i][attackCoordinates.Column + j] == "X")
                        {
                            return false;
                        }
                    }
                    if (i == 1 && j == 0)
                    {
                        if (_player.PlayerBoard[attackCoordinates.Row + i][attackCoordinates.Column + j] == "X")
                        {
                            return true;
                        }
                    }
                }
            }

            return null;
        }

        private bool CheckShipState(Coordinate attackCoordinates)
        {
            //Starts from -1 because need to check left side
            var rowCheckStartIndex = -1;
            var columnCheckStartIndex = -1;

            var rowCheckEndIndex = 1;
            var columnCheckEndIndex = 1;

            if (attackCoordinates.Row == 0)
            {
                rowCheckStartIndex = 0;
            }
            else if (attackCoordinates.Row == 9)
            {
                rowCheckEndIndex = 0;
            }

            if (attackCoordinates.Column == 0)
            {
                columnCheckStartIndex = 0;
            }
            else if (attackCoordinates.Column == 9)
            {
                columnCheckEndIndex = 0;
            }

            for (int i = rowCheckStartIndex; i <= rowCheckEndIndex; i++)
            {
                for (int j = columnCheckStartIndex; j <= columnCheckEndIndex; j++)
                {
                    if (_player.PlayerBoard[attackCoordinates.Row + i][attackCoordinates.Column + j] == "O")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Coordinate PlanCoordinates(Coordinate successfulAttackCoordinates, bool? verticalAttackedShip)
        {
            var rowCheckStartIndex = -1;
            var columnCheckStartIndex = -1;

            var rowCheckEndIndex = 1;
            var columnCheckEndIndex = 1;

            if (successfulAttackCoordinates.Row == 0)
            {
                rowCheckStartIndex = 0;
            }
            else if (successfulAttackCoordinates.Row == 9)
            {
                rowCheckEndIndex = 0;
            }

            if (successfulAttackCoordinates.Column == 0)
            {
                columnCheckStartIndex = 0;
            }
            else if (successfulAttackCoordinates.Column == 9)
            {
                columnCheckEndIndex = 0;
            }

            for (int i = rowCheckStartIndex; i <= rowCheckEndIndex; i++)
            {
                for (int j = columnCheckStartIndex; j <= columnCheckEndIndex; j++)
                {
                    if ((verticalAttackedShip == true || verticalAttackedShip == null) && i == -1 && j == 0)
                    {
                        if (string.IsNullOrWhiteSpace(_player.PlayerBoard[successfulAttackCoordinates.Row + i][successfulAttackCoordinates.Column + j]) ||
                            _player.PlayerBoard[successfulAttackCoordinates.Row + i][successfulAttackCoordinates.Column + j] == "O")
                        {
                            return new Coordinate() { Row = successfulAttackCoordinates.Row + i, Column = successfulAttackCoordinates.Column + j };
                        }
                    }
                    if ((verticalAttackedShip == false || verticalAttackedShip == null) && i == 0 && j == -1)
                    {
                        if (string.IsNullOrWhiteSpace(_player.PlayerBoard[successfulAttackCoordinates.Row + i][successfulAttackCoordinates.Column + j]) ||
                            _player.PlayerBoard[successfulAttackCoordinates.Row + i][successfulAttackCoordinates.Column + j] == "O")
                        {
                            return new Coordinate() { Row = successfulAttackCoordinates.Row + i, Column = successfulAttackCoordinates.Column + j };
                        }

                    }
                    if ((verticalAttackedShip == false || verticalAttackedShip == null) && i == 0 && j == 1)
                    {
                        if (string.IsNullOrWhiteSpace(_player.PlayerBoard[successfulAttackCoordinates.Row + i][successfulAttackCoordinates.Column + j]) ||
                            _player.PlayerBoard[successfulAttackCoordinates.Row + i][successfulAttackCoordinates.Column + j] == "O")
                        {
                            return new Coordinate() { Row = successfulAttackCoordinates.Row + i, Column = successfulAttackCoordinates.Column + j };
                        }
                    }
                    if ((verticalAttackedShip == true || verticalAttackedShip == null) && i == 1 && j == 0)
                    {
                        if (string.IsNullOrWhiteSpace(_player.PlayerBoard[successfulAttackCoordinates.Row + i][successfulAttackCoordinates.Column + j]) ||
                            _player.PlayerBoard[successfulAttackCoordinates.Row + i][successfulAttackCoordinates.Column + j] == "O")
                        {
                            return new Coordinate() { Row = successfulAttackCoordinates.Row + i, Column = successfulAttackCoordinates.Column + j };
                        }
                    }
                }
            }

            return null;
        }

        private bool PlayerAttack()
        {
            bool attackCoordinatesCollected;
            Coordinate attackCoordinates = new Coordinate();
            do
            {
                attackCoordinatesCollected = Dialogs.BattleDialog(ref attackCoordinates);
            }
            while (!attackCoordinatesCollected);
            return AttackShip(_opponent.PlayerBoard, _player.OpponentBoard, attackCoordinates);
        }

        private bool OpponentAttack(Coordinate planedAttackCoordinates, out Coordinate attackCoordinates)
        {
            attackCoordinates = new Coordinate();
            if (planedAttackCoordinates == null)
            {
                do
                {
                    var random = new Random();
                    attackCoordinates.Row = random.Next(0, 10);
                    attackCoordinates.Column = random.Next(0, 10);

                    if (string.IsNullOrWhiteSpace(_player.PlayerBoard[attackCoordinates.Row][attackCoordinates.Column]) || _player.PlayerBoard[attackCoordinates.Row][attackCoordinates.Column] == "O")
                    {
                        break;
                    }
                }
                while (true);
            }
            else
            {
                attackCoordinates = planedAttackCoordinates;
            }

            return AttackShip(_player.PlayerBoard, _opponent.OpponentBoard, attackCoordinates);
        }

        private bool AttackShip(List<List<string>> boardToAttack,
            List<List<string>> boardToMarkAttacks,
            Coordinate attackCoordinates)
        {
            var attackedCell = boardToAttack[attackCoordinates.Row][attackCoordinates.Column];

            boardToMarkAttacks[attackCoordinates.Row][attackCoordinates.Column] = attackedCell == "O" ? "X" : "*";
            boardToAttack[attackCoordinates.Row][attackCoordinates.Column] = attackedCell == "O" ? "X" : "*";

            return attackedCell == "O";
        }

        private void PrepareGame()
        {
            PreparePlayerShips();
            PrepareOpponentShips();
            Console.Clear();
        }

        private void PreparePlayerShips()
        {
            Dialogs.DisplayBoard(_player.PlayerBoard);
            foreach (var ship in _player.PlayerShips)
            {
                do
                {
                    var shipPlacementInfoCollected = false;
                    var shipPlacementInfo = string.Empty;
                    while (!shipPlacementInfoCollected)
                    {
                        shipPlacementInfoCollected = Dialogs.PlaceShip(ref shipPlacementInfo);
                    }

                    var shipPlacementInfoArray = shipPlacementInfo.Split(",");

                    var available = CheckAvailability(
                             _player.PlayerBoard,
                             shipPlacementInfoArray[0].ToUpper() == "V" ? true : false,
                             int.Parse(shipPlacementInfoArray[1]) - 1, // -1 because user imput starts from 1 not from 0
                             int.Parse(shipPlacementInfoArray[2]) - 1,
                             ship.Size,
                             out List<Coordinate> coordinates);

                    if (available)
                    {
                        PlaceShip(
                            _player.PlayerBoard,
                            coordinates);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("You can not place ship here");
                    }
                }
                while (true);

                Console.Clear();
                Dialogs.DisplayBoard(_player.PlayerBoard);
            }
        }

        private void PrepareOpponentShips()
        {
            var random = new Random();
            foreach (var ship in _opponent.PlayerShips)
            {
                while (true)
                {
                    bool vertical = random.Next(0, 2) == 1 ? true : false;
                    int rowToStart = random.Next(0, 10);
                    int columnToStart = random.Next(0, 10);

                    var available = CheckAvailability(
                        _opponent.PlayerBoard,
                        vertical,
                        rowToStart,
                        columnToStart,
                        ship.Size,
                        out List<Coordinate> coordinates);

                    if (available)
                    {
                        PlaceShip(
                            _opponent.PlayerBoard,
                            coordinates);
                        break;
                    }
                }
            }
        }

        private bool CheckAvailability(
            List<List<string>> board,
            bool vertical,
            int rowToStart,
            int columnToStart,
            int size,
            out List<Coordinate> coordinates)
        {
            coordinates = new List<Coordinate>();
            for (int i = 0; i < size; i++)
            {
                int row = rowToStart + (vertical ? i : 0);
                int column = columnToStart + (vertical ? 0 : i);

                if (!ShipPlacementValidation(board, row, column))
                {
                    return false;
                }

                coordinates.Add(new Coordinate()
                {
                    Row = row,
                    Column = column
                });
            }

            return true;
        }

        private bool ShipPlacementValidation(List<List<string>> board, int row, int column)
        {
            //Check that ship would not be placed outside the board
            if (row >= 10 || column >= 10)
            {
                return false;
            }

            //Starts from -1 because need to check left side
            var rowCheckStartIndex = -1;
            var columnCheckStartIndex = -1;

            var rowCheckEndIndex = 1;
            var columnCheckEndIndex = 1;

            if (row == 0)
            {
                rowCheckStartIndex = 0;
            }
            else if (row == 9)
            {
                rowCheckEndIndex = 0;
            }

            if (column == 0)
            {
                columnCheckStartIndex = 0;
            }
            else if (column == 9)
            {
                columnCheckEndIndex = 0;
            }

            for (int i = rowCheckStartIndex; i <= rowCheckEndIndex; i++)
            {
                for (int j = columnCheckStartIndex; j <= columnCheckEndIndex; j++)
                {
                    if (!string.IsNullOrWhiteSpace(board[row + i][column + j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void PlaceShip(List<List<string>> board, List<Coordinate> coordinates)
        {
            foreach (var coordinate in coordinates)
            {
                board[coordinate.Row][coordinate.Column] = "O";
            }
        }
    }
}
