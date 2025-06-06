using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEditor;
using UnityEngine;

namespace Game.OnlyEditor
{
    [CustomEditor(typeof(LevelContainer))]
    public class LevelContainerEditor : Editor
    {
        private bool _enableEditing = false;
        public override void OnInspectorGUI()
        {
            LevelContainer levelContainer = (LevelContainer)target;

            //color green

            _enableEditing = EditorGUILayout.Toggle("Enable Editing", _enableEditing);

            EditorGUILayout.Space(3);

            if (!_enableEditing) //disable fields
            {
                EditorGUI.BeginDisabledGroup(true);
                DrawDefaultInspector();
                EditorGUI.EndDisabledGroup();
            }
            else //enable fields
            {
                GUI.backgroundColor = Color.red;
                EditorGUILayout.HelpBox("Editing is enabled. Be careful with changes!", MessageType.Warning);
                DrawDefaultInspector();
                GUI.backgroundColor = Color.white;

            }



        }
    }
}
