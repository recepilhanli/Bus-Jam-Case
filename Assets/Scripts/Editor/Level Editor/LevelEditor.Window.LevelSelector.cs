using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.OnlyEditor
{

    public class LevelContainerSelectorWindow : EditorWindow
    {
        private static LevelContainerSelectorWindow _instance;
        private static LevelEditor _levelEditor;


        [MenuItem("Window/Level Editor/Level Container")]
        public static void ShowWindow()
        {
            if (_instance != null)
            {
                _instance.Focus();
                return;
            }

            _levelEditor = LevelEditor.instance;
            if (_levelEditor == null)
            {
                Debug.LogError("LevelEditor instance is not available. Please ensure LevelEditor is initialized before opening the selector.");
                return;
            }

            LevelContainerSelectorWindow window = GetWindow<LevelContainerSelectorWindow>("Level Container Selector");
            window.minSize = new Vector2(400, 65);
            window.maxSize = window.minSize;
            window.Show();
        }

        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                _instance.Close();
                DestroyImmediate(_instance);
                _instance = null;
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("To edit level containers, please select a level container from the list (click (o) button) below.", EditorStyles.wordWrappedLabel);
            if (_levelEditor == null)
            {
                _levelEditor = LevelEditor.instance;
                Debug.LogWarning("LevelEditor instance was null, re-initialized.");
                if (_levelEditor == null)
                {
                    DestroyInstance();
                    return;
                }
                return;
            }

            var levelContainer = EditorGUILayout.ObjectField("Selected Level Container", _levelEditor.selectedLevelContainer, typeof(LevelContainer), false);
            if (levelContainer != _levelEditor.selectedLevelContainer) _levelEditor.selectedLevelContainer = levelContainer as LevelContainer;

        }

        private void OnEnable()
        {
            _instance = this;
        }
    }

}