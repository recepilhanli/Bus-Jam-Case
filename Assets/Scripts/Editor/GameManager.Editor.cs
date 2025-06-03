using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Utils;
using UnityEditor;
using UnityEngine;


namespace Game.OnlyEditor
{

    [CustomEditor(typeof(GameManager))]
    class GameManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Open Game Manager Editor"))
            {
                GameManagerEditorWindow.Open((GameManager)target);
            }
        }
    }

    class GameManagerEditorWindow : EditorWindow
    {
        private GameManager _manager;

        private bool _assignColors = false;
        private Vector2 _colorScrollPosition;

        public static void Open(GameManager manager)
        {
            GameManagerEditorWindow window = GetWindow<GameManagerEditorWindow>("Game Manager Editor");
            window.minSize = new Vector2(400, 300);
            window._manager = manager;
            window.Show();
        }

        private void SetManagerDirty()
        {
            EditorUtility.SetDirty(_manager);
        }




        private void OnGUI()
        {
            GUILayout.Label("Game Manager Editor", EditorStyles.boldLabel);

            if (_manager == null)
            {
                EditorGUILayout.HelpBox("No GameManager assigned.", MessageType.Error);
                return;
            }

            AssignBuses();
        }


        private void AssignBuses()
        {
            EditorGUILayout.Space();

            _assignColors = EditorGUILayout.Toggle("Assign Buses", _assignColors);

            if (_assignColors)
            {
                EditorGUILayout.BeginVertical("Box");

                _colorScrollPosition = EditorGUILayout.BeginScrollView(_colorScrollPosition);

                EditorGUILayout.HelpBox("All the buses will arrive one after another.", MessageType.Info);
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                foreach (ColorList colorList in System.Enum.GetValues(typeof(ColorList)))
                {

                    GUI.color = colorList.ToColor();
                    if (GUILayout.Button("*"))
                    {
                        _manager.busList.Add(colorList);
                        SetManagerDirty();
                    }
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (GUILayout.Button("Clear All Colors"))
                {
                    _manager.busList.Clear();
                    SetManagerDirty();
                }

                EditorGUILayout.EndVertical();

            }

        }


    }

}