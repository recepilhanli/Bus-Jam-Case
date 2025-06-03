using System;
using System.Collections;
using System.Collections.Generic;
using Game.Level.Pooling;
using Game.Utils;
using UnityEngine;

namespace Game.Level
{

    public sealed class GridCell : MonoCached<GridCell>, IPoolable
    {
        private static readonly Vector2Int[] _directions = {
                    new Vector2Int(1, 0), // Right
                    new Vector2Int(-1, 0), // Left
                    new Vector2Int(0, 1), // Up
                    new Vector2Int(0, -1) // Down
                };

        public bool isObstacle = false;
        public bool isInPrimaryGrid = false;
        public Collider cellCollider;

        [Header("Read-Only Values")]
        [SerializeField] private Vector2Int _position;
        [SerializeField] private Passenger _passenger;

        public Vector2Int position { get => _position; }
        public Passenger passenger { get => _passenger; }
        public bool isEmpty => _passenger == null;

        public Grid attachedGrid => isInPrimaryGrid ? GameManager.instance.primaryGrid : GameManager.instance.secondaryGrid;
        public Vector3 worldPosition => attachedGrid.GetCellWorldPosition(_position);

        public void SetPassenger(Passenger passenger)
        {
            Debug.Assert(_passenger == null, $"GridCell at {_position} already has a passenger!");
            _passenger = passenger;
            _passenger.attachedCell = this;
            cellCollider.enabled = true; // Enable collider when a passenger is present
        }

        public Passenger RemovePassenger()
        {
            var oldPassenger = _passenger;

            if (oldPassenger != null)
            {
                oldPassenger.attachedCell = null;
                _passenger = null;
            }
            cellCollider.enabled = false; // Disable collider when no passenger is present
            return oldPassenger;
        }

        public bool hasSpaceToMove
        {
            get
            {
                //check neighbors
                foreach (var direction in _directions)
                {
                    Vector2Int neighborPosition = _position + direction;
                    GridCell[,] cells = attachedGrid;
                    if ((IsValidPosition(neighborPosition) && cells[neighborPosition.x, neighborPosition.y].isEmpty) ||
                      position.y == attachedGrid.width - 1)
                    {
                        // If the neighbor cell is empty or if this is the last row, we can move
                        return true;
                    }
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < attachedGrid.width &&
                   position.y >= 0 && position.y < attachedGrid.height;
        }


        #region  Pooling

        public void OnSpawn(in Vector3 position, in Quaternion rotation = default)
        {
            gameObject.SetActive(true);
            transform.position = position;
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);
            _passenger = null;
            _position = Vector2Int.zero;
            isObstacle = false;
        }

        public void ReturnToPool() => PoolManager.GetPool(PoolTypes.GridCell).ReturnToPool(this);

        public static GridCell GetFromPool(Vector2Int position, Grid parent)
        {
            bool isInPrimaryGrid = parent == GameManager.instance.primaryGrid;
            Grid grid = isInPrimaryGrid ? GameManager.instance.primaryGrid : GameManager.instance.secondaryGrid;
            Vector3 worldPosition = grid.GetCellWorldPosition(position);

            GridCell cell = PoolManager.GetObject<GridCell>(PoolTypes.GridCell, in worldPosition);
            Debug.Assert(cell != null, "Failed to get GridCell from pool.");
            cell._position = position;
            cell.transform.localScale = new Vector3(grid.cellSize, 1, grid.cellSize);
            cell.isInPrimaryGrid = isInPrimaryGrid;

            if (!isInPrimaryGrid) Passenger.GetFromPool(cell, ColorHelper.GetRandomColor());
            return cell;
        }
        #endregion
    }

}