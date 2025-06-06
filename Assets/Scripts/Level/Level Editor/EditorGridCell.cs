using System.Collections;
using System.Collections.Generic;
using Game.Utils;
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
            UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, $"({position.x}, {position.y})");

            switch (cellType)
            {
                case EditorCellType.Empty:
                    Gizmos.DrawWireCube(transform.position, Vector3.one);
                    //draw text
                    break;
                case EditorCellType.Obstacle:
                    Gizmos.DrawCube(transform.position, Vector3.one);
                    break;
                case EditorCellType.HasPasenger:
                    Gizmos.DrawMesh(LevelEditor.instance.PassengerMesh, transform.position, Quaternion.Euler(-90, 0, 0), Vector3.one);
                    break;
                case EditorCellType.Primary:
                    Gizmos.DrawWireCube(transform.position, Vector3.one);
                    break;
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