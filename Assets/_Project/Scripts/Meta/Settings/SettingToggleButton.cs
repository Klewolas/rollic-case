using RollicCase.Systems.PlayerData.Settings;
using RollicCase.UI.Buttons;
using UnityEngine;

namespace RollicCase.Meta.Settings
{
    /// <summary>Asks to switch its setting on or off.</summary>
    public sealed class SettingToggleButton : SignalButton<SettingToggleRequestedSignal>
    {
        [SerializeField] private SettingType _setting;

        public SettingType Setting => _setting;

        protected override SettingToggleRequestedSignal CreateSignal()
        {
            return new SettingToggleRequestedSignal(_setting);
        }
    }
}
