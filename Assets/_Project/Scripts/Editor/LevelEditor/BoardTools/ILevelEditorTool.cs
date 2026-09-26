using RollicCase.Editor.LevelEditor.Board;

namespace RollicCase.Editor.LevelEditor.BoardTools
{
    /// <summary>A board tool of the level editor; add a new tool by implementing this interface.</summary>
    public interface ILevelEditorTool
    {
        /// <summary>Short name shown on the tool button.</summary>
        string Label { get; }

        /// <summary>One or two sentences that tell a designer how to use the tool.</summary>
        string Help { get; }

        void OnPointerDown(LevelEditorContext context, BoardHit hit);

        void OnPointerDrag(LevelEditorContext context, BoardHit hit);

        void OnPointerUp(LevelEditorContext context);

        /// <summary>Draws the preview of what a click at the hovered position would do.</summary>
        void DrawHover(BoardPainter painter, LevelEditorContext context, BoardHit hover);
    }
}
