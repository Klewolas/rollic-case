using System;
using UnityEngine;

namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>Stored amount of one consumable.</summary>
    [Serializable]
    public sealed class ConsumableBalance
    {
        [SerializeField] private string _itemId;
        [SerializeField] private int _amount;

        public ConsumableBalance(string itemId, int amount)
        {
            _itemId = itemId;
            _amount = amount;
        }

        public string ItemId => _itemId;

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }
    }
}
