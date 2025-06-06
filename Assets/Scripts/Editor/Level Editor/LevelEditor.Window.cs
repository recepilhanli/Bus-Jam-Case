using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Data;
using Game.Level;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.OnlyEditor
{

    public partial class LevelEditorWindow : EditorWindow
    {
        private static LevelEditorWindow _instance = null;
        private static LevelEditor _levelEditor => LevelEditor.instance;
        internal VisualElement root => rootVisualElement;


        private GridData _primaryGridData = new();
        private GridData _secondaryGridData = new();



        public static void ShowWindow()
        {
            if (_instance != null)
            {
                _instance.Focus();
                return;
            }

            _instance = GetWindow<LevelEditorWindow>("Edit Level");
            _instance.minSize = new Vector2(300, 400);
            _instance.maxSize = new Vector2(300, 400);

            _instance.Show();
        }

        private void OnEnable()
        {
            _instance = this;
            LoadWindowAsync().Forget();
        }

        private async UniTask LoadWindowAsync()
        {
            await UniTask.WaitUntil(() => _levelEditor != null);
            await UniTask.WaitUntil(() => _levelEditor.isLoaded);

            //Draw Default
            root.style.flexDirection = FlexDirection.Column;
            root.style.paddingLeft = 10;
            root.style.paddingRight = 10;
            root.style.paddingTop = 10;
            root.style.paddingBottom = 10;
            root.style.width = 300;
            root.style.height = 400;
            root.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
            root.style.borderLeftColor = Color.gray;
            root.style.borderLeftWidth = 1;

            Label titleLabel = new Label("Edit Level");
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.style.fontSize = 20;
            titleLabel.style.marginBottom = 10;
            root.Add(titleLabel);
            CreateToolButtons();

            CreateGridPanelContent();
            CreateCellPanelContent();

            UpdateWindow();
            UpdateGridData();

        }




        private void UpdateWindow()
        {
            Debug.Assert(_gridOverlayRoot != null, "Grid Overlay Root is null");
            Debug.Assert(_cellsOverlayRoot != null, "Cells Overlay Root is null");

            switch (_selectedButtonIndex)
            {
                case 0:
                    _gridOverlayRoot.style.display = DisplayStyle.Flex;
                    _cellsOverlayRoot.style.display = DisplayStyle.None;
                    break;
                case 1:
                    _cellsOverlayRoot.style.display = DisplayStyle.Flex;
                    _gridOverlayRoot.style.display = DisplayStyle.None;
                    break;
                case 2:
                    _cellsOverlayRoot.style.display = DisplayStyle.None;
                    _gridOverlayRoot.style.display = DisplayStyle.None;
                    break;
            }


        }


        private VisualElement CreateTitle(string title)
        {
            var visualElement = new VisualElement();
            visualElement.style.flexDirection = FlexDirection.Column;
            visualElement.style.borderTopColor = Color.gray;
            visualElement.style.borderBottomColor = Color.gray;

            Label titleLabel = new Label(title);
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.style.fontSize = 20;
            titleLabel.style.marginBottom = 10;
            visualElement.Add(titleLabel);
            root.Add(visualElement);

            return visualElement;
        }


        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                _instance.Close();
                DestroyImmediate(_levelEditor);
                _instance = null;
            }
        }


        private void OnDestroy()
        {
            if (_levelEditor)
            {
                LevelEditor.onLevelContainerUpdated -= UpdateGridData;
                LevelEditor.onLevelContainerUpdated -= UpdateCells;
                LevelEditor.onEditorCellUpdated -= UpdateCell;
                LevelEditor.onCellsRefreshed -= UpdateCells;
            }
            _instance = null;
        }




    }
}