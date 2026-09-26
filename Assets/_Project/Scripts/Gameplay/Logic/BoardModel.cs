using System;
using System.Collections.Generic;
using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Grid occupancy of the blocks, with the movement and exit rules.</summary>
    public sealed class BoardModel
    {
        private readonly BlockModel[] _occupancy;
        private readonly List<BlockModel> _blocks;
        private readonly List<DoorModel> _doors;

        public BoardModel(int width, int height, IReadOnlyList<BlockModel> blocks, IReadOnlyList<DoorModel> doors)
        {
            Width = width;
            Height = height;
            _occupancy = new BlockModel[width * height];
            _blocks = new List<BlockModel>(blocks);
            _doors = new List<DoorModel>(doors);

            for (int i = 0; i < _blocks.Count; i++)
            {
                Place(_blocks[i]);
            }
        }

        public int Width { get; }
        public int Height { get; }
        public IReadOnlyList<BlockModel> Blocks => _blocks;
        public IReadOnlyList<DoorModel> Doors => _doors;

        /// <summary>Returns the block that covers the cell, or null when the cell is empty or outside the board.</summary>
        public BlockModel GetBlockAt(Vector2Int cell)
        {
            return IsInside(cell) ? _occupancy[ToIndex(cell)] : null;
        }

        /// <summary>Returns how many cells the block can move in the direction before it hits a wall or another block.</summary>
        public int GetFreeDistance(BlockModel block, Vector2Int direction)
        {
            if (!block.CanMove(direction))
            {
                return 0;
            }

            int distance = 0;

            while (CanShift(block, direction * (distance + 1)))
            {
                distance++;
            }

            return distance;
        }

        /// <summary>Moves the block; the steps must not exceed the free distance.</summary>
        public void Move(BlockModel block, Vector2Int direction, int steps)
        {
            if (steps > GetFreeDistance(block, direction))
            {
                throw new InvalidOperationException($"Block {block.Id} cannot move {steps} cells toward {direction}.");
            }

            Fill(block, null);
            block.Position += direction * steps;
            Fill(block, block);
        }

        /// <summary>Returns the door the block can exit through on the side, or null when it cannot exit there.</summary>
        public DoorModel FindExitDoor(BlockModel block, BoardSide side)
        {
            RectInt bounds = block.Bounds;

            if (!IsFlush(bounds, side) || !block.CanMove(side.ToDirection()))
            {
                return null;
            }

            int first = side.IsHorizontal() ? bounds.xMin : bounds.yMin;
            int last = (side.IsHorizontal() ? bounds.xMax : bounds.yMax) - 1;

            for (int i = 0; i < _doors.Count; i++)
            {
                DoorModel door = _doors[i];

                if (door.Side == side && door.Color == block.Color && door.Start <= first && last <= door.End)
                {
                    return door;
                }
            }

            return null;
        }

        /// <summary>Takes the block off the board and frees its cells.</summary>
        public void Remove(BlockModel block)
        {
            Fill(block, null);
            _blocks.Remove(block);
        }

        private void Place(BlockModel block)
        {
            for (int i = 0; i < block.Cells.Count; i++)
            {
                Vector2Int cell = block.Position + block.Cells[i];

                if (!IsInside(cell))
                {
                    throw new ArgumentException($"Block {block.Id} covers {cell}, which is outside the board.");
                }

                if (_occupancy[ToIndex(cell)] != null)
                {
                    throw new ArgumentException($"Block {block.Id} overlaps another block at {cell}.");
                }

                _occupancy[ToIndex(cell)] = block;
            }
        }

        private bool CanShift(BlockModel block, Vector2Int offset)
        {
            for (int i = 0; i < block.Cells.Count; i++)
            {
                Vector2Int target = block.Position + block.Cells[i] + offset;

                if (!IsInside(target))
                {
                    return false;
                }

                BlockModel occupant = _occupancy[ToIndex(target)];

                if (occupant != null && occupant != block)
                {
                    return false;
                }
            }

            return true;
        }

        private void Fill(BlockModel block, BlockModel value)
        {
            for (int i = 0; i < block.Cells.Count; i++)
            {
                _occupancy[ToIndex(block.Position + block.Cells[i])] = value;
            }
        }

        private bool IsFlush(RectInt bounds, BoardSide side)
        {
            switch (side)
            {
                case BoardSide.Bottom: return bounds.yMin == 0;
                case BoardSide.Top: return bounds.yMax == Height;
                case BoardSide.Left: return bounds.xMin == 0;
                case BoardSide.Right: return bounds.xMax == Width;
                default: throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private bool IsInside(Vector2Int cell)
        {
            return LevelGeometry.IsInside(Width, Height, cell);
        }

        private int ToIndex(Vector2Int cell)
        {
            return cell.y * Width + cell.x;
        }
    }
}
