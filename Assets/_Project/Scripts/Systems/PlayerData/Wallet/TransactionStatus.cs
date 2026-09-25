namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>Outcome of a transaction or of a single validation.</summary>
    public enum TransactionStatus
    {
        Success,
        EmptyTransaction,
        InvalidAmount,
        UnknownItem,
        InsufficientBalance,
        CapacityExceeded
    }
}
