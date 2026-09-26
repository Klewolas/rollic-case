using System.Collections.Generic;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor.Editing
{
    /// <summary>Edit operations on level data; every operation keeps the level free of overlaps and out-of-bounds content.</summary>
    public sealed class LevelEditing
    {
        public const int None = -1;

        /// <summary>Returns the index of the block that covers the cell, or None.</summary>
        public int FindBlockAt(LevelData level, Vector2Int cell)
        {
            for (int i = 0; i < level.Blocks.Count; i++)
            {
                BlockData block = level.Blocks[i];

                for (int c = 0; c < block.Cells.Count; c++)
                {
                    if (block.Origin + block.Cells[c] == cell)
                    {
                        return i;
                    }
                }
            }

            return None;
        }

        /// <summary>Returns whether the cells fit inside the board without covering another block.</summary>
        public bool CanPlace(LevelData level, Vector2Int origin, IReadOnlyList<Vector2Int> cells, int ignoredBlock = None)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                Vector2Int cell = origin + cells[i];

                if (!LevelGeometry.IsInside(level.Width, level.Height, cell))
                {
                    return false;
                }

                int occupant = FindBlockAt(level, cell);

                if (occupant != None && occupant != ignoredBlock)
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryAddBlock(LevelData level, BlockColor color, Vector2Int origin, IReadOnlyList<Vector2Int> cells)
        {
            if (!CanPlace(level, origin, cells))
            {
                return false;
            }

            level.AddBlock(new BlockData(color, origin, cells));
            return true;
        }

        public bool TryMoveBlock(LevelData level, int index, Vector2Int origin)
        {
            BlockData block = level.Blocks[index];

            if (!CanPlace(level, origin, block.Cells, index))
            {
                return false;
            }

            level.SetBlock(index, new BlockData(block.Color, origin, block.Cells));
            return true;
        }

        /// <summary>Turns the block clockwise in place when the rotated shape fits.</summary>
        public bool TryRotateBlock(LevelData level, int index)
        {
            BlockData block = level.Blocks[index];
            Vector2Int[] rotated = ShapeRotation.RotateClockwise(block.Cells);

            if (!CanPlace(level, block.Origin, rotated, index))
            {
                return false;
            }

            level.SetBlock(index, new BlockData(block.Color, block.Origin, rotated));
            return true;
        }

        public void RemoveBlock(LevelData level, int index)
        {
            level.RemoveBlockAt(index);
        }

        /// <summary>Returns the index of the door that covers the position on the side, or None.</summary>
        public int FindDoorAt(LevelData level, BoardSide side, int position)
        {
            for (int i = 0; i < level.Doors.Count; i++)
            {
                if (LevelGeometry.DoorsOverlap(level.Doors[i], side, position, 1))
                {
                    return i;
                }
            }

            return None;
        }

        /// <summary>Adds a one-cell door at the position when the spot is free.</summary>
        public bool TryAddDoor(LevelData level, BoardSide side, int position, BlockColor color)
        {
            if (!IsSpanFree(level, side, position, 1, None))
            {
                return false;
            }

            level.AddDoor(new DoorData(side, position, 1, color));
            return true;
        }

        /// <summary>Changes where the door starts and how long it is when the new span is free and inside the side.</summary>
        public bool TrySetDoorSpan(LevelData level, int index, int start, int length)
        {
            DoorData door = level.Doors[index];

            if (!IsSpanFree(level, door.Side, start, length, index))
            {
                return false;
            }

            level.SetDoor(index, new DoorData(door.Side, start, length, door.Color));
            return true;
        }

        public void RecolorDoor(LevelData level, int index, BlockColor color)
        {
            DoorData door = level.Doors[index];
            level.SetDoor(index, new DoorData(door.Side, door.Start, door.Length, color));
        }

        public void RemoveDoor(LevelData level, int index)
        {
            level.RemoveDoorAt(index);
        }

        /// <summary>Returns how many blocks and doors a resize to the new size would remove.</summary>
        public int CountContentOutside(LevelData level, int width, int height)
        {
            int count = 0;

            for (int i = 0; i < level.Blocks.Count; i++)
            {
                if (!FitsBoard(level.Blocks[i], width, height))
                {
                    count++;
                }
            }

            for (int i = 0; i < level.Doors.Count; i++)
            {
                if (!FitsBoard(level.Doors[i], width, height))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>Changes the board size and removes blocks and doors that no longer fit.</summary>
        public void Resize(LevelData level, int width, int height)
        {
            for (int i = level.Blocks.Count - 1; i >= 0; i--)
            {
                if (!FitsBoard(level.Blocks[i], width, height))
                {
                    level.RemoveBlockAt(i);
                }
            }

            for (int i = level.Doors.Count - 1; i >= 0; i--)
            {
                if (!FitsBoard(level.Doors[i], width, height))
                {
                    level.RemoveDoorAt(i);
                }
            }

            level.SetSize(width, height);
        }

        private bool IsSpanFree(LevelData level, BoardSide side, int start, int length, int ignoredDoor)
        {
            if (!LevelGeometry.IsDoorSpanInside(level.Width, level.Height, side, start, length))
            {
                return false;
            }

            for (int i = 0; i < level.Doors.Count; i++)
            {
                if (i != ignoredDoor && LevelGeometry.DoorsOverlap(level.Doors[i], side, start, length))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool FitsBoard(BlockData block, int width, int height)
        {
            for (int i = 0; i < block.Cells.Count; i++)
            {
                if (!LevelGeometry.IsInside(width, height, block.Origin + block.Cells[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool FitsBoard(DoorData door, int width, int height)
        {
            return LevelGeometry.IsDoorSpanInside(width, height, door.Side, door.Start, door.Length);
        }
    }
}
