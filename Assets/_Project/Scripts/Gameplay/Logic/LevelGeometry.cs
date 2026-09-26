using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Grid geometry shared by the board, the validator, and the level editor.</summary>
    public static class LevelGeometry
    {
        /// <summary>Returns whether the cell lies on a board of the given size.</summary>
        public static bool IsInside(int width, int height, Vector2Int cell)
        {
            return cell.x >= 0 && cell.y >= 0 && cell.x < width && cell.y < height;
        }

        /// <summary>Returns how many door positions the side of a board of the given size has.</summary>
        public static int GetSideLength(int width, int height, BoardSide side)
        {
            return side.IsHorizontal() ? width : height;
        }

        /// <summary>Returns whether the door span fits on its side of a board of the given size.</summary>
        public static bool IsDoorSpanInside(int width, int height, BoardSide side, int start, int length)
        {
            return length >= 1 && start >= 0 && start + length <= GetSideLength(width, height, side);
        }

        /// <summary>Returns whether two doors on the same side share at least one position.</summary>
        public static bool DoorsOverlap(DoorData first, BoardSide side, int start, int length)
        {
            return first.Side == side && first.Start < start + length && start < first.Start + first.Length;
        }
    }
}
