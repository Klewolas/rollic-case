using UnityEngine;

namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>A consumable item such as coins, lives, or a booster.</summary>
    [CreateAssetMenu(fileName = "SO_Consumable_", menuName = "RollicCase/Wallet/Consumable")]
    public sealed class ConsumableDefinition : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField, Min(0)] private int _initialAmount;
        [SerializeField] private bool _hasCapacity;
        [SerializeField, Min(0)] private int _capacity;

        /// <summary>Stable key used in saved data.</summary>
        public string Id => _id;
        public int InitialAmount => _initialAmount;
        public bool HasCapacity => _hasCapacity;
        public int Capacity => _capacity;
    }
}
