using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>A block feature that can forbid its block from moving.</summary>
    public interface IBlockMoveRule
    {
        /// <summary>Returns whether the block may move in the direction.</summary>
        bool CanMove(Vector2Int direction);
    }
}
