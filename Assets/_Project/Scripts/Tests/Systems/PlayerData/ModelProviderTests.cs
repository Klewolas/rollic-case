using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using RollicCase.Systems.PlayerData;
using RollicCase.Systems.PlayerData.Core;
using RollicCase.Tests.Fakes;
using UnityEngine;
using UnityEngine.TestTools;

namespace RollicCase.Tests.Systems.PlayerData
{
    public sealed class ModelProviderTests
    {
        private const string Key = "core.json";

        private InMemoryDataStorage _storage;
        private JsonDataSerializer _serializer;
        private ManualSaveScheduler _scheduler;
        private ModelProvider<CoreModel> _provider;

        [SetUp]
        public void SetUp()
        {
            _storage = new InMemoryDataStorage();
            _serializer = new JsonDataSerializer();
            _scheduler = new ManualSaveScheduler();
            _provider = new ModelProvider<CoreModel>(Key, () => new CoreModel(), _storage, _serializer, _scheduler);
        }

        [TearDown]
        public void TearDown()
        {
            _provider.Dispose();
        }

        [Test]
        public void Model_NothingStored_ReturnsDefault()
        {
            Assert.AreEqual(0, _provider.Model.CompletedLevelCount);
        }

        [Test]
        public void Model_AccessedBeforeLoad_ReadsStoredModel()
        {
            _storage.Seed(Key, Serialize(4));

            Assert.AreEqual(4, _provider.Model.CompletedLevelCount);
        }

        [Test]
        public void LoadAsync_StoredModel_RestoresValues()
        {
            _storage.Seed(Key, Serialize(7));

            _provider.LoadAsync(CancellationToken.None).GetAwaiter().GetResult();

            Assert.AreEqual(7, _provider.Model.CompletedLevelCount);
        }

        [Test]
        public void LoadAsync_CorruptContent_ReturnsDefaultAndWarns()
        {
            _storage.Seed(Key, "{ not json");
            LogAssert.Expect(LogType.Warning, new Regex(Key));

            _provider.LoadAsync(CancellationToken.None).GetAwaiter().GetResult();

            Assert.AreEqual(0, _provider.Model.CompletedLevelCount);
        }

        [Test]
        public void MarkDirty_BeforeScheduledSave_DoesNotWrite()
        {
            _provider.MarkDirty();

            Assert.AreEqual(0, _storage.WriteCount);
            Assert.AreEqual(1, _scheduler.PendingCount);
        }

        [Test]
        public void MarkDirty_ManyTimesInOneFrame_WritesOnce()
        {
            _provider.Model.CompletedLevelCount = 1;
            _provider.MarkDirty();
            _provider.Model.CompletedLevelCount = 2;
            _provider.MarkDirty();
            _provider.MarkDirty();

            _scheduler.Tick();

            Assert.AreEqual(1, _storage.WriteCount);
            Assert.AreEqual(2, Deserialize(_storage.Read(Key)));
        }

        [Test]
        public void MarkDirty_AfterPreviousSave_WritesAgain()
        {
            _provider.MarkDirty();
            _scheduler.Tick();

            _provider.MarkDirty();
            _scheduler.Tick();

            Assert.AreEqual(2, _storage.WriteCount);
        }

        [Test]
        public void FlushAsync_NotDirty_DoesNotWrite()
        {
            _provider.FlushAsync(CancellationToken.None).GetAwaiter().GetResult();

            Assert.AreEqual(0, _storage.WriteCount);
        }

        [Test]
        public void FlushAsync_Dirty_WritesLatestState()
        {
            _provider.Model.CompletedLevelCount = 3;
            _provider.MarkDirty();

            _provider.FlushAsync(CancellationToken.None).GetAwaiter().GetResult();

            Assert.AreEqual(1, _storage.WriteCount);
            Assert.AreEqual(3, Deserialize(_storage.Read(Key)));
        }

        [Test]
        public void Flush_Dirty_WritesOnceEvenAfterScheduledSave()
        {
            _provider.MarkDirty();

            _provider.Flush();
            _scheduler.Tick();

            Assert.AreEqual(1, _storage.WriteCount);
        }

        private string Serialize(int completedLevelCount)
        {
            return _serializer.Serialize(new CoreModel { CompletedLevelCount = completedLevelCount });
        }

        private int Deserialize(string content)
        {
            return _serializer.Deserialize<CoreModel>(content).CompletedLevelCount;
        }
    }
}
