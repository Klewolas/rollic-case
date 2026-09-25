using NUnit.Framework;
using RollicCase.Systems.PlayerData.Core;
using RollicCase.Tests.Fakes;

namespace RollicCase.Tests.Systems.PlayerData.Core
{
    public sealed class CoreHandlerTests
    {
        private FakeModelProvider<CoreModel> _provider;
        private CoreHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _provider = new FakeModelProvider<CoreModel>(new CoreModel());
            _handler = new CoreHandler(_provider);
        }

        [Test]
        public void CurrentLevelNumber_NoCompletedLevels_ReturnsOne()
        {
            Assert.AreEqual(1, _handler.CurrentLevelNumber);
        }

        [Test]
        public void CompleteCurrentLevel_AdvancesLevelNumber()
        {
            _handler.CompleteCurrentLevel();
            _handler.CompleteCurrentLevel();

            Assert.AreEqual(3, _handler.CurrentLevelNumber);
        }

        [Test]
        public void CompleteCurrentLevel_MarksModelDirty()
        {
            _handler.CompleteCurrentLevel();

            Assert.AreEqual(1, _provider.DirtyCount);
        }
    }
}
