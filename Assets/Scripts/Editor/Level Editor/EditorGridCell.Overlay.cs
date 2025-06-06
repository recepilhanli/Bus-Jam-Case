using System;
using System.Collections;
using System.Collections.Generic;
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
                    _selectedCell.cellType = (EditorCellType)evt.newValue;
                }
            });


            OnSelectionChanged();

            return _root;
        }

        void UpdateVisibility()
        {
            if (_selectedCell == null) return;
            _positionLabel.text = $"Cell ({_selectedCell.position.x}, {_selectedCell.position.y})";
    
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
            displayed = true;


        }


        ~EditorGridCellOverlay()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }
    }

}