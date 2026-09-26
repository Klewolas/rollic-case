using RollicCase.Gameplay.Data;
using RollicCase.Editor.LevelEditor.Board;
using RollicCase.Editor.LevelEditor.Editing;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor.BoardTools
{
    /// <summary>Removes the block or door under the pointer while it is pressed.</summary>
    public sealed class EraseTool : ILevelEditorTool
    {
        public string Label => "Erase";
        public string Help => "Click or drag over blocks and doors to remove them.";

        public void OnPointerDown(LevelEditorContext context, BoardHit hit)
        {
            Erase(context, hit);
        }

        public void OnPointerDrag(LevelEditorContext context, BoardHit hit)
        {
            Erase(context, hit);
        }

        public void OnPointerUp(LevelEditorContext context)
        {
        }

        public void DrawHover(BoardPainter painter, LevelEditorContext context, BoardHit hover)
        {
            if (hover.Kind == BoardHitKind.Cell)
            {
                int block = context.Editing.FindBlockAt(context.Level, hover.Cell);

                if (block != LevelEditing.None)
                {
                    BlockData data = context.Level.Blocks[block];

                    foreach (Vector2Int cell in data.Cells)
                    {
                        painter.OutlineCell(data.Origin + cell, EditorColors.EraseHighlight);
                    }
                }
            }
            else if (hover.Kind == BoardHitKind.Edge)
            {
                int door = context.Editing.FindDoorAt(context.Level, hover.Side, hover.Position);

                if (door != LevelEditing.None)
                {
                    DoorData data = context.Level.Doors[door];
                    painter.OutlineEdge(data.Side, data.Start, data.Length, EditorColors.EraseHighlight);
                }
            }
        }

        private static void Erase(LevelEditorContext context, BoardHit hit)
        {
            LevelData level = context.Level;

            if (hit.Kind == BoardHitKind.Cell)
            {
                int block = context.Editing.FindBlockAt(level, hit.Cell);

                if (block != LevelEditing.None)
                {
                    context.SelectedBlock = LevelEditing.None;
                    context.Apply("Erase Block", () =>
                    {
                        context.Editing.RemoveBlock(level, block);
                        return true;
                    });
                }
            }
            else if (hit.Kind == BoardHitKind.Edge)
            {
                int door = context.Editing.FindDoorAt(level, hit.Side, hit.Position);

                if (door != LevelEditing.None)
                {
                    context.Apply("Erase Door", () =>
                    {
                        context.Editing.RemoveDoor(level, door);
                        return true;
                    });
                }
            }
        }
    }
}
