using System;

namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>Reads consumable balances and changes them through validated transactions.</summary>
    public interface IWalletHandler
    {
        /// <summary>Raised for every applied change after a transaction succeeds.</summary>
        event Action<ConsumableBalanceChange> BalanceChanged;

        /// <summary>Returns the current balance of the item.</summary>
        int GetBalance(ConsumableDefinition item);

        /// <summary>Validates every change and applies all of them, or none when any change is invalid.</summary>
        TransactionResult TryExecute(ConsumableTransaction transaction);
    }
}
