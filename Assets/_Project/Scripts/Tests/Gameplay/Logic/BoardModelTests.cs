using System;
using System.Collections.Generic;
using NUnit.Framework;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using RollicCase.Tests.Fakes;
using UnityEngine;

namespace RollicCase.Tests.Gameplay.Logic
{
    public sealed class BoardModelTests
    {
        private const int Size = 5;
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
        public void Constructor_BlockOutsideBoard_Throws()
        {
            BlockModel block = Block(_red, 4, 0, Shapes.Horizontal2);

            Assert.Throws<ArgumentException>(() => Board(new[] { block }));
        }

        [Test]
        public void Constructor_OverlappingBlocks_Throws()
        {
            BlockModel first = Block(_red, 0, 0, Shapes.Horizontal2);
            BlockModel second = Block(_blue, 1, 0, Shapes.Single);

            Assert.Throws<ArgumentException>(() => Board(new[] { first, second }));
        }

        [Test]
        public void GetBlockAt_CoveredCell_ReturnsBlock()
        {
            BlockModel block = Block(_red, 1, 1, Shapes.L);
            BoardModel board = Board(new[] { block });

            Assert.AreSame(block, board.GetBlockAt(new Vector2Int(1, 2)));
            Assert.IsNull(board.GetBlockAt(new Vector2Int(2, 2)));
            Assert.IsNull(board.GetBlockAt(new Vector2Int(-1, 0)));
        }

        [Test]
        public void GetFreeTravel_OpenRow_ReturnsDistanceToWalls()
        {
            BlockModel block = Block(_red, 1, 2, Shapes.Single);
            BoardModel board = Board(new[] { block });

            Assert.AreEqual(3f, board.GetFreeTravel(block, new Vector2(1f, 2f), Vector2Int.right), Tolerance);
            Assert.AreEqual(1f, board.GetFreeTravel(block, new Vector2(1f, 2f), Vector2Int.left), Tolerance);
        }

        [Test]
        public void GetFreeTravel_OtherBlockInPath_StopsFlushAgainstIt()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Single);
            BlockModel blocker = Block(_blue, 3, 0, Shapes.Single);
            BoardModel board = Board(new[] { block, blocker });

