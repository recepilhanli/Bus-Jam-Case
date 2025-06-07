
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Utils;
using UnityEditor;
using UnityEngine;

namespace Game.OnlyEditor
{


    [ExecuteInEditMode]
    public class EditorGridCell : MonoBehaviour
    {

        public Vector2Int position;
        [SerializeField] private EditorCellType _cellType = EditorCellType.Empty;
        private ColorList _passengerColor;

        private float cellSize
        {
            get
            {
                if (LevelEditor.instance == null || LevelEditor.instance.selectedLevelContainer == null)
                    return 1f;

                float size = _cellType == EditorCellType.Primary
                    ? LevelEditor.instance.selectedLevelContainer.primaryGrid.cellSize
                    : LevelEditor.instance.selectedLevelContainer.secondaryGrid.cellSize;

                return size;
            }
        }

        public ColorList passengerColor
        {
            get => _passengerColor;
            set
            {
                if (_passengerColor == value) return;
                _passengerColor = value;
                LevelEditor.onEditorCellUpdated?.Invoke(this);
                UpdateContainerCellContent();
            }
        }



        public EditorCellType cellType
        {
            get => _cellType;
            set
            {
                if (_cellType == value) return;
                else if (_cellType == EditorCellType.Primary)
                {
                    Debug.LogError("Cannot change cell type of primary cell!");
                    return;
                }
                _cellType = value;
                LevelEditor.onEditorCellUpdated?.Invoke(this);
                UpdateContainerCellContent();
            }
        }

        public void InitType(EditorCellType type)
        {
            _cellType = type;
            LevelEditor.onEditorCellUpdated?.Invoke(this);
        }

        public void InitColor(ColorList color) => _passengerColor = color;


        private void Awake()
        {
            gameObject.hideFlags = HideFlags.NotEditable | HideFlags.DontSaveInEditor;
        }

        private void UpdateContainerCellContent()
        {
            Debug.Assert(LevelEditor.instance != null, "LevelEditor instance is null.");
            Debug.Assert(cellType != EditorCellType.Primary, "You used primary type in secondary grid.");

            var container = LevelEditor.instance.selectedLevelContainer;

            if (container == null)
            {
                Debug.LogError("Selected Level Container is null. Cannot update cell content.");
                return;
            }

            ClearContainerCellContent(container);

            if (cellType == EditorCellType.HasPassenger)
            {
                //add passenger
                container.secondaryGrid.passengers.Add(new PassengerData(position, _passengerColor));
            }
            else if (cellType == EditorCellType.Obstacle)
            {
                //add obstacle
                container.secondaryGrid.obstacles.Add(new ObstacleData(position));
            }
            else if (cellType == EditorCellType.Empty)
            {
                //do nothing
            }
            else
            {
                Debug.LogError($"Unknown cell type: {cellType}");
            }



            ClearContainerCellContent(LevelEditor.instance.selectedLevelContainer);

            switch (cellType)
            {
                case EditorCellType.Empty:
                    // Do nothing, already cleared
                    break;
                case EditorCellType.Obstacle:
                    container.secondaryGrid.obstacles.Add(new ObstacleData(position));
                    break;
                case EditorCellType.HasPassenger:
                    container.secondaryGrid.passengers.Add(new PassengerData(position, _passengerColor));
                    break;
                case EditorCellType.Primary:
                    Debug.LogError("Primary cell type should not be used in secondary grid.");
                    break;
                default:
                    Debug.LogError($"Unknown cell type: {cellType}");
                    break;
            }

        }

        private void ClearContainerCellContent(LevelContainer container)
        {
            Debug.Assert(container != null, "LevelContainer is null. Cannot clear cell content.");

            var passengers = container.secondaryGrid.passengers;
            passengers.RemoveAll(p => p.gridPosition == position);

            var obstacles = container.secondaryGrid.obstacles;
            obstacles.RemoveAll(o => o.gridPosition == position);
        }




        private void OnDrawGizmos()
        {
            Gizmos.color = GetGizmosColor();
            DrawGizmos();
        }


        public Color GetGizmosColor()
        {
            switch (cellType)
            {
                case EditorCellType.Empty:
                    return Color.white;
                case EditorCellType.Obstacle:
                    return Color.black;
                case EditorCellType.HasPassenger:
                    return _passengerColor.ToColor();
                case EditorCellType.Primary:
                    return Color.cyan;
                default:
                    return Color.white;
            }
        }


        private void DrawGizmos()
        {

            switch (cellType)
            {
                case EditorCellType.Empty:
                    Gizmos.DrawCube(transform.position, Vector3.one * cellSize);
                    //draw text
                    break;
                case EditorCellType.Obstacle:
                    Gizmos.DrawCube(transform.position, Vector3.one * cellSize);
                    break;
                case EditorCellType.HasPassenger:
                    Gizmos.DrawMesh(LevelEditor.instance.PassengerMesh, transform.position, Quaternion.Euler(-90, 0, 0), Vector3.one);
                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(transform.position, Vector3.one / 2 * cellSize);
                    break;
                case EditorCellType.Primary:
                    Gizmos.DrawWireCube(transform.position, Vector3.one * cellSize);
                    break;
            }


            //get selection list
            if (LevelEditor.instance == null || LevelEditor.instance.selectedLevelContainer == null)
                return;


            if (Selection.Contains(gameObject))
            {
                Handles.color = Color.yellow;
                Handles.DrawSolidDisc(transform.position, Vector3.up, .85f * cellSize);
            }

        }

    }

    public enum EditorCellType
    {

        /// <summary>
        /// Cannot be selected in secondary grid.
        /// </summary>
        Primary,
        
        //Secondary grid cell types
        Empty,
        Obstacle,
        HasPassenger,
    }

}
#endif