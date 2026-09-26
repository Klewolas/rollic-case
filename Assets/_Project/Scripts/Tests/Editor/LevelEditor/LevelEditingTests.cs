using NUnit.Framework;
using RollicCase.Gameplay.Data;
using RollicCase.Tests.Fakes;
using RollicCase.Tests.Gameplay;
using RollicCase.Editor.LevelEditor.Editing;
using UnityEngine;

namespace RollicCase.Tests.Editor.LevelEditor
{
    public sealed class LevelEditingTests
    {
        private const int Size = 5;

        private readonly LevelEditing _editing = new LevelEditing();
        private TestAssets _assets;
        private BlockColor _red;
        private BlockColor _blue;
        private LevelData _level;

        [SetUp]
        public void SetUp()
        {
            _assets = new TestAssets();
            _red = _assets.CreateBlockColor();
            _blue = _assets.CreateBlockColor();
            _level = _assets.CreateLevel(Size, Size, 60);
        }

        [TearDown]
        public void TearDown()
        {
            _assets.DestroyAll();
        }

        [Test]
        public void TryAddBlock_FreeCells_AddsBlock()
        {
            bool added = _editing.TryAddBlock(_level, _red, new Vector2Int(1, 1), Shapes.L);

            Assert.IsTrue(added);
            Assert.AreEqual(1, _level.Blocks.Count);
            Assert.AreEqual(0, _editing.FindBlockAt(_level, new Vector2Int(1, 2)));
        }

        [Test]
        public void TryAddBlock_PartlyOutsideBoard_IsRejected()
        {
            Assert.IsFalse(_editing.TryAddBlock(_level, _red, new Vector2Int(4, 0), Shapes.Horizontal2));
            Assert.AreEqual(0, _level.Blocks.Count);
        }

        [Test]
        public void TryAddBlock_OnAnotherBlock_IsRejected()
        {
            _editing.TryAddBlock(_level, _red, new Vector2Int(0, 0), Shapes.Horizontal2);

            Assert.IsFalse(_editing.TryAddBlock(_level, _blue, new Vector2Int(1, 0), Shapes.Single));
        }

        [Test]
        public void FindBlockAt_EmptyCell_ReturnsNone()
        {
            Assert.AreEqual(LevelEditing.None, _editing.FindBlockAt(_level, new Vector2Int(2, 2)));
        }

        [Test]
        public void TryMoveBlock_ToFreeCells_MovesIt()
        {
            _editing.TryAddBlock(_level, _red, new Vector2Int(0, 0), Shapes.Horizontal2);

            bool moved = _editing.TryMoveBlock(_level, 0, new Vector2Int(1, 0));

            Assert.IsTrue(moved);
            Assert.AreEqual(new Vector2Int(1, 0), _level.Blocks[0].Origin);
        }

        [Test]
        public void TryMoveBlock_OntoAnotherBlock_KeepsPosition()
        {
            _editing.TryAddBlock(_level, _red, new Vector2Int(0, 0), Shapes.Single);
            _editing.TryAddBlock(_level, _blue, new Vector2Int(2, 0), Shapes.Single);

            Assert.IsFalse(_editing.TryMoveBlock(_level, 0, new Vector2Int(2, 0)));
            Assert.AreEqual(new Vector2Int(0, 0), _level.Blocks[0].Origin);
        }

        [Test]
        public void TryRotateBlock_Fits_TurnsCellsAndKeepsColor()
        {
            _editing.TryAddBlock(_level, _red, new Vector2Int(0, 0), Shapes.Horizontal2);

            bool rotated = _editing.TryRotateBlock(_level, 0);

            Assert.IsTrue(rotated);
            CollectionAssert.AreEquivalent(Shapes.Vertical2, _level.Blocks[0].Cells);
            Assert.AreSame(_red, _level.Blocks[0].Color);
        }

        [Test]
        public void TryRotateBlock_WouldLeaveBoard_KeepsShape()
        {
            _editing.TryAddBlock(_level, _red, new Vector2Int(0, 4), Shapes.Horizontal2);

            Assert.IsFalse(_editing.TryRotateBlock(_level, 0));
            CollectionAssert.AreEquivalent(Shapes.Horizontal2, _level.Blocks[0].Cells);
        }

        [Test]
        public void RemoveBlock_Index_FreesCells()
        {
            _editing.TryAddBlock(_level, _red, new Vector2Int(0, 0), Shapes.Single);

            _editing.RemoveBlock(_level, 0);

            Assert.AreEqual(0, _level.Blocks.Count);
        }