            Assert.AreEqual(2f, board.GetFreeTravel(block, new Vector2(0f, 0f), Vector2Int.right), Tolerance);
        }

        [Test]
        public void GetFreeTravel_BetweenCells_ReturnsTheRemainingFraction()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Single);
            BlockModel blocker = Block(_blue, 3, 0, Shapes.Single);
            BoardModel board = Board(new[] { block, blocker });

            Assert.AreEqual(1.6f, board.GetFreeTravel(block, new Vector2(0.4f, 0f), Vector2Int.right), Tolerance);
        }

        [Test]
        public void GetFreeTravel_TouchingTwoRows_IsStoppedByEitherRow()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Single);
            BlockModel blocker = Block(_blue, 2, 1, Shapes.Single);
            BoardModel board = Board(new[] { block, blocker });

            Assert.AreEqual(1f, board.GetFreeTravel(block, new Vector2(0f, 0.5f), Vector2Int.right), Tolerance);
        }

        [Test]
        public void GetFreeTravel_OneCellOfShapeBlocked_LimitsWholeShape()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.L);
            BlockModel blocker = Block(_blue, 3, 1, Shapes.Single);
            BoardModel board = Board(new[] { block, blocker });

            Assert.AreEqual(2f, board.GetFreeTravel(block, new Vector2(0f, 0f), Vector2Int.right), Tolerance);
        }

        [Test]
        public void GetFreeTravel_OwnCellsInPath_AreNotObstacles()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Vertical2);
            BoardModel board = Board(new[] { block });

            Assert.AreEqual(3f, board.GetFreeTravel(block, new Vector2(0f, 0f), Vector2Int.up), Tolerance);
        }

        [Test]
        public void GetFreeTravel_MoveRuleForbidsDirection_ReturnsZero()
        {
            BlockModel block = Block(_red, 1, 1, Shapes.Single, new FakeMoveRule(Vector2Int.right));
            BoardModel board = Board(new[] { block });

            Assert.AreEqual(0f, board.GetFreeTravel(block, new Vector2(1f, 1f), Vector2Int.right), Tolerance);
        }

        [Test]
        public void GetFreeTravel_MoveRuleForbidsOtherDirection_ReturnsFreeCells()
        {
            BlockModel block = Block(_red, 1, 1, Shapes.Single, new FakeMoveRule(Vector2Int.right));
            BoardModel board = Board(new[] { block });

            Assert.AreEqual(1f, board.GetFreeTravel(block, new Vector2(1f, 1f), Vector2Int.left), Tolerance);
        }

        [Test]
        public void CanPlace_FreeCells_ReturnsTrue()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Horizontal2);
            BoardModel board = Board(new[] { block });

            Assert.IsTrue(board.CanPlace(block, new Vector2Int(1, 0)));
        }

        [Test]
        public void CanPlace_OnAnotherBlock_ReturnsFalse()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Single);
            BlockModel other = Block(_blue, 2, 2, Shapes.Single);
            BoardModel board = Board(new[] { block, other });

            Assert.IsFalse(board.CanPlace(block, new Vector2Int(2, 2)));
        }

        [Test]
        public void CanPlace_PartlyOutsideBoard_ReturnsFalse()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Horizontal2);
            BoardModel board = Board(new[] { block });

            Assert.IsFalse(board.CanPlace(block, new Vector2Int(4, 0)));
        }

        [Test]
        public void Place_FreeCells_UpdatesPositionAndCells()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Single);
            BoardModel board = Board(new[] { block });

            board.Place(block, new Vector2Int(2, 3));

            Assert.AreEqual(new Vector2Int(2, 3), block.Position);
            Assert.AreSame(block, board.GetBlockAt(new Vector2Int(2, 3)));
            Assert.IsNull(board.GetBlockAt(new Vector2Int(0, 0)));
        }

        [Test]
        public void Place_OnAnotherBlock_Throws()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Single);
            BlockModel other = Block(_blue, 1, 0, Shapes.Single);
            BoardModel board = Board(new[] { block, other });

            Assert.Throws<InvalidOperationException>(() => board.Place(block, new Vector2Int(1, 0)));
        }

        [Test]
        public void FindExitDoor_FlushWithMatchingDoor_ReturnsDoor()
        {
            BlockModel block = Block(_red, 1, 4, Shapes.Horizontal2);
            var door = new DoorModel(BoardSide.Top, 1, 2, _red);
            BoardModel board = Board(new[] { block }, door);

            Assert.AreSame(door, board.FindExitDoor(block, BoardSide.Top));
        }

        [Test]
        public void FindExitDoor_OnVerticalSide_UsesBlockHeight()
        {
            BlockModel block = Block(_red, 0, 1, Shapes.Vertical2);
            var door = new DoorModel(BoardSide.Left, 1, 2, _red);
            BoardModel board = Board(new[] { block }, door);

            Assert.AreSame(door, board.FindExitDoor(block, BoardSide.Left));
        }

        [Test]
        public void FindExitDoor_MoveRuleForbidsSide_ReturnsNull()
        {
            BlockModel block = Block(_red, 1, 4, Shapes.Horizontal2, new FakeMoveRule(Vector2Int.up));
            BoardModel board = Board(new[] { block }, new DoorModel(BoardSide.Top, 1, 2, _red));

            Assert.IsNull(board.FindExitDoor(block, BoardSide.Top));
        }

        [Test]
        public void FindExitDoor_NotFlushWithSide_ReturnsNull()
        {
            BlockModel block = Block(_red, 1, 3, Shapes.Horizontal2);
            BoardModel board = Board(new[] { block }, new DoorModel(BoardSide.Top, 1, 2, _red));

            Assert.IsNull(board.FindExitDoor(block, BoardSide.Top));
        }

        [Test]
        public void FindExitDoor_DifferentColor_ReturnsNull()
        {
            BlockModel block = Block(_red, 1, 4, Shapes.Horizontal2);
            BoardModel board = Board(new[] { block }, new DoorModel(BoardSide.Top, 1, 2, _blue));

            Assert.IsNull(board.FindExitDoor(block, BoardSide.Top));
        }

        [Test]
        public void FindExitDoor_DoorNarrowerThanBlock_ReturnsNull()
        {
            BlockModel block = Block(_red, 1, 4, Shapes.Horizontal2);
            BoardModel board = Board(new[] { block }, new DoorModel(BoardSide.Top, 1, 1, _red));

            Assert.IsNull(board.FindExitDoor(block, BoardSide.Top));
        }

        [Test]
        public void FindExitDoor_BlockOutsideDoorSpan_ReturnsNull()
        {
            BlockModel block = Block(_red, 1, 4, Shapes.Horizontal2);
            BoardModel board = Board(new[] { block }, new DoorModel(BoardSide.Top, 2, 2, _red));

            Assert.IsNull(board.FindExitDoor(block, BoardSide.Top));
        }

        [Test]
        public void Remove_Block_FreesCellsAndDropsIt()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Horizontal2);
            BoardModel board = Board(new[] { block });

            board.Remove(block);

            Assert.IsNull(board.GetBlockAt(new Vector2Int(1, 0)));
            Assert.AreEqual(0, board.Blocks.Count);
        }

        private static BlockModel Block(BlockColor color, int x, int y, Vector2Int[] cells, params BlockFeature[] features)
        {
            return new BlockModel(0, color, new Vector2Int(x, y), cells, features);
        }

        private static BoardModel Board(IReadOnlyList<BlockModel> blocks, params DoorModel[] doors)
        {
            return new BoardModel(Size, Size, blocks, doors);
        }
    }
}
