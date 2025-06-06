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

        [Space]
        [SerializeField] private EditorGridCell _cellPrefab;
        [SerializeField] private Mesh passengerMesh;

        public LevelContainer selectedLevelContainer
        {
            get => _selectedLevelContainer;
            set
            {
                _selectedLevelContainer = value;
                RefreshCells();
            }
        }

        public Mesh PassengerMesh
        {
            get => passengerMesh;
            set => passengerMesh = value;
        }

        protected override void Awake()
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

            if (Application.isPlaying)
            {
                LevelLoader.InitPersistent(_selectedLevelContainer);
            }
        }


        public void RefreshCells()
        {

            Debug.Assert(_cellPrefab != null, "Cell prefab is not assigned in the Level Editor.");
            Debug.Assert(_gameManager != null, "Game Manager is not assigned in the Level Editor.");

            ClearAllCells();

            if (_selectedLevelContainer == null) return;

            foreach (var cell in _gameManager.primaryGrid.cells)
            {
                EditorGridCell newCell = Instantiate(_cellPrefab, cell.transform.position, Quaternion.identity);
                newCell.cellType = CellTypes.Primary;
                newCell.position = cell.position;
            }

            foreach (var cell in _gameManager.secondaryGrid.cells)
            {
                EditorGridCell newCell = Instantiate(_cellPrefab, cell.transform.position, Quaternion.identity);
                newCell.cellType = FindSecondaryCellType(cell.position);
                newCell.position = cell.position;
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

            foreach (var passenger in passengers)
            {
                if (passenger.gridPosition == position)
                {
                    return CellTypes.HasPasenger;
                }
            }

            var obstacles = _selectedLevelContainer.secondaryGrid.obstacles;
            foreach (var obstacle in obstacles)
            {
                if (obstacle.gridPosition == position)
                {
                    return CellTypes.Obstacle;
                }
            }


            return CellTypes.Empty;
        }







        private void ClearAllCells()
        {
            foreach (var cell in _cells)
            {
                Destroy(cell.gameObject);
            }
            _cells.Clear();
        }


    }

}
#endif