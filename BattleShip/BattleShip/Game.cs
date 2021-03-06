using System;
using System.Collections.Generic;
using System.Linq;

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
                    Dialogs.DisplayAllPlayerBoards(_player);
                    successfulAttack = PlayerAttack();

                    if (successfulAttack)
                    {
                        UpdateShipsState(_opponent.PlayerBoard, _opponent.PlayerShips);
                        if (CheckForGameEnd(_opponent.PlayerShips))
                        {
                            Dialogs.DisplayAllPlayerBoards(_player);
                            return;
                        }
                    }
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
                        UpdateShipsState(_player.PlayerBoard, _player.PlayerShips);
                        if (CheckForGameEnd(_player.PlayerShips))
                        {
                            Dialogs.DisplayAllPlayerBoards(_player);
                            return;
                        }

                        if (!shipLocated)
                        {
                            plannedCoordinates = null;
                        }
                    }
                }
                while (successfulAttack);
            }
        }

        private bool CheckForGameEnd(List<Ship> playerShips)
        {
            return !playerShips.Any(x => x.Alive == true);
        }

        private void UpdateShipsState(List<List<string>> board, List<Ship> playerShips)
        {
            foreach (var ship in playerShips)
            {
                int damagedPartsCount = 0;
                foreach (var coordinates in ship.Coordinates)
                {
                    if (board[coordinates.Row][coordinates.Column] == "X")
                    {
                        damagedPartsCount++;
                    }

                    if (damagedPartsCount == ship.Size)
                    {
                        ship.Alive = false;
                    }
                }
            }
        }

        private bool? CheckShipPossition(Coordinate attackCoordinates)
        {
            BoardIndex boardIndex = GetIndexes(attackCoordinates.Row, attackCoordinates.Column);

            for (int i = boardIndex.RowCheckStartIndex; i <= boardIndex.RowCheckEndIndex; i++)
            {
                for (int j = boardIndex.ColumnCheckStartIndex; j <= boardIndex.ColumnCheckEndIndex; j++)
                {
                    if (_player.PlayerBoard[attackCoordinates.Row + i][attackCoordinates.Column + j] != "X")
                    {
                        continue;
                    }

                    if (i == -1 && j == 0)
                    {
                        return true;
                    }
                    if (i == 0 && j == -1)
                    {
                        return false;
                    }
                    if (i == 0 && j == 1)
                    {
                        return false;
                    }
                    if (i == 1 && j == 0)
                    {
                        return true;
                    }
                }
            }

            return null;
        }

        private bool CheckShipState(Coordinate attackCoordinates)
        {
            BoardIndex boardIndex = GetIndexes(attackCoordinates.Row, attackCoordinates.Column);

            for (int i = boardIndex.RowCheckStartIndex; i <= boardIndex.RowCheckEndIndex; i++)
            {
                for (int j = boardIndex.ColumnCheckStartIndex; j <= boardIndex.ColumnCheckEndIndex; j++)
                {
                    if (_player.PlayerBoard[attackCoordinates.Row + i][attackCoordinates.Column + j] == "O")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckForNearShips(Coordinate attackCoordinates)
        {
            BoardIndex boardIndex = GetIndexes(attackCoordinates.Row, attackCoordinates.Column);

            for (int i = boardIndex.RowCheckStartIndex; i <= boardIndex.RowCheckEndIndex; i++)
            {
                for (int j = boardIndex.ColumnCheckStartIndex; j <= boardIndex.ColumnCheckEndIndex; j++)
                {
                    if (_player.PlayerBoard[attackCoordinates.Row + i][attackCoordinates.Column + j] == "X")
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private Coordinate PlanCoordinates(Coordinate successfulAttackCoordinates, bool? verticalAttackedShip)
        {
            BoardIndex boardIndex = GetIndexes(successfulAttackCoordinates.Row, successfulAttackCoordinates.Column);

            for (int i = boardIndex.RowCheckStartIndex; i <= boardIndex.RowCheckEndIndex; i++)
            {
                for (int j = boardIndex.ColumnCheckStartIndex; j <= boardIndex.ColumnCheckEndIndex; j++)
                {
                    var boardCell = _player.PlayerBoard[successfulAttackCoordinates.Row + i][successfulAttackCoordinates.Column + j];

                    if (!string.IsNullOrWhiteSpace(boardCell) && boardCell != "O")
                    {
                        continue;
                    }

                    if ((verticalAttackedShip == true || verticalAttackedShip == null) && i == -1 && j == 0)
                    {
                        return new Coordinate() { Row = successfulAttackCoordinates.Row + i, Column = successfulAttackCoordinates.Column + j };
                    }
                    if ((verticalAttackedShip == false || verticalAttackedShip == null) && i == 0 && j == -1)
                    {
                        return new Coordinate() { Row = successfulAttackCoordinates.Row + i, Column = successfulAttackCoordinates.Column + j };
                    }
                    if ((verticalAttackedShip == false || verticalAttackedShip == null) && i == 0 && j == 1)
                    {
                        return new Coordinate() { Row = successfulAttackCoordinates.Row + i, Column = successfulAttackCoordinates.Column + j };
                    }
                    if ((verticalAttackedShip == true || verticalAttackedShip == null) && i == 1 && j == 0)
                    {
                        return new Coordinate() { Row = successfulAttackCoordinates.Row + i, Column = successfulAttackCoordinates.Column + j };
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

                    var boardCell = _player.PlayerBoard[attackCoordinates.Row][attackCoordinates.Column];

                    if (string.IsNullOrWhiteSpace(boardCell) && CheckForNearShips(attackCoordinates))
                    {
                        break;
                    }

                    if (boardCell == "O")
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

        private bool AttackShip(List<List<string>> boardToAttack, List<List<string>> boardToMarkAttacks, Coordinate attackCoordinates)
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
                             int.Parse(shipPlacementInfoArray[1]) - 1,
                             int.Parse(shipPlacementInfoArray[2]) - 1,
                             ship.Size,
                             out List<Coordinate> coordinates);

                    if (available)
                    {
                        PlaceShip(_player.PlayerBoard, ship, coordinates);
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

                    var available = CheckAvailability(_opponent.PlayerBoard, vertical, rowToStart, columnToStart, ship.Size, out List<Coordinate> coordinates);

                    if (available)
                    {
                        PlaceShip(_opponent.PlayerBoard, ship, coordinates);
                        break;
                    }
                }
            }
        }

        private bool CheckAvailability(List<List<string>> board, bool vertical, int rowToStart, int columnToStart, int size, out List<Coordinate> coordinates)
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

                coordinates.Add(new Coordinate() { Row = row, Column = column });
            }

            return true;
        }

        private bool ShipPlacementValidation(List<List<string>> board, int row, int column)
        {
            if (row >= 10 || column >= 10)
            {
                return false;
            }

            BoardIndex boardIndex = GetIndexes(row, column);

            for (int i = boardIndex.RowCheckStartIndex; i <= boardIndex.RowCheckEndIndex; i++)
            {
                for (int j = boardIndex.ColumnCheckStartIndex; j <= boardIndex.ColumnCheckEndIndex; j++)
                {
                    if (!string.IsNullOrWhiteSpace(board[row + i][column + j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private BoardIndex GetIndexes(int row, int column)
        {
            BoardIndex boardIndex = new BoardIndex();

            if (row == 0)
            {
                boardIndex.RowCheckStartIndex = 0;
            }
            else if (row == 9)
            {
                boardIndex.RowCheckEndIndex = 0;
            }

            if (column == 0)
            {
                boardIndex.ColumnCheckStartIndex = 0;
            }
            else if (column == 9)
            {
                boardIndex.ColumnCheckEndIndex = 0;
            }

            return boardIndex;
        }

        private void PlaceShip(List<List<string>> board, Ship ship, List<Coordinate> coordinates)
        {
            ship.Coordinates = new List<Coordinate>();
            ship.Coordinates.AddRange(coordinates);

            foreach (var coordinate in coordinates)
            {
                board[coordinate.Row][coordinate.Column] = "O";
            }
        }
    }
}
