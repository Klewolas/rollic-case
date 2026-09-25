namespace RollicCase.Systems.PlayerData.Wallet.Validators
{
    /// <summary>Rejects changes that do not change the balance.</summary>
    public sealed class NonZeroAmountValidator : ITransactionValidator
    {
        public TransactionStatus Validate(in ConsumableChange change, int currentBalance)
        {
            return change.Amount == 0 ? TransactionStatus.InvalidAmount : TransactionStatus.Success;
        }
    }
}
