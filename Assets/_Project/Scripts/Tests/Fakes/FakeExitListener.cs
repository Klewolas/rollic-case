using System.Collections.Generic;
using RollicCase.Gameplay.Logic;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Feature that records every block it hears leaving the board.</summary>
    public sealed class FakeExitListener : BlockFeature, IBlockExitListener
    {
        private readonly List<BlockModel> _exitedBlocks = new List<BlockModel>();

        public IReadOnlyList<BlockModel> ExitedBlocks => _exitedBlocks;

        public void HandleBlockExited(BlockModel block)
        {
            _exitedBlocks.Add(block);
        }
    }
}
