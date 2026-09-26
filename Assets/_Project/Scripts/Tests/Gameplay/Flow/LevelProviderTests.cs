using NUnit.Framework;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Flow;
using RollicCase.Systems.PlayerData.Core;
using RollicCase.Tests.Fakes;

namespace RollicCase.Tests.Gameplay.Flow
{
    public sealed class LevelProviderTests
    {
        private TestAssets _assets;
        private LevelData _first;
        private LevelData _second;
        private LevelCatalog _catalog;
        private CoreModel _core;
        private LevelProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _assets = new TestAssets();
            _first = _assets.CreateLevel(5, 5, 60);
            _second = _assets.CreateLevel(5, 5, 60);
            _catalog = _assets.CreateLevelCatalog(_first, _second);
            _core = new CoreModel();
            _provider = new LevelProvider(_catalog, new CoreHandler(new FakeModelProvider<CoreModel>(_core)));
        }

        [TearDown]
        public void TearDown()
        {
            _assets.DestroyAll();
        }

        [Test]
        public void CurrentLevel_NewPlayer_IsFirstCatalogLevel()
        {
            Assert.AreEqual(1, _provider.CurrentLevelNumber);
            Assert.AreSame(_first, _provider.CurrentLevel);
        }

        [Test]
        public void CurrentLevel_AfterOneWin_IsSecondCatalogLevel()
        {
            _core.CompletedLevelCount = 1;

            Assert.AreSame(_second, _provider.CurrentLevel);
        }

        [Test]
        public void CurrentLevel_PastTheLastLevel_LoopsBackAndKeepsCounting()
        {
            _core.CompletedLevelCount = 2;

            Assert.AreEqual(3, _provider.CurrentLevelNumber);
            Assert.AreSame(_first, _provider.CurrentLevel);
        }
    }
}
