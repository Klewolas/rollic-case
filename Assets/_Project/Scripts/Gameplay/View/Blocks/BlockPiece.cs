using UnityEngine;

namespace RollicCase.Gameplay.View.Blocks
{
    /// <summary>One placed mesh piece of a block, in cell units relative to the block origin.</summary>
    public readonly struct BlockPiece
    {
        public BlockPiece(BlockPieceType type, Vector2 position, float yaw)
        {
            Type = type;
            Position = position;
            Yaw = yaw;
        }

        public BlockPieceType Type { get; }

        /// <summary>Quadrant center for quadrant pieces, or the shared cell corner for inner corners.</summary>
        public Vector2 Position { get; }

        /// <summary>Rotation around the up axis in degrees.</summary>
        public float Yaw { get; }
    }
}
