using System;
using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Systems.PlayerData.Settings
{
    /// <summary>Stored on or off state of every setting.</summary>
    [Serializable]
    public sealed class SettingsModel
    {
        /// <summary>State of a setting the player has never changed.</summary>
        public const bool DefaultIsEnabled = true;

        [SerializeField] private List<SettingEntry> _entries;

        public SettingsModel()
        {
            _entries = new List<SettingEntry>
            {
                new SettingEntry(SettingType.Vibration, DefaultIsEnabled),
                new SettingEntry(SettingType.Sound, DefaultIsEnabled),
                new SettingEntry(SettingType.Music, DefaultIsEnabled)
            };
        }

        public List<SettingEntry> Entries => _entries;
    }
}
