using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Where a dragged block should be drawn, or through which side it has left the board.</summary>
    public readonly struct BlockDragResult
    {
        private BlockDragResult(Vector2 position, bool hasExited, BoardSide exitSide)
        {
            Position = position;
            HasExited = hasExited;
            ExitSide = exitSide;
        }

        /// <summary>Continuous board position in cells.</summary>
        public Vector2 Position { get; }
        public bool HasExited { get; }
        public BoardSide ExitSide { get; }

        public static BlockDragResult At(Vector2 position)
        {
            return new BlockDragResult(position, false, default);
        }

        public static BlockDragResult Exited(Vector2 position, BoardSide side)
        {
            return new BlockDragResult(position, true, side);
        }
    }
}
