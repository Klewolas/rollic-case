using System;
using System.Collections.Generic;
using RollicCase.Systems.PlayerData.Wallet.Validators;

namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>Only writer of the wallet model; applies atomic, validated transactions.</summary>
    public sealed class WalletHandler : IWalletHandler
    {
        private readonly IModelProvider<WalletModel> _provider;
        private readonly List<ITransactionValidator> _validators;
        private readonly Dictionary<ConsumableDefinition, int> _projectedBalances = new Dictionary<ConsumableDefinition, int>();

        public WalletHandler(IModelProvider<WalletModel> provider, List<ITransactionValidator> validators)
        {
            _provider = provider;
            _validators = validators;
        }

        public event Action<ConsumableBalanceChange> BalanceChanged;

        public int GetBalance(ConsumableDefinition item)
        {
            ConsumableBalance entry = FindEntry(item.Id);
            return entry?.Amount ?? item.InitialAmount;
        }

        public TransactionResult TryExecute(ConsumableTransaction transaction)
        {
            IReadOnlyList<ConsumableChange> changes = transaction.Changes;

            if (changes.Count == 0)
            {
                return TransactionResult.Failure(TransactionStatus.EmptyTransaction, default);
            }

            _projectedBalances.Clear();

            for (int i = 0; i < changes.Count; i++)
            {
                ConsumableChange change = changes[i];
                int balance = GetProjectedBalance(change.Item);
                TransactionStatus status = Validate(change, balance);

                if (status != TransactionStatus.Success)
                {
                    return TransactionResult.Failure(status, change);
                }

                _projectedBalances[change.Item] = balance + change.Amount;
            }

            for (int i = 0; i < changes.Count; i++)
            {
                Apply(changes[i]);
            }

            _provider.MarkDirty();
            return TransactionResult.Success();
        }

        private int GetProjectedBalance(ConsumableDefinition item)
        {
            if (item == null)
            {
                return 0;
            }

            return _projectedBalances.TryGetValue(item, out int projected) ? projected : GetBalance(item);
        }

        private TransactionStatus Validate(in ConsumableChange change, int balance)
        {
            for (int i = 0; i < _validators.Count; i++)
            {
                TransactionStatus status = _validators[i].Validate(change, balance);

                if (status != TransactionStatus.Success)
                {
                    return status;
                }
            }

            return TransactionStatus.Success;
        }

        private void Apply(in ConsumableChange change)
        {
            int previousBalance = GetBalance(change.Item);
            int newBalance = previousBalance + change.Amount;
            ConsumableBalance entry = FindEntry(change.Item.Id);

            if (entry == null)
            {
                _provider.Model.Balances.Add(new ConsumableBalance(change.Item.Id, newBalance));
            }
            else
            {
                entry.Amount = newBalance;
            }

            BalanceChanged?.Invoke(new ConsumableBalanceChange(change.Item, previousBalance, newBalance));
        }

        private ConsumableBalance FindEntry(string itemId)
        {
            List<ConsumableBalance> balances = _provider.Model.Balances;

            for (int i = 0; i < balances.Count; i++)
            {
                if (string.Equals(balances[i].ItemId, itemId, StringComparison.Ordinal))
                {
                    return balances[i];
                }
            }

            return null;
        }
    }
}
