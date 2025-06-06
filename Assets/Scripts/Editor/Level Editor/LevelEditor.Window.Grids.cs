using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Game.Data;
using Game.Level;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.OnlyEditor
{


    public partial class LevelEditorWindow
    {

        private VisualElement _gridOverlayRoot;

        private void CreateGridPanelContent()
        {

            _gridOverlayRoot = CreateTitle("Edit Grids");
            LevelEditor.onLevelContainerUpdated += UpdateGridData;


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

            _gridOverlayRoot.Add(primaryGridContainer);
            _gridOverlayRoot.Add(secondaryGridContainer);
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
                Vector2Int newSize = evt.newValue;

                if (newSize.x < 1 || newSize.y < 1)
                {
                    Debug.LogWarning("Grid size must be at least 1x1.");
                    if (newSize.x < 1) newSize.x = 1;
                    if (newSize.y < 1) newSize.y = 1;
                    gridSizeField.value = newSize;
                    return;
                }

                data.gridSize = newSize;

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
                _levelEditor.RefreshGrids();
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
                _levelEditor.RefreshGrids();
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

        private void SaveGrid(bool isPrimaryGrid, GridData data)
        {
            if (isPrimaryGrid)
                _levelEditor.selectedLevelContainer.ChangePrimaryGrid(data);
            else
                _levelEditor.selectedLevelContainer.ChangeSecondaryGrid(data);
        }

        public void UpdateGridData()
        {
            if (_levelEditor == null)
            {
                Debug.LogError("LevelEditor instance is null. Cannot update grid data.");
                DestroyInstance();
                return;
            }

            else if (_levelEditor.selectedLevelContainer == null)
            {
                _primaryGridData = GridData.defaultPrimaryGrid;
                _secondaryGridData = GridData.defaultSecondaryGrid;
                return;
            }

            GridData.Copy(_levelEditor.selectedLevelContainer.primaryGrid, _primaryGridData);
            GridData.Copy(_levelEditor.selectedLevelContainer.secondaryGrid, _secondaryGridData);

            Repaint();
        }


    }



}