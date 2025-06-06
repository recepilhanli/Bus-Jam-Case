using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Level;
using UnityEngine;


#if UNITY_EDITOR
namespace Game.OnlyEditor
{
    using CellTypes = OnlyEditor.EditorCellType;
    [ExecuteAlways]
    public class LevelEditor : MonoSingleton<LevelEditor>
    {
        [SerializeField] private LevelContainer _selectedLevelContainer;
        [SerializeField] private List<EditorGridCell> _cells;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Transform _cellParent;

        [Space]
        [SerializeField] private EditorGridCell _cellPrefab;
        [SerializeField] private Mesh passengerMesh;

        public LevelContainer selectedLevelContainer
        {
            get => _selectedLevelContainer;
            set
            {
                _selectedLevelContainer = value;
                RefreshGrids();
            }
        }

        public Mesh PassengerMesh
        {
            get => passengerMesh;
            set => passengerMesh = value;
        }

        private void OnEnable()
        {
            if (SceneHelper.isGameScene || SceneHelper.isHomeScene)
            {
                Debug.LogError("Level Editor should not be used in game or home scenes. Disabling the component.");
                return;
            }

            FindType();
            gameObject.hideFlags = HideFlags.None;
            _gameManager.gameObject.hideFlags = HideFlags.None;
            _cells = new List<EditorGridCell>();

            EditorGridCell[] oldCells = _cellParent.GetComponentsInChildren<EditorGridCell>();
            foreach (var cell in oldCells)
            {
                if (cell != null)
                {
                    DestroyImmediate(cell.gameObject);
                }
            }

            if (Application.isPlaying)
            {
                LevelLoader.InitPersistent(_selectedLevelContainer);
            }
        }


        private void OnDisable()
        {
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



            for (int x = 0; x < pX; x++)
            {
                for (int y = 0; y < pY; y++)
                {
                    EditorGridCell newCell = Instantiate(_cellPrefab, _gameManager.primaryGrid.GetCellWorldPosition(new Vector2Int(x, y)), Quaternion.identity);
                    newCell.transform.SetParent(_cellParent, true);
                    newCell.cellType = CellTypes.Primary;
                    newCell.position = new Vector2Int(x, y);
                    _cells.Add(newCell);
                }
            }

            int sX = _selectedLevelContainer.secondaryGrid.gridSize.x;
            int sY = _selectedLevelContainer.secondaryGrid.gridSize.y;
            _gameManager.secondaryGrid.width = sX;
            _gameManager.secondaryGrid.height = sY;
            _gameManager.secondaryGrid.cellSize = _selectedLevelContainer.secondaryGrid.cellSize;
            _gameManager.secondaryGrid.padding = _selectedLevelContainer.secondaryGrid.padding;
            _gameManager.secondaryGrid.spacing = _selectedLevelContainer.secondaryGrid.spacing;

            for (int x = 0; x < sX; x++)
            {
                for (int y = 0; y < sY; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    EditorGridCell newCell = Instantiate(_cellPrefab, _gameManager.secondaryGrid.GetCellWorldPosition(position), Quaternion.identity);
                    newCell.transform.SetParent(_cellParent, true);
                    newCell.cellType = FindSecondaryCellType(position);
                    newCell.position = position;
                    _cells.Add(newCell);
                }
            }
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
                        return CellTypes.HasPasenger;
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
            foreach (var cell in _cells)
            {
                if (cell != null)
                {
                    DestroyImmediate(cell.gameObject);
                }
            }
            _cells.Clear();
        }


    }

}
#endif