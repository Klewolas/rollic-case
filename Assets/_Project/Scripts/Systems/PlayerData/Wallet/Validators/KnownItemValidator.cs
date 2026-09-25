namespace RollicCase.Systems.PlayerData.Wallet.Validators
{
    /// <summary>Rejects changes to items that are not in the consumable catalog.</summary>
    public sealed class KnownItemValidator : ITransactionValidator
    {
        private readonly ConsumableCatalog _catalog;

        public KnownItemValidator(ConsumableCatalog catalog)
        {
            _catalog = catalog;
        }

        public TransactionStatus Validate(in ConsumableChange change, int currentBalance)
        {
            return change.Item != null && _catalog.Contains(change.Item) ? TransactionStatus.Success : TransactionStatus.UnknownItem;
        }
    }
}