        [Test]
        public void TryAddDoor_FreeSpot_AddsOneCellDoor()
        {
            bool added = _editing.TryAddDoor(_level, BoardSide.Top, 2, _red);

            Assert.IsTrue(added);
            Assert.AreEqual(1, _level.Doors[0].Length);
            Assert.AreEqual(0, _editing.FindDoorAt(_level, BoardSide.Top, 2));
            Assert.AreEqual(LevelEditing.None, _editing.FindDoorAt(_level, BoardSide.Bottom, 2));
        }

        [Test]
        public void TryAddDoor_OnExistingDoor_IsRejected()
        {
            _editing.TryAddDoor(_level, BoardSide.Top, 2, _red);

            Assert.IsFalse(_editing.TryAddDoor(_level, BoardSide.Top, 2, _blue));
        }

        [Test]
        public void TryAddDoor_OutsideSide_IsRejected()
        {
            Assert.IsFalse(_editing.TryAddDoor(_level, BoardSide.Left, Size, _red));
        }

        [Test]
        public void TrySetDoorSpan_FreeSpan_ResizesDoor()
        {
            _editing.TryAddDoor(_level, BoardSide.Top, 1, _red);

            bool resized = _editing.TrySetDoorSpan(_level, 0, 1, 3);

            Assert.IsTrue(resized);
            Assert.AreEqual(3, _level.Doors[0].Length);
            Assert.AreEqual(0, _editing.FindDoorAt(_level, BoardSide.Top, 3));
        }

        [Test]
        public void TrySetDoorSpan_OverlapsAnotherDoor_KeepsSpan()
        {
            _editing.TryAddDoor(_level, BoardSide.Top, 0, _red);
            _editing.TryAddDoor(_level, BoardSide.Top, 3, _blue);

            Assert.IsFalse(_editing.TrySetDoorSpan(_level, 0, 0, 4));
            Assert.AreEqual(1, _level.Doors[0].Length);
        }

        [Test]
        public void TrySetDoorSpan_PastSideEnd_KeepsSpan()
        {
            _editing.TryAddDoor(_level, BoardSide.Right, 3, _red);

            Assert.IsFalse(_editing.TrySetDoorSpan(_level, 0, 3, 3));
        }

        [Test]
        public void RecolorDoor_NewColor_KeepsSpan()
        {
            _editing.TryAddDoor(_level, BoardSide.Top, 1, _red);
            _editing.TrySetDoorSpan(_level, 0, 1, 2);

            _editing.RecolorDoor(_level, 0, _blue);

            Assert.AreSame(_blue, _level.Doors[0].Color);
            Assert.AreEqual(2, _level.Doors[0].Length);
        }

        [Test]
        public void RemoveDoor_Index_RemovesIt()
        {
            _editing.TryAddDoor(_level, BoardSide.Top, 1, _red);

            _editing.RemoveDoor(_level, 0);

            Assert.AreEqual(0, _level.Doors.Count);
        }

        [Test]
        public void CountContentOutside_SmallerBoard_CountsBlocksAndDoorsThatNoLongerFit()
        {
            _editing.TryAddBlock(_level, _red, new Vector2Int(0, 0), Shapes.Single);
            _editing.TryAddBlock(_level, _blue, new Vector2Int(3, 3), Shapes.Square2);
            _editing.TryAddDoor(_level, BoardSide.Top, 4, _red);
            _editing.TryAddDoor(_level, BoardSide.Top, 0, _blue);

            Assert.AreEqual(2, _editing.CountContentOutside(_level, 4, 4));
        }

        [Test]
        public void Resize_SmallerBoard_RemovesOnlyContentThatNoLongerFits()
        {
            _editing.TryAddBlock(_level, _red, new Vector2Int(0, 0), Shapes.Single);
            _editing.TryAddBlock(_level, _blue, new Vector2Int(3, 3), Shapes.Square2);
            _editing.TryAddDoor(_level, BoardSide.Top, 4, _red);
            _editing.TryAddDoor(_level, BoardSide.Top, 0, _blue);

            _editing.Resize(_level, 4, 4);

            Assert.AreEqual(4, _level.Width);
            Assert.AreEqual(4, _level.Height);
            Assert.AreEqual(1, _level.Blocks.Count);
            Assert.AreSame(_red, _level.Blocks[0].Color);
            Assert.AreEqual(1, _level.Doors.Count);
            Assert.AreSame(_blue, _level.Doors[0].Color);
        }
    }
}
