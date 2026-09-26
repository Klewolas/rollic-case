using UnityEngine;

namespace RollicCase.Gameplay.View.Blocks
{
    /// <summary>Receives the pointer input of block views.</summary>
    public interface IBlockDragHandler
    {
        /// <summary>A pointer pressed the block at the world point.</summary>
        void HandlePressed(BlockView block, Vector3 worldPoint);

        /// <summary>The pointer that pressed the block moved to the screen position.</summary>
        void HandleDragged(BlockView block, Vector2 screenPosition);

        /// <summary>The pointer that pressed the block was released.</summary>
        void HandleReleased(BlockView block);
    }
}
