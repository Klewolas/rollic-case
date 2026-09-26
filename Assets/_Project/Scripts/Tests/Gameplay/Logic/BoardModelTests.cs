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
        public void GetFreeDistance_OpenRow_ReturnsDistanceToWalls()
        {
            BlockModel block = Block(_red, 1, 2, Shapes.Single);
            BoardModel board = Board(new[] { block });

            Assert.AreEqual(3, board.GetFreeDistance(block, Vector2Int.right));
            Assert.AreEqual(1, board.GetFreeDistance(block, Vector2Int.left));
        }

        [Test]
        public void GetFreeDistance_OtherBlockInPath_StopsBeforeIt()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Single);
            BlockModel blocker = Block(_blue, 3, 0, Shapes.Single);
            BoardModel board = Board(new[] { block, blocker });

            Assert.AreEqual(2, board.GetFreeDistance(block, Vector2Int.right));
        }

        [Test]
        public void GetFreeDistance_OneCellOfShapeBlocked_LimitsWholeShape()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.L);
            BlockModel blocker = Block(_blue, 3, 1, Shapes.Single);
            BoardModel board = Board(new[] { block, blocker });

            Assert.AreEqual(2, board.GetFreeDistance(block, Vector2Int.right));
        }

        [Test]
        public void GetFreeDistance_OwnCellsInPath_AreNotObstacles()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Vertical2);
            BoardModel board = Board(new[] { block });

            Assert.AreEqual(3, board.GetFreeDistance(block, Vector2Int.up));
        }

        [Test]
        public void GetFreeDistance_MoveRuleForbidsDirection_ReturnsZero()
        {
            BlockModel block = Block(_red, 1, 1, Shapes.Single, new FakeMoveRule(Vector2Int.right));
            BoardModel board = Board(new[] { block });

            Assert.AreEqual(0, board.GetFreeDistance(block, Vector2Int.right));
        }

        [Test]
        public void GetFreeDistance_MoveRuleForbidsOtherDirection_ReturnsFreeCells()
        {
            BlockModel block = Block(_red, 1, 1, Shapes.Single, new FakeMoveRule(Vector2Int.right));
            BoardModel board = Board(new[] { block });

            Assert.AreEqual(1, board.GetFreeDistance(block, Vector2Int.left));
        }

        [Test]
        public void Move_WithinFreeDistance_UpdatesPositionAndCells()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Single);
            BoardModel board = Board(new[] { block });

            board.Move(block, Vector2Int.right, 2);

            Assert.AreEqual(new Vector2Int(2, 0), block.Position);
            Assert.AreSame(block, board.GetBlockAt(new Vector2Int(2, 0)));
            Assert.IsNull(board.GetBlockAt(new Vector2Int(0, 0)));
        }

        [Test]
        public void Move_BeyondFreeDistance_Throws()
        {
            BlockModel block = Block(_red, 0, 0, Shapes.Single);
            BoardModel board = Board(new[] { block });

            Assert.Throws<InvalidOperationException>(() => board.Move(block, Vector2Int.left, 1));
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
