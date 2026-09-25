namespace RollicCase.Systems.PlayerData.Wallet.Validators
{
    /// <summary>Rejects changes that would take a balance below zero.</summary>
    public sealed class NonNegativeBalanceValidator : ITransactionValidator
    {
        public TransactionStatus Validate(in ConsumableChange change, int currentBalance)
        {
            return currentBalance + change.Amount < 0 ? TransactionStatus.InsufficientBalance : TransactionStatus.Success;
        }
    }
}
