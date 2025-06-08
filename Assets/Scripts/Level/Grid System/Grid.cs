using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Data;
using UnityEngine;


namespace Game.Level
{

    public class Grid : MonoBehaviour
    {
        public static readonly Vector2Int[] directions = {
                    new Vector2Int(0, 1), // Up
                    new Vector2Int(0, -1), // Down
                    new Vector2Int(1, 0), // Right
                    new Vector2Int(-1, 0) // Left
                };

        public GridCell[,] cells;


        public float cellSize = .5f; //uniform
        public Vector2 padding = Vector2.zero;
        public Vector2 spacing = Vector2.zero;
        public int width = 10;
        public int height = 10;
        [SerializeField] private bool initOnAwake = false;

        private bool _wasInitialized = false;

        public bool wasInitialized => _wasInitialized;
        public bool hasSpace => GetEmptyCell() != null;
        public Vector3 origin => transform.position + new Vector3(padding.x, 0, padding.y);

        private void Awake()
        {
            if (initOnAwake) Init(width, height);
        }

        public bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < width &&
                   position.y >= 0 && position.y < height;
        }

        public void Init(int width, int height)
        {
            cells = new GridCell[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = GridCell.GetFromPool(new Vector2Int(x, y), this);
                }
            }
            _wasInitialized = true;
        }


        public void Init(GridData data)
        {
            if (data == null)
            {
                Debug.LogError("GridData is null. Cannot initialize grid.");
                return;
            }

            width = data.gridSize.x;
            height = data.gridSize.y;
            cellSize = data.cellSize;
            padding = data.padding;
            spacing = data.spacing;

            Init(width, height);
        }





        public Vector3 GetCellWorldPosition(in Vector2Int position)
        {
            if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height)
            {
                Debug.LogError("Invalid cell position: " + position);
                return Vector3.zero;
            }

            Vector3 worldPosition = origin + new Vector3(position.x * (cellSize + spacing.x), 0, position.y * (cellSize + spacing.y));
            return worldPosition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 GetCellWorldPosition(GridCell cell) => GetCellWorldPosition(cell.position);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GridCell GetEmptyCell()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (cells[x, y].isEmpty)
                    {
                        return cells[x, y];
                    }
                }
            }
            return null;
        }


        public static implicit operator GridCell[,](Grid grid)
        {
            return grid.cells;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, $"Grid ({width}x{height})");
            Gizmos.color = Color.red;
            Vector3 firstColumn = GetCellWorldPosition(new Vector2Int(0, 0));
            Vector3 lastColumn = GetCellWorldPosition(new Vector2Int(width - 1, 0));
            Vector3 firstRow = GetCellWorldPosition(new Vector2Int(0, 0));
            Vector3 lastRow = GetCellWorldPosition(new Vector2Int(0, height - 1));
            Gizmos.DrawLine(firstColumn, lastColumn);
            Gizmos.DrawLine(firstRow, lastRow);
        }
#endif
    }
}