using System;
using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor.Board
{
    /// <summary>Maps board cells and rim positions to GUI rects and back; cell (0, 0) is drawn bottom-left.</summary>
    public readonly struct BoardLayout
    {
        private const float RimCells = 1f;

        private readonly int _width;
        private readonly int _height;

        public BoardLayout(Rect area, int width, int height)
        {
            _width = width;
            _height = height;
            CellSize = Mathf.Min(area.width / (width + 2f * RimCells), area.height / (height + 2f * RimCells));

            var size = new Vector2(width * CellSize, height * CellSize);
            BoardRect = new Rect(area.center - size * 0.5f, size);
        }

        public float CellSize { get; }
        public Rect BoardRect { get; }
        private float Rim => CellSize * RimCells;

        public Rect GetCellRect(Vector2Int cell)
        {
            return new Rect(BoardRect.xMin + cell.x * CellSize, BoardRect.yMax - (cell.y + 1) * CellSize, CellSize, CellSize);
        }

        /// <summary>Returns the rim rect beside the board that a door span occupies.</summary>
        public Rect GetEdgeRect(BoardSide side, int start, int length)
        {
            float along = start * CellSize;
            float span = length * CellSize;

            switch (side)
            {
                case BoardSide.Bottom: return new Rect(BoardRect.xMin + along, BoardRect.yMax, span, Rim);
                case BoardSide.Top: return new Rect(BoardRect.xMin + along, BoardRect.yMin - Rim, span, Rim);
                case BoardSide.Left: return new Rect(BoardRect.xMin - Rim, BoardRect.yMax - along - span, Rim, span);
                case BoardSide.Right: return new Rect(BoardRect.xMax, BoardRect.yMax - along - span, Rim, span);
                default: throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        /// <summary>Returns the cell or rim position under the point.</summary>
        public BoardHit HitTest(Vector2 point)
        {
            int column = Mathf.FloorToInt((point.x - BoardRect.xMin) / CellSize);
            int row = Mathf.FloorToInt((BoardRect.yMax - point.y) / CellSize);
            bool isInColumns = column >= 0 && column < _width;
            bool isInRows = row >= 0 && row < _height;

            if (isInColumns && isInRows)
            {
                return BoardHit.AtCell(new Vector2Int(column, row));
            }

            if (isInColumns && point.y < BoardRect.yMin && point.y >= BoardRect.yMin - Rim)
            {
                return BoardHit.AtEdge(BoardSide.Top, column);
            }

            if (isInColumns && point.y >= BoardRect.yMax && point.y < BoardRect.yMax + Rim)
            {
                return BoardHit.AtEdge(BoardSide.Bottom, column);
            }

            if (isInRows && point.x < BoardRect.xMin && point.x >= BoardRect.xMin - Rim)
            {
                return BoardHit.AtEdge(BoardSide.Left, row);
            }

            if (isInRows && point.x >= BoardRect.xMax && point.x < BoardRect.xMax + Rim)
            {
                return BoardHit.AtEdge(BoardSide.Right, row);
            }

            return BoardHit.None;
        }
    }
}
