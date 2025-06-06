using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.OnlyEditor
{

    [Overlay(typeof(SceneView), "Editor Grid Cell Overlay", true)]
    public class EditorGridCellOverlay : Overlay
    {
        
        private VisualElement _root;
        private EditorGridCell _selectedCell;

        private Label _positionLabel;
        private EnumField _cellTypeField;

        private VisualElement _colorButtonContainer;


        public override VisualElement CreatePanelContent()
        {
            _root = new VisualElement();

            Selection.selectionChanged += OnSelectionChanged;

            _positionLabel = new Label("Cell (0,0)");
            _positionLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            _positionLabel.style.marginBottom = 4;

            //enum dropdown
            _cellTypeField = new EnumField("Cell Type", _selectedCell?.cellType ?? EditorCellType.Empty);



            _root.Add(_positionLabel);
            _root.Add(_cellTypeField);

            _cellTypeField.RegisterValueChangedCallback(evt =>
            {
                if (_selectedCell != null)
                {
                    var type = (EditorCellType)evt.newValue;
                    _selectedCell.cellType = type;
                    UpdateSelectedCell();


                }
            });

            OnSelectionChanged();

            return _root;
        }



        private void OnSelectionChanged()
        {
            var go = Selection.activeGameObject;
            if (!go || !go.TryGetComponent<EditorGridCell>(out var cell))
            {
                displayed = false;
                return;
            }
            

            _selectedCell = null;
            _cellTypeField.value = cell.cellType;
            _selectedCell = cell;

            UpdateSelectedCell();


            displayed = true;
        }


        private void UpdateSelectedCell()
        {
            if (_selectedCell == null) return;

            if (_selectedCell.cellType == EditorCellType.HasPasenger) ShowColors(true);
            else
            {
                ShowColors(false);
                Debug.Log("Hiding colors");
            }

            _positionLabel.text = $"Cell ({_selectedCell.position.x}, {_selectedCell.position.y})";

        }


        private void ShowColors(bool show)
        {
            if (_colorButtonContainer != null && _root.Contains(_colorButtonContainer) && !show)
            {
                _root.Remove(_colorButtonContainer);
                _root.MarkDirtyRepaint();
                return;
            }

            else if (show)
            {
                if (_colorButtonContainer != null)
                {

                    if (_root.Contains(_colorButtonContainer)) return; // already added
                    _root.Add(_colorButtonContainer);
                    _root.MarkDirtyRepaint();
                    return;
                }
                else CreateColorButtons();
            }

        }

        private void CreateColorButtons()
        {
            _colorButtonContainer = new VisualElement();
            _colorButtonContainer.style.flexDirection = FlexDirection.Row;
            _colorButtonContainer.style.flexWrap = Wrap.Wrap;

            var colorTypes = Enum.GetValues(typeof(ColorList));
            Button[] _colorButtons = new Button[colorTypes.Length]; // -1 for empty type

            Label colorButtonsLabel = new Label("Select Passenger Color:");
            colorButtonsLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            colorButtonsLabel.style.marginBottom = 4;
            _colorButtonContainer.Add(colorButtonsLabel);

            for (int i = 0; i < colorTypes.Length; i++)
            {
                var colorType = (ColorList)colorTypes.GetValue(i);
                Color color = colorType.ToColor();
                _colorButtons[i] = new Button(() =>
                {
                    _selectedCell._passengerColor = colorType;
                })
                {
                    text = "*",
                    style =
                    {
                        backgroundColor = color,
                        width = 20,
                        height = 20,
                        marginBottom = 2
                    }
                };
                _colorButtonContainer.Add(_colorButtons[i]);
            }

            _root.Add(_colorButtonContainer);
            _root.MarkDirtyRepaint();
        }



        ~EditorGridCellOverlay()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }
    }

}




