using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Level;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.OnlyEditor
{

    [Overlay(typeof(SceneView), "Level Editor Overlay", true)]
    public class LevelEditorOverlay : Overlay
    {


        private VisualElement _root;

        private ObjectField _selectedLevelContainerField;
        private LevelEditor _levelEditor;
        private Button _createLevel;

        private Vector2Int _selectCellPosition;

        public override VisualElement CreatePanelContent()
        {
            _root = new VisualElement();
            _root.style.flexDirection = FlexDirection.Column;
            _root.style.paddingLeft = 10;
            _root.style.paddingRight = 10;
            _root.style.paddingTop = 10;

            _levelEditor = LevelEditor.instance;

            Label titleLabel = new Label("Level Editor");
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.style.fontSize = 20;
            titleLabel.style.marginBottom = 10;

            EditorSceneManager.sceneOpened += OnSceneChanged;


            _selectedLevelContainerField = new ObjectField("Editing Level")
            {
                objectType = typeof(LevelContainer),
                style =
                {
                    marginBottom = 10,
                    width = 300
                }
            };

            if (_levelEditor) _selectedLevelContainerField.value = LevelEditor.instance.selectedLevelContainer;

            _selectedLevelContainerField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is LevelContainer levelContainer)
                {
                    LevelEditor.instance.selectedLevelContainer = levelContainer;
                }
            });


            _createLevel = new Button(() =>
           {
               int lastLevelNumberInFolder = FindLastLevelInFolder();
               if (_levelEditor.selectedLevelContainer != null) EditorUtility.SetDirty(_levelEditor.selectedLevelContainer);


               LevelContainer newLevelContainer = ScriptableObject.CreateInstance<LevelContainer>();
               AssetDatabase.CreateAsset(newLevelContainer, $"{LevelLoader.LEVEL_FOLDER}{LevelLoader.LEVEL_PREFIX}{lastLevelNumberInFolder + 1}.asset");
                _levelEditor.selectedLevelContainer = newLevelContainer;
                _selectedLevelContainerField.value = newLevelContainer;
                
               EditorUtility.FocusProjectWindow();
               AssetDatabase.SaveAssets();
           })
            {
                text = "Create A New Level",
                style =
                {
                    width = 300,
                    marginBottom = 10
                }
            };

            _root.Add(titleLabel);
            _root.Add(_selectedLevelContainerField);
            _root.Add(_createLevel);



            if (SceneHelper.isGameScene || SceneHelper.isHomeScene) displayed = false;
            return _root;

        }

        private int FindLastLevelInFolder()
        {
            string[] guids = AssetDatabase.FindAssets($"{LevelLoader.LEVEL_PREFIX}*", new[] { LevelLoader.LEVEL_FOLDER });
            int lastLevelNumber = 0;
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.EndsWith(".asset"))
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                    if (int.TryParse(fileName.Replace(LevelLoader.LEVEL_PREFIX, ""), out int levelNumber))
                    {
                        if (levelNumber > lastLevelNumber)
                        {
                            lastLevelNumber = levelNumber;
                        }
                    }
                }
            }
            return lastLevelNumber;
        }



        private void OnSceneChanged(Scene scene, OpenSceneMode mode)
        {
            if (SceneHelper.isGameScene || SceneHelper.isHomeScene) displayed = false;
            displayed = true;
            _levelEditor = LevelEditor.instance;
        }

        ~LevelEditorOverlay()
        {
            EditorSceneManager.sceneOpened -= OnSceneChanged;
        }
    }

}