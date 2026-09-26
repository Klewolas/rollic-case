using System;
using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Grid helpers for board sides.</summary>
    public static class BoardSideExtensions
    {
        /// <summary>Returns the unit step that moves a block toward the side.</summary>
        public static Vector2Int ToDirection(this BoardSide side)
        {
            switch (side)
            {
                case BoardSide.Bottom: return Vector2Int.down;
                case BoardSide.Top: return Vector2Int.up;
                case BoardSide.Left: return Vector2Int.left;
                case BoardSide.Right: return Vector2Int.right;
                default: throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        /// <summary>Returns whether doors on the side run along the x axis.</summary>
        public static bool IsHorizontal(this BoardSide side)
        {
            return side == BoardSide.Bottom || side == BoardSide.Top;
        }
    }
}
