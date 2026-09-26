using System;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Countdown that expires once when it reaches zero.</summary>
    public sealed class LevelTimer
    {
        private float _remainingSeconds;

        public LevelTimer(float durationSeconds)
        {
            _remainingSeconds = durationSeconds;
        }

        /// <summary>Raised once when the remaining time reaches zero.</summary>
        public event Action Expired;

        public float RemainingSeconds => _remainingSeconds;
        public bool IsExpired => _remainingSeconds <= 0f;

        /// <summary>Advances the countdown by the elapsed time.</summary>
        public void Tick(float deltaSeconds)
        {
            if (IsExpired)
            {
                return;
            }

            _remainingSeconds = Mathf.Max(0f, _remainingSeconds - deltaSeconds);

            if (IsExpired)
            {
                Expired?.Invoke();
            }
        }
    }
}
