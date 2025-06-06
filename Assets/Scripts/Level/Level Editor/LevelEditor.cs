using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Level;
using UnityEngine;


#if UNITY_EDITOR
namespace Game.OnlyEditor
{
    using CellTypes = EditorCellType;
    [ExecuteAlways]
    public class LevelEditor : MonoSingleton<LevelEditor>
    {
        public static event Action onLevelContainerUpdated;
        public static event Action onCellsRefreshed;
        public static Action<EditorGridCell> onEditorCellUpdated;

        [SerializeField] private LevelContainer _selectedLevelContainer;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Transform _cellParent;

        private EditorGridCell[,] _primaryCells = new EditorGridCell[0, 0];
        private EditorGridCell[,] _secondaryCells = new EditorGridCell[0, 0];

        [Space]
        [SerializeField] private EditorGridCell _cellPrefab;
        [SerializeField] private Mesh _passengerMesh;

        public bool isLoaded = false;

        public EditorGridCell[,] primaryCells => _primaryCells;
        public EditorGridCell[,] secondaryCells => _secondaryCells;


        public GameManager gameManager => _gameManager;

        public LevelContainer selectedLevelContainer
        {
            get => _selectedLevelContainer;
            set
            {
                _selectedLevelContainer = value;
                RefreshGrids();
                onLevelContainerUpdated?.Invoke();
                UnityEditor.Selection.activeObject = _selectedLevelContainer;
            }
        }


        public Mesh PassengerMesh
        {
            get => _passengerMesh;
            set => _passengerMesh = value;
        }

        private void OnEnable()
        {
            if (SceneHelper.isGameScene || SceneHelper.isHomeScene)
            {
                Debug.LogError("Level Editor should not be used in game or home scenes. Disabling the component.");
                return;
            }

            FindType();

            gameObject.hideFlags = HideFlags.NotEditable;
            _gameManager.gameObject.hideFlags = HideFlags.NotEditable;
            _cellParent.gameObject.hideFlags = HideFlags.NotEditable;

            EditorGridCell[] oldCells = _cellParent.GetComponentsInChildren<EditorGridCell>();
            foreach (var cell in oldCells)
            {
                if (cell != null)
                {
                    DestroyImmediate(cell.gameObject);
                }
            }

            if (Application.isPlaying) LevelLoader.InitPersistent(_selectedLevelContainer);
            else if (_selectedLevelContainer != null) RefreshGrids();

            isLoaded = true;
        }


        private void OnDisable()
        {
            isLoaded = false;
            ClearAllCells();
        }

        public void RefreshGrids()
        {

            Debug.Assert(_cellPrefab != null, "Cell prefab is not assigned in the Level Editor.");
            Debug.Assert(_gameManager != null, "Game Manager is not assigned in the Level Editor.");

            ClearAllCells();

            if (_selectedLevelContainer == null) return;

            int pX = _selectedLevelContainer.primaryGrid.gridSize.x;
            int pY = _selectedLevelContainer.primaryGrid.gridSize.y;
            _gameManager.primaryGrid.width = pX;
            _gameManager.primaryGrid.height = pY;
            _gameManager.primaryGrid.cellSize = _selectedLevelContainer.primaryGrid.cellSize;
            _gameManager.primaryGrid.padding = _selectedLevelContainer.primaryGrid.padding;
            _gameManager.primaryGrid.spacing = _selectedLevelContainer.primaryGrid.spacing;
            _primaryCells = new EditorGridCell[pX, pY];

            for (int x = 0; x < pX; x++)
            {
                for (int y = 0; y < pY; y++)
                {
                    EditorGridCell newCell = Instantiate(_cellPrefab, _gameManager.primaryGrid.GetCellWorldPosition(new Vector2Int(x, y)), Quaternion.identity);
                    newCell.transform.SetParent(_cellParent, true);
                    newCell.cellType = CellTypes.Primary;
                    newCell.position = new Vector2Int(x, y);
                    _primaryCells[x, y] = newCell;
                }
            }


            int sX = _selectedLevelContainer.secondaryGrid.gridSize.x;
            int sY = _selectedLevelContainer.secondaryGrid.gridSize.y;
            _gameManager.secondaryGrid.width = sX;
            _gameManager.secondaryGrid.height = sY;
            _gameManager.secondaryGrid.cellSize = _selectedLevelContainer.secondaryGrid.cellSize;
            _gameManager.secondaryGrid.padding = _selectedLevelContainer.secondaryGrid.padding;
            _gameManager.secondaryGrid.spacing = _selectedLevelContainer.secondaryGrid.spacing;
            _secondaryCells = new EditorGridCell[sX, sY];

            for (int x = 0; x < sX; x++)
            {
                for (int y = 0; y < sY; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    EditorGridCell newCell = Instantiate(_cellPrefab, _gameManager.secondaryGrid.GetCellWorldPosition(position), Quaternion.identity);
                    newCell.transform.SetParent(_cellParent, true);
                    newCell.cellType = FindSecondaryCellType(position);
                    newCell.position = position;
                    _secondaryCells[x, y] = newCell;
                }
            }

            onCellsRefreshed?.Invoke();
        }


        private CellTypes FindSecondaryCellType(in Vector2Int position)
        {
            if (_selectedLevelContainer == null)
            {
                Debug.LogError("Selected level container is null. Cannot find secondary cell type.");
                return CellTypes.Empty;
            }

            var passengers = _selectedLevelContainer.secondaryGrid.passengers;

            if (passengers != null)
            {

                foreach (var passenger in passengers)
                {
                    if (passenger.gridPosition == position)
                    {
                        return CellTypes.HasPassenger;
                    }
                }
            }

            var obstacles = _selectedLevelContainer.secondaryGrid.obstacles;

            if (obstacles != null)
            {
                foreach (var obstacle in obstacles)
                {
                    if (obstacle.gridPosition == position)
                    {
                        return CellTypes.Obstacle;
                    }
                }

            }

            return CellTypes.Empty;
        }






        private void ClearAllCells() //TO DO: Add cells to a pool instead of destroying them?
        {
            foreach (var cell in primaryCells)
            {
                if (cell != null)
                {
                    DestroyImmediate(cell.gameObject);
                }
            }

            foreach (var cell in secondaryCells)
            {
                if (cell != null)
                {
                    DestroyImmediate(cell.gameObject);
                }
            }

        }


    }

}
#endif