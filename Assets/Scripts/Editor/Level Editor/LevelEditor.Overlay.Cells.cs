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
        private EditorGridCell _selectedCell;

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

            _cellTypeField = new EnumField("Cell Type", _selectedCell?.cellType ?? EditorCellType.Empty);

            _editCellToggle.RegisterValueChangedCallback(evt =>
              {
                  _cellBottomOverlayRoot.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
              });

            _cellTypeField.RegisterValueChangedCallback(evt =>
            {
                if (_selectedCell != null)
                {
                    var type = (EditorCellType)evt.newValue;
                    _selectedCell.cellType = type;
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
            var go = Selection.activeGameObject;
            if (!go || !go.TryGetComponent<EditorGridCell>(out var cell))
            {
                ShowCellContent(false);
                return;
            }

            ShowCellContent(true);

            _selectedCell = null;
            _cellTypeField.value = cell.cellType;
            _selectedCell = cell;

            UpdateSelectedCell();
        }


        private void UpdateSelectedCell()
        {
            if (_selectedCell == null) return;

            if (_selectedCell.cellType == EditorCellType.HasPassenger) ShowColors(true);
            else ShowColors(false);

            _editCellToggle.text = $"({_selectedCell.position.x}, {_selectedCell.position.y})";

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
                    _selectedCell.passengerColor = colorType;
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