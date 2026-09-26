using System.Collections.Generic;
using System.Text;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using RollicCase.Editor.LevelEditor.Board;
using RollicCase.Editor.LevelEditor.BoardTools;
using RollicCase.Editor.LevelEditor.Editing;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RollicCase.Editor.LevelEditor
{
    /// <summary>Visual level editor: board size, timer, blocks, and doors, with validation before saving.</summary>
    public sealed class LevelEditorWindow : EditorWindow
    {
        private const string WindowTitle = "Level Editor";
        private const int DefaultBoardSize = 6;
        private const int DefaultTimerSeconds = 60;
        private const float SidePanelWidth = 300f;
        private const float SectionSpacing = 12f;
        private const float SwatchSize = 34f;
        private const float ShapeButtonSize = 48f;
        private const float SelectionBorder = 3f;

        private static readonly Color SelectedBorderColor = Color.white;
        private static readonly Color ShapeCellColor = new Color(0.8f, 0.82f, 0.86f);

        [SerializeField] private LevelData _level;

        private readonly LevelValidator _validator = new LevelValidator();
        private readonly Dictionary<ILevelEditorTool, Button> _toolButtons = new Dictionary<ILevelEditorTool, Button>();
        private readonly Dictionary<BlockColor, VisualElement> _colorSwatches = new Dictionary<BlockColor, VisualElement>();
        private readonly Dictionary<BlockShape, VisualElement> _shapeButtons = new Dictionary<BlockShape, VisualElement>();

        private LevelEditorContext _context;
        private BoardView _board;
        private VisualElement _levelControls;
        private ObjectField _levelField;
        private SliderInt _widthField;
        private SliderInt _heightField;
        private SliderInt _timerField;
        private Label _helpLabel;
        private Label _selectionLabel;
        private Label _catalogLabel;
        private Button _addToCatalogButton;
        private VisualElement _selectionButtons;
        private VisualElement _problems;

        [MenuItem("Tools/Level Editor")]
        public static void Open()
        {
            GetWindow<LevelEditorWindow>(WindowTitle);
        }

        /// <summary>Opens the editor with the level loaded.</summary>
        public static void Open(LevelData level)
        {
            GetWindow<LevelEditorWindow>(WindowTitle).SetLevel(level);
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= HandleUndoRedo;
        }

        private void CreateGUI()
        {
            _context = new LevelEditorContext(new LevelEditing());
            _context.LevelChanged += Refresh;

            rootVisualElement.style.flexDirection = FlexDirection.Row;

            var sidePanel = new ScrollView { style = { width = SidePanelWidth, flexShrink = 0f } };
            sidePanel.contentContainer.style.paddingLeft = SectionSpacing;
            sidePanel.contentContainer.style.paddingRight = SectionSpacing;
            rootVisualElement.Add(sidePanel);

            _board = new BoardView(_context);
            _board.RegisterCallback<KeyDownEvent>(HandleKeyDown);
            rootVisualElement.Add(_board);

            sidePanel.Add(BuildLevelSection());
            _levelControls = new VisualElement();
            sidePanel.Add(_levelControls);
            _levelControls.Add(BuildBoardSection());
            _levelControls.Add(BuildToolSection());
            _levelControls.Add(BuildColorSection());
            _levelControls.Add(BuildShapeSection());
            _levelControls.Add(BuildSelectionSection());
            _levelControls.Add(BuildProblemsSection());

            SetLevel(_level);
            Undo.undoRedoPerformed += HandleUndoRedo;
        }

        private VisualElement BuildLevelSection()
        {
            VisualElement section = CreateSection("Level");

            _levelField = new ObjectField("Level") { objectType = typeof(LevelData), allowSceneObjects = false, tooltip = "The level you are editing. Pick any level asset to open it." };
            _levelField.RegisterValueChangedCallback(change => SetLevel(change.newValue as LevelData));
            section.Add(_levelField);

            VisualElement fileButtons = CreateRow();
            fileButtons.Add(new Button(CreateNewLevel) { text = "New", tooltip = "Create a new empty level." });
            fileButtons.Add(new Button(Save) { text = "Save", tooltip = "Check the level and save it." });
            fileButtons.Add(new Button(SaveCopy) { text = "Save As", tooltip = "Save a copy of this level under a new name." });
            section.Add(fileButtons);

            _catalogLabel = new Label { style = { whiteSpace = WhiteSpace.Normal } };
            _addToCatalogButton = new Button(AddToCatalog) { text = "Add to Level Catalog", tooltip = "Add this level to the end of the game's level order." };
            section.Add(_catalogLabel);
            section.Add(_addToCatalogButton);
            return section;
        }

        private VisualElement BuildBoardSection()
        {
            VisualElement section = CreateSection("Board");

            _widthField = CreateSlider("Width", LevelLimits.MinBoardSize, LevelLimits.MaxBoardSize, "Number of columns on the board.");
            _heightField = CreateSlider("Height", LevelLimits.MinBoardSize, LevelLimits.MaxBoardSize, "Number of rows on the board.");
            _timerField = CreateSlider("Timer (seconds)", LevelLimits.MinTimerSeconds, LevelLimits.MaxTimerSeconds, "Time the player has to clear the board.");

            _widthField.RegisterValueChangedCallback(change => ResizeBoard(change.newValue, _level.Height));
            _heightField.RegisterValueChangedCallback(change => ResizeBoard(_level.Width, change.newValue));
            _timerField.RegisterValueChangedCallback(change => ChangeTimer(change.newValue));

            section.Add(_widthField);
            section.Add(_heightField);
            section.Add(_timerField);
            return section;
        }

        private VisualElement BuildToolSection()
        {
            VisualElement section = CreateSection("Tool");
            VisualElement row = CreateRow();
            ILevelEditorTool[] tools = { new BlockTool(), new DoorTool(), new EraseTool() };

            foreach (ILevelEditorTool tool in tools)
            {
                var button = new Button(() => SelectTool(tool)) { text = tool.Label, tooltip = tool.Help };
                _toolButtons.Add(tool, button);
                row.Add(button);
            }

            _helpLabel = new Label { style = { whiteSpace = WhiteSpace.Normal, marginTop = SectionSpacing / 2f } };
            section.Add(row);
            section.Add(_helpLabel);
            SelectTool(tools[0]);
            return section;
        }

        private VisualElement BuildColorSection()
        {
            VisualElement section = CreateSection("Color");
            VisualElement row = CreateWrappingRow();

            foreach (BlockColor color in LevelAssets.FindAll<BlockColor>())
            {
                var swatch = new Button(() => SelectColor(color))
                {
                    tooltip = color.name,
                    style = { width = SwatchSize, height = SwatchSize, backgroundColor = color.DisplayColor }
                };

                _colorSwatches.Add(color, swatch);
                row.Add(swatch);

                if (_context.SelectedColor == null)
                {
                    _context.SelectedColor = color;
                }
            }

            section.Add(row);
            return section;
        }

        private VisualElement BuildShapeSection()
        {
            VisualElement section = CreateSection("Shape");
            VisualElement row = CreateWrappingRow();

            foreach (BlockShape shape in LevelAssets.FindAll<BlockShape>())
            {
                var button = new Button(() => SelectShape(shape)) { tooltip = shape.name, style = { width = ShapeButtonSize, height = ShapeButtonSize } };
                var preview = new IMGUIContainer(() => DrawShapePreview(shape)) { style = { flexGrow = 1f } };
                button.Add(preview);
                _shapeButtons.Add(shape, button);
                row.Add(button);

                if (_context.SelectedShape == null)
                {
                    _context.SelectedShape = shape;
                }
            }

            section.Add(row);
            return section;
        }

        private VisualElement BuildSelectionSection()
        {
            VisualElement section = CreateSection("Selected Block");
            _selectionLabel = new Label();
            _selectionButtons = CreateRow();
            _selectionButtons.Add(new Button(RotateSelected) { text = "Rotate (R)", tooltip = "Turn the selected block clockwise." });
            _selectionButtons.Add(new Button(DeleteSelected) { text = "Delete (Del)", tooltip = "Remove the selected block." });
            section.Add(_selectionLabel);
            section.Add(_selectionButtons);
            return section;
        }

        private VisualElement BuildProblemsSection()
        {
            VisualElement section = CreateSection("Problems");
            _problems = new VisualElement();
            section.Add(_problems);
            return section;
        }

        private void SetLevel(LevelData level)
        {
            _level = level;
            _context.Level = level;
            _context.SelectedBlock = LevelEditing.None;
            _levelField.SetValueWithoutNotify(level);
            Refresh();
        }

        private void Refresh()
        {
            bool hasLevel = _level != null;
            _levelControls.SetEnabled(hasLevel);
            _board.MarkDirtyRepaint();

            if (!hasLevel)
            {
                _catalogLabel.text = string.Empty;
                _addToCatalogButton.style.display = DisplayStyle.None;
                return;
            }

            _widthField.SetValueWithoutNotify(_level.Width);
            _heightField.SetValueWithoutNotify(_level.Height);
            _timerField.SetValueWithoutNotify(_level.TimerSeconds);
            RefreshSelection();
            RefreshCatalog();
            RefreshProblems();
        }

        private void RefreshSelection()
        {
            bool hasSelection = _context.SelectedBlock != LevelEditing.None;
            _selectionLabel.text = hasSelection ? $"Block {_context.SelectedBlock + 1}" : "Click a block with the Block tool to select it.";
            _selectionButtons.SetEnabled(hasSelection);

            foreach (KeyValuePair<BlockColor, VisualElement> swatch in _colorSwatches)
            {
                SetHighlighted(swatch.Value, swatch.Key == _context.SelectedColor);
            }

            foreach (KeyValuePair<BlockShape, VisualElement> shape in _shapeButtons)
            {
                SetHighlighted(shape.Value, shape.Key == _context.SelectedShape);
            }
        }

        private void RefreshCatalog()
        {
            List<LevelCatalog> catalogs = LevelAssets.FindAll<LevelCatalog>();
            int index = catalogs.Count > 0 ? catalogs[0].IndexOf(_level) : LevelEditing.None;

            _catalogLabel.text = catalogs.Count == 0
                ? "No level catalog asset exists yet."
                : index == LevelEditing.None ? "This level is not in the game's level order yet." : $"This is level {index + 1} in the game.";
            _addToCatalogButton.style.display = catalogs.Count > 0 && index == LevelEditing.None ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void RefreshProblems()
        {
            _problems.Clear();
            IReadOnlyList<LevelIssue> issues = _validator.Validate(_level);

            if (issues.Count == 0)
            {
                _problems.Add(new HelpBox("The level is valid and ready to play.", HelpBoxMessageType.Info));
                return;
            }

            foreach (LevelIssue issue in issues)
            {
                _problems.Add(new HelpBox(LevelIssueMessages.Describe(issue), HelpBoxMessageType.Warning));
            }
        }

        private void SelectTool(ILevelEditorTool tool)
        {
            _board.ActiveTool = tool;
            _helpLabel.text = tool.Help;

            foreach (KeyValuePair<ILevelEditorTool, Button> toolButton in _toolButtons)
            {
                SetHighlighted(toolButton.Value, toolButton.Key == tool);
            }
        }

        private void SelectColor(BlockColor color)
        {
            _context.SelectedColor = color;
            RefreshSelection();
            _board.MarkDirtyRepaint();
        }

        private void SelectShape(BlockShape shape)
        {
            _context.SelectedShape = shape;
            RefreshSelection();
            _board.MarkDirtyRepaint();
        }

        private void ResizeBoard(int width, int height)
        {
            int removed = _context.Editing.CountContentOutside(_level, width, height);

            if (removed > 0 && !EditorUtility.DisplayDialog("Shrink the board?",
                    $"{removed} block(s) or door(s) will no longer fit and will be removed.", "Shrink", "Cancel"))
            {
                Refresh();
                return;
            }

            _context.SelectedBlock = LevelEditing.None;
            _context.Apply("Resize Board", () =>
            {
                _context.Editing.Resize(_level, width, height);
                return true;
            });
        }

        private void ChangeTimer(int seconds)
        {
            _context.Apply("Change Timer", () =>
            {
                _level.SetTimer(seconds);
                return true;
            });
        }

        private void RotateSelected()
        {
            int index = _context.SelectedBlock;

            if (index != LevelEditing.None && !_context.Apply("Rotate Block", () => _context.Editing.TryRotateBlock(_level, index)))
            {
                ShowNotification(new GUIContent("The rotated block does not fit here."));
            }
        }

        private void DeleteSelected()
        {
            int index = _context.SelectedBlock;

            if (index == LevelEditing.None)
            {
                return;
            }

            _context.SelectedBlock = LevelEditing.None;
            _context.Apply("Delete Block", () =>
            {
                _context.Editing.RemoveBlock(_level, index);
                return true;
            });
        }

        private void HandleKeyDown(KeyDownEvent keyDown)
        {
            if (keyDown.keyCode == KeyCode.R)
            {
                RotateSelected();
                keyDown.StopPropagation();
            }
            else if (keyDown.keyCode == KeyCode.Delete || keyDown.keyCode == KeyCode.Backspace)
            {
                DeleteSelected();
                keyDown.StopPropagation();
            }
        }

        private void HandleUndoRedo()
        {
            if (_level != null && _context.SelectedBlock >= _level.Blocks.Count)
            {
                _context.SelectedBlock = LevelEditing.None;
            }

            Refresh();
        }

        private void CreateNewLevel()
        {
            LevelData level = LevelAssets.CreateLevel(DefaultBoardSize, DefaultBoardSize, DefaultTimerSeconds);

            if (level != null)
            {
                SetLevel(level);
            }
        }

        private void SaveCopy()
        {
            LevelData copy = _level != null ? LevelAssets.SaveCopy(_level) : null;

            if (copy != null)
            {
                SetLevel(copy);
            }
        }

        private void Save()
        {
            if (_level == null)
            {
                return;
            }

            IReadOnlyList<LevelIssue> issues = _validator.Validate(_level);

            if (issues.Count > 0 && !EditorUtility.DisplayDialog("This level has problems", Describe(issues) + "\nSave it anyway?", "Save Anyway", "Cancel"))
            {
                return;
            }

            AssetDatabase.SaveAssetIfDirty(_level);
            ShowNotification(new GUIContent("Level saved"));
        }

        private void AddToCatalog()
        {
            LevelCatalog catalog = LevelAssets.FindAll<LevelCatalog>()[0];
            Undo.RecordObject(catalog, "Add Level to Catalog");
            catalog.Add(_level);
            EditorUtility.SetDirty(catalog);
            AssetDatabase.SaveAssetIfDirty(catalog);
            RefreshCatalog();
        }

        private void DrawShapePreview(BlockShape shape)
        {
            Rect area = GUILayoutUtility.GetRect(ShapeButtonSize, ShapeButtonSize);
            RectInt bounds = CellBounds.Calculate(shape.Cells);
            float cellSize = Mathf.Min(area.width / bounds.width, area.height / bounds.height);
            Vector2 offset = area.center - new Vector2(bounds.width, bounds.height) * cellSize * 0.5f;

            foreach (Vector2Int cell in shape.Cells)
            {
                Vector2Int local = cell - bounds.position;
                var rect = new Rect(offset.x + local.x * cellSize, offset.y + (bounds.height - 1 - local.y) * cellSize, cellSize - 1f, cellSize - 1f);
                EditorGUI.DrawRect(rect, ShapeCellColor);
            }
        }

        private static string Describe(IReadOnlyList<LevelIssue> issues)
        {
            var text = new StringBuilder();

            foreach (LevelIssue issue in issues)
            {
                text.AppendLine("• " + LevelIssueMessages.Describe(issue));
            }

            return text.ToString();
        }

        private static VisualElement CreateSection(string title)
        {
            var section = new VisualElement { style = { marginTop = SectionSpacing } };
            section.Add(new Label(title) { style = { unityFontStyleAndWeight = FontStyle.Bold, marginBottom = SectionSpacing / 3f } });
            return section;
        }

        private static VisualElement CreateRow()
        {
            return new VisualElement { style = { flexDirection = FlexDirection.Row } };
        }

        private static VisualElement CreateWrappingRow()
        {
            return new VisualElement { style = { flexDirection = FlexDirection.Row, flexWrap = Wrap.Wrap } };
        }

        private static SliderInt CreateSlider(string label, int min, int max, string tooltip)
        {
            return new SliderInt(label, min, max) { showInputField = true, tooltip = tooltip };
        }

        private static void SetHighlighted(VisualElement element, bool isHighlighted)
        {
            float width = isHighlighted ? SelectionBorder : 0f;
            element.style.borderTopWidth = width;
            element.style.borderBottomWidth = width;
            element.style.borderLeftWidth = width;
            element.style.borderRightWidth = width;
            element.style.borderTopColor = SelectedBorderColor;
            element.style.borderBottomColor = SelectedBorderColor;
            element.style.borderLeftColor = SelectedBorderColor;
            element.style.borderRightColor = SelectedBorderColor;
        }
    }
}
