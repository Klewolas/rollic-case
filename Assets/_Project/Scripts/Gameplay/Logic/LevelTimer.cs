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

        /// <summary>Raised with the new value when the remaining time, rounded up to whole seconds, changes.</summary>
        public event Action<int> WholeSecondsChanged;

        public float RemainingSeconds => _remainingSeconds;
        public bool IsExpired => _remainingSeconds <= 0f;

        /// <summary>Remaining time rounded up, as a countdown shows it.</summary>
        public int RemainingWholeSeconds => Mathf.CeilToInt(_remainingSeconds);

        /// <summary>Advances the countdown by the elapsed time.</summary>
        public void Tick(float deltaSeconds)
        {
            if (IsExpired)
            {
                return;
            }

            int previousWholeSeconds = RemainingWholeSeconds;
            _remainingSeconds = Mathf.Max(0f, _remainingSeconds - deltaSeconds);

            if (RemainingWholeSeconds != previousWholeSeconds)
            {
                WholeSecondsChanged?.Invoke(RemainingWholeSeconds);
            }

            if (IsExpired)
            {
                Expired?.Invoke();
            }
        }
    }
}
