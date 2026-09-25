using System;

namespace RollicCase.Systems.PlayerData.Settings
{
    /// <summary>Only writer of the settings model.</summary>
    public sealed class SettingsHandler : ISettingsHandler
    {
        private readonly IModelProvider<SettingsModel> _provider;

        public SettingsHandler(IModelProvider<SettingsModel> provider)
        {
            _provider = provider;
        }

        public event Action<SettingType, bool> SettingChanged;

        public bool IsEnabled(SettingType type)
        {
            SettingEntry entry = FindEntry(type);
            return entry?.IsEnabled ?? SettingsModel.DefaultIsEnabled;
        }

        public void SetEnabled(SettingType type, bool isEnabled)
        {
            if (IsEnabled(type) == isEnabled)
            {
                return;
            }

            SettingEntry entry = FindEntry(type);

            if (entry == null)
            {
                _provider.Model.Entries.Add(new SettingEntry(type, isEnabled));
            }
            else
            {
                entry.IsEnabled = isEnabled;
            }

            _provider.MarkDirty();
            SettingChanged?.Invoke(type, isEnabled);
        }

        private SettingEntry FindEntry(SettingType type)
        {
            var entries = _provider.Model.Entries;

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Type == type)
                {
                    return entries[i];
                }
            }

            return null;
        }
    }
}
