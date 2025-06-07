using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Level;
using Game.Utils;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.OnlyEditor
{


    public partial class LevelEditorOverlay
    {

        private VisualElement _cellOverlayRoot;
        private VisualElement _cellBottomOverlayRoot; //for toggle

        private VisualElement _colorButtonContainer;

        private List<EditorGridCell> _selectedCells = new List<EditorGridCell>();

        private EnumField _cellTypeField;
        private Toggle _editCellToggle;

        private void ShowCellContent(bool show)
        {
            _cellOverlayRoot.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
            _root.MarkDirtyRepaint();
        }


        private void CreateCellPanelContent()
        {
            _cellOverlayRoot = new VisualElement();
            _cellBottomOverlayRoot = new VisualElement();

            _editCellToggle = new Toggle("Edit Cell")
            {
                value = true,
                style =
                {
                    marginBottom = 4
                }
            };

            _cellTypeField = new EnumField("Cell Type", EditorCellType.Empty);

            _editCellToggle.RegisterValueChangedCallback(evt =>
              {
                  _cellBottomOverlayRoot.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
              });

            _cellTypeField.RegisterValueChangedCallback(evt =>
            {
                if (_selectedCells.Count > 0)
                {

                    if (!_levelEditor || _selectedCells.Count == 0 || !_levelEditor.selectedLevelContainer) return;

                    var type = (EditorCellType)evt.newValue;
                    if (type == EditorCellType.Primary)
                    {
                        type = EditorCellType.Empty;
                        _cellTypeField.value = type; // Reset to empty if primary is selected
                        Debug.LogWarning("Primary cell type cannot be selected. Resetting to Empty.");
                    }

                    foreach (var cell in _selectedCells)
                    {
                        cell.cellType = type;
                    }

                    _levelEditor.selectedLevelContainer.MakeDirtyEditor();

                    UpdateSelectedCell();
                }
            });

            _cellOverlayRoot.Add(_editCellToggle);
            _cellOverlayRoot.Add(_cellBottomOverlayRoot);
            _cellBottomOverlayRoot.Add(_cellTypeField);
            CreateColorButtons();
            _root.Add(_cellOverlayRoot);

            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {

            EditorGridCell[] selectedCells = Selection.GetFiltered<EditorGridCell>(SelectionMode.Unfiltered);
            _selectedCells.Clear();
            _selectedCells.AddRange(selectedCells);

            if (_selectedCells.Count == 0)
            {
                ShowCellContent(false);
                return;
            }

            ShowCellContent(true);

            if (_selectedCells.Count == 1) _cellTypeField.value = _selectedCells[0].cellType;

            UpdateSelectedCell();
        }


        private void UpdateSelectedCell()
        {
            if (_selectedCells.Count >= 2)
            {
                _editCellToggle.text = "Multiple Cells Selected";
                ShowColors(true);
                return;
            }

            if (_selectedCells.Count == 0)
            {
                _editCellToggle.text = "No Cell Selected";
                ShowColors(false);
                return;
            }

            var selectedCell = _selectedCells[0];
            if (selectedCell == null) return;

            if (selectedCell.cellType == EditorCellType.HasPassenger) ShowColors(true);
            else ShowColors(false);

            _editCellToggle.text = $"({selectedCell.position.x}, {selectedCell.position.y})";
        }



        private void ShowColors(bool show)
        {
            _colorButtonContainer.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void CreateColorButtons()
        {
            _colorButtonContainer = new VisualElement();
            _colorButtonContainer.style.flexDirection = FlexDirection.Row;
            _colorButtonContainer.style.flexWrap = Wrap.Wrap;

            var colorTypes = Enum.GetValues(typeof(ColorList));
            Button[] _colorButtons = new Button[colorTypes.Length]; // -1 for empty type

            Label colorButtonsLabel = new Label("Passenger Color:");
            colorButtonsLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            colorButtonsLabel.style.marginBottom = 4;
            _colorButtonContainer.Add(colorButtonsLabel);

            for (int i = 0; i < colorTypes.Length; i++)
            {
                var colorType = (ColorList)colorTypes.GetValue(i);
                Color color = colorType.ToColor();
                _colorButtons[i] = new Button(() =>
                {

                    if (!_levelEditor || _selectedCells.Count == 0 || !_levelEditor.selectedLevelContainer) return;

                    foreach (var cell in _selectedCells)
                    {
                        cell.passengerColor = colorType;
                    }

                    _levelEditor.selectedLevelContainer.MakeDirtyEditor();

                })
                {
                    text = "*",
                    style =
                    {
                        backgroundColor = color,
                        width = 20,
                        height = 20,
                        marginBottom = 2
                    },
                    tooltip = colorType.ToString()
                };
                _colorButtonContainer.Add(_colorButtons[i]);
            }

            _cellBottomOverlayRoot.Add(_colorButtonContainer);
            _cellBottomOverlayRoot.MarkDirtyRepaint();
        }



    }

}