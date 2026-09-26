using RollicCase.Systems.PlayerData.Settings;

namespace RollicCase.Meta.Settings
{
    /// <summary>Asks to switch one setting on or off.</summary>
    public readonly struct SettingToggleRequestedSignal
    {
        public SettingToggleRequestedSignal(SettingType setting)
        {
            Setting = setting;
        }

        public SettingType Setting { get; }
    }
}
