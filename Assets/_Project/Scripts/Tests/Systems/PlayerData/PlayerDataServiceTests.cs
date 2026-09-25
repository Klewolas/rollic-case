using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using RollicCase.Systems.PlayerData;
using RollicCase.Tests.Fakes;

namespace RollicCase.Tests.Systems.PlayerData
{
    public sealed class PlayerDataServiceTests
    {
        private FakePersistentModel _first;
        private FakePersistentModel _second;
        private PlayerDataService _service;

        [SetUp]
        public void SetUp()
        {
            _first = new FakePersistentModel();
            _second = new FakePersistentModel();
            _service = new PlayerDataService(new List<IPersistentModel> { _first, _second });
        }

        [Test]
        public void LoadAllAsync_LoadsEveryModel()
        {
            _service.LoadAllAsync(CancellationToken.None).GetAwaiter().GetResult();

            Assert.AreEqual(1, _first.LoadCount);
            Assert.AreEqual(1, _second.LoadCount);
        }

        [Test]
        public void OnBeforeSceneChangeAsync_FlushesEveryModel()
        {
            _service.OnBeforeSceneChangeAsync(CancellationToken.None).GetAwaiter().GetResult();

            Assert.AreEqual(1, _first.FlushAsyncCount);
            Assert.AreEqual(1, _second.FlushAsyncCount);
        }
    }
}
