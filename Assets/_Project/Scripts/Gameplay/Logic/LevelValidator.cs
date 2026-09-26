using System.Collections.Generic;
using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Checks a level for problems that would make it broken or unplayable.</summary>
    public sealed class LevelValidator
    {
        /// <summary>Returns every problem found; an empty list means the level is valid.</summary>
        public IReadOnlyList<LevelIssue> Validate(LevelData level)
        {
            var issues = new List<LevelIssue>();

            if (!IsInRange(level.Width, LevelLimits.MinBoardSize, LevelLimits.MaxBoardSize)
                || !IsInRange(level.Height, LevelLimits.MinBoardSize, LevelLimits.MaxBoardSize))
            {
                issues.Add(new LevelIssue(LevelIssueType.BoardSizeOutOfRange));
            }

            if (!IsInRange(level.TimerSeconds, LevelLimits.MinTimerSeconds, LevelLimits.MaxTimerSeconds))
            {
                issues.Add(new LevelIssue(LevelIssueType.TimerOutOfRange));
            }

            if (level.Blocks.Count == 0)
            {
                issues.Add(new LevelIssue(LevelIssueType.NoBlocks));
            }

            ValidateBlocks(level, issues);
            ValidateDoors(level, issues);
            return issues;
        }

        private static void ValidateBlocks(LevelData level, List<LevelIssue> issues)
        {
            var occupied = new HashSet<Vector2Int>();

            for (int i = 0; i < level.Blocks.Count; i++)
            {
                BlockData block = level.Blocks[i];
                bool isOutOfBounds = false;
                bool overlaps = false;

                for (int c = 0; c < block.Cells.Count; c++)
                {
                    Vector2Int cell = block.Origin + block.Cells[c];

                    if (!IsInside(level, cell))
                    {
                        isOutOfBounds = true;
                    }
                    else if (!occupied.Add(cell))
                    {
                        overlaps = true;
                    }
                }

                if (isOutOfBounds)
                {
                    issues.Add(new LevelIssue(LevelIssueType.BlockOutOfBounds, i));
                }

                if (overlaps)
                {
                    issues.Add(new LevelIssue(LevelIssueType.BlocksOverlap, i));
                }

                ValidateExit(level, block, i, issues);
            }
        }

        private static void ValidateExit(LevelData level, BlockData block, int index, List<LevelIssue> issues)
        {
            RectInt bounds = CellBounds.Calculate(block.Cells);
            bool hasDoor = false;

            for (int i = 0; i < level.Doors.Count; i++)
            {
                DoorData door = level.Doors[i];

                if (door.Color != block.Color)
                {
                    continue;
                }

                hasDoor = true;
                int extent = door.Side.IsHorizontal() ? bounds.width : bounds.height;

                if (extent <= door.Length)
                {
                    return;
                }
            }

            issues.Add(new LevelIssue(hasDoor ? LevelIssueType.BlockDoesNotFitAnyDoor : LevelIssueType.MissingDoorForColor, index));
        }

        private static void ValidateDoors(LevelData level, List<LevelIssue> issues)
        {
            for (int i = 0; i < level.Doors.Count; i++)
            {
                DoorData door = level.Doors[i];
                int sideLength = door.Side.IsHorizontal() ? level.Width : level.Height;

                if (door.Length < 1 || door.Start < 0 || door.Start + door.Length > sideLength)
                {
                    issues.Add(new LevelIssue(LevelIssueType.DoorOutOfBounds, i));
                }

                for (int j = 0; j < i; j++)
                {
                    if (Overlap(door, level.Doors[j]))
                    {
                        issues.Add(new LevelIssue(LevelIssueType.DoorsOverlap, i));
                        break;
                    }
                }
            }
        }

        private static bool Overlap(DoorData first, DoorData second)
        {
            return first.Side == second.Side
                && first.Start < second.Start + second.Length
                && second.Start < first.Start + first.Length;
        }

        private static bool IsInside(LevelData level, Vector2Int cell)
        {
            return cell.x >= 0 && cell.y >= 0 && cell.x < level.Width && cell.y < level.Height;
        }

        private static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}
