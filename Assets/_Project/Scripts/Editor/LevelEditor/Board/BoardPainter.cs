using RollicCase.Gameplay.Data;
using UnityEditor;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor.Board
{
    /// <summary>Draws cells, rim segments, outlines, and labels on the editor board.</summary>
    public readonly struct BoardPainter
    {
        private const float CellGap = 2f;
        private const float OutlineThickness = 3f;

        public BoardPainter(BoardLayout layout)
        {
            Layout = layout;
        }

        public BoardLayout Layout { get; }

        public void FillRect(Rect rect, Color color)
        {
            EditorGUI.DrawRect(rect, color);
        }

        public void FillCell(Vector2Int cell, Color color)
        {
            EditorGUI.DrawRect(Inset(Layout.GetCellRect(cell)), color);
        }

        public void OutlineCell(Vector2Int cell, Color color)
        {
            Outline(Inset(Layout.GetCellRect(cell)), color);
        }

        public void FillEdge(BoardSide side, int start, int length, Color color)
        {
            EditorGUI.DrawRect(Inset(Layout.GetEdgeRect(side, start, length)), color);
        }

        public void OutlineEdge(BoardSide side, int start, int length, Color color)
        {
            Outline(Inset(Layout.GetEdgeRect(side, start, length)), color);
        }

        public void Label(Rect rect, string text, GUIStyle style)
        {
            GUI.Label(rect, text, style);
        }

        private static void Outline(Rect rect, Color color)
        {
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, OutlineThickness), color);
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - OutlineThickness, rect.width, OutlineThickness), color);
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, OutlineThickness, rect.height), color);
            EditorGUI.DrawRect(new Rect(rect.xMax - OutlineThickness, rect.yMin, OutlineThickness, rect.height), color);
        }

        private static Rect Inset(Rect rect)
        {
            return new Rect(rect.xMin + CellGap, rect.yMin + CellGap, rect.width - 2f * CellGap, rect.height - 2f * CellGap);
        }
    }
}
