using System;
using RollicCase.Systems.PlayerData.Settings;
using RollicCase.UI.Popups;
using Zenject;

namespace RollicCase.Meta.Settings
{
    /// <summary>Opens the settings on request and switches a setting when its toggle is pressed.</summary>
    public sealed class SettingsPresenter : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IPopupService _popupService;
        private readonly ISettingsHandler _settings;

        public SettingsPresenter(SignalBus signalBus, IPopupService popupService, ISettingsHandler settings)
        {
            _signalBus = signalBus;
            _popupService = popupService;
            _settings = settings;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<SettingsRequestedSignal>(Open);
            _signalBus.Subscribe<SettingToggleRequestedSignal>(Toggle);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<SettingsRequestedSignal>(Open);
            _signalBus.Unsubscribe<SettingToggleRequestedSignal>(Toggle);
        }

        private void Open()
        {
            _popupService.Show<SettingsPopup>();
        }

        private void Toggle(SettingToggleRequestedSignal signal)
        {
            _settings.SetEnabled(signal.Setting, !_settings.IsEnabled(signal.Setting));
        }
    }
}
