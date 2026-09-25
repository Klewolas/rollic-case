using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>All consumable items known to the game.</summary>
    [CreateAssetMenu(fileName = "SO_ConsumableCatalog", menuName = "RollicCase/Wallet/Consumable Catalog")]
    public sealed class ConsumableCatalog : ScriptableObject
    {
        [SerializeField] private List<ConsumableDefinition> _items = new List<ConsumableDefinition>();

        /// <summary>Returns whether the item is part of the catalog.</summary>
        public bool Contains(ConsumableDefinition item)
        {
            return _items.Contains(item);
        }
    }
}
