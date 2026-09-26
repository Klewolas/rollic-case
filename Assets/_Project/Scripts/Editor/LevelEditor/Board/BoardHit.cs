using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor.Board
{
    /// <summary>The board cell or rim position under the pointer.</summary>
    public readonly struct BoardHit
    {
        private BoardHit(BoardHitKind kind, Vector2Int cell, BoardSide side, int position)
        {
            Kind = kind;
            Cell = cell;
            Side = side;
            Position = position;
        }

        public static BoardHit None => default;

        public BoardHitKind Kind { get; }

        /// <summary>The board cell, valid when Kind is Cell.</summary>
        public Vector2Int Cell { get; }

        /// <summary>The rim side, valid when Kind is Edge.</summary>
        public BoardSide Side { get; }

        /// <summary>The door position along the side, valid when Kind is Edge.</summary>
        public int Position { get; }

        public static BoardHit AtCell(Vector2Int cell)
        {
            return new BoardHit(BoardHitKind.Cell, cell, default, default);
        }

        public static BoardHit AtEdge(BoardSide side, int position)
        {
            return new BoardHit(BoardHitKind.Edge, default, side, position);
        }
    }
}
