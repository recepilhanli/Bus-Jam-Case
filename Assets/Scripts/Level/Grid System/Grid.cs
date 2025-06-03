using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace Game.Level
{

    public class Grid : MonoBehaviour
    {
        public GridCell[,] cells;

        public float cellSize = .5f; //uniform
        public Vector2 padding = Vector2.zero;
        public Vector2 spacing = Vector2.zero;
        public int width = 10;
        public int height = 10;

        private bool _wasInitialized = false;

        public bool wasInitialized => _wasInitialized;
        public Vector3 origin => transform.position + new Vector3(padding.x, 0, padding.y);


        private void Awake()
        {
            Init(width, height);
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

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(origin, 0.1f);

            Gizmos.color = Color.green;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 cellPosition = GetCellWorldPosition(new Vector2Int(x, y));
                    Gizmos.DrawWireCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
                }
            }
        }
    }
#endif

}