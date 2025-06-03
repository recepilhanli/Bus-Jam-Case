using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    [Serializable]
    public class GridCell
    {
        private static readonly Vector2Int[] _directions = {
                    new Vector2Int(1, 0), // Right
                    new Vector2Int(-1, 0), // Left
                    new Vector2Int(0, 1), // Up
                    new Vector2Int(0, -1) // Down
                };

        public bool isPrimaryCell = false;
        private Vector2Int _position;
        private Passenger _passenger;

        public Vector2Int position { get => _position; }
        public Passenger passenger { get => _passenger; }
        public bool isEmpty => _passenger == null;
        public GridCell[,] grid =>
            isPrimaryCell ? GridManager.instance.primaryGrid : GridManager.instance.secondaryGrid;

        public bool hasSpaceToMove
        {
            get
            {
                //check neighbors
                foreach (var direction in _directions)
                {
                    Vector2Int neighborPosition = _position + direction;
                    if (IsValidPosition(neighborPosition) && grid[neighborPosition.x, neighborPosition.y].isEmpty)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

      


        private GridCell() { } // Private constructor to prevent instantiation without parameters

        public GridCell(Vector2Int position, Passenger passenger = null)
        {
            _position = position;
            _passenger = passenger;
        }

        public bool IsValidPosition(Vector2Int position)
        {
            if (isPrimaryCell)
            {
                return position.x >= 0 && position.x < GridManager.instance.primaryWidth &&
                       position.y >= 0 && position.y < GridManager.instance.primaryHeight;
            }
            else
            {
                return position.x >= 0 && position.x < GridManager.instance.secondaryWidth &&
                       position.y >= 0 && position.y < GridManager.instance.secondaryHeight;
            }
        }

    }

}