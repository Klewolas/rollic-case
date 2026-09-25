using NUnit.Framework;
using RollicCase.Systems.PlayerData.Wallet;
using RollicCase.Systems.PlayerData.Wallet.Validators;
using RollicCase.Tests.Fakes;

namespace RollicCase.Tests.Systems.PlayerData.Wallet
{
    public sealed class TransactionValidatorTests
    {
        private TestAssets _assets;
        private ConsumableDefinition _coin;
        private ConsumableDefinition _life;

        [SetUp]
        public void SetUp()
        {
            _assets = new TestAssets();
            _coin = _assets.CreateConsumable("coin", 0);
            _life = _assets.CreateConsumable("life", 5, 5);
        }

        [TearDown]
        public void TearDown()
        {
            _assets.DestroyAll();
        }

        [Test]
        public void NonZeroAmount_ZeroAmount_ReturnsInvalidAmount()
        {
            var validator = new NonZeroAmountValidator();

            Assert.AreEqual(TransactionStatus.InvalidAmount, validator.Validate(new ConsumableChange(_coin, 0), 10));
        }

        [Test]
        public void NonZeroAmount_NonZeroAmount_ReturnsSuccess()
        {
            var validator = new NonZeroAmountValidator();

            Assert.AreEqual(TransactionStatus.Success, validator.Validate(new ConsumableChange(_coin, -1), 10));
        }

        [Test]
        public void KnownItem_ItemInCatalog_ReturnsSuccess()
        {
            var validator = new KnownItemValidator(_assets.CreateCatalog(_coin));

            Assert.AreEqual(TransactionStatus.Success, validator.Validate(new ConsumableChange(_coin, 1), 0));
        }

        [Test]
        public void KnownItem_ItemNotInCatalog_ReturnsUnknownItem()
        {
            var validator = new KnownItemValidator(_assets.CreateCatalog(_coin));

            Assert.AreEqual(TransactionStatus.UnknownItem, validator.Validate(new ConsumableChange(_life, 1), 0));
        }

        [Test]
        public void KnownItem_NullItem_ReturnsUnknownItem()
        {
            var validator = new KnownItemValidator(_assets.CreateCatalog(_coin));

            Assert.AreEqual(TransactionStatus.UnknownItem, validator.Validate(new ConsumableChange(null, 1), 0));
        }

        [Test]
        public void NonNegativeBalance_ResultIsZero_ReturnsSuccess()
        {
            var validator = new NonNegativeBalanceValidator();

            Assert.AreEqual(TransactionStatus.Success, validator.Validate(new ConsumableChange(_coin, -3), 3));
        }

        [Test]
        public void NonNegativeBalance_ResultBelowZero_ReturnsInsufficientBalance()
        {
            var validator = new NonNegativeBalanceValidator();

            Assert.AreEqual(TransactionStatus.InsufficientBalance, validator.Validate(new ConsumableChange(_coin, -4), 3));
        }

        [Test]
        public void Capacity_ResultAtCapacity_ReturnsSuccess()
        {
            var validator = new CapacityValidator();

            Assert.AreEqual(TransactionStatus.Success, validator.Validate(new ConsumableChange(_life, 1), 4));
        }

        [Test]
        public void Capacity_ResultAboveCapacity_ReturnsCapacityExceeded()
        {
            var validator = new CapacityValidator();

            Assert.AreEqual(TransactionStatus.CapacityExceeded, validator.Validate(new ConsumableChange(_life, 2), 4));
        }

        [Test]
        public void Capacity_ItemWithoutCapacity_ReturnsSuccess()
        {
            var validator = new CapacityValidator();

            Assert.AreEqual(TransactionStatus.Success, validator.Validate(new ConsumableChange(_coin, 100000), 0));
        }

        [Test]
        public void Capacity_DecreaseWhileAboveCapacity_ReturnsSuccess()
        {
            var validator = new CapacityValidator();

            Assert.AreEqual(TransactionStatus.Success, validator.Validate(new ConsumableChange(_life, -1), 8));
        }
    }
}
