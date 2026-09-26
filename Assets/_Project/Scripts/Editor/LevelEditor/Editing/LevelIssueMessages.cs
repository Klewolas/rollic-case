using System.Collections.Generic;
using RollicCase.Gameplay.Logic;

namespace RollicCase.Editor.LevelEditor.Editing
{
    /// <summary>Plain-language descriptions of level problems for designers.</summary>
    public static class LevelIssueMessages
    {
        private static readonly Dictionary<LevelIssueType, string> s_templates = new Dictionary<LevelIssueType, string>
        {
            { LevelIssueType.BoardSizeOutOfRange, $"The board must be {LevelLimits.MinBoardSize} to {LevelLimits.MaxBoardSize} cells wide and tall." },
            { LevelIssueType.TimerOutOfRange, $"The timer must be {LevelLimits.MinTimerSeconds} to {LevelLimits.MaxTimerSeconds} seconds." },
            { LevelIssueType.NoBlocks, "The level has no blocks. Place at least one block." },
            { LevelIssueType.BlockOutOfBounds, "Block {0} is partly outside the board." },
            { LevelIssueType.BlocksOverlap, "Block {0} overlaps another block." },
            { LevelIssueType.DoorOutOfBounds, "Door {0} runs past the end of its side." },
            { LevelIssueType.DoorsOverlap, "Door {0} overlaps another door on the same side." },
            { LevelIssueType.MissingDoorForColor, "Block {0} has no door of its color, so it can never leave the board." },
            { LevelIssueType.BlockDoesNotFitAnyDoor, "Block {0} is wider than every door of its color, so it can never leave the board." }
        };

        /// <summary>Returns a sentence that explains the problem and names the block or door it concerns.</summary>
        public static string Describe(LevelIssue issue)
        {
            return string.Format(s_templates[issue.Type], issue.Index + 1);
        }
    }
}
