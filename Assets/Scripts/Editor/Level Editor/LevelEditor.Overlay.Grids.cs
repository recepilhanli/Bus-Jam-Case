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

    public partial class LevelEditorOverlay
    {
        private VisualElement _gridOverlayRoot;


        private void CreateGridPanelContent()
        {
            _gridOverlayRoot = new VisualElement();
            _gridOverlayRoot.style.flexDirection = FlexDirection.Column;

            Button editGridsButton = new Button(() =>
            {
                LevelEditorWindow.ShowWindow();
            })
            {
                text = "Edit All",
                style =
                {
                    marginBottom = 10,
                    width = 250,
                    color = new StyleColor(Color.cyan),
                },
                tooltip = "Edit grids & cells & buses in a single window."
            };

            _gridOverlayRoot.Add(editGridsButton);
            _root.Add(_gridOverlayRoot);
        }

    }

}