using NUnit.Framework;
using RollicCase.Systems.PlayerData.Settings;
using RollicCase.Tests.Fakes;

namespace RollicCase.Tests.Systems.PlayerData.Settings
{
    public sealed class SettingsHandlerTests
    {
        private FakeModelProvider<SettingsModel> _provider;
        private SettingsHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _provider = new FakeModelProvider<SettingsModel>(new SettingsModel());
            _handler = new SettingsHandler(_provider);
        }

        [TestCase(SettingType.Vibration)]
        [TestCase(SettingType.Sound)]
        [TestCase(SettingType.Music)]
        public void IsEnabled_NeverChanged_ReturnsModelDefault(SettingType type)
        {
            Assert.AreEqual(SettingsModel.DefaultIsEnabled, _handler.IsEnabled(type));
        }

        [Test]
        public void SetEnabled_NewValue_StoresValue()
        {
            _handler.SetEnabled(SettingType.Sound, false);

            Assert.IsFalse(_handler.IsEnabled(SettingType.Sound));
        }

        [Test]
        public void SetEnabled_NewValue_RaisesSettingChangedAndMarksDirty()
        {
            SettingType changedType = default;
            bool changedValue = true;
            _handler.SettingChanged += (type, isEnabled) =>
            {
                changedType = type;
                changedValue = isEnabled;
            };

            _handler.SetEnabled(SettingType.Music, false);

            Assert.AreEqual(SettingType.Music, changedType);
            Assert.IsFalse(changedValue);
            Assert.AreEqual(1, _provider.DirtyCount);
        }

        [Test]
        public void SetEnabled_SameValue_DoesNothing()
        {
            int raisedCount = 0;
            _handler.SettingChanged += (type, isEnabled) => raisedCount++;

            _handler.SetEnabled(SettingType.Sound, SettingsModel.DefaultIsEnabled);

            Assert.AreEqual(0, raisedCount);
            Assert.AreEqual(0, _provider.DirtyCount);
        }

        [Test]
        public void IsEnabled_EntryMissingFromOldSave_ReturnsModelDefault()
        {
            _provider.Model.Entries.Clear();

            Assert.AreEqual(SettingsModel.DefaultIsEnabled, _handler.IsEnabled(SettingType.Vibration));
        }
    }
}
