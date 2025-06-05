using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Level;
using UnityEditor;
using UnityEngine;

namespace Game.OnlyEditor
{

    public class LevelLoaderEditor : EditorWindow
    {
        [MenuItem("Debug/Level Loader")]
        private static void CreateWindow()
        {
            LevelLoaderEditor window = GetWindow<LevelLoaderEditor>("Level Loader");
            window.minSize = new Vector2(300, 200);
            window.maxSize = new Vector2(600, 400);
            window.Show();
        }

        private void OnGUI()
        {

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("This window is only available in the play mode.", MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginVertical("box");
            foreach (var level in LevelLoader.levelContainers)
            {
                if (!level)
                {
                    GUI.enabled = false;
                    EditorGUILayout.ObjectField("Null Container", level, typeof(LevelContainer), false);
                    GUI.enabled = true;
                    continue;
                }

                EditorGUILayout.BeginHorizontal();
                GUI.enabled = false;
                EditorGUILayout.ObjectField(level.name, level, typeof(LevelContainer), false);
                GUI.enabled = true;
                if (GUILayout.Button($"Load")) GameManager.instance.LoadLevel(level);
                EditorGUILayout.EndHorizontal();
            }


        }

    }

}