using NUnit.Framework;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using RollicCase.Tests.Fakes;
using UnityEngine;

namespace RollicCase.Tests.Gameplay.Logic
{
    public sealed class LevelSessionTests
    {
        private const float Duration = 30f;

        private TestAssets _assets;
        private BlockModel _red;
        private BlockModel _blue;
        private LevelSession _session;

        [SetUp]
        public void SetUp()
        {
            _assets = new TestAssets();
            BlockColor red = _assets.CreateBlockColor();
            BlockColor blue = _assets.CreateBlockColor();
            _red = new BlockModel(0, red, new Vector2Int(2, 4), Shapes.Single);
            _blue = new BlockModel(1, blue, new Vector2Int(0, 0), Shapes.Single);

            var board = new BoardModel(5, 5, new[] { _red, _blue }, new[]
            {
                new DoorModel(BoardSide.Top, 2, 1, red),
                new DoorModel(BoardSide.Bottom, 0, 1, blue)
            });

            _session = new LevelSession(board, new LevelTimer(Duration));
        }

        [TearDown]
        public void TearDown()
        {
            _assets.DestroyAll();
        }

        [Test]
        public void State_NewSession_IsPlaying()
        {
            Assert.AreEqual(LevelState.Playing, _session.State);
        }

        [Test]
        public void TryExit_MatchingDoor_RemovesBlockAndKeepsPlaying()
        {
            bool exited = _session.TryExit(_red, BoardSide.Top);

            Assert.IsTrue(exited);
            Assert.AreEqual(1, _session.Board.Blocks.Count);
            Assert.AreEqual(LevelState.Playing, _session.State);
        }

        [Test]
        public void TryExit_NoMatchingDoor_ReturnsFalse()
        {
            Assert.IsFalse(_session.TryExit(_red, BoardSide.Bottom));
            Assert.AreEqual(2, _session.Board.Blocks.Count);
        }

        [Test]
        public void TryExit_LastBlock_WinsAndRaisesStateChanged()
        {
            LevelState? raised = null;
            _session.StateChanged += state => raised = state;

            _session.TryExit(_red, BoardSide.Top);
            _session.TryExit(_blue, BoardSide.Bottom);

            Assert.AreEqual(LevelState.Won, _session.State);
            Assert.AreEqual(LevelState.Won, raised);
        }

        [Test]
        public void Tick_TimeRunsOut_Fails()
        {
            _session.Tick(Duration);

            Assert.AreEqual(LevelState.Failed, _session.State);
        }

        [Test]
        public void Tick_AfterWin_DoesNotFail()
        {
            _session.TryExit(_red, BoardSide.Top);
            _session.TryExit(_blue, BoardSide.Bottom);

            _session.Tick(Duration);

            Assert.AreEqual(LevelState.Won, _session.State);
        }

        [Test]
        public void Move_StepsBeyondFreeDistance_ClampsToFreeDistance()
        {
            int moved = _session.Move(_blue, Vector2Int.right, 10);

            Assert.AreEqual(4, moved);
            Assert.AreEqual(new Vector2Int(4, 0), _blue.Position);
        }

        [Test]
        public void Move_AfterFail_DoesNothing()
        {
            _session.Tick(Duration);

            int moved = _session.Move(_blue, Vector2Int.right, 1);

            Assert.AreEqual(0, moved);
            Assert.AreEqual(new Vector2Int(0, 0), _blue.Position);
        }

        [Test]
        public void TryExit_AfterFail_ReturnsFalse()
        {
            _session.Tick(Duration);

            Assert.IsFalse(_session.TryExit(_red, BoardSide.Top));
        }
    }
}
