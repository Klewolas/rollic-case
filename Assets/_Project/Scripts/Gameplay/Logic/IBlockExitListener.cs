namespace RollicCase.Gameplay.Logic
{
    /// <summary>A block feature that reacts when another block leaves the board.</summary>
    public interface IBlockExitListener
    {
        /// <summary>Called after the block has left the board.</summary>
        void HandleBlockExited(BlockModel block);
    }
}
