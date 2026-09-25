namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>A signed amount to add to or remove from one consumable.</summary>
    public readonly struct ConsumableChange
    {
        public ConsumableChange(ConsumableDefinition item, int amount)
        {
            Item = item;
            Amount = amount;
        }

        public ConsumableDefinition Item { get; }
        public int Amount { get; }
    }
}
