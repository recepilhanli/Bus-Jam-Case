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
        private const float DEFAULT_CELL_BUTTON_ALPHA = 0.75f;

        private VisualElement _cellsOverlayRoot;
        private HelpBox _multipleCellSelectionHelpBox;
        private ScrollView _cellsScrollView;
        private bool _multipleCellSelection = false;
        private double _selectionCoolDown = 0;

        private Dictionary<int, Button> _cellButtons = new Dictionary<int, Button>();

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
            LevelEditor.onCellsRefreshed += UpdateCells;
            EditorGridCell.onEditorCellUpdated += UpdateCell;
            Selection.selectionChanged += CheckCellSelection;

            UpdateCells();

            _cellsOverlayRoot.Add(_cellsScrollView);

            _multipleCellSelection = false;
        }

        private void CheckCellSelection()
        {
            if (_selectionCoolDown > EditorApplication.timeSinceStartup) return;
            RefreshMarks();
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

            _cellButtons.Clear();

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

            RefreshMarks();
        }





        private Button CreateCellButton(EditorGridCell cell)
        {
            if (cell == null)
            {
                Debug.LogError("Cell is null, cannot create button.");
                return null;
            }

            Color color = cell.GetGizmosColor();
            color.a = DEFAULT_CELL_BUTTON_ALPHA;

            Button selectButton = new Button(() =>
          {

              if (!_multipleCellSelection)
              {
                  Selection.activeGameObject = cell.gameObject;
                  ClearAllButtonMarks();
                  MarkCellButton(cell);
              }
              else
              {
                  if (Selection.Contains(cell.gameObject))
                  {
                      var selectedObjects = new List<UnityEngine.Object>();
                      selectedObjects.AddRange(Selection.objects);
                      selectedObjects.Remove(cell.gameObject);
                      Selection.objects = selectedObjects.ToArray();
                      UnmarkCellButton(cell);

                      Debug.Log($"Deselected {cell.gameObject.name}. Total selected: {Selection.objects.Length}.");
                  }
                  else
                  {
                      var selectedObjects = new List<UnityEngine.Object>()
                      {
                          cell.gameObject
                      };
                      selectedObjects.AddRange(Selection.objects);
                      MarkCellButton(cell);

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
                        backgroundColor = color,
                        color =  (cell.cellType != EditorCellType.HasPassenger) ? Color.white :  Color.black,
                    },
                tooltip = $"({cell.position.x}, {cell.position.y})\nType: {cell.cellType}\nClick to select this cell."
            };

            _cellButtons.TryAdd(cell.gameObject.GetInstanceID(), selectButton);

            return selectButton;
        }

        private void MarkCellButton(EditorGridCell cell)
        {
            MarkCellButton(cell.gameObject.GetInstanceID());
        }

        private void MarkCellButton(int instanceId)
        {
            _selectionCoolDown = EditorApplication.timeSinceStartup + 0.1f; // Cooldown to prevent spamming selection

            if (_cellButtons.TryGetValue(instanceId, out Button button))
            {
                button.style.borderBottomColor = Color.yellow;
                button.style.borderBottomWidth = 1.5f;

                Color color = button.style.backgroundColor.value;
                color.a = 1f;
                button.style.backgroundColor = color;
            }
        }





        private void UnmarkCellButton(EditorGridCell cell)
        {
            if (_cellButtons.TryGetValue(cell.gameObject.GetInstanceID(), out Button button))
            {
                button.style.borderBottomColor = Color.clear;
                button.style.borderBottomWidth = 0;

                Color color = button.style.backgroundColor.value;
                color.a = DEFAULT_CELL_BUTTON_ALPHA;
                button.style.backgroundColor = color;
            }
            else
            {
                Debug.LogWarning($"Button for cell {cell.gameObject.name} not found.");
            }
        }

        private void ClearAllButtonMarks()
        {
            foreach (var button in _cellButtons.Values)
            {
                button.style.borderBottomColor = Color.clear;
                button.style.borderBottomWidth = 0;

                Color color = button.style.backgroundColor.value;
                color.a = DEFAULT_CELL_BUTTON_ALPHA;
                button.style.backgroundColor = color;
            }
        }

        private void RefreshMarks()
        {
            ClearAllButtonMarks();

            if (Selection.objects.Length == 0) return;

            foreach (var obj in Selection.objects)
            {
                if (obj is GameObject cellGo)
                {
                    MarkCellButton(cellGo.GetInstanceID());
                }
            }
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

            if (_cellsOverlayRoot == null || _cellsOverlayRoot.style.display == DisplayStyle.None)
                return;

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



