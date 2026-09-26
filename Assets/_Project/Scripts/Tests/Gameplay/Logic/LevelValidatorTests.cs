using System.Collections.Generic;
using NUnit.Framework;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using RollicCase.Tests.Fakes;
using UnityEngine;

namespace RollicCase.Tests.Gameplay.Logic
{
    public sealed class LevelValidatorTests
    {
        private const int Size = 5;
        private const int Timer = 60;

        private readonly LevelValidator _validator = new LevelValidator();
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
        public void Validate_PlayableLevel_ReturnsNoIssues()
        {
            LevelData level = ValidLevel();

            Assert.IsEmpty(_validator.Validate(level));
        }

        [TestCase(LevelLimits.MinBoardSize - 1)]
        [TestCase(LevelLimits.MaxBoardSize + 1)]
        public void Validate_BoardSizeOutOfRange_ReportsIt(int width)
        {
            LevelData level = ValidLevel();
            level.SetSize(width, Size);

            AssertHasIssue(_validator.Validate(level), LevelIssueType.BoardSizeOutOfRange);
        }

        [TestCase(LevelLimits.MinTimerSeconds - 1)]
        [TestCase(LevelLimits.MaxTimerSeconds + 1)]
        public void Validate_TimerOutOfRange_ReportsIt(int seconds)
        {
            LevelData level = ValidLevel();
            level.SetTimer(seconds);

            AssertHasIssue(_validator.Validate(level), LevelIssueType.TimerOutOfRange);
        }

        [Test]
        public void Validate_NoBlocks_ReportsIt()
        {
            LevelData level = _assets.CreateLevel(Size, Size, Timer);

            AssertHasIssue(_validator.Validate(level), LevelIssueType.NoBlocks);
        }

        [Test]
        public void Validate_BlockOutsideBoard_ReportsBlockIndex()
        {
            LevelData level = ValidLevel();
            level.AddBlock(new BlockData(_red, new Vector2Int(4, 0), Shapes.Horizontal2));

            AssertHasIssue(_validator.Validate(level), LevelIssueType.BlockOutOfBounds, 1);
        }

        [Test]
        public void Validate_OverlappingBlocks_ReportsLaterBlock()
        {
            LevelData level = ValidLevel();
            level.AddBlock(new BlockData(_red, new Vector2Int(1, 1), Shapes.Single));

            AssertHasIssue(_validator.Validate(level), LevelIssueType.BlocksOverlap, 1);
        }

        [Test]
        public void Validate_DoorPastSideEnd_ReportsDoorIndex()
        {
            LevelData level = ValidLevel();
            level.AddDoor(new DoorData(BoardSide.Right, 4, 2, _red));

            AssertHasIssue(_validator.Validate(level), LevelIssueType.DoorOutOfBounds, 1);
        }

        [Test]
        public void Validate_OverlappingDoorsOnSameSide_ReportsLaterDoor()
        {
            LevelData level = ValidLevel();
            level.AddDoor(new DoorData(BoardSide.Top, 2, 1, _blue));

            AssertHasIssue(_validator.Validate(level), LevelIssueType.DoorsOverlap, 1);
        }

        [Test]
        public void Validate_BlockColorWithoutDoor_ReportsMissingDoorOnly()
        {
            LevelData level = ValidLevel();
            level.AddBlock(new BlockData(_blue, new Vector2Int(4, 4), Shapes.Single));

            IReadOnlyList<LevelIssue> issues = _validator.Validate(level);

            AssertHasIssue(issues, LevelIssueType.MissingDoorForColor, 1);
            Assert.AreEqual(1, issues.Count);
        }

        [Test]
        public void Validate_BlockWiderThanEveryDoorOfItsColor_ReportsIt()
        {
            LevelData level = ValidLevel();
            level.AddBlock(new BlockData(_blue, new Vector2Int(4, 3), Shapes.Vertical2));
            level.AddDoor(new DoorData(BoardSide.Left, 0, 1, _blue));

            AssertHasIssue(_validator.Validate(level), LevelIssueType.BlockDoesNotFitAnyDoor, 1);
        }

        private LevelData ValidLevel()
        {
            LevelData level = _assets.CreateLevel(Size, Size, Timer);
            level.AddBlock(new BlockData(_red, new Vector2Int(1, 1), Shapes.Horizontal2));
            level.AddDoor(new DoorData(BoardSide.Top, 1, 2, _red));
            return level;
        }

        private static void AssertHasIssue(IReadOnlyList<LevelIssue> issues, LevelIssueType type, int index = LevelIssue.NoIndex)
        {
            foreach (LevelIssue issue in issues)
            {
                if (issue.Type == type && issue.Index == index)
                {
                    return;
                }
            }

            Assert.Fail($"Expected issue {type} at index {index}.");
        }
    }
}
