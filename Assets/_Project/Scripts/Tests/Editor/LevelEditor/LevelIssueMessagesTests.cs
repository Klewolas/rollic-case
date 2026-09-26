using System;
using NUnit.Framework;
using RollicCase.Gameplay.Logic;
using RollicCase.Editor.LevelEditor.Editing;

namespace RollicCase.Tests.Editor.LevelEditor
{
    public sealed class LevelIssueMessagesTests
    {
        [Test]
        public void Describe_EveryIssueType_ReturnsAMessage()
        {
            foreach (LevelIssueType type in Enum.GetValues(typeof(LevelIssueType)))
            {
                Assert.IsNotEmpty(LevelIssueMessages.Describe(new LevelIssue(type, 0)), type.ToString());
            }
        }

        [Test]
        public void Describe_BlockIssue_NamesTheBlockCountingFromOne()
        {
            string message = LevelIssueMessages.Describe(new LevelIssue(LevelIssueType.BlocksOverlap, 2));

            StringAssert.Contains("Block 3", message);
        }

        [Test]
        public void Describe_DoorIssue_NamesTheDoorCountingFromOne()
        {
            string message = LevelIssueMessages.Describe(new LevelIssue(LevelIssueType.DoorsOverlap, 0));

            StringAssert.Contains("Door 1", message);
        }
    }
}
