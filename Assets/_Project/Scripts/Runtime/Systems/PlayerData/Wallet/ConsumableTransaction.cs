using System.Collections.Generic;

namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>A group of consumable changes that is applied completely or not at all.</summary>
    public sealed class ConsumableTransaction
    {
        public ConsumableTransaction(params ConsumableChange[] changes)
        {
            Changes = changes;
        }

        public IReadOnlyList<ConsumableChange> Changes { get; }
    }
}
