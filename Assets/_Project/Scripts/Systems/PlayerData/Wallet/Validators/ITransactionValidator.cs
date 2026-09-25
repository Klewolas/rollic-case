namespace RollicCase.Systems.PlayerData.Wallet.Validators
{
    /// <summary>One rule that a consumable change must satisfy before it is applied.</summary>
    public interface ITransactionValidator
    {
        /// <summary>Checks the change against the balance the item has at that point of the transaction.</summary>
        TransactionStatus Validate(in ConsumableChange change, int currentBalance);
    }
}
