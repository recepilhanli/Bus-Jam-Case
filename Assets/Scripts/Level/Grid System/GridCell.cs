using System;
using System.Collections;
using System.Collections.Generic;
using Game.Level.Pooling;
using Game.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Level
{

    public sealed class GridCell : MonoCached<GridCell>, IPoolable
    {
        private static Vector2Int[] _directions => Grid.directions;

        public Collider cellCollider;
        public Renderer cellRenderer;
        public NavMeshObstacle navMeshObstacle;
        public GameObject cellObstacle; // Used for visual representation of obstacles


        [Header("Read-Only Values")]
        [SerializeField] private Vector2Int _position;
        [SerializeField] private Passenger _passenger;
        [SerializeField] private bool _isObstacle = false;
        [SerializeField] private bool _isInPrimaryGrid = false;

        public Vector2Int position { get => _position; }
        public Passenger passenger { get => _passenger; }
        public bool isEmpty => _passenger == null || _isObstacle;

        public Grid attachedGrid => isInPrimaryGrid ? GameManager.instance.primaryGrid : GameManager.instance.secondaryGrid;
        public Vector3 worldPosition => attachedGrid.GetCellWorldPosition(_position);

        public bool navMeshObstacleEnabled
        {
            get => navMeshObstacle.enabled;
            set => navMeshObstacle.enabled = value;
        }

        public bool isObstacle
        {
            get => _isObstacle;
            set
            {
                if (_isObstacle == value) return;
                cellCollider.enabled = !value;
                cellRenderer.enabled = !value;

                cellObstacle.SetActive(value);
                _isObstacle = value;
            }
        }

        public bool isInPrimaryGrid
        {
            get => _isInPrimaryGrid;
            set
            {
                if (_isInPrimaryGrid == value) return;
                _isInPrimaryGrid = value;
                navMeshObstacleEnabled = !value;
            }
        }

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
                if (position.y == attachedGrid.height - 1) return true; // If it's the last row, it can always move up

                //check neighbors
                foreach (var direction in _directions)
                {
                    Vector2Int neighborPosition = _position + direction;
                    GridCell[,] cells = attachedGrid;

                    if (attachedGrid.IsValidPosition(neighborPosition) && cells[neighborPosition.x, neighborPosition.y].isEmpty) return true;
                }

                return false;
            }
        }



        #region  Pooling

        public void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);
            _passenger = null;
            _position = Vector2Int.zero;
            isObstacle = false;
            navMeshObstacleEnabled = true;
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
            cell.isInPrimaryGrid = isInPrimaryGrid;

            cell.transform.position = worldPosition;
            cell.transform.localScale = new Vector3(grid.cellSize, 1, grid.cellSize);
            return cell;
        }
        #endregion
    }

}