namespace RollicCase.Systems.PlayerData.Wallet.Validators
{
    /// <summary>Rejects increases that would take a capped balance above its capacity.</summary>
    public sealed class CapacityValidator : ITransactionValidator
    {
        public TransactionStatus Validate(in ConsumableChange change, int currentBalance)
        {
            bool exceedsCapacity = change.Item.HasCapacity
                && change.Amount > 0
                && currentBalance + change.Amount > change.Item.Capacity;

            return exceedsCapacity ? TransactionStatus.CapacityExceeded : TransactionStatus.Success;
        }
    }
}
