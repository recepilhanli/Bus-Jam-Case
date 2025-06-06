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
        private VisualElement _busOverlayRoot;


        private void CreateBusPanelContent()
        {
            _busOverlayRoot = new VisualElement();
            _busOverlayRoot.style.flexDirection = FlexDirection.Column;

            Button editBusesButton = new Button(() =>
            {
                EditGridsWindow.ShowWindow();
            })
            {
                text = "Edit Buses",
                style =
                {
                    marginBottom = 10,
                    width = 250,
                    color = new StyleColor(Color.cyan),
                },
                tooltip = "Change Bus List Of the Level."
            };

            _busOverlayRoot.Add(editBusesButton);
            _root.Add(_busOverlayRoot);
        }

    }

}