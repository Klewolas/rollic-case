using RollicCase.Editor.LevelEditor.Board;
using RollicCase.Editor.LevelEditor.Editing;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor.BoardTools
{
    /// <summary>Places the selected shape in the selected color, and selects and drags existing blocks.</summary>
    public sealed class BlockTool : ILevelEditorTool
    {
        private Vector2Int _grabOffset;
        private bool _isDragging;

        public string Label => "Block";
        public string Help => "Click an empty cell to place the selected shape in the selected color. Drag a block to move it. Select a block, then press R to rotate it or Delete to remove it.";

        public void OnPointerDown(LevelEditorContext context, BoardHit hit)
        {
            if (hit.Kind != BoardHitKind.Cell)
            {
                return;
            }

            int index = context.Editing.FindBlockAt(context.Level, hit.Cell);

            if (index != LevelEditing.None)
            {
                context.SelectedBlock = index;
                _grabOffset = hit.Cell - context.Level.Blocks[index].Origin;
                _isDragging = true;
                return;
            }

            if (context.SelectedShape == null || context.SelectedColor == null)
            {
                return;
            }

            bool isPlaced = context.Apply("Place Block",
                () => context.Editing.TryAddBlock(context.Level, context.SelectedColor, hit.Cell, context.SelectedShape.Cells));

            context.SelectedBlock = isPlaced ? context.Level.Blocks.Count - 1 : LevelEditing.None;
        }

        public void OnPointerDrag(LevelEditorContext context, BoardHit hit)
        {
            if (!_isDragging || hit.Kind != BoardHitKind.Cell)
            {
                return;
            }

            int index = context.SelectedBlock;
            Vector2Int origin = hit.Cell - _grabOffset;

            if (origin != context.Level.Blocks[index].Origin)
            {
                context.Apply("Move Block", () => context.Editing.TryMoveBlock(context.Level, index, origin));
            }
        }

        public void OnPointerUp(LevelEditorContext context)
        {
            _isDragging = false;
        }

        public void DrawHover(BoardPainter painter, LevelEditorContext context, BoardHit hover)
        {
            if (_isDragging || hover.Kind != BoardHitKind.Cell || context.SelectedShape == null || context.SelectedColor == null
                || context.Editing.FindBlockAt(context.Level, hover.Cell) != LevelEditing.None)
            {
                return;
            }

            bool canPlace = context.Editing.CanPlace(context.Level, hover.Cell, context.SelectedShape.Cells);
            Color ghost = context.SelectedColor.DisplayColor;
            ghost.a = EditorColors.GhostAlpha;

            foreach (Vector2Int cell in context.SelectedShape.Cells)
            {
                painter.FillCell(hover.Cell + cell, canPlace ? ghost : EditorColors.Invalid);
            }
        }
    }
}
