using System.Collections.Generic;
using NUnit.Framework;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using RollicCase.Tests.Fakes;
using UnityEngine;

namespace RollicCase.Tests.Gameplay.Logic
{
    public sealed class BlockDragTests
    {
        private const int Size = 5;
        private const float ExitThreshold = 0.35f;
        private const float Duration = 60f;
        private const float Tolerance = 1e-4f;

        private TestAssets _assets;
        private BlockColor _red;
        private BlockColor _blue;

        [SetUp]
        public void SetUp()
        {
            _assets = new TestAssets();
            _red = _assets.CreateBlockColor();
            _blue = _assets.CreateBlockColor();
        }

        [TearDown]
        public void TearDown()
        {
            _assets.DestroyAll();
        }

        [Test]
        public void Update_FreeRow_MovesWholeCellsAndKeepsTheFraction()
        {
            BlockModel block = Block(_red, 0, 0);
            BlockDrag drag = Drag(out _, block);

            BlockDragResult result = drag.Update(new Vector2(2.4f, 0f));

            Assert.AreEqual(new Vector2Int(2, 0), block.Position);
            AssertPosition(new Vector2(2.4f, 0f), result.Position);
        }

        [Test]
        public void Update_TowardAnotherBlock_StopsBesideIt()
        {
            BlockModel block = Block(_red, 0, 0);
            BlockDrag drag = Drag(out _, block, Block(_blue, 2, 0));

            BlockDragResult result = drag.Update(new Vector2(3f, 0f));

            Assert.AreEqual(new Vector2Int(1, 0), block.Position);
            AssertPosition(new Vector2(1f, 0f), result.Position);
        }

        [Test]
        public void Update_BlockedOnTheMainAxis_GoesAroundThroughTheFreeAxis()
        {
            BlockModel block = Block(_red, 0, 0);
            BlockDrag drag = Drag(out _, block, Block(_blue, 1, 0));

            drag.Update(new Vector2(2f, 1f));

            Assert.AreEqual(new Vector2Int(2, 1), block.Position);
        }

        [Test]
        public void Update_DiagonalOffset_KeepsTheFractionOnTheDominantAxisOnly()
        {
            BlockModel block = Block(_red, 0, 0);
            BlockDrag drag = Drag(out _, block);

            BlockDragResult result = drag.Update(new Vector2(0.3f, 0.6f));

            AssertPosition(new Vector2(0f, 0.6f), result.Position);
        }

        [Test]
        public void Update_BackToTheStart_ReturnsTheBlock()
        {
            BlockModel block = Block(_red, 0, 0);
            BlockDrag drag = Drag(out _, block);

            drag.Update(new Vector2(2f, 0f));
            drag.Update(Vector2.zero);

            Assert.AreEqual(new Vector2Int(0, 0), block.Position);
        }

        [Test]
        public void Update_PastTheWallWithoutDoor_StaysAtTheWall()
        {
            BlockModel block = Block(_red, 2, 4);
            BlockDrag drag = Drag(out _, block);

            BlockDragResult result = drag.Update(new Vector2(0f, 0.8f));

            Assert.IsFalse(result.HasExited);
            AssertPosition(new Vector2(2f, 4f), result.Position);
        }

        [Test]
        public void Update_PastTheThresholdTowardMatchingDoor_ExitsTheBlock()
        {
            BlockModel block = Block(_red, 2, 4);
            BlockDrag drag = Drag(out LevelSession session, block, door: new DoorModel(BoardSide.Top, 2, 1, _red));

            BlockDragResult result = drag.Update(new Vector2(0f, ExitThreshold + 0.1f));

            Assert.IsTrue(result.HasExited);
            Assert.AreEqual(BoardSide.Top, result.ExitSide);
            Assert.AreEqual(0, session.Board.Blocks.Count);
        }

        [Test]
        public void Update_BelowTheThresholdTowardMatchingDoor_DoesNotExit()
        {
            BlockModel block = Block(_red, 2, 4);
            BlockDrag drag = Drag(out LevelSession session, block, door: new DoorModel(BoardSide.Top, 2, 1, _red));

            BlockDragResult result = drag.Update(new Vector2(0f, ExitThreshold - 0.1f));

            Assert.IsFalse(result.HasExited);
            Assert.AreEqual(1, session.Board.Blocks.Count);
        }

        [Test]
        public void Update_FarPastTheDoor_SlidesToTheWallAndExits()
        {
            BlockModel block = Block(_red, 2, 1);
            BlockDrag drag = Drag(out LevelSession session, block, door: new DoorModel(BoardSide.Top, 2, 1, _red));

            BlockDragResult result = drag.Update(new Vector2(0f, 5f));

            Assert.IsTrue(result.HasExited);
            Assert.AreEqual(0, session.Board.Blocks.Count);
        }

        [Test]
        public void Update_AfterTheLevelFailed_DoesNotMove()
        {
            BlockModel block = Block(_red, 0, 0);
            BlockDrag drag = Drag(out LevelSession session, block);
            session.Tick(float.MaxValue);

            BlockDragResult result = drag.Update(new Vector2(2f, 0f));

            Assert.AreEqual(new Vector2Int(0, 0), block.Position);
            AssertPosition(Vector2.zero, result.Position);
        }

        [Test]
        public void End_PastHalfACell_ReturnsTheNearestCell()
        {
            BlockModel block = Block(_red, 0, 0);
            BlockDrag drag = Drag(out _, block);
            drag.Update(new Vector2(1.7f, 0f));

            Assert.AreEqual(new Vector2Int(2, 0), drag.End());
        }

        private BlockDrag Drag(out LevelSession session, BlockModel dragged, BlockModel other = null, DoorModel door = null)
        {
            var blocks = new List<BlockModel> { dragged };
            var doors = new List<DoorModel>();

            if (other != null)
            {
                blocks.Add(other);
            }

            if (door != null)
            {
                doors.Add(door);
            }

            session = new LevelSession(new BoardModel(Size, Size, blocks, doors), new LevelTimer(Duration));
            var drag = new BlockDrag(session, ExitThreshold);
            drag.Begin(dragged);
            return drag;
        }

        private static BlockModel Block(BlockColor color, int x, int y)
        {
            return new BlockModel(0, color, new Vector2Int(x, y), Shapes.Single);
        }

        private static void AssertPosition(Vector2 expected, Vector2 actual)
        {
            Assert.AreEqual(expected.x, actual.x, Tolerance);
            Assert.AreEqual(expected.y, actual.y, Tolerance);
        }
    }
}
