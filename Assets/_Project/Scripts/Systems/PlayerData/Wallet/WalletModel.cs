using System;
using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>Stored balances of the consumables the player has changed.</summary>
    [Serializable]
    public sealed class WalletModel
    {
        [SerializeField] private List<ConsumableBalance> _balances = new List<ConsumableBalance>();

        public List<ConsumableBalance> Balances => _balances;
    }
}
