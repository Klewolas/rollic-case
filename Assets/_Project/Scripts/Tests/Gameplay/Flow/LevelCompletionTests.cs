using System.Collections.Generic;
using NUnit.Framework;
using RollicCase.Gameplay.Flow;
using RollicCase.Systems.PlayerData.Core;
using RollicCase.Systems.PlayerData.Wallet;
using RollicCase.Systems.PlayerData.Wallet.Validators;
using RollicCase.Tests.Fakes;

namespace RollicCase.Tests.Gameplay.Flow
{
    public sealed class LevelCompletionTests
    {
        private const int StartCoins = 10;
        private const int Reward = 25;

        private TestAssets _assets;
        private ConsumableDefinition _coin;
        private CoreHandler _core;
        private WalletHandler _wallet;
        private LevelCompletion _completion;

        [SetUp]
        public void SetUp()
        {
            _assets = new TestAssets();
            _coin = _assets.CreateConsumable("coin", StartCoins);
            var validators = new List<ITransactionValidator> { new KnownItemValidator(_assets.CreateCatalog(_coin)) };

            _core = new CoreHandler(new FakeModelProvider<CoreModel>(new CoreModel()));
            _wallet = new WalletHandler(new FakeModelProvider<WalletModel>(new WalletModel()), validators);
            _completion = new LevelCompletion(_core, _wallet, _assets.CreateLevelReward(_coin, Reward));
        }

        [TearDown]
        public void TearDown()
        {
            _assets.DestroyAll();
        }

        [Test]
        public void TryComplete_FirstTime_PaysTheReward()
        {
            bool completed = _completion.TryComplete();

            Assert.IsTrue(completed);
            Assert.AreEqual(StartCoins + Reward, _wallet.GetBalance(_coin));
        }

        [Test]
        public void TryComplete_FirstTime_AdvancesToTheNextLevel()
        {
            _completion.TryComplete();

            Assert.AreEqual(2, _core.CurrentLevelNumber);
        }

        [Test]
        public void TryComplete_SecondTime_ReturnsFalseAndChangesNothing()
        {
            _completion.TryComplete();

            bool completedAgain = _completion.TryComplete();

            Assert.IsFalse(completedAgain);
            Assert.AreEqual(StartCoins + Reward, _wallet.GetBalance(_coin));
            Assert.AreEqual(2, _core.CurrentLevelNumber);
        }
    }
}
