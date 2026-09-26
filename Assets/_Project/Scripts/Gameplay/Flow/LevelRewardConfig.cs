using RollicCase.Systems.PlayerData.Wallet;
using UnityEngine;

namespace RollicCase.Gameplay.Flow
{
    /// <summary>What the player earns for winning a level.</summary>
    [CreateAssetMenu(fileName = "SO_LevelRewardConfig", menuName = "RollicCase/Gameplay/Level Reward Config")]
    public sealed class LevelRewardConfig : ScriptableObject
    {
        [Tooltip("The consumable paid out on a win.")]
        [SerializeField] private ConsumableDefinition _coin;
        [Tooltip("Coins paid once for each won level.")]
        [SerializeField, Min(1)] private int _coinAmount;

        public ConsumableDefinition Coin => _coin;
        public int CoinAmount => _coinAmount;
    }
}
