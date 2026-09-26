using RollicCase.Systems.PlayerData.Core;
using RollicCase.Systems.PlayerData.Wallet;
using UnityEngine;

namespace RollicCase.Gameplay.Flow
{
    /// <summary>Completes the current level once: pays the coin reward and advances the player's progress.</summary>
    public sealed class LevelCompletion
    {
        private readonly ICoreHandler _core;
        private readonly IWalletHandler _wallet;
        private readonly LevelRewardConfig _reward;

        private bool _isCompleted;

        public LevelCompletion(ICoreHandler core, IWalletHandler wallet, LevelRewardConfig reward)
        {
            _core = core;
            _wallet = wallet;
            _reward = reward;
        }

        /// <summary>Pays the reward and advances to the next level; returns false when the level was already completed.</summary>
        public bool TryComplete()
        {
            if (_isCompleted)
            {
                return false;
            }

            _isCompleted = true;
            TransactionResult result = _wallet.TryExecute(new ConsumableTransaction(new ConsumableChange(_reward.Coin, _reward.CoinAmount)));

            if (!result.IsSuccess)
            {
                Debug.LogWarning($"The level reward was not paid: {result.Status}.");
            }

            _core.CompleteCurrentLevel();
            return true;
        }
    }
}
