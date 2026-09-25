using System;
using UnityEngine;

namespace RollicCase.Systems.PlayerData.Settings
{
    /// <summary>The on or off state of one setting.</summary>
    [Serializable]
    public sealed class SettingEntry
    {
        [SerializeField] private SettingType _type;
        [SerializeField] private bool _isEnabled;

        public SettingEntry(SettingType type, bool isEnabled)
        {
            _type = type;
            _isEnabled = isEnabled;
        }

        public SettingType Type => _type;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }
    }
}
