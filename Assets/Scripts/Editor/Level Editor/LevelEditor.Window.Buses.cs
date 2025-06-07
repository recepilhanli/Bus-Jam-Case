using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Game.Data;
using Game.Level;
using Game.Utils;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.OnlyEditor
{


    public partial class LevelEditorWindow
    {
        private List<ColorList> _busColors = new List<ColorList>();
        private VisualElement _busOverlayRoot;
        private ListView _busListView;


        private void UpdateContainerBusData()
        {
            if (_levelEditor.selectedLevelContainer)
            {
                _levelEditor.selectedLevelContainer.busData.buses.Clear();
                _levelEditor.selectedLevelContainer.busData.buses.AddRange(_busColors);
            }
        }

        private void SyncBusData()
        {
            if (_levelEditor.selectedLevelContainer)
            {
                _busColors.Clear();
                _busColors.AddRange(_levelEditor.selectedLevelContainer.busData.buses);
                _busListView.Rebuild();
            }
        }




        private void CreateBusPanelContent()
        {
            _busOverlayRoot = CreateTitle("Edit Buses");
            _busOverlayRoot.style.flexDirection = FlexDirection.Column;

            var helpBox = new HelpBox("You can add, remove and change bus colors.\nFirst color will arrive as first bus.\nLast color will arrive as last bus.", HelpBoxMessageType.Info);
            helpBox.style.marginBottom = 5;
            _busOverlayRoot.Add(helpBox);

            LevelEditor.onLevelContainerUpdated += SyncBusData;

            ScrollView scrollView = new ScrollView(ScrollViewMode.Vertical);
            scrollView.style.flexGrow = 1;

            _busListView = new ListView();
            _busListView.style.flexGrow = 1;
            _busListView.itemsSource = _busColors;
            _busListView.selectionType = SelectionType.Single;

            if (_levelEditor.selectedLevelContainer)
            {
                _busColors.Clear();
                _busColors.AddRange(_levelEditor.selectedLevelContainer.busData.buses);
            }

            _busListView.makeItem = GetBusColorField;

            _busListView.bindItem = BindBusColorField;

            scrollView.Add(_busListView);

            var addButton = new Button(() =>
            {
                if (!CheckLevelContainer()) return;
                _busColors.Add(ColorList.Red);
                _busListView.Rebuild();
                UpdateContainerBusData();
            })

            {
                text = "Add Bus Color",
                style =
            {
            marginTop = 5,
            color = Color.cyan
            }

            };

            var clearButton = new Button(() =>
            {
                _busColors.Clear();
                _busListView.Rebuild();
                UpdateContainerBusData();
            })
            {
                text = "Clear All",
                style = {
                    color = Color.red
                }
            };


            _busOverlayRoot.Add(scrollView);
            _busOverlayRoot.Add(addButton);
            _busOverlayRoot.Add(clearButton);
        }

        private void BindBusColorField(VisualElement element, int index)
        {
            var container = element as VisualElement;
            var enumField = container.ElementAt(0) as EnumField;
            var removeButton = container.ElementAt(1) as Button;

            enumField.Init(_busColors[index]);
            enumField.value = _busColors[index];

            enumField.RegisterValueChangedCallback(evt =>
            {
                _busColors[index] = (ColorList)evt.newValue;
                UpdateContainerBusData();
            });

            removeButton.clickable.clicked -= null;
            removeButton.clickable.clicked += () =>
            {
                if (index >= 0 && index < _busColors.Count)
                {
                    _busColors.RemoveAt(index);
                    _busListView.Rebuild();
                    UpdateContainerBusData();
                }
            };

        }

        private VisualElement GetBusColorField()
        {
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;
            container.style.alignItems = Align.Stretch;
            container.style.flexGrow = 1;

            var enumField = new EnumField(ColorList.White);
            enumField.style.flexGrow = 1;
            enumField.style.marginRight = 4;

            var removeButton = new Button { text = "X" };
            removeButton.style.width = 25;
            removeButton.style.marginLeft = 4;
            removeButton.style.unityTextAlign = TextAnchor.MiddleCenter;
            removeButton.style.backgroundColor = Color.red;
            removeButton.style.unityFontStyleAndWeight = FontStyle.Bold;

            container.Add(enumField);
            container.Add(removeButton);
            return container;
        }


    }



}