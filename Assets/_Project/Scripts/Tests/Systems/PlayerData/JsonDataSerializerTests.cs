using System;
using NUnit.Framework;
using RollicCase.Systems.PlayerData;
using RollicCase.Systems.PlayerData.Settings;
using RollicCase.Systems.PlayerData.Wallet;

namespace RollicCase.Tests.Systems.PlayerData
{
    public sealed class JsonDataSerializerTests
    {
        private readonly JsonDataSerializer _serializer = new JsonDataSerializer();

        [Test]
        public void Deserialize_SerializedModel_RestoresAllEntries()
        {
            var model = new WalletModel();
            model.Balances.Add(new ConsumableBalance("coin", 120));
            model.Balances.Add(new ConsumableBalance("hammer", 2));

            WalletModel restored = _serializer.Deserialize<WalletModel>(_serializer.Serialize(model));

            Assert.AreEqual(2, restored.Balances.Count);
            Assert.AreEqual("coin", restored.Balances[0].ItemId);
            Assert.AreEqual(120, restored.Balances[0].Amount);
            Assert.AreEqual("hammer", restored.Balances[1].ItemId);
            Assert.AreEqual(2, restored.Balances[1].Amount);
        }

        [Test]
        public void Deserialize_SettingsModel_ReplacesConstructorDefaultsWithStoredEntries()
        {
            var model = new SettingsModel();
            model.Entries[0].IsEnabled = false;

            SettingsModel restored = _serializer.Deserialize<SettingsModel>(_serializer.Serialize(model));

            Assert.AreEqual(model.Entries.Count, restored.Entries.Count);
            Assert.IsFalse(restored.Entries[0].IsEnabled);
        }

        [Test]
        public void Deserialize_InvalidContent_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _serializer.Deserialize<WalletModel>("{ not json"));
        }
    }
}
