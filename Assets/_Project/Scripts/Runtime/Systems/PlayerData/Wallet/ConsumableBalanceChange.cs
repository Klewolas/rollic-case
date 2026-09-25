namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>Describes how the balance of one consumable changed.</summary>
    public readonly struct ConsumableBalanceChange
    {
        public ConsumableBalanceChange(ConsumableDefinition item, int previousBalance, int newBalance)
        {
            Item = item;
            PreviousBalance = previousBalance;
            NewBalance = newBalance;
        }

        public ConsumableDefinition Item { get; }
        public int PreviousBalance { get; }
        public int NewBalance { get; }
    }
}
