using System.Collections.Generic;
using NUnit.Framework;
using RollicCase.Systems.PlayerData.Wallet;
using RollicCase.Systems.PlayerData.Wallet.Validators;
using RollicCase.Tests.Fakes;

namespace RollicCase.Tests.Systems.PlayerData.Wallet
{
    public sealed class WalletHandlerTests
    {
        private TestAssets _assets;
        private ConsumableDefinition _coin;
        private ConsumableDefinition _life;
        private ConsumableDefinition _unknown;
        private FakeModelProvider<WalletModel> _provider;
        private WalletHandler _wallet;

        [SetUp]
        public void SetUp()
        {
            _assets = new TestAssets();
            _coin = _assets.CreateConsumable("coin", 5);
            _life = _assets.CreateConsumable("life", 5, 5);
            _unknown = _assets.CreateConsumable("unknown", 0);
            ConsumableCatalog catalog = _assets.CreateCatalog(_coin, _life);

            var validators = new List<ITransactionValidator>
            {
                new NonZeroAmountValidator(),
                new KnownItemValidator(catalog),
                new NonNegativeBalanceValidator(),
                new CapacityValidator()
            };

            _provider = new FakeModelProvider<WalletModel>(new WalletModel());
            _wallet = new WalletHandler(_provider, validators);
        }

        [TearDown]
        public void TearDown()
        {
            _assets.DestroyAll();
        }

        [Test]
        public void GetBalance_ItemNeverChanged_ReturnsInitialAmount()
        {
            Assert.AreEqual(5, _wallet.GetBalance(_coin));
        }

        [Test]
        public void TryExecute_ValidIncrease_UpdatesBalance()
        {
            TransactionResult result = _wallet.TryExecute(new ConsumableTransaction(new ConsumableChange(_coin, 20)));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(25, _wallet.GetBalance(_coin));
        }

        [Test]
        public void TryExecute_BalanceWouldBeNegative_ReturnsInsufficientBalanceAndKeepsBalance()
        {
            TransactionResult result = _wallet.TryExecute(new ConsumableTransaction(new ConsumableChange(_coin, -6)));

            Assert.AreEqual(TransactionStatus.InsufficientBalance, result.Status);
            Assert.AreEqual(5, _wallet.GetBalance(_coin));
        }

        [Test]
        public void TryExecute_OneChangeInvalid_AppliesNoChange()
        {
            var transaction = new ConsumableTransaction(new ConsumableChange(_coin, 10), new ConsumableChange(_life, 1));

            TransactionResult result = _wallet.TryExecute(transaction);

            Assert.AreEqual(TransactionStatus.CapacityExceeded, result.Status);
            Assert.AreEqual(5, _wallet.GetBalance(_coin));
            Assert.AreEqual(5, _wallet.GetBalance(_life));
        }

        [Test]
        public void TryExecute_SameItemTwice_ValidatesAgainstProjectedBalance()
        {
            var transaction = new ConsumableTransaction(new ConsumableChange(_coin, -3), new ConsumableChange(_coin, -3));

            TransactionResult result = _wallet.TryExecute(transaction);

            Assert.AreEqual(TransactionStatus.InsufficientBalance, result.Status);
            Assert.AreEqual(5, _wallet.GetBalance(_coin));
        }

        [Test]
        public void TryExecute_FailedChange_IsReported()
        {
            var failing = new ConsumableChange(_unknown, 1);

            TransactionResult result = _wallet.TryExecute(new ConsumableTransaction(new ConsumableChange(_coin, 1), failing));

            Assert.AreEqual(TransactionStatus.UnknownItem, result.Status);
            Assert.AreSame(_unknown, result.FailedChange.Item);
        }

        [Test]
        public void TryExecute_EmptyTransaction_ReturnsEmptyTransaction()
        {
            TransactionResult result = _wallet.TryExecute(new ConsumableTransaction());

            Assert.AreEqual(TransactionStatus.EmptyTransaction, result.Status);
        }

        [Test]
        public void TryExecute_Success_RaisesBalanceChangedForEachChange()
        {
            var changes = new List<ConsumableBalanceChange>();
            _wallet.BalanceChanged += changes.Add;

            _wallet.TryExecute(new ConsumableTransaction(new ConsumableChange(_coin, 3), new ConsumableChange(_life, -2)));

            Assert.AreEqual(2, changes.Count);
            Assert.AreSame(_coin, changes[0].Item);
            Assert.AreEqual(5, changes[0].PreviousBalance);
            Assert.AreEqual(8, changes[0].NewBalance);
            Assert.AreSame(_life, changes[1].Item);
            Assert.AreEqual(3, changes[1].NewBalance);
        }

        [Test]
        public void TryExecute_Failure_DoesNotRaiseOrMarkDirty()
        {
            int raisedCount = 0;
            _wallet.BalanceChanged += change => raisedCount++;

            _wallet.TryExecute(new ConsumableTransaction(new ConsumableChange(_coin, -100)));

            Assert.AreEqual(0, raisedCount);
            Assert.AreEqual(0, _provider.DirtyCount);
        }

        [Test]
        public void TryExecute_Success_MarksWalletDirtyOnce()
        {
            _wallet.TryExecute(new ConsumableTransaction(new ConsumableChange(_coin, 1), new ConsumableChange(_life, -1)));

            Assert.AreEqual(1, _provider.DirtyCount);
        }
    }
}
