namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>Result of a transaction with the change that failed, if any.</summary>
    public readonly struct TransactionResult
    {
        private TransactionResult(TransactionStatus status, ConsumableChange failedChange)
        {
            Status = status;
            FailedChange = failedChange;
        }

        public TransactionStatus Status { get; }
        public ConsumableChange FailedChange { get; }
        public bool IsSuccess => Status == TransactionStatus.Success;

        /// <summary>Creates a successful result.</summary>
        public static TransactionResult Success()
        {
            return new TransactionResult(TransactionStatus.Success, default);
        }

        /// <summary>Creates a failed result for the given change.</summary>
        public static TransactionResult Failure(TransactionStatus status, ConsumableChange failedChange)
        {
            return new TransactionResult(status, failedChange);
        }
    }
}
