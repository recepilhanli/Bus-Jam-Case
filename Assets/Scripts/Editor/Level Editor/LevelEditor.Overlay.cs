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
    public partial class LevelEditorOverlay : Overlay
    {
        public static LevelEditorOverlay instance;

        private VisualElement _root;

        private ObjectField _selectedLevelContainerField;
        private FloatField _timeConstraintField;
        private LevelEditor _levelEditor;

        public override VisualElement CreatePanelContent()
        {
            _levelEditor = LevelEditor.instance;

            EditorSceneManager.sceneOpened += OnSceneChanged;
            Selection.selectionChanged += OnSelectionChanged;
            LevelEditor.onLevelContainerUpdated += OnLevelContainerUpdated;

            CreateDefaultContent();
            CreateGridPanelContent();
            CreateCellPanelContent();

            if (SceneHelper.isGameScene || SceneHelper.isHomeScene) displayed = false;
            instance = this;

            return _root;

        }


        private void CreateDefaultContent()
        {
            _root = new VisualElement();
            _root.style.flexDirection = FlexDirection.Column;
            _root.style.paddingLeft = 10;
            _root.style.paddingRight = 10;
            _root.style.paddingTop = 10;

            Label titleLabel = new Label("Level Editor");
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.style.fontSize = 20;
            titleLabel.style.marginBottom = 10;

            _selectedLevelContainerField = new ObjectField("Editing Level")
            {
                objectType = typeof(LevelContainer),

                style =
                {
                    marginBottom = 10,
                    width = 250
                },

                tooltip = "Select a level to edit. If you don't have any levels, create one using the button below.",
                value = _levelEditor ? _levelEditor.selectedLevelContainer : null,

            };

            //Unity had removed search functionality from ObjectField, so we need to handle it manually

            _selectedLevelContainerField.RegisterCallback<ClickEvent>(evt =>
             {
                 LevelContainerSelectorWindow.ShowWindow();
             });

            _timeConstraintField = new FloatField("Time Constraint")
            {
                value = _levelEditor ? _levelEditor.selectedLevelContainer.timeConstraint : 60f,
                style =
                {
                    marginBottom = 10,
                    width = 250
                },
                tooltip = "Set the time constraint (In Seconds) for the level. This is the time limit for completing the level."
            };

            _timeConstraintField.RegisterValueChangedCallback(evt =>
            {
                if (_levelEditor && _levelEditor.selectedLevelContainer)
                {
                    _levelEditor.selectedLevelContainer.timeConstraint = evt.newValue;
                }
            });

            var createLevelButton = new Button(() =>
            {
                int lastLevelNumberInFolder = FindLastLevelInFolder();
                if (_levelEditor.selectedLevelContainer != null) EditorUtility.SetDirty(_levelEditor.selectedLevelContainer);


                LevelContainer newLevelContainer = ScriptableObject.CreateInstance<LevelContainer>();

                newLevelContainer.primaryGrid.cellSize = GridData.defaultPrimaryGrid.cellSize;
                newLevelContainer.primaryGrid.gridSize = GridData.defaultPrimaryGrid.gridSize;
                newLevelContainer.primaryGrid.padding = GridData.defaultPrimaryGrid.padding;
                newLevelContainer.primaryGrid.spacing = GridData.defaultPrimaryGrid.spacing;

                newLevelContainer.secondaryGrid.cellSize = GridData.defaultSecondaryGrid.cellSize;
                newLevelContainer.secondaryGrid.gridSize = GridData.defaultSecondaryGrid.gridSize;
                newLevelContainer.secondaryGrid.padding = GridData.defaultSecondaryGrid.padding;
                newLevelContainer.secondaryGrid.spacing = GridData.defaultSecondaryGrid.spacing;

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
                    width = 250,
                    marginBottom = 10
                },
                tooltip = $"Create a new level. The level will be saved in the {LevelLoader.LEVEL_FOLDER} folder with the name {LevelLoader.LEVEL_PREFIX}X, where X is the next available level number."
            };


            _selectedLevelContainerField.RegisterValueChangedCallback(evt =>
             {
                 if (evt.newValue is LevelContainer levelContainer)
                 {
                     LevelEditor.instance.selectedLevelContainer = levelContainer;
                 }
             });


            if (_levelEditor) _selectedLevelContainerField.value = LevelEditor.instance.selectedLevelContainer;


            _root.Add(titleLabel);
            _root.Add(_selectedLevelContainerField);
            _root.Add(_timeConstraintField);
            _root.Add(createLevelButton);
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

        private void CloseAllWindows()
        {
            LevelEditorWindow.DestroyInstance();
            LevelContainerSelectorWindow.DestroyInstance();
        }




        private void OnSceneChanged(Scene scene, OpenSceneMode mode)
        {
            if (SceneHelper.isGameScene || SceneHelper.isHomeScene)
            {
                CloseAllWindows();
                displayed = false;
                return;
            }

            displayed = true;
            _levelEditor = LevelEditor.instance;
            _selectedLevelContainerField.value = _levelEditor.selectedLevelContainer;
        }



        private void OnLevelContainerUpdated()
        {
            if (_levelEditor == null)
            {
                Debug.LogError("LevelEditor instance is null. Cannot update level container.");
                return;
            }

            _selectedLevelContainerField.value = _levelEditor.selectedLevelContainer;
            _timeConstraintField.value = _levelEditor.selectedLevelContainer.timeConstraint;
        }

        ~LevelEditorOverlay()
        {
            EditorSceneManager.sceneOpened -= OnSceneChanged;
            Selection.selectionChanged -= OnSelectionChanged;
            LevelEditor.onLevelContainerUpdated -= OnLevelContainerUpdated;
        }
    }

}