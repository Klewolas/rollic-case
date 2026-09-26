using UnityEngine;

namespace RollicCase.Editor.LevelEditor.Board
{
    /// <summary>Built-in colors of the level editor board.</summary>
    public static class EditorColors
    {
        public static readonly Color Background = new Color(0.16f, 0.17f, 0.2f);
        public static readonly Color Rim = new Color(0.26f, 0.28f, 0.33f);
        public static readonly Color Cell = new Color(0.36f, 0.39f, 0.45f);
        public static readonly Color Selection = Color.white;
        public static readonly Color Invalid = new Color(0.95f, 0.25f, 0.25f, 0.6f);
        public static readonly Color EraseHighlight = new Color(0.95f, 0.25f, 0.25f);
        public const float GhostAlpha = 0.5f;
    }
}
