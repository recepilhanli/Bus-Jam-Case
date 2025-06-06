using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
namespace Game.OnlyEditor
{


    [ExecuteInEditMode]
    public class EditorGridCell : MonoBehaviour
    {


        public Vector2Int position;
        [SerializeField] private EditorCellType _cellType = EditorCellType.Empty;
        private ColorList _passengerColor;

        private static GUIStyle _labelStyle;


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
                _passengerColor = value;
                LevelEditor.onEditorCellUpdated?.Invoke(this);
            }
        }


        public EditorCellType cellType
        {
            get => _cellType;
            set
            {
                _cellType = value;
                LevelEditor.onEditorCellUpdated?.Invoke(this);
            }
        }

        private void Awake()
        {
            gameObject.hideFlags = HideFlags.NotEditable | HideFlags.DontSaveInEditor;
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

        private void OnGUI()
        {
            if (_labelStyle == null)
            {
                _labelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 12,
                    normal = { textColor = Color.white },
                    alignment = TextAnchor.MiddleCenter
                };
            }


        }


        private void DrawGizmos()
        {
            if (_labelStyle != null) Handles.Label(transform.position + Vector3.up, $"({position.x}, {position.y})", _labelStyle);


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

            if (Selection.activeGameObject == gameObject)
            {
                Handles.color = Color.yellow;
                Handles.DrawSolidDisc(transform.position, Vector3.up, .85f * cellSize);
            }

        }

    }

    public enum EditorCellType
    {
        Empty, //Secondary
        Primary,
        Obstacle,
        HasPassenger,
    }

}
#endif