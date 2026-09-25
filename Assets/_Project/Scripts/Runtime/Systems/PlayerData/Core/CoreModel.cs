using System;
using UnityEngine;

namespace RollicCase.Systems.PlayerData.Core
{
    /// <summary>Stored core player data that every system can rely on, such as level progression.</summary>
    [Serializable]
    public sealed class CoreModel
    {
        [SerializeField] private int _completedLevelCount;

        public int CompletedLevelCount
        {
            get => _completedLevelCount;
            set => _completedLevelCount = value;
        }
    }
}
