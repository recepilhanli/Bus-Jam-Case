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
        public ColorList _passengerColor;
        [SerializeField] private EditorCellType _cellType = EditorCellType.Empty;

        private static GUIStyle _labelStyle;


        public EditorCellType cellType
        {
            get => _cellType;
            set
            {
                _cellType = value;
            }
        }

        private void Awake()
        {
            gameObject.hideFlags = HideFlags.NotEditable | HideFlags.DontSaveInEditor;
            
            if (_labelStyle == null) _labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.red },
            };
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = GetGizmosColor();
            DrawGizmos();
        }

 
        private Color GetGizmosColor()
        {
            switch (cellType)
            {
                case EditorCellType.Empty:
                    return Color.white;
                case EditorCellType.Obstacle:
                    return Color.black;
                case EditorCellType.HasPasenger:
                    return _passengerColor.ToColor();
                case EditorCellType.Primary:
                    return Color.cyan;
                default:
                    return Color.white;
            }
        }


        private void DrawGizmos()
        {
            //draw position as text
            Handles.Label(transform.position + Vector3.up, $"({position.x}, {position.y})", _labelStyle);


            switch (cellType)
            {
                case EditorCellType.Empty:
                    Gizmos.DrawCube(transform.position, Vector3.one);
                    //draw text
                    break;
                case EditorCellType.Obstacle:
                    Gizmos.DrawCube(transform.position, Vector3.one);
                    break;
                case EditorCellType.HasPasenger:
                    Gizmos.DrawMesh(LevelEditor.instance.PassengerMesh, transform.position, Quaternion.Euler(-90, 0, 0), Vector3.one);
                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(transform.position, Vector3.one / 2);
                    break;
                case EditorCellType.Primary:
                    Gizmos.DrawWireCube(transform.position, Vector3.one);
                    break;
            }

            if (Selection.activeGameObject == gameObject)
            {
                Handles.color = Color.yellow;
                Handles.DrawSolidDisc(transform.position, Vector3.up, .85f);
            }

        }

    }

    public enum EditorCellType
    {
        Empty, //Secondary
        Primary,
        Obstacle,
        HasPasenger,
    }

}
#endif