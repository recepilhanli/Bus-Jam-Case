using System.Collections;
using System.Collections.Generic;
using Game.Level;
using UnityEditor;
using UnityEngine;


namespace Game.OnlyEditor
{

    [CustomEditor(typeof(GridCell))]
    public class GridCellEditor : Editor
    {
        GridCell _gridCell;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"World Position: {_gridCell.worldPosition}", EditorStyles.boldLabel);

        }

        private void OnEnable()
        {
            _gridCell = (GridCell)target;
        }
    }

}