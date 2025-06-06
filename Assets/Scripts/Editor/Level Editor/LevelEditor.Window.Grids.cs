using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Level;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.OnlyEditor
{

    public class EditGridsWindow : EditorWindow
    {
        private static EditGridsWindow _instance = null;
        private static LevelEditor _levelEditor => LevelEditor.instance;
        private static LevelContainer _selectedLevelContainer => _levelEditor.selectedLevelContainer;

        private GridData _primaryGridData = new();
        private GridData _secondaryGridData = new();


        [MenuItem("Window/Edit Grids")]
        public static void ShowWindow()
        {
            if (_instance != null)
            {
                _instance.Focus();
                return;
            }

            _instance = GetWindow<EditGridsWindow>("Edit Grids");
            _instance.minSize = new Vector2(300, 400);
            _instance.maxSize = new Vector2(300, 400);

            _levelEditor.onLevelContainerUpdated += _instance.UpdateGridData;

            _instance.Show();
        }


        public void UpdateGridData()
        {
            if (_levelEditor == null)
            {
                Debug.LogError("LevelEditor instance is null. Cannot update grid data.");
                DestroyInstance();
                return;
            }
            if (_levelEditor.selectedLevelContainer == null)
            {
                _primaryGridData = GridData.defaultPrimaryGrid;
                _secondaryGridData = GridData.defaultSecondaryGrid;
                return;
            }

            GridData.Copy(_levelEditor.selectedLevelContainer.primaryGrid, _primaryGridData);
            GridData.Copy(_levelEditor.selectedLevelContainer.secondaryGrid, _secondaryGridData);

            Repaint();
        }

        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                _instance.Close();
                _instance = null;
                _levelEditor.onLevelContainerUpdated -= _instance.UpdateGridData;
            }
        }

        private void OnEnable()
        {
            UpdateGridData();

            //Draw UI
            var root = rootVisualElement;
            root.style.flexDirection = FlexDirection.Column;
            root.style.paddingLeft = 10;
            root.style.paddingRight = 10;
            root.style.paddingTop = 10;
            root.style.paddingBottom = 10;
            root.style.width = 300;
            root.style.height = 400;
            root.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
            root.style.borderLeftColor = Color.gray;
            root.style.borderLeftWidth = 1;

            Label titleLabel = new Label("Edit Grids");
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.style.fontSize = 20;
            titleLabel.style.marginBottom = 10;
            root.Add(titleLabel);

            // Primary Grid
            var primaryGridContainer = new VisualElement();
            primaryGridContainer.style.flexDirection = FlexDirection.Column;
            primaryGridContainer.style.marginBottom = 10;
            primaryGridContainer.style.borderBottomColor = Color.gray;
            primaryGridContainer.style.borderBottomWidth = 1;
            root.Add(primaryGridContainer);
            CreateGridElements(_primaryGridData, primaryGridContainer, "Primary Grid");

            // Secondary Grid
            var secondaryGridContainer = new VisualElement();
            secondaryGridContainer.style.flexDirection = FlexDirection.Column;
            secondaryGridContainer.style.marginBottom = 10;
            secondaryGridContainer.style.borderBottomColor = Color.gray;
            secondaryGridContainer.style.borderBottomWidth = 1;
            root.Add(secondaryGridContainer);

            CreateGridElements(_secondaryGridData, secondaryGridContainer, "Secondary Grid");
        }

        private void SaveGrid(bool isPrimaryGrid, GridData data)
        {
            if (isPrimaryGrid)
                _levelEditor.selectedLevelContainer.ChangePrimaryGrid(data);
            else
                _levelEditor.selectedLevelContainer.ChangeSecondaryGrid(data);
        }

        private void CreateGridElements(GridData data, VisualElement container, string title)
        {
            Label gridTitleLabel = new Label(title);
            gridTitleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            gridTitleLabel.style.fontSize = 16;
            gridTitleLabel.style.marginBottom = 5;
            container.Add(gridTitleLabel);

            var gridSizeField = new Vector2IntField("Grid Size")
            {
                value = data.gridSize,
                style =
                {
                    marginBottom = 5
                }
            };
            gridSizeField.RegisterValueChangedCallback(evt =>
            {
                data.gridSize = evt.newValue;

                if (evt.newValue.x < 1 || evt.newValue.y < 1)
                {
                    Debug.LogWarning("Grid size must be at least 1x1.");
                    return;
                }

                bool isPrimaryGrid = data == _primaryGridData;
                SaveGrid(isPrimaryGrid, data);
                _levelEditor.RefreshGrids();
            });
            container.Add(gridSizeField);

            var paddingField = new Vector2Field("Padding")
            {
                value = data.padding,
                style =
                {
                    marginBottom = 5
                }
            };
            paddingField.RegisterValueChangedCallback(evt =>
            {
                data.padding = evt.newValue;
                bool isPrimaryGrid = data == _primaryGridData;
                SaveGrid(isPrimaryGrid, data);
            });
            container.Add(paddingField);

            var spacingField = new Vector2Field("Spacing")
            {
                value = data.spacing,
                style =
                {
                    marginBottom = 5
                }
            };
            spacingField.RegisterValueChangedCallback(evt =>
            {
                data.spacing = evt.newValue;
                bool isPrimaryGrid = data == _primaryGridData;
                SaveGrid(isPrimaryGrid, data);
            });
            container.Add(spacingField);

            var cellSizeField = new FloatField("Cell Size")
            {
                value = data.cellSize,
                style =
                {
                    marginBottom = 5
                }
            };
            cellSizeField.RegisterValueChangedCallback(evt =>
            {
                data.cellSize = evt.newValue;
                bool isPrimaryGrid = data == _primaryGridData;
                SaveGrid(isPrimaryGrid, data);
            });
            container.Add(cellSizeField);
        }





    }
}