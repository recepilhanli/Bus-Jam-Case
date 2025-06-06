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


    public partial class LevelEditorWindow
    {

        private VisualElement _toolsContainer;

        private Button _editCellsButton;
        private Button _editGridsButton;
        private Button _editBusesButton;

        private Button[] _toolButtons = new Button[3];

        private readonly Color _defaultButtonColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        private readonly Color _selectedButtonColor = Color.gray;

        private int _selectedButtonIndex = 0;


        private void SelectButton(int index)
        {
            for (int i = 0; i < _toolButtons.Length; i++)
            {
                if (i == index)
                {
                    _toolButtons[i].style.backgroundColor = new StyleColor(_selectedButtonColor);
                }
                else
                {
                    _toolButtons[i].style.backgroundColor = new StyleColor(_defaultButtonColor);
                }
            }

            _selectedButtonIndex = index;
            UpdateWindow();
        }


        private void CreateToolButtons()
        {

            _toolsContainer = new VisualElement();
            _toolsContainer.style.flexDirection = FlexDirection.Row;
            _toolsContainer.style.marginBottom = 10;
            _toolsContainer.style.justifyContent = Justify.FlexStart;

            _editGridsButton = new Button(() =>
           {
               SelectButton(0);
           })
            {
                text = "Edit Grids",
                style =
                {
                    width = 90,
                    height = 30,
                    backgroundColor = new StyleColor(_selectedButtonColor)
                },
                tooltip = "Edit grids"
            };


            _editCellsButton = new Button(() =>
           {
               SelectButton(1);

           })
            {
                text = "Edit Cells",
                style =
                {
                    width = 90,
                    height = 30,
                    backgroundColor = new StyleColor(_defaultButtonColor)
                },
                tooltip = "Edit cells."
            };

            _editBusesButton = new Button(() =>
           {
               SelectButton(2);

           })
            {
                text = "Edit Buses",
                style =
                {
                    width = 90,
                    height = 30,
                    backgroundColor = new StyleColor(_defaultButtonColor)
                },
                tooltip = "Edit buses."
            };

            _toolButtons[0] = _editGridsButton;
            _toolButtons[1] = _editCellsButton;
            _toolButtons[2] = _editBusesButton;



            _toolsContainer.Add(_editGridsButton);
            _toolsContainer.Add(_editCellsButton);
            _toolsContainer.Add(_editBusesButton);
            root.Add(_toolsContainer);
        }


    }



}