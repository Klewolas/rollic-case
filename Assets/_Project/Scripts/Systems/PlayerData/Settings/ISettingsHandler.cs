using System;

namespace RollicCase.Systems.PlayerData.Settings
{
    /// <summary>Reads and changes player settings.</summary>
    public interface ISettingsHandler
    {
        /// <summary>Raised after a setting changes its state.</summary>
        event Action<SettingType, bool> SettingChanged;

        /// <summary>Returns whether the setting is on.</summary>
        bool IsEnabled(SettingType type);

        /// <summary>Turns the setting on or off.</summary>
        void SetEnabled(SettingType type, bool isEnabled);
    }
}
