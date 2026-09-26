using RollicCase.Gameplay.Data;
using RollicCase.Editor.LevelEditor.Board;
using RollicCase.Editor.LevelEditor.Editing;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor.BoardTools
{
    /// <summary>Places doors on the rim in the selected color, stretches them by dragging, and recolors them by clicking.</summary>
    public sealed class DoorTool : ILevelEditorTool
    {
        private int _door = LevelEditing.None;
        private int _anchor;

        public string Label => "Door";
        public string Help => "Click the rim beside the board to add a door in the selected color. Drag along the rim to make it longer. Click an existing door to give it the selected color.";

        public void OnPointerDown(LevelEditorContext context, BoardHit hit)
        {
            if (hit.Kind != BoardHitKind.Edge || context.SelectedColor == null)
            {
                return;
            }

            LevelData level = context.Level;
            int index = context.Editing.FindDoorAt(level, hit.Side, hit.Position);

            if (index != LevelEditing.None)
            {
                if (level.Doors[index].Color != context.SelectedColor)
                {
                    context.Apply("Recolor Door", () =>
                    {
                        context.Editing.RecolorDoor(level, index, context.SelectedColor);
                        return true;
                    });
                }

                _door = index;
                _anchor = level.Doors[index].Start;
                return;
            }

            if (context.Apply("Place Door", () => context.Editing.TryAddDoor(level, hit.Side, hit.Position, context.SelectedColor)))
            {
                _door = level.Doors.Count - 1;
                _anchor = hit.Position;
            }
        }

        public void OnPointerDrag(LevelEditorContext context, BoardHit hit)
        {
            if (_door == LevelEditing.None || hit.Kind != BoardHitKind.Edge || hit.Side != context.Level.Doors[_door].Side)
            {
                return;
            }

            int door = _door;
            int start = Mathf.Min(_anchor, hit.Position);
            int length = Mathf.Abs(hit.Position - _anchor) + 1;
            DoorData current = context.Level.Doors[door];

            if (start != current.Start || length != current.Length)
            {
                context.Apply("Resize Door", () => context.Editing.TrySetDoorSpan(context.Level, door, start, length));
            }
        }

        public void OnPointerUp(LevelEditorContext context)
        {
            _door = LevelEditing.None;
        }

        public void DrawHover(BoardPainter painter, LevelEditorContext context, BoardHit hover)
        {
            if (hover.Kind != BoardHitKind.Edge || context.SelectedColor == null)
            {
                return;
            }

            int index = context.Editing.FindDoorAt(context.Level, hover.Side, hover.Position);

            if (index != LevelEditing.None)
            {
                DoorData door = context.Level.Doors[index];
                painter.OutlineEdge(door.Side, door.Start, door.Length, EditorColors.Selection);
                return;
            }

            Color ghost = context.SelectedColor.DisplayColor;
            ghost.a = EditorColors.GhostAlpha;
            painter.FillEdge(hover.Side, hover.Position, 1, ghost);
        }
    }
}
