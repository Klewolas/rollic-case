using NUnit.Framework;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using RollicCase.Tests.Fakes;
using UnityEngine;

namespace RollicCase.Tests.Gameplay.Logic
{
    public sealed class BoardFactoryTests
    {
        private readonly BoardFactory _factory = new BoardFactory();
        private TestAssets _assets;
        private BlockColor _red;
        private LevelData _level;

        [SetUp]
        public void SetUp()
        {
            _assets = new TestAssets();
            _red = _assets.CreateBlockColor();
            _level = _assets.CreateLevel(6, 4, 60);
            _level.AddBlock(new BlockData(_red, new Vector2Int(1, 2), Shapes.L));
            _level.AddDoor(new DoorData(BoardSide.Right, 1, 2, _red));
        }

        [TearDown]
        public void TearDown()
        {
            _assets.DestroyAll();
        }

        [Test]
        public void Create_Level_CopiesSizeBlocksAndDoors()
        {
            BoardModel board = _factory.Create(_level);

            Assert.AreEqual(6, board.Width);
            Assert.AreEqual(4, board.Height);
            Assert.AreEqual(1, board.Blocks.Count);
            Assert.AreSame(_red, board.Blocks[0].Color);
            Assert.AreSame(board.Blocks[0], board.GetBlockAt(new Vector2Int(1, 3)));
            Assert.AreEqual(1, board.Doors.Count);
            Assert.AreEqual(BoardSide.Right, board.Doors[0].Side);
            Assert.AreEqual(2, board.Doors[0].Length);
        }

        [Test]
        public void Create_CalledTwice_ReturnsIndependentBoards()
        {
            BoardModel first = _factory.Create(_level);
            BoardModel second = _factory.Create(_level);

            first.Move(first.Blocks[0], Vector2Int.right, 1);

            Assert.AreEqual(new Vector2Int(1, 2), second.Blocks[0].Position);
        }

        [Test]
        public void Create_BlockWithFeatureData_AttachesCreatedFeature()
        {
            _level.AddBlock(new BlockData(_red, new Vector2Int(4, 0), Shapes.Single, new[] { new FakeFeatureData() }));

            BoardModel board = _factory.Create(_level);

            Assert.AreEqual(1, board.Blocks[1].Features.Count);
            Assert.IsInstanceOf<FakeExitListener>(board.Blocks[1].Features[0]);
        }
    }
}
