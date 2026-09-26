using RollicCase.Gameplay.Data;
using RollicCase.Editor.LevelEditor.BoardTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RollicCase.Editor.LevelEditor.Board
{
    /// <summary>Draws the edited level and forwards pointer input to the active tool; one pointer press is one undo step.</summary>
    public sealed class BoardView : IMGUIContainer
    {
        private const string EmptyMessage = "Create a new level or pick one in the Level field to start.";
        private const string DoorLabelPrefix = "D";

        private readonly LevelEditorContext _context;
        private readonly GUIStyle _labelStyle;
        private int _undoGroup;

        public BoardView(LevelEditorContext context)
        {
            _context = context;
            _labelStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } };
            onGUIHandler = Draw;
            focusable = true;
            style.flexGrow = 1f;
            RegisterCallback<MouseMoveEvent>(_ => MarkDirtyRepaint());
            RegisterCallback<MouseLeaveEvent>(_ => MarkDirtyRepaint());
        }

        public ILevelEditorTool ActiveTool { get; set; }

        private void Draw()
        {
            var area = new Rect(Vector2.zero, contentRect.size);
            LevelData level = _context.Level;

            if (level == null)
            {
                GUI.Label(area, EmptyMessage, _labelStyle);
                return;
            }

            var layout = new BoardLayout(area, level.Width, level.Height);
            Event current = Event.current;
            BoardHit hit = area.Contains(current.mousePosition) ? layout.HitTest(current.mousePosition) : BoardHit.None;

            HandleInput(current, hit);

            if (current.type == EventType.Repaint)
            {
                Paint(new BoardPainter(layout), level, hit);
            }
        }

        private void HandleInput(Event current, BoardHit hit)
        {
            if (ActiveTool == null || current.button != 0)
            {
                return;
            }

            switch (current.type)
            {
                case EventType.MouseDown:
                    Focus();
                    Undo.IncrementCurrentGroup();
                    _undoGroup = Undo.GetCurrentGroup();
                    ActiveTool.OnPointerDown(_context, hit);
                    current.Use();
                    break;
                case EventType.MouseDrag:
                    ActiveTool.OnPointerDrag(_context, hit);
                    current.Use();
                    break;
                case EventType.MouseUp:
                    ActiveTool.OnPointerUp(_context);
                    Undo.CollapseUndoOperations(_undoGroup);
                    current.Use();
                    break;
            }
        }

        private void Paint(BoardPainter painter, LevelData level, BoardHit hover)
        {
            BoardLayout layout = painter.Layout;
            Rect rim = layout.BoardRect;
            float rimSize = layout.CellSize;
            painter.FillRect(new Rect(rim.xMin - rimSize, rim.yMin - rimSize, rim.width + 2f * rimSize, rim.height + 2f * rimSize), EditorColors.Rim);
            painter.FillRect(rim, EditorColors.Background);

            for (int x = 0; x < level.Width; x++)
            {
                for (int y = 0; y < level.Height; y++)
                {
                    painter.FillCell(new Vector2Int(x, y), EditorColors.Cell);
                }
            }

            for (int i = 0; i < level.Doors.Count; i++)
            {
                DoorData door = level.Doors[i];
                painter.FillEdge(door.Side, door.Start, door.Length, ColorOf(door.Color));
                painter.Label(layout.GetEdgeRect(door.Side, door.Start, door.Length), DoorLabelPrefix + (i + 1), _labelStyle);
            }

            for (int i = 0; i < level.Blocks.Count; i++)
            {
                PaintBlock(painter, level.Blocks[i], i);
            }

            ActiveTool?.DrawHover(painter, _context, hover);
        }

        private void PaintBlock(BoardPainter painter, BlockData block, int index)
        {
            bool isSelected = index == _context.SelectedBlock;

            foreach (Vector2Int cell in block.Cells)
            {
                painter.FillCell(block.Origin + cell, ColorOf(block.Color));

                if (isSelected)
                {
                    painter.OutlineCell(block.Origin + cell, EditorColors.Selection);
                }
            }

            painter.Label(painter.Layout.GetCellRect(block.Origin + block.Cells[0]), (index + 1).ToString(), _labelStyle);
        }

        private static Color ColorOf(BlockColor color)
        {
            return color != null ? color.DisplayColor : EditorColors.Invalid;
        }
    }
}
