using NUnit.Framework;
using RollicCase.Tests.Fakes;
using RollicCase.UI.Popups;

namespace RollicCase.Tests.UI.Popups
{
    public sealed class PopupServiceTests
    {
        private FakePopup _first;
        private SecondFakePopup _second;
        private FakeArgsPopup _withArgs;
        private PopupService _service;

        [SetUp]
        public void SetUp()
        {
            _first = new FakePopup();
            _second = new SecondFakePopup();
            _withArgs = new FakeArgsPopup();

            var factory = new FakePopupFactory();
            factory.Register(_first);
            factory.Register(_second);
            factory.Register(_withArgs);
            _service = new PopupService(factory);
        }

        [Test]
        public void Show_ClosedPopup_OpensIt()
        {
            FakePopup popup = _service.Show<FakePopup>();

            Assert.AreSame(_first, popup);
            Assert.IsTrue(_first.IsOpen);
        }

        [Test]
        public void Show_AlreadyOpenPopup_DoesNotOpenItAgain()
        {
            _service.Show<FakePopup>();
            _service.Show<FakePopup>();

            Assert.AreEqual(1, _first.OpenCount);
        }

        [Test]
        public void CloseTop_TwoOpenPopups_ClosesOnlyTheLastOpened()
        {
            _service.Show<FakePopup>();
            _service.Show<SecondFakePopup>();

            bool closed = _service.CloseTop();

            Assert.IsTrue(closed);
            Assert.IsFalse(_second.IsOpen);
            Assert.IsTrue(_first.IsOpen);
        }

        [Test]
        public void CloseTop_CalledTwice_ClosesInReverseOrder()
        {
            _service.Show<FakePopup>();
            _service.Show<SecondFakePopup>();

            _service.CloseTop();
            _service.CloseTop();

            Assert.IsFalse(_first.IsOpen);
            Assert.IsFalse(_second.IsOpen);
        }

        [Test]
        public void CloseTop_NoOpenPopup_ReturnsFalse()
        {
            Assert.IsFalse(_service.CloseTop());
        }

        [Test]
        public void CloseRequested_PopupBelowTop_ClosesOnlyThatPopup()
        {
            _service.Show<FakePopup>();
            _service.Show<SecondFakePopup>();

            _first.RequestClose();

            Assert.IsFalse(_first.IsOpen);
            Assert.IsTrue(_second.IsOpen);
        }

        [Test]
        public void CloseRequested_ClosedPopup_IsIgnored()
        {
            _service.Show<FakePopup>();
            _service.CloseTop();
            _service.Show<SecondFakePopup>();

            _first.RequestClose();

            Assert.IsTrue(_second.IsOpen);
            Assert.IsTrue(_service.CloseTop());
            Assert.IsFalse(_service.CloseTop());
        }

        [Test]
        public void Show_ClosedPopupAgain_OpensItAgain()
        {
            _service.Show<FakePopup>();
            _service.CloseTop();

            _service.Show<FakePopup>();

            Assert.AreEqual(2, _first.OpenCount);
            Assert.IsTrue(_first.IsOpen);
        }

        [Test]
        public void Show_WithArgs_BindsBeforeOpening()
        {
            _service.Show<FakeArgsPopup, int>(42);

            Assert.AreEqual(42, _withArgs.BoundArgs);
            Assert.IsTrue(_withArgs.WasBoundBeforeOpen);
            Assert.IsTrue(_withArgs.IsOpen);
        }

        [Test]
        public void DismissTop_TopCanBeDismissed_ClosesIt()
        {
            _service.Show<FakePopup>();

            bool dismissed = _service.DismissTop();

            Assert.IsTrue(dismissed);
            Assert.IsFalse(_first.IsOpen);
        }

        [Test]
        public void DismissTop_TopNeedsADecision_KeepsItOpen()
        {
            _first.CanDismiss = false;
            _service.Show<FakePopup>();

            bool dismissed = _service.DismissTop();

            Assert.IsFalse(dismissed);
            Assert.IsTrue(_first.IsOpen);
        }

        private sealed class SecondFakePopup : FakePopup
        {
        }
    }
}
