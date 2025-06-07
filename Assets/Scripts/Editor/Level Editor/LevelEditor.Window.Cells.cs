using System;
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
        private EditorGridCell _lastSelectedCell = null;
        private VisualElement _cellsOverlayRoot;
        private HelpBox _multipleCellSelectionHelpBox;
        private ScrollView _cellsScrollView;
        private bool _multipleCellSelection = false;

        private void CreateCellPanelContent()
        {
            _cellsOverlayRoot = CreateTitle("Edit Cells");

            _multipleCellSelectionHelpBox = new HelpBox(" ", HelpBoxMessageType.Info);
            _multipleCellSelectionHelpBox.style.display = DisplayStyle.Flex;
            _cellsOverlayRoot.Add(_multipleCellSelectionHelpBox);
            UpdateMultipleSelectionHelpBox();


            //row line
            var rowLine = new VisualElement();
            rowLine.style.height = 1;
            rowLine.style.backgroundColor = Color.gray;
            rowLine.style.marginTop = 5;
            rowLine.style.marginBottom = 5;
            _cellsOverlayRoot.Add(rowLine);


            _cellsScrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            _cellsScrollView.style.overflow = Overflow.Visible;
            _cellsScrollView.style.flexGrow = 1;
            _cellsScrollView.style.justifyContent = Justify.FlexStart;

            LevelEditor.onLevelContainerUpdated += UpdateCells;
            LevelEditor.onEditorCellUpdated += UpdateCell;
            LevelEditor.onCellsRefreshed += UpdateCells;

            UpdateCells();

            _cellsOverlayRoot.Add(_cellsScrollView);

            _multipleCellSelection = false;
        }

        private void UpdateCell(EditorGridCell cell)
        {
            UpdateCells(); //TO DO: Update specific cell instead of all cells
        }

        private void UpdateCells()
        {
            _cellsScrollView.Clear();

            if (_levelEditor == null)
            {
                Debug.LogError("LevelEditor is null, cannot update cells.");
                return;
            }

            var primaryCells = _levelEditor.primaryCells;
            var secondaryCells = _levelEditor.secondaryCells;

            Label primaryLabel = new Label($"Primary Cells ({primaryCells.Length})")
            {
                style =
                {
                    fontSize = 12,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginBottom = 2
                }
            };

            Label secondaryLabel = new Label($"Secondary Cells ({secondaryCells.Length})")
            {
                style =
                {
                    fontSize = 12,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginBottom = 5
                }
            };

            _cellsScrollView.Add(primaryLabel);
            _cellsScrollView.Add(secondaryLabel);


            var scroll = new ScrollView(ScrollViewMode.Horizontal);
            scroll.style.overflow = Overflow.Visible;
            scroll.style.flexGrow = 1;
            scroll.style.justifyContent = Justify.FlexStart;

            _cellsScrollView.Add(scroll);

            if (primaryCells == null || primaryCells.Length == 0)
            {
                Debug.LogWarning("Primary cells are empty or null.");
                return;
            }

            if (secondaryCells == null || secondaryCells.Length == 0)
            {
                Debug.LogWarning("Secondary cells are empty or null.");
                return;
            }


            int maxPrimaryCellWidth = primaryCells.GetLength(0);
            int maxSecondaryCellWidth = secondaryCells.GetLength(0);
            int secondaryHeight = secondaryCells.GetLength(1);
            int currentWidth = 0;


            //primary 
            foreach (var cell in primaryCells)
            {
                if (!cell) continue;

                if (currentWidth >= maxPrimaryCellWidth)
                {
                    currentWidth = 0;
                    scroll = CreateHorizontalScrollView();
                }

                Button selectButton = CreateCellButton(cell);
                scroll.Add(selectButton);
                currentWidth++;

                Repaint();
            }

            //add new line after primary cells
            scroll = CreateHorizontalScrollView();
            currentWidth = 0;



            //secondary
            for (int y = secondaryHeight - 1; y >= 0; y--)
            {
                scroll = CreateHorizontalScrollView();
                for (int x = 0; x < maxSecondaryCellWidth; x++)
                {
                    var cell = secondaryCells[x, y];
                    if (!cell) continue;

                    Button selectButton = CreateCellButton(cell);
                    scroll.Add(selectButton);
                    currentWidth++;
                }
            }

        }





        private Button CreateCellButton(EditorGridCell cell)
        {
            Button selectButton = new Button(() =>
          {

              if (!_multipleCellSelection) Selection.activeGameObject = cell.gameObject;
              else
              {
                  if (Selection.Contains(cell.gameObject))
                  {
                      var selectedObjects = new List<UnityEngine.Object>();
                      selectedObjects.AddRange(Selection.objects);
                      selectedObjects.Remove(cell.gameObject);
                      Selection.objects = selectedObjects.ToArray();

                      Debug.Log($"Deselected {cell.gameObject.name}. Total selected: {Selection.objects.Length}.");
                  }
                  else
                  {
                      var selectedObjects = new List<UnityEngine.Object>()
                      {
                          cell.gameObject
                      };
                      selectedObjects.AddRange(Selection.objects);

                      Selection.objects = selectedObjects.ToArray();
                      Debug.Log($"Selected {selectedObjects.Count} cells.");
                  }

                  _multipleCellSelectionHelpBox.Focus();
                  
              }


          })
            {
                text = (cell.cellType != EditorCellType.HasPassenger) ? "*" : "P",
                style =
                    {
                        width = 20,
                        backgroundColor = cell.GetGizmosColor(),
                        color =  (cell.cellType != EditorCellType.HasPassenger) ? Color.white :  Color.black,
                    },
                tooltip = $"({cell.position.x}, {cell.position.y})\nType: {cell.cellType}\nClick to select this cell."
            };
            return selectButton;
        }

        private ScrollView CreateHorizontalScrollView()
        {
            var scrollView = new ScrollView(ScrollViewMode.Horizontal);
            scrollView.style.overflow = Overflow.Visible;
            scrollView.style.flexGrow = 1;
            scrollView.style.justifyContent = Justify.FlexStart;
            _cellsScrollView.Add(scrollView);
            return scrollView;
        }


        private void UpdateCellGUI()
        {
            Event e = Event.current;

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.M)
            {
                _multipleCellSelection = !_multipleCellSelection;
                UpdateMultipleSelectionHelpBox();
            }
        }

        private void UpdateMultipleSelectionHelpBox()
        {
            if (_multipleCellSelection)
            {
                _multipleCellSelectionHelpBox.style.display = DisplayStyle.Flex;
                _multipleCellSelectionHelpBox.text = "Multiple cell selection is enabled.\nSelect this window press M to toggle.";
                _multipleCellSelectionHelpBox.style.color = Color.green;
            }
            else
            {
                _multipleCellSelectionHelpBox.style.display = DisplayStyle.Flex;
                _multipleCellSelectionHelpBox.text = "Multiple cell selection is disabled.\nSelect this window press M to toggle.";
                _multipleCellSelectionHelpBox.style.color = Color.white;
            }
        }

    }


}



